using CitasMedicas.Models.Models;
using CitasMedicas.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System;
namespace CitasMedicas.API.Controllers
{
    [Route("api/doctores")] 
    [ApiController]
    public class HorariosDoctorController : ControllerBase
    {
        // Usamos la clase normal sin la "I"
        private readonly ClinicaService _clinicaService;

        // Se lo pedimos al servidor en el constructor
        public HorariosDoctorController(ClinicaService clinicaService)
        {
            _clinicaService = clinicaService;
        }

        [HttpGet("{doctorId}/horarios")]
        // ... (todo lo demás hacia abajo se queda exactamente igual)
        public IActionResult ObtenerHorarios(int doctorId)
        {
            try
            {
                var horarios = _clinicaService.ObtenerHorarios(doctorId);
                return Ok(horarios); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost("horarios")]
        public IActionResult CrearHorario([FromBody] HorarioDoctorDTO horario)
        {
            if (horario == null)
                return BadRequest("Los datos del horario no pueden estar vacíos.");

            var resultado = _clinicaService.CrearHorario(horario);

            if (resultado.CodeStatus == 1)
                return Ok(resultado); 
            else
                return BadRequest(resultado); 
        }

        [HttpPut("horarios")]
        public IActionResult ActualizarHorario([FromBody] HorarioDoctorDTO horario)
        {
            if (horario == null || horario.HorarioId == 0)
                return BadRequest("Datos inválidos para actualizar el horario.");

            var resultado = _clinicaService.ActualizarHorario(horario);

            if (resultado.CodeStatus == 1)
                return Ok(resultado);
                
            return BadRequest(resultado);
        }

        [HttpDelete("horarios/{horarioId}")]
        public IActionResult EliminarHorario(int horarioId)
        {
            var resultado = _clinicaService.EliminarHorario(horarioId);

            if (resultado.CodeStatus == 1)
                return Ok(resultado);
                
            return BadRequest(resultado);
        }
    }
}