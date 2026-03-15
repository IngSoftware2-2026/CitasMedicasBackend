using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Clinica
{
    [Route("[controller]")]
    [ApiController]
    public class PacientesController : Controller
    {
        private readonly ClinicaService _clinicaService;

        public PacientesController(ClinicaService clinicaService)
        {
            _clinicaService = clinicaService ?? throw new ArgumentNullException(nameof(clinicaService));
        }

        [HttpGet("Listar")]
        public IActionResult Listar()
        {
            var result = _clinicaService.ListarPacientes();
            return StatusCode(result.Code, result);
        }

        [HttpGet("ObtenerPorId")]
        public IActionResult ObtenerPorId(int pacienteId)
        {
            var result = _clinicaService.ObtenerPacientePorId(pacienteId);
            return StatusCode(result.Code, result);
        }

        [HttpPost("Insertar")]
        public IActionResult Insertar([FromBody] PacientesDTO paciente)
        {
            var result = _clinicaService.PacientesInsertar(paciente);
            return StatusCode(result.Code, result);
        }

        [HttpPost("Editar")]
        public IActionResult Editar([FromBody] PacientesDTO paciente)
        {
            var result = _clinicaService.PacientesEditar(paciente);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("Eliminar")]
        public IActionResult Eliminar(int pacienteId)
        {
            var result = _clinicaService.PacientesEliminar(pacienteId);
            return StatusCode(result.Code, result);
        }
    }
}
