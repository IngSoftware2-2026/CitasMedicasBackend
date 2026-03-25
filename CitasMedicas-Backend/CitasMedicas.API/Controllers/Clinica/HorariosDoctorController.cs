using CitasMedicas.Models.Models;
using CitasMedicas.DataAccess.Repositories.Clinica;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CitasMedicas.API.Controllers
{
    // Esta es la ruta principal en internet para llegar a tus funciones
    [Route("api/doctores")] 
    [ApiController]
    public class HorariosDoctorController : ControllerBase
    {
        private readonly HorariosDoctorRepository _horariosRepository;

        // Constructor: Aquí inicializamos tu Repositorio
        public HorariosDoctorController()
        {
            _horariosRepository = new HorariosDoctorRepository();
        }

        // 1. GET: api/doctores/5/horarios
        [HttpGet("{doctorId}/horarios")]
        public IActionResult ObtenerHorarios(int doctorId)
        {
            try
            {
                var horarios = _horariosRepository.ObtenerHorarios(doctorId);
                return Ok(horarios); // Devuelve un código HTTP 200 OK con la lista de horarios
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // 2. POST: api/doctores/horarios
        [HttpPost("horarios")]
        public IActionResult CrearHorario([FromBody] HorarioDoctorDTO horario)
        {
            if (horario == null)
            {
                return BadRequest("Los datos del horario no pueden estar vacíos.");
            }

            var resultado = _horariosRepository.CrearHorario(horario);

            // Verificamos el CodeStatus que viene desde tu SQL (1 = Éxito, 0 = Error)
            if (resultado.CodeStatus == 1)
                return Ok(resultado); 
            else
                return BadRequest(resultado); // Falló alguna validación (ej. chocan las horas)
        }

        // 3. PUT: api/doctores/horarios
        [HttpPut("horarios")]
        public IActionResult ActualizarHorario([FromBody] HorarioDoctorDTO horario)
        {
            if (horario == null || horario.HorarioId == 0)
            {
                return BadRequest("Datos inválidos para actualizar el horario.");
            }

            var resultado = _horariosRepository.ActualizarHorario(horario);

            if (resultado.CodeStatus == 1)
                return Ok(resultado);
                
            return BadRequest(resultado);
        }

        // 4. DELETE: api/doctores/horarios/10
        [HttpDelete("horarios/{horarioId}")]
        public IActionResult EliminarHorario(int horarioId)
        {
            var resultado = _horariosRepository.EliminarHorario(horarioId);

            if (resultado.CodeStatus == 1)
                return Ok(resultado);
                
            return BadRequest(resultado);
        }
    }
}