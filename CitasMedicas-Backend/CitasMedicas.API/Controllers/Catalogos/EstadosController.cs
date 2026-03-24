using CitasMedicas.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Catalogos
{
    [Route("[controller]")]
    [ApiController]
    public class EstadosController : Controller
    {
        private readonly CatalogoService _catalogoService;

        public EstadosController(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        [HttpGet("EstadosCita")]
        public IActionResult EstadosCita()
        {
            var result = _catalogoService.ListarEstadosCita();
            return StatusCode(result.Code, result);
        }

        [HttpGet("EstadosSolicitud")]
        public IActionResult EstadosSolicitud()
        {
            var result = _catalogoService.ListarEstadosSolicitud();
            return StatusCode(result.Code, result);
        }
    }
}