using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult ObtenerPorFiltro([FromBody] CitasFiltroDTO filtro)
        {
            var result = _clinicaService.CitasObtenerPorFiltro(filtro);
            return StatusCode(result.Code, result);
        }

        [HttpGet("ObtenerPorId")]
        public IActionResult ObtenerPorId(int citaId)
        {
            var result = _clinicaService.CitaObtenerPorId(citaId);
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
    }
}
