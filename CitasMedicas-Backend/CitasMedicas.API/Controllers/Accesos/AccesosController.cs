using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Accesos
{
    [Route("[controller]")]
    [ApiController]
    public class AccesosController : Controller
    {
        private readonly AccesoService _accesoService;

        public AccesosController(AccesoService accesoService)
        {
            _accesoService = accesoService ?? throw new ArgumentNullException(nameof(accesoService));
        }

        #region Roles
        [HttpGet("Roles/Listar")]
        public IActionResult ListarRoles()
        {
            var result = _accesoService.ListarRoles();
            return StatusCode(result.Code, result);
        }

        [HttpPost("Roles/Insertar")]
        public IActionResult RolesInsertar([FromBody] RolDTO rol)
        {
            var result = _accesoService.RolesInsertar(rol);
            return StatusCode(result.Code, result);
        }

        [HttpPost("Roles/Editar")]
        public IActionResult RolesEditar([FromBody] RolDTO rol)
        {
            var result = _accesoService.RolesEditar(rol);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("Roles/Eliminar")]
        public IActionResult RolesEliminar(int rolId)
        {
            var result = _accesoService.RolesEliminar(rolId);
            return StatusCode(result.Code, result);
        }
        #endregion

        #region Usuarios
        [HttpGet("Usuarios/Listar")]
        public IActionResult ListarUsuarios()
        {
            var result = _accesoService.ListarUsuarios();
            return StatusCode(result.Code, result);
        }

        [HttpPost("Usuarios/Insertar")]
        public IActionResult UsuariosInsertar([FromBody] UsuariosDTO usuario)
        {
            var result = _accesoService.UsuariosInsertar(usuario);
            return StatusCode(result.Code, result);
        }

        [HttpPost("Usuarios/Editar")]
        public IActionResult UsuariosEditar([FromBody] UsuariosDTO usuario)
        {
            var result = _accesoService.UsuariosEditar(usuario);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("Usuarios/Eliminar")]
        public IActionResult UsuariosEliminar(int usuarioId)
        {
            var result = _accesoService.UsuariosEliminar(usuarioId);
            return StatusCode(result.Code, result);
        }
        #endregion
    }
}
