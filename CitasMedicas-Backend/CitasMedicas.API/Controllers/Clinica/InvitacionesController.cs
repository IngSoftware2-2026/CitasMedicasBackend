using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers.Clinica
{
    [Route("api/invitaciones")]
    [ApiController]
    [Tags("Invitaciones")]
    public class InvitacionesController : ControllerBase
    {
        private readonly InvitacionesService _invitacionesService;

        public InvitacionesController(InvitacionesService invitacionesService)
        {
            _invitacionesService = invitacionesService
                ?? throw new ArgumentNullException(nameof(invitacionesService));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,RECEP")]
        public IActionResult GenerarInvitacion([FromBody] GenerarInvitacionRequest request)
        {
            var result = _invitacionesService.GenerarInvitacion(request.PacienteId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("validar")]
        [AllowAnonymous]
        public IActionResult ValidarInvitacion([FromQuery] string token)
        {
            var result = _invitacionesService.ValidarInvitacion(token);
            return StatusCode(result.Code, result);
        }

        [HttpPost("usar")]
        [AllowAnonymous]
        public IActionResult UsarInvitacion([FromBody] UsarInvitacionRequest request)
        {
            var result = _invitacionesService.UsarInvitacion(request.Token!);
            return StatusCode(result.Code, result);
        }

        [HttpGet("/api/pacientes/{pacienteId}/invitaciones")]
        [Authorize(Roles = "ADMIN,RECEP")]
        [Tags("Invitaciones")]
        public IActionResult ObtenerInvitacionesPorPaciente(int pacienteId)
        {
            var result = _invitacionesService.ObtenerInvitacionesPorPaciente(pacienteId);
            return StatusCode(result.Code, result);
        }
    }
}