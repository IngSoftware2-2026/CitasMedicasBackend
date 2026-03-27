using CitasMedicas.DataAccess;
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
        private readonly CitasRepository _citasRepository;
        private readonly PacientesRepository _pacientesRepository;

        
        public ClinicaService(PropuestasReprogramacionRepository propuestasReprogramacionRepository, SolicitudesRepository solicitudesRepository, CitasRepository citasRepository, PacientesRepository pacientesRepository)
        {
            _solicitudesRepository = solicitudesRepository;
            _propuestasReprogramacionRepository = propuestasReprogramacionRepository;
            _citasRepository = citasRepository;
            _pacientesRepository = pacientesRepository;
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
                    return result.Ok(response.MessageStatus!, response);

                case -1:
                case -2:
                case -3:
                case -4:
                case -5:
                case -6:
                    return result.Conflict(response.MessageStatus, response);

                case 0:
                    return result.Error(response.MessageStatus!);

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
                return new ServiceResult().BadRequest("El tel�fono es requerido");

            if (solicitud.MedicoId <= 0)
                return new ServiceResult().BadRequest("Debe seleccionar un m�dico");

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
                return new ServiceResult().BadRequest("Debe seleccionar un m�dico");

            if (solicitud.FechaHoraInicio == default)
                return new ServiceResult().BadRequest("Debe seleccionar una fecha y hora");

            if (solicitud.DuracionMinutos <= 0)
                return new ServiceResult().BadRequest("La duraci�n debe ser mayor a cero");

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

        public ServiceResult SolicitudesPublicasListar(SolicitudesFiltroDTO filtro)
        {
            if (filtro == null)
                return new ServiceResult().BadRequest("Los filtros son requeridos");

            if (filtro.Desde.HasValue && filtro.Hasta.HasValue && filtro.Desde > filtro.Hasta)
                return new ServiceResult().BadRequest("La fecha inicial no puede ser mayor a la fecha final");

            try
            {
                var lista = _solicitudesRepository.SolicitudesPublicasListar(filtro);
                return new ServiceResult().Ok(lista);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al listar solicitudes públicas: {ex.Message}");
            }
        }

        public ServiceResult SolicitudesPublicasObtenerPorId(int solicitudId)
        {
            if (solicitudId <= 0)
                return new ServiceResult().BadRequest("El id de la solicitud es requerido");

            try
            {
                var detalle = _solicitudesRepository.SolicitudesPublicasObtenerPorId(solicitudId);

                if (detalle == null)
                    return new ServiceResult().NotFound("No se encontró la solicitud solicitada");

                return new ServiceResult().Ok(detalle);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al obtener la solicitud: {ex.Message}");
            }
        }

        public ServiceResult SolicitudesPublicasCambiarEstado(CambiarEstadoSolicitudDTO cambio)
        {
            if (cambio == null)
                return new ServiceResult().BadRequest("Los datos para cambiar estado son requeridos");

            if (cambio.SolicitudId <= 0)
                return new ServiceResult().BadRequest("El id de la solicitud es requerido");

            if (string.IsNullOrWhiteSpace(cambio.CodigoEstado))
                return new ServiceResult().BadRequest("El código de estado es requerido");

            try
            {
                var response = _solicitudesRepository.SolicitudesPublicasCambiarEstado(cambio);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al cambiar el estado: {ex.Message}");
            }
        }

        public ServiceResult SolicitudesCitaListar(SolicitudesFiltroDTO filtro)
        {
            if (filtro == null)
                return new ServiceResult().BadRequest("Los filtros son requeridos");

            if (filtro.Desde.HasValue && filtro.Hasta.HasValue && filtro.Desde > filtro.Hasta)
                return new ServiceResult().BadRequest("La fecha inicial no puede ser mayor a la fecha final");

            try
            {
                var lista = _solicitudesRepository.SolicitudesCitaListar(filtro);
                return new ServiceResult().Ok(lista);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al listar solicitudes: {ex.Message}");
            }
        }

        public ServiceResult SolicitudesCitaObtenerPorId(int solicitudId)
        {
            if (solicitudId <= 0)
                return new ServiceResult().BadRequest("El id de la solicitud es requerido");

            try
            {
                var detalle = _solicitudesRepository.SolicitudesCitaObtenerPorId(solicitudId);

                if (detalle == null)
                    return new ServiceResult().NotFound("No se encontró la solicitud solicitada");

                return new ServiceResult().Ok(detalle);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al obtener la solicitud: {ex.Message}");
            }
        }

        public ServiceResult SolicitudesCitaCambiarEstado(CambiarEstadoSolicitudDTO cambio)
        {
            if (cambio == null)
                return new ServiceResult().BadRequest("Los datos para cambiar estado son requeridos");

            if (cambio.SolicitudId <= 0)
                return new ServiceResult().BadRequest("El id de la solicitud es requerido");

            if (string.IsNullOrWhiteSpace(cambio.CodigoEstado))
                return new ServiceResult().BadRequest("El código de estado es requerido");

            try
            {
                var response = _solicitudesRepository.SolicitudesCitaCambiarEstado(cambio);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al cambiar el estado: {ex.Message}");
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



        #region Citas
        public ServiceResult CitaInsertar(CitasInsertarDTO cita)
        {
            if (cita == null)
                return new ServiceResult().BadRequest("Los datos de la cita son requeridos");

            if (cita.PacienteId <= 0)
                return new ServiceResult().BadRequest("El paciente es requerido");

            if (cita.MedicoId <= 0)
                return new ServiceResult().BadRequest("Debe seleccionar un m dico");

            if (cita.SalaId <= 0)
                return new ServiceResult().BadRequest("Debe seleccionar una sala");

            if (cita.Inicio == default)
                return new ServiceResult().BadRequest("La fecha y hora de inicio son requeridas");

            if (cita.Fin == default)
                return new ServiceResult().BadRequest("La fecha y hora de fin son requeridas");

            if (cita.DuracionMinutos <= 0)
                return new ServiceResult().BadRequest("La duraci n debe ser mayor a cero");

            if (cita.Inicio >= cita.Fin)
                return new ServiceResult().BadRequest("La fecha y hora de inicio debe ser menor que la fecha y hora de fin");

            try
            {
                var response = _citasRepository.CitaInsertar(cita);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al crear la cita: {ex.Message}");
            }
        }

        public ServiceResult CitasObtenerPorFiltro(CitasFiltroDTO filtro)
        {
            if (filtro == null)
                return new ServiceResult().BadRequest("Los filtros son requeridos");

            if (filtro.Desde.HasValue && filtro.Hasta.HasValue && filtro.Desde > filtro.Hasta)
                return new ServiceResult().BadRequest("La fecha inicial no puede ser mayor a la fecha final");

            try
            {
                var lista = _citasRepository.CitasObtenerPorFiltro(filtro);
                return new ServiceResult().Ok(lista);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al obtener citas por filtro: {ex.Message}");
            }
        }

        public ServiceResult CitaObtenerPorId(int citaId)
        {
            if (citaId <= 0)
                return new ServiceResult().BadRequest("El id de la cita es requerido");

            try
            {
                var detalle = _citasRepository.CitaObtenerPorId(citaId);

                if (detalle == null)
                    return new ServiceResult().NotFound("No se encontr  la cita solicitada");

                return new ServiceResult().Ok(detalle);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al obtener el detalle de la cita: {ex.Message}");
            }
        }

        public ServiceResult CitaCambiarEstado(CitasCambiarEstadoDTO cambioEstado)
        {
            if (cambioEstado == null)
                return new ServiceResult().BadRequest("Los datos para cambiar estado son requeridos");

            if (cambioEstado.CitaId <= 0)
                return new ServiceResult().BadRequest("El id de la cita es requerido");

            if (string.IsNullOrWhiteSpace(cambioEstado.CodigoEstado))
                return new ServiceResult().BadRequest("El código de estado es requerido");

            try
            {
                var response = _citasRepository.CitaCambiarEstado(cambioEstado);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al cambiar el estado de la cita: {ex.Message}");
            }
        }

        public ServiceResult CitaActualizarSala(CitasActualizarSalaDTO actualizacionSala)
        {
            if (actualizacionSala == null)
                return new ServiceResult().BadRequest("Los datos para actualizar sala son requeridos");

            if (actualizacionSala.CitaId <= 0)
                return new ServiceResult().BadRequest("El id de la cita es requerido");

            if (actualizacionSala.SalaId <= 0)
                return new ServiceResult().BadRequest("El id de la sala es requerido");

            try
            {
                var response = _citasRepository.CitaActualizarSala(actualizacionSala);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al actualizar la sala de la cita: {ex.Message}");
            }
        }
        #endregion


        #region Pacientes
        public ServiceResult ListarPacientes()
        {
            var result = new ServiceResult();
            try
            {
                var response = _pacientesRepository.Listar();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error inesperado al listar pacientes: {ex.Message}");
            }
        }

        public ServiceResult ObtenerPacientePorId(int pacienteId)
        {
            if (pacienteId <= 0)
                return new ServiceResult().BadRequest("El id del paciente debe ser mayor que cero.");

            try
            {
                var paciente = _pacientesRepository.ObtenerPorId(pacienteId);

                if (paciente == null)
                    return new ServiceResult().NotFound("Paciente no encontrado.");

                return new ServiceResult().Ok(paciente);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al obtener paciente: {ex.Message}");
            }
        }

        public ServiceResult ObtenerPacientePorUsuarioId(int usuarioId)
        {
            if (usuarioId <= 0)
                return new ServiceResult().BadRequest("El id del usuario debe ser mayor que cero.");

            try
            {
                var paciente = _pacientesRepository.ObtenerPorUsuarioId(usuarioId);

                if (paciente == null)
                    return new ServiceResult().NotFound("Paciente no encontrado para el usuario autenticado.");

                return new ServiceResult().Ok(paciente);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al obtener paciente por usuario: {ex.Message}");
            }
        }

        public ServiceResult PacientesInsertar(PacientesDTO paciente)
        {
            if (paciente == null)
                return new ServiceResult().BadRequest("Los datos del paciente son requeridos");

            if (!paciente.UsuarioId.HasValue || paciente.UsuarioId <= 0)
                return new ServiceResult().BadRequest("El UsuarioId es requerido y debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(paciente.Nombres))
                return new ServiceResult().BadRequest("El nombre del paciente es requerido");

            if (string.IsNullOrWhiteSpace(paciente.Apellidos))
                return new ServiceResult().BadRequest("Los apellidos del paciente son requeridos");

            if (string.IsNullOrWhiteSpace(paciente.NumeroIdentidad))
                return new ServiceResult().BadRequest("El número de identidad es requerido");

            try
            {
                var response = _pacientesRepository.PacienteInsertar(paciente);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al insertar paciente: {ex.Message}");
            }
        }

        public ServiceResult PacientesEditar(PacientesDTO paciente)
        {
            if (paciente == null)
                return new ServiceResult().BadRequest("Los datos del paciente son requeridos");

            if (paciente.PacienteId <= 0)
                return new ServiceResult().BadRequest("El ID del paciente es requerido");

            if (!paciente.UsuarioId.HasValue || paciente.UsuarioId <= 0)
                return new ServiceResult().BadRequest("El UsuarioId es requerido y debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(paciente.Nombres))
                return new ServiceResult().BadRequest("El nombre del paciente es requerido");

            if (string.IsNullOrWhiteSpace(paciente.Apellidos))
                return new ServiceResult().BadRequest("Los apellidos del paciente son requeridos");

            try
            {
                var response = _pacientesRepository.PacienteEditar(paciente);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al editar paciente: {ex.Message}");
            }
        }

        public ServiceResult PacientesEliminar(int pacienteId)
        {
            if (pacienteId <= 0)
                return new ServiceResult().BadRequest("El ID del paciente es requerido");

            try
            {
                var response = _pacientesRepository.PacienteEliminar(pacienteId);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al eliminar paciente: {ex.Message}");
            }
        }
        #endregion
    }
}
