using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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

        [HttpDelete("Eliminar")]
        [Authorize(Roles = "ADMIN,DEV")]
        public IActionResult Eliminar(int pacienteId)
        {
            var result = _clinicaService.PacientesEliminar(pacienteId);
            return StatusCode(result.Code, result);
        }
    }
}
