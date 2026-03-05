using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Catalogos
{
    [Route("[controller]")]
    [ApiController]
    public class EspecialidadesController : Controller
    {
        private readonly CatalogoService _catalogoService;

        public EspecialidadesController(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
        }

        [HttpGet("Listar")]
        public IActionResult Listar()
        {
            var result = _catalogoService.ListarEspecialidades();
            return StatusCode(result.Code, result);
        }

        [HttpPost("Insertar")]
        public IActionResult Insertar([FromBody] EspecialidadesDTO especialidad)
        {
            var result = _catalogoService.EspecialidadesInsertar(especialidad);
            return StatusCode(result.Code, result);
        }

        [HttpPost("Editar")]
        public IActionResult Editar([FromBody] EspecialidadesDTO especialidad)
        {
            var result = _catalogoService.EspecialidadesEditar(especialidad);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("Eliminar")]
        public IActionResult Eliminar(int especialidadId)
        {
            var result = _catalogoService.EspecialidadesEliminar(especialidadId);
            return StatusCode(result.Code, result);
        }
    }
}
