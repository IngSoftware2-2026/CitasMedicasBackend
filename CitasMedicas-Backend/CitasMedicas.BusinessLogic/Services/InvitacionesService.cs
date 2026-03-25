using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Clinica;
using CitasMedicas.Models.Models;
using System.Security.Cryptography;

namespace CitasMedicas.BusinessLogic.Services
{
    public class InvitacionesService
    {
        private readonly InvitacionesRepository _invitacionesRepository;

        public InvitacionesService(InvitacionesRepository invitacionesRepository)
        {
            _invitacionesRepository = invitacionesRepository
                ?? throw new ArgumentNullException(nameof(invitacionesRepository));
        }

        // Genera token SHA-256 seguro: 32 bytes aleatorios → 64 chars hex (criterio #36: nunca texto plano)
        private static string GenerarHashToken()
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToHexString(bytes).ToLower();
        }

        // Mismo patrón de mapeo que usa ClinicaService.MapRequestStatusToServiceResult
        private ServiceResult MapRequestStatus(RequestStatus response)
        {
            var result = new ServiceResult();

            if (response == null)
                return result.Error("La operación no devolvió resultados.");

            switch (response.CodeStatus)
            {
                case 1:
                    return result.Ok(response.MessageStatus!, response);
                case -1:
                    // 410 Gone — expirada o ya usada (criterio #37)
                    return result.Disabled(response.MessageStatus!);
                case 0:
                    return result.Error(response.MessageStatus!);
                default:
                    return result.Error("Ocurrió un error desconocido.");
            }
        }

        // Solo RECEPCION y ADMIN llaman esto (se controla en el Controller con [Authorize])
        // Las invitaciones pendientes anteriores se invalidan en el SP (criterio #39)
        public ServiceResult GenerarInvitacion(int pacienteId)
        {
            if (pacienteId <= 0)
                return new ServiceResult().BadRequest("El PacienteId es requerido.");

            try
            {
                string token = GenerarHashToken();
                var response = _invitacionesRepository.GenerarInvitacion(pacienteId, token);
                var result = MapRequestStatus(response);

                // Si fue exitoso devolvemos el token para que recepción lo entregue al paciente
                if (result.Success)
                    result.Data = new { Token = token };

                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error("Error al generar invitación: " + ex.Message);
            }
        }

        // Este método lo usa el endpoint PÚBLICO — sin JWT (criterio #40)
        public ServiceResult ValidarInvitacion(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return new ServiceResult().BadRequest("El token es requerido.");

            try
            {
                var invitacion = _invitacionesRepository.ValidarInvitacion(token);

                if (invitacion == null)
                    return new ServiceResult().NotFound("Token inválido.");

                switch (invitacion.CodeStatus)
                {
                    case 1:
                        return new ServiceResult().Ok("Token válido.", invitacion);
                    case -1:
                        // 410 Gone (criterio #37)
                        return new ServiceResult().Disabled("La invitación ya fue usada o ha expirado.");
                    default:
                        return new ServiceResult().NotFound("Token inválido.");
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error("Error al validar invitación: " + ex.Message);
            }
        }

        public ServiceResult UsarInvitacion(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return new ServiceResult().BadRequest("El token es requerido.");

            try
            {
                var response = _invitacionesRepository.UsarInvitacion(token);
                return MapRequestStatus(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error("Error al usar invitación: " + ex.Message);
            }
        }

        public ServiceResult ObtenerInvitacionesPorPaciente(int pacienteId)
        {
            if (pacienteId <= 0)
                return new ServiceResult().BadRequest("El PacienteId es requerido.");

            try
            {
                var lista = _invitacionesRepository.ObtenerPorPaciente(pacienteId);
                return new ServiceResult().Ok(lista);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error("Error al obtener invitaciones: " + ex.Message);
            }
        }
    }
}