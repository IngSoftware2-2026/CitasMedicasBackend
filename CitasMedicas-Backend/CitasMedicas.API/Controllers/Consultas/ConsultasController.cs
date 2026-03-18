using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Consultas
{
    [Route("[controller]")]
    [ApiController]
    public class ConsultasController : Controller
    {
        private readonly ConsultasService _consultasService;

        public ConsultasController(ConsultasService consultasService)
        {
            _consultasService = consultasService ?? throw new ArgumentNullException(nameof(consultasService));
        }
        
        [HttpPost("Insertar-consulta")]
        public IActionResult Insertar([FromBody] ConsultaDTO consulta)
        {
            var result = _consultasService.ConsultaInsertar(consulta);
            return StatusCode(result.Code, result);
        }
        
        [HttpGet("obtener-consulta-por-cita")]
        public IActionResult ObtenerPorCita(int citaId)
        {
            var result = _consultasService.ConsultaObtenerPorCita(citaId);
            return StatusCode(result.Code, result);
        }
        
        [HttpPut("actualizar-consulta-por-cita")]
        public IActionResult Actualizar(int citaId, [FromBody] ConsultaDTO consulta)
        {
            consulta.CitaId = citaId;
            var result = _consultasService.ConsultaActualizar(consulta);
            return StatusCode(result.Code, result);
        }
        
        [HttpGet("obtener-consulta")]
        public IActionResult ObtenerHistorial(int pacienteId)
        {
            var result = _consultasService.ObtenerHistorialPaciente(pacienteId);
            return StatusCode(result.Code, result);
        }
    }
}