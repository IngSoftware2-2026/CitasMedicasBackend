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
    public class SolicitudesController : Controller
    {
        private readonly ClinicaService _clinicaService;

        public SolicitudesController(ClinicaService clinicaService)
        {
            _clinicaService = clinicaService ?? throw new ArgumentNullException(nameof(clinicaService));
        }

        [HttpPost("/Publicas/Insertar")]
        public IActionResult Insertar([FromBody] SolicitudesPublicasDTO solicitud)
        {
            var result = _clinicaService.SolicitudPublicaInsertar(solicitud);
            return StatusCode(result.Code, result);
        }

        [HttpPost("/Usuarios/Insertar")]
        [Authorize(Roles = "ADMIN,RECEP,DEV,PACIENTE")]
        public IActionResult Insertar([FromBody] SolicitudesDTO solicitud)
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

                solicitud.PacienteId = pacienteId.Value;
            }

            var result = _clinicaService.SolicitudCitaInsertar(solicitud);
            return StatusCode(result.Code, result);
        }

        [HttpPost("/Publicas/Listar")]
        [Authorize(Roles = "ADMIN,RECEP,DEV")]
        public IActionResult ListarPublicas([FromBody] SolicitudesFiltroDTO filtro)
        {
            var result = _clinicaService.SolicitudesPublicasListar(filtro);
            return StatusCode(result.Code, result);
        }

        [HttpGet("/Publicas/ObtenerPorId")]
        [Authorize(Roles = "ADMIN,RECEP,DEV")]
        public IActionResult ObtenerPublicaPorId(int solicitudId)
        {
            var result = _clinicaService.SolicitudesPublicasObtenerPorId(solicitudId);
            return StatusCode(result.Code, result);
        }

        [HttpPost("/Publicas/CambiarEstado")]
        [Authorize(Roles = "ADMIN,RECEP,DEV")]
        public IActionResult CambiarEstadoPublica([FromBody] CambiarEstadoSolicitudDTO cambio)
        {
            var result = _clinicaService.SolicitudesPublicasCambiarEstado(cambio);
            return StatusCode(result.Code, result);
        }

        [HttpPost("/Usuarios/Listar")]
        [Authorize(Roles = "ADMIN,RECEP,DEV,PACIENTE")]
        public IActionResult ListarUsuarios([FromBody] SolicitudesFiltroDTO filtro)
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

            var result = _clinicaService.SolicitudesCitaListar(filtro);
            return StatusCode(result.Code, result);
        }

        [HttpGet("/Usuarios/ObtenerPorId")]
        [Authorize(Roles = "ADMIN,RECEP,DEV,PACIENTE")]
        public IActionResult ObtenerUsuarioPorId(int solicitudId)
        {
            var result = _clinicaService.SolicitudesCitaObtenerPorId(solicitudId);

            var rolUsuario = User.FindFirst(ClaimTypes.Role)?.Value;
            if (rolUsuario == "PACIENTE" && result.Success && result.Data is SolicitudesCitaListadoDTO solicitud)
            {
                var pacienteId = ObtenerPacienteIdDelUsuarioAutenticado();
                if (!pacienteId.HasValue || solicitud.PacienteId != pacienteId.Value)
                {
                    var forbidden = new BusinessLogic.ServiceResult().Forbidden("Un paciente solo puede ver sus propias solicitudes.");
                    return StatusCode(forbidden.Code, forbidden);
                }
            }

            return StatusCode(result.Code, result);
        }

        [HttpPost("/Usuarios/CambiarEstado")]
        [Authorize(Roles = "ADMIN,RECEP,DEV")]
        public IActionResult CambiarEstadoUsuario([FromBody] CambiarEstadoSolicitudDTO cambio)
        {
            var result = _clinicaService.SolicitudesCitaCambiarEstado(cambio);
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
    }
}
