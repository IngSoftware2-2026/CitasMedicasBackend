using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Clinica
{
    [Route("[controller]")]
    [ApiController]
    public class PropuestasReprogramacionController : Controller
    {
        private readonly ClinicaService _clinicaService;

        public PropuestasReprogramacionController(ClinicaService clinicaService)
        {
            _clinicaService = clinicaService ?? throw new ArgumentNullException(nameof(clinicaService));
        }

        [HttpPost("Crear")]
        public IActionResult Crear([FromBody] PropuestasReprogramacionDTO propuesta)
        {
            var result = _clinicaService.CrearPropuestaReprogramacion(propuesta);
            return StatusCode(result.Code, result);
        }

        [HttpPost("Aceptar")]
        public IActionResult Aceptar([FromBody] AceptarPropuestaReprogramacionDTO propuesta)
        {
            var result = _clinicaService.AceptarPropuestaReprogramacion(propuesta);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("Rechazar")]
        public IActionResult Rechazar(int propuestaId)
        {
            var result = _clinicaService.RechazarPropuestaReprogramacion(propuestaId);
            return StatusCode(result.Code, result);
        }
    }
}
