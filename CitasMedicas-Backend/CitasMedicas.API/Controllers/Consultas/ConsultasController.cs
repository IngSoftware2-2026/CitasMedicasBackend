using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.DataAccess.Repositories.Consultas;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Consultas
{
    [Route("[controller]")]
    [ApiController]
    public class ConsultasController : Controller
    {
        private readonly ConsultasService _consultasService;
        private readonly ConsultasRepository  _consultasRepository;

        public ConsultasController(ConsultasService consultasService, ConsultasRepository consultasRepository)
        {
            _consultasService = consultasService ?? throw new ArgumentNullException(nameof(consultasService));
            _consultasRepository = consultasRepository;
        }
        
        [HttpPost("Insertar-consulta")]
        public IActionResult Insertar([FromBody] ConsultaDto consulta)
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
        
        // [HttpPut("actualizar-consulta-por-cita")]
        // public IActionResult Actualizar(int citaId, [FromBody] ConsultaDto consulta)
        // {
        //     consulta.CitaId = citaId;
        //     var result = ConsultaActualizar(consulta);
        //     return StatusCode(result.Code, result);
        // }
        
        [HttpGet("obtener-consulta")]
        public IActionResult ObtenerHistorial(int pacienteId)
        {
            var result = _consultasService.HistorialObtenerPorPaciente(pacienteId);
            return StatusCode(result.Code, result);
        }
        
        [HttpGet("obtener-todas-las-consultas")]
        public IActionResult ObtenerTodasLasConsultas()
        {
            var result = _consultasService.ObtenerTodasLasConsultas();
            return StatusCode(result.Code, result);
        }
        
    }
    
}