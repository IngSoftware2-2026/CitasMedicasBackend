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
        private readonly CitasRepository _citasRepository;

        public ClinicaService(SolicitudesRepository solicitudesRepository, CitasRepository citasRepository)
        {
            _solicitudesRepository = solicitudesRepository;
            _citasRepository = citasRepository;
        }

        #region M�todo gen�rico de mapeo
        private ServiceResult MapRequestStatusToServiceResult(RequestStatus response)
        {
            var result = new ServiceResult();

            if (response == null)
                return result.Error("La operaci�n no devolvi� resultados.");

            switch (response.CodeStatus)
            {
                case 1:
                    return result.Ok(response.MessageStatus!, response);

                case -2:
                case -3:
                case -4:
                    return result.Conflict(response.MessageStatus!, response);

                case 0:
                    return result.Error(response.MessageStatus!);

                default:
                    return result.Error("Ocurri� un error desconocido.");
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
        #endregion

        #region Citas
        public ServiceResult CitaInsertar(CitasInsertarDTO cita)
        {
            if (cita == null)
                return new ServiceResult().BadRequest("Los datos de la cita son requeridos");

            if (cita.PacienteId <= 0)
                return new ServiceResult().BadRequest("El paciente es requerido");

            if (cita.MedicoId <= 0)
                return new ServiceResult().BadRequest("Debe seleccionar un m�dico");

            if (cita.SalaId <= 0)
                return new ServiceResult().BadRequest("Debe seleccionar una sala");

            if (cita.Inicio == default)
                return new ServiceResult().BadRequest("La fecha y hora de inicio son requeridas");

            if (cita.Fin == default)
                return new ServiceResult().BadRequest("La fecha y hora de fin son requeridas");

            if (cita.DuracionMinutos <= 0)
                return new ServiceResult().BadRequest("La duraci�n debe ser mayor a cero");

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
                    return new ServiceResult().NotFound("No se encontr� la cita solicitada");

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
                return new ServiceResult().BadRequest("El c�digo de estado es requerido");

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
    }
}
