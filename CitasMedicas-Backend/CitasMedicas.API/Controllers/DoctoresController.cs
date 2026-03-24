using CitasMedicas.BusinessLogic;
using CitasMedicas.DataAccess.Repositories.Clinica;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers
{
    [ApiController]
    [Route("api/doctores")]
    public class DoctoresController : ControllerBase
    {
        private readonly DoctoresRepository _repo = new DoctoresRepository();

        [HttpGet]
        public IActionResult ObtenerDoctores(bool? activo, int? especialidadId)
        {
            var data = _repo.Listar(activo, especialidadId);
            return Ok(data);
        }

        [HttpGet("~/Doctores/Listar")]
        public IActionResult Listar(bool? activo, int? especialidadId)
        {
            var result = new ServiceResult().Ok(_repo.Listar(activo, especialidadId));
            return StatusCode(result.Code, result);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerDoctorPorId(int id)
        {
            var data = _repo.ObtenerPorId(id);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPut("{id}/activo")]
        public IActionResult CambiarActivo(int id, bool activo)
        {
            _repo.CambiarActivo(id, activo);
            return Ok();
        }

        [HttpPost]
        public IActionResult CrearDoctor([FromBody] DoctoresDTO doctor)
        {
            _repo.Crear(doctor);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditarDoctor(int id, [FromBody] DoctoresDTO doctor)
        {
            _repo.Editar(id, doctor);
            return Ok();
        }

        [HttpPost("{id}/especialidades")]
        public IActionResult AsignarEspecialidad(int id, [FromQuery] int especialidadId)
        {
            _repo.AsignarEspecialidad(id, especialidadId);
            return Ok();
        }

        [HttpDelete("{id}/especialidades/{especialidadId}")]
        public IActionResult RemoverEspecialidad(int id, int especialidadId)
        {
            _repo.RemoverEspecialidad(id, especialidadId);
            return Ok();
        }

        [HttpPut("{id}/especialidades/{especialidadId}/principal")]
        public IActionResult SetEspecialidadPrincipal(int id, int especialidadId)
        {
            _repo.SetEspecialidadPrincipal(id, especialidadId);
            return Ok();
        }
    }
}
