using CitasMedicas.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Catalogos
{
    [Route("[controller]")]
    [ApiController]
    public class PacientesController : Controller
    {
        private readonly CatalogoService _catalogoService;

        public PacientesController(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService ?? throw new ArgumentNullException(nameof(catalogoService));
        }

        [HttpGet("Listar")]
        public IActionResult Listar()
        {
            var result = _catalogoService.ListarPacientes();
            return StatusCode(result.Code, result);
        }
    }
}
