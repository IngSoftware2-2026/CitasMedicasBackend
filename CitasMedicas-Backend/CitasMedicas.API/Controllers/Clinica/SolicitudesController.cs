using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
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

        [HttpPost("/Publicas/Insertar")]
        public IActionResult Insertar([FromBody] SolicitudesPublicasDTO solicitud)
        {
            var result = _clinicaService.SolicitudPublicaInsertar(solicitud);
            return StatusCode(result.Code, result);
        }

        [HttpPost("/Usuarios/Insertar")]
        public IActionResult Insertar([FromBody] SolicitudesDTO solicitud)
        {
            var result = _clinicaService.SolicitudCitaInsertar(solicitud);
            return StatusCode(result.Code, result);
        }
    }
    
}
