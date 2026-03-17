using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Accesos
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AccesoService _accesoService;

        public AuthController(AccesoService accesoService)
        {
            _accesoService = accesoService ?? throw new ArgumentNullException(nameof(accesoService));
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            var result = _accesoService.Login(login);
            return StatusCode(result.Code, result);
        }
    }
}
