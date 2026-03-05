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

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Insertar")]
        public IActionResult Insertar([FromBody] EspecialidadesDTO especialidad)
        {
            try
            {
                var result = _catalogoService.EspecialidadesInsertar(especialidad);

                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Internal server error",
                    Details = ex.Message
                });
            }
        }


        [HttpPost("Editar")]
        public IActionResult Editar([FromBody] EspecialidadesDTO especialidad)
        {
            try
            {
                var result = _catalogoService.EspecialidadesEditar(especialidad);

                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Internal server error",
                    Details = ex.Message
                });
            }
        }


        [HttpDelete("Eliminar")]
        public IActionResult Eliminar(int especialidadId)
        {
            var result = _catalogoService.EspecialidadesEliminar(especialidadId);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
