using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CitasMedicas.API.Controllers.Clinica
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PacientesController : Controller
    {
        private readonly ClinicaService _clinicaService;

        public PacientesController(ClinicaService clinicaService)
        {
            _clinicaService = clinicaService ?? throw new ArgumentNullException(nameof(clinicaService));
        }

        [HttpGet("Listar")]
        [Authorize(Roles = "ADMIN,DOCTOR,RECEP,DEV")]
        public IActionResult Listar()
        {
            var result = _clinicaService.ListarPacientes();
            return StatusCode(result.Code, result);
        }

        [HttpGet("ObtenerPorId")]
        [Authorize(Roles = "ADMIN,DOCTOR,RECEP,DEV,PACIENTE")]
        public IActionResult ObtenerPorId(int pacienteId)
        {
            var rolUsuario = User.FindFirst(ClaimTypes.Role)?.Value;
            var usuarioIdClaim = ObtenerUsuarioIdClaim();

            var result = _clinicaService.ObtenerPacientePorId(pacienteId);

            // Si es PACIENTE, solo puede ver sus propios datos
            if (rolUsuario == "PACIENTE" && result.Success && result.Data != null)
            {
                int usuarioIdPaciente = 0;

                if (result.Data is PacientesDTO dto)
                    usuarioIdPaciente = dto.UsuarioId ?? 0;
                else
                    usuarioIdPaciente = (int)result.Data.UsuarioId;

                if (int.TryParse(usuarioIdClaim, out int usuarioId) && usuarioIdPaciente != usuarioId)
                {
                    var forbidden = new BusinessLogic.ServiceResult().Forbidden("Un paciente solo puede ver sus propios datos.");
                    return StatusCode(forbidden.Code, forbidden);
                }
            }

            return StatusCode(result.Code, result);
        }

        [HttpPost("Insertar")]
        [Authorize(Roles = "ADMIN,RECEP,DEV")]
        public IActionResult Insertar([FromBody] PacientesDTO paciente)
        {
            var result = _clinicaService.PacientesInsertar(paciente);
            return StatusCode(result.Code, result);
        }

        [HttpPost("Editar")]
        [Authorize(Roles = "ADMIN,DOCTOR,RECEP,DEV,PACIENTE")]
        public IActionResult Editar([FromBody] PacientesDTO paciente)
        {
            var rolUsuario = User.FindFirst(ClaimTypes.Role)?.Value;
            var usuarioIdClaim = ObtenerUsuarioIdClaim();

            // Si es PACIENTE, solo puede editar sus propios datos
            if (rolUsuario == "PACIENTE" && int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                if (paciente.UsuarioId != usuarioId)
                {
                    var forbidden = new BusinessLogic.ServiceResult().Forbidden("Un paciente solo puede editar sus propios datos.");
                    return StatusCode(forbidden.Code, forbidden);
                }
            }

            var result = _clinicaService.PacientesEditar(paciente);
            return StatusCode(result.Code, result);
        }

        [HttpPost("CompletarPerfil")]
        [Authorize(Roles = "PACIENTE")]
        public IActionResult CompletarPerfil([FromBody] PacientesDTO paciente)
        {
            var usuarioIdClaim = ObtenerUsuarioIdClaim();
            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                var badRequest = new BusinessLogic.ServiceResult().BadRequest("No se pudo identificar el usuario autenticado.");
                return StatusCode(badRequest.Code, badRequest);
            }

            paciente.UsuarioId = usuarioId;
            paciente.Activo = true;

            var existenteResult = _clinicaService.ObtenerPacientePorUsuarioId(usuarioId);
            if (existenteResult.Success && existenteResult.Data is PacientesDTO existente)
            {
                paciente.PacienteId = existente.PacienteId;
                var editar = _clinicaService.PacientesEditar(paciente);
                return StatusCode(editar.Code, editar);
            }

            var insertar = _clinicaService.PacientesInsertar(paciente);
            return StatusCode(insertar.Code, insertar);
        }

        [HttpGet("PerfilActual")]
        [Authorize(Roles = "PACIENTE")]
        public IActionResult PerfilActual()
        {
            var usuarioIdClaim = ObtenerUsuarioIdClaim();
            var rolClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            Console.WriteLine($"[PerfilActual] role={rolClaim ?? "null"}, userIdClaim={usuarioIdClaim ?? "null"}");

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                var badRequest = new BusinessLogic.ServiceResult().BadRequest("No se pudo identificar el usuario autenticado.");
                return StatusCode(badRequest.Code, badRequest);
            }

            var result = _clinicaService.ObtenerPacientePorUsuarioId(usuarioId);
            Console.WriteLine($"[PerfilActual] resultCode={result.Code}, success={result.Success}, message={result.Message}");
            if (result.Code == 404)
            {
                var sinPerfil = new BusinessLogic.ServiceResult()
                    .Ok("Perfil de paciente aun no completado.", null);
                return StatusCode(sinPerfil.Code, sinPerfil);
            }

            return StatusCode(result.Code, result);
        }

        [HttpDelete("Eliminar")]
        [Authorize(Roles = "ADMIN,DEV")]
        public IActionResult Eliminar(int pacienteId)
        {
            var result = _clinicaService.PacientesEliminar(pacienteId);
            return StatusCode(result.Code, result);
        }

        private string? ObtenerUsuarioIdClaim()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                   ?? User.FindFirst("sub")?.Value;
        }
    }
}
