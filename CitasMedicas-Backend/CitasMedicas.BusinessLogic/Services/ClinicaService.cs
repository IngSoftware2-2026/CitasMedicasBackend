using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Catalogos;
using CitasMedicas.DataAccess.Repositories.Clinica;
using CitasMedicas.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.BusinessLogic.Services
{
    public class ClinicaService
    {
        private readonly SolicitudesRepository _solicitudesRepository;
        private readonly PropuestasReprogramacionRepository _propuestasReprogramacionRepository;


        
        public ClinicaService(PropuestasReprogramacionRepository propuestasReprogramacionRepository, SolicitudesRepository solicitudesRepository)
        {
            _solicitudesRepository = solicitudesRepository;
            _propuestasReprogramacionRepository = propuestasReprogramacionRepository;

        }

        #region Método genérico de mapeo
        private ServiceResult MapRequestStatusToServiceResult(RequestStatus response)
        {
            var result = new ServiceResult();

            if (response == null)
                return result.Error("La operación no devolvió resultados.");

            switch (response.CodeStatus)
            {
                case 1:
                    return result.Ok(response.MessageStatus, response);

                case -2:
                case -3:
                case -4:
                case -5:
                case -6:
                    return result.Conflict(response.MessageStatus, response);

                case 0:
                    return result.Error(response.MessageStatus);

                default:
                    return result.Error("Ocurrió un error desconocido.");
            }
        }
        #endregion

        #region Solicitudes
        public ServiceResult SolicitudPublicaInsertar(SolicitudesPublicasDTO solicitud)
        {
            if (solicitud == null)
                return new ServiceResult().BadRequest("Los datos de la solicitud son requeridos");

            if (string.IsNullOrWhiteSpace(solicitud.NombrePaciente))
                return new ServiceResult().BadRequest("El nombre del paciente es requerido");

            if (string.IsNullOrWhiteSpace(solicitud.Telefono))
                return new ServiceResult().BadRequest("El teléfono es requerido");

            if (solicitud.MedicoId <= 0)
                return new ServiceResult().BadRequest("Debe seleccionar un médico");

            if (solicitud.FechaHoraInicio == default)
                return new ServiceResult().BadRequest("Debe seleccionar una fecha y hora");

            try
            {
                var response = _solicitudesRepository.SolicitudPublicaInsertar(solicitud);

                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al enviar la solicitud: {ex.Message}");
            }
        }


        public ServiceResult SolicitudCitaInsertar(SolicitudesDTO solicitud)
        {
            if (solicitud == null)
                return new ServiceResult().BadRequest("Los datos de la solicitud son requeridos");

            if (solicitud.PacienteId <= 0)
                return new ServiceResult().BadRequest("El paciente es requerido");

            if (solicitud.MedicoId <= 0)
                return new ServiceResult().BadRequest("Debe seleccionar un médico");

            if (solicitud.FechaHoraInicio == default)
                return new ServiceResult().BadRequest("Debe seleccionar una fecha y hora");

            if (solicitud.DuracionMinutos <= 0)
                return new ServiceResult().BadRequest("La duración debe ser mayor a cero");

            try
            {
                var response = _solicitudesRepository.SolicitudCitaInsertar(solicitud);

                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al crear la solicitud: {ex.Message}");
            }
        }
        #endregion

        #region Propuestas reprogramacion
        public ServiceResult CrearPropuestaReprogramacion(PropuestasReprogramacionDTO propuesta)
        {
            if (propuesta == null)
                return new ServiceResult().BadRequest("Los datos de la propuesta son requeridos");

            if (propuesta.SolicitudCitaId == null && propuesta.SolicitudPublicaId == null)
                return new ServiceResult().BadRequest("Debe enviarse una solicitud válida");

            if (propuesta.SolicitudCitaId != null && propuesta.SolicitudPublicaId != null)
                return new ServiceResult().BadRequest("Solo puede enviarse un tipo de solicitud");

            if (propuesta.OpcionInicio <= DateTime.Now)
                return new ServiceResult().BadRequest("No se puede proponer un horario en el pasado");

            if (propuesta.UsuarioProponeId <= 0)
                return new ServiceResult().BadRequest("El usuario que propone es requerido");

            try
            {
                var response = _propuestasReprogramacionRepository.CrearPropuesta(propuesta);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al crear la propuesta: {ex.Message}");
            }
        }

        public ServiceResult AceptarPropuestaReprogramacion(AceptarPropuestaReprogramacionDTO propuesta)
        {
            if (propuesta == null)
                return new ServiceResult().BadRequest("Los datos de la propuesta son requeridos");

            if (propuesta.PropuestaId <= 0)
                return new ServiceResult().BadRequest("El id de la propuesta es requerido");

            if (propuesta.UsuarioId <= 0)
                return new ServiceResult().BadRequest("El id del usuario es requerido");

            try
            {
                var response = _propuestasReprogramacionRepository.AceptarPropuesta(propuesta);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al aceptar la propuesta: {ex.Message}");
            }
        }

        public ServiceResult RechazarPropuestaReprogramacion(int propuestaId)
        {
            if (propuestaId <= 0)
                return new ServiceResult().BadRequest("El id de la propuesta es requerido");

            try
            {
                var response = _propuestasReprogramacionRepository.RechazarPropuesta(propuestaId);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al rechazar la propuesta: {ex.Message}");
            }
        }
        #endregion
    }
}
