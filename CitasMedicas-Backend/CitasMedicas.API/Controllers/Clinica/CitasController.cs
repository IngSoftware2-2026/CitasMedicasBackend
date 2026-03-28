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
    public class CitasController : Controller
    {
        private readonly ClinicaService _clinicaService;

        public CitasController(ClinicaService clinicaService)
        {
            _clinicaService = clinicaService ?? throw new ArgumentNullException(nameof(clinicaService));
        }

        [HttpPost("Insertar")]
        public IActionResult Insertar([FromBody] CitasInsertarDTO cita)
        {
            Console.WriteLine($"[Citas/Insertar] solicitudId={cita?.SolicitudId}, pacienteId={cita?.PacienteId}, medicoId={cita?.MedicoId}, salaId={cita?.SalaId}, inicio={cita?.Inicio:O}, fin={cita?.Fin:O}, duracion={cita?.DuracionMinutos}, creadaPorUsuarioId={cita?.CreadaPorUsuarioId}");
            var result = _clinicaService.CitaInsertar(cita);
            Console.WriteLine($"[Citas/Insertar] resultCode={result.Code}, success={result.Success}, message={result.Message}");
            return StatusCode(result.Code, result);
        }

        [HttpPost("ObtenerPorFiltro")]
        [Authorize(Roles = "ADMIN,RECEP,DEV,PACIENTE,DOCTOR")]
        public IActionResult ObtenerPorFiltro([FromBody] CitasFiltroDTO filtro)
        {
            var rolUsuario = User.FindFirst(ClaimTypes.Role)?.Value;
            if (rolUsuario == "PACIENTE")
            {
                var pacienteId = ObtenerPacienteIdDelUsuarioAutenticado();
                if (!pacienteId.HasValue)
                {
                    var forbidden = new BusinessLogic.ServiceResult().Forbidden("No se pudo identificar el perfil de paciente autenticado.");
                    return StatusCode(forbidden.Code, forbidden);
                }

                filtro.PacienteId = pacienteId.Value;
            }
            else if (rolUsuario == "DOCTOR")
            {
                var medicoId = ObtenerDoctorIdDelUsuarioAutenticado();
                if (!medicoId.HasValue)
                {
                    var forbidden = new BusinessLogic.ServiceResult().Forbidden("No se pudo identificar el perfil de doctor autenticado.");
                    return StatusCode(forbidden.Code, forbidden);
                }

                filtro.MedicoId = medicoId.Value;
            }

            var result = _clinicaService.CitasObtenerPorFiltro(filtro);
            return StatusCode(result.Code, result);
        }

        [HttpGet("ObtenerPorId")]
        [Authorize(Roles = "ADMIN,RECEP,DEV,PACIENTE,DOCTOR")]
        public IActionResult ObtenerPorId(int citaId)
        {
            var result = _clinicaService.CitaObtenerPorId(citaId);

            var rolUsuario = User.FindFirst(ClaimTypes.Role)?.Value;
            if (rolUsuario == "PACIENTE" && result.Success && result.Data is CitasDetalleDTO citaPaciente)
            {
                var pacienteId = ObtenerPacienteIdDelUsuarioAutenticado();
                if (!pacienteId.HasValue || citaPaciente.PacienteId != pacienteId.Value)
                {
                    var forbidden = new BusinessLogic.ServiceResult().Forbidden("Un paciente solo puede ver sus propias citas.");
                    return StatusCode(forbidden.Code, forbidden);
                }
            }
            else if (rolUsuario == "DOCTOR" && result.Success && result.Data is CitasDetalleDTO citaDoctor)
            {
                var medicoId = ObtenerDoctorIdDelUsuarioAutenticado();
                if (!medicoId.HasValue || citaDoctor.MedicoId != medicoId.Value)
                {
                    var forbidden = new BusinessLogic.ServiceResult().Forbidden("Un doctor solo puede ver sus propias citas.");
                    return StatusCode(forbidden.Code, forbidden);
                }
            }

            return StatusCode(result.Code, result);
        }

        [HttpPost("CambiarEstado")]
        public IActionResult CambiarEstado([FromBody] CitasCambiarEstadoDTO cambioEstado)
        {
            var result = _clinicaService.CitaCambiarEstado(cambioEstado);
            return StatusCode(result.Code, result);
        }

        [HttpPost("ActualizarSala")]
        public IActionResult ActualizarSala([FromBody] CitasActualizarSalaDTO actualizacionSala)
        {
            var result = _clinicaService.CitaActualizarSala(actualizacionSala);
            return StatusCode(result.Code, result);
        }

        private string? ObtenerUsuarioIdClaim()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                   ?? User.FindFirst("sub")?.Value;
        }

        private int? ObtenerPacienteIdDelUsuarioAutenticado()
        {
            var usuarioIdClaim = ObtenerUsuarioIdClaim();
            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return null;
            }

            var pacienteResult = _clinicaService.ObtenerPacientePorUsuarioId(usuarioId);
            if (!pacienteResult.Success || pacienteResult.Data is not PacientesDTO paciente)
            {
                return null;
            }

            return paciente.PacienteId;
        }

        private int? ObtenerDoctorIdDelUsuarioAutenticado()
        {
            var usuarioIdClaim = ObtenerUsuarioIdClaim();
            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return null;
            }

            var doctorResult = _clinicaService.ObtenerDoctorPorUsuarioId(usuarioId);
            if (!doctorResult.Success || doctorResult.Data is not DoctoresDTO doctor)
            {
                return null;
            }

            return doctor.MedicoId;
        }
    }
}
