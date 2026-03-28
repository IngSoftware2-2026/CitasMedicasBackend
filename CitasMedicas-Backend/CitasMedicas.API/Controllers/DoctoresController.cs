using CitasMedicas.BusinessLogic;
using CitasMedicas.DataAccess.Repositories.Accesos;
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
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly HorariosDoctorRepository _horariosRepo = new HorariosDoctorRepository();

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

        [HttpGet("~/Doctores/ListarOperativos")]
        public IActionResult ListarOperativos()
        {
            var doctores = _repo.Listar(true, null).ToList();
            var duplicados = doctores
                .GroupBy(d => d.UsuarioId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToHashSet();

            var operativos = doctores.Where(d =>
            {
                var usuario = _userRepo.ObtenerPorId(d.UsuarioId);
                if (usuario == null || usuario.Activo != true || (usuario.RolId ?? 0) != 2)
                    return false;

                if (duplicados.Contains(d.UsuarioId))
                    return false;

                if (string.IsNullOrWhiteSpace(d.NombreEspecialidad))
                    return false;

                var horarios = _horariosRepo.ObtenerHorarios(d.MedicoId);
                return horarios.Any(h => h.Activo);
            }).ToList();

            var result = new ServiceResult().Ok(operativos);
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

        [HttpDelete("{id}")]
        public IActionResult EliminarDoctor(int id)
        {
            _repo.CambiarActivo(id, false);
            return Ok(new { message = "Doctor desactivado correctamente." });
        }

        [HttpPost]
        public IActionResult CrearDoctor([FromBody] DoctoresDTO doctor)
        {
            if (doctor.UsuarioId <= 0)
                return BadRequest(new { message = "Debe seleccionar un usuario valido para vincular al doctor." });

            var usuario = _userRepo.ObtenerPorId(doctor.UsuarioId);
            if (usuario == null)
                return BadRequest(new { message = "El usuario seleccionado no existe." });

            if ((usuario.RolId ?? 0) != 2)
                return BadRequest(new { message = "El usuario seleccionado no tiene rol DOCTOR." });

            var doctorExistente = _repo.Listar(null, null).FirstOrDefault(d => d.UsuarioId == doctor.UsuarioId);
            if (doctorExistente != null)
                return Conflict(new { message = "El usuario seleccionado ya esta vinculado a otro doctor." });

            var newId = _repo.Crear(doctor);
            return Ok(new { medicoId = newId });
        }

        [HttpPut("{id}")]
        public IActionResult EditarDoctor(int id, [FromBody] DoctoresDTO doctor)
        {
            _repo.Editar(id, doctor);
            return Ok();
        }

        [HttpPatch("{id}/imagen")]
        public IActionResult ActualizarImagen(int id, [FromBody] ActualizarImagenDTO dto)
        {
            var doctor = _repo.ObtenerPorId(id);
            if (doctor == null)
                return NotFound(new { message = "Doctor no encontrado" });

            doctor.Imagen = dto.Imagen;
            _repo.Editar(id, doctor);
            return Ok(new { message = "Imagen actualizada correctamente", imagen = dto.Imagen });
        }

        /// <summary>
        /// Lista las especialidades asignadas a un doctor.
        /// </summary>
        [HttpGet("{id}/especialidades")]
        public IActionResult ListarEspecialidades(int id)
        {
            var data = _repo.ListarEspecialidades(id);
            return Ok(data);
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

        /// <summary>
        /// Lista las salas activas.
        /// Se ubica en este controller auxiliarmente para el formulario de doctores.
        /// </summary>
        [HttpGet("salas")]
        public IActionResult ListarSalas()
        {
            var data = _repo.ListarSalas();
            return Ok(data);
        }
    }
}
