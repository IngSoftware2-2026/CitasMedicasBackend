using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Catalogos
{
    [Route("[controller]")]
    [ApiController]
    public class SalasController : Controller
    {
        private readonly CatalogoService _catalogoService;

        public SalasController(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
        }

        [HttpGet("Listar")]
        public IActionResult Listar()
        {
            var result = _catalogoService.ListarSalas();
            return StatusCode(result.Code, result);
        }

        [HttpPost("Insertar")]
        public IActionResult Insertar([FromBody] SalasDTO sala)
        {
            var result = _catalogoService.SalaInsertar(sala);
            return StatusCode(result.Code, result);
        }

        [HttpPost("Editar")]
        public IActionResult Editar([FromBody] SalasDTO sala)
        {
            var result = _catalogoService.SalaEditar(sala);
            return StatusCode(result.Code, result);
        }

        [HttpPost("CambiarEstado")]
        public IActionResult CambiarEstado(int salaId)
        {
            var result = _catalogoService.SalaCambiarEstado(salaId);
            return StatusCode(result.Code, result);
        }
    }
}