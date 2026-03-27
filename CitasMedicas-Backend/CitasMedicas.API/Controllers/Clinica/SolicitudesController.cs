using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // ── Endpoints públicos (sin autenticación) ──

        [HttpPost("/Publicas/Insertar")]
        public IActionResult Insertar([FromBody] SolicitudesPublicasDTO solicitud)
        {
            var result = _clinicaService.SolicitudPublicaInsertar(solicitud);
            return StatusCode(result.Code, result);
        }

        // ── Endpoints de usuario registrado ──

        [HttpPost("/Usuarios/Insertar")]
        [Authorize]
        public IActionResult Insertar([FromBody] SolicitudesDTO solicitud)
        {
            var result = _clinicaService.SolicitudCitaInsertar(solicitud);
            return StatusCode(result.Code, result);
        }

        // ── Endpoints de administración (requieren rol) ──

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
        [Authorize(Roles = "ADMIN,RECEP,DEV")]
        public IActionResult ListarUsuarios([FromBody] SolicitudesFiltroDTO filtro)
        {
            var result = _clinicaService.SolicitudesCitaListar(filtro);
            return StatusCode(result.Code, result);
        }

        [HttpGet("/Usuarios/ObtenerPorId")]
        [Authorize(Roles = "ADMIN,RECEP,DEV")]
        public IActionResult ObtenerUsuarioPorId(int solicitudId)
        {
            var result = _clinicaService.SolicitudesCitaObtenerPorId(solicitudId);
            return StatusCode(result.Code, result);
        }

        [HttpPost("/Usuarios/CambiarEstado")]
        [Authorize(Roles = "ADMIN,RECEP,DEV")]
        public IActionResult CambiarEstadoUsuario([FromBody] CambiarEstadoSolicitudDTO cambio)
        {
            var result = _clinicaService.SolicitudesCitaCambiarEstado(cambio);
            return StatusCode(result.Code, result);
        }
    }
}

