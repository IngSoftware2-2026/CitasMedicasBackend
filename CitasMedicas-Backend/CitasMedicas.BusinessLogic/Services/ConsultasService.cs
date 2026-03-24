using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Clinica;
using CitasMedicas.DataAccess.Repositories.Consultas;
using CitasMedicas.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CitasMedicas.BusinessLogic.Services
{
    public class ConsultasService
    {
        private readonly ConsultasRepository _consultasRepository;
        private readonly CitasRepository _citasRepository;

        public ConsultasService(ConsultasRepository consultasRepository, CitasRepository citasRepository)
        {
            _consultasRepository = consultasRepository;
            _citasRepository = citasRepository;
        }

        #region Método genérico de mapeo (Manteniendo tu estructura)
        private ServiceResult MapRequestStatusToServiceResult(RequestStatus response)
        {
            var result = new ServiceResult();

            if (response == null)
                return result.Error("La operación no devolvió resultados.");

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
                    return result.Error("Ocurrió un error desconocido.");
            }
        }
        #endregion

        #region Consultas Clínicas
        
        // public ServiceResult ConsultaInsertar(int citaId, int medicoIdLogueado, ConsultaDTO consulta)
        public ServiceResult ConsultaInsertar(ConsultaDto consulta)
        {
            // if (citaId <= 0)
            //     return new ServiceResult().BadRequest("El id de la cita es requerido");

            // if (consulta == null)
            //     return new ServiceResult().BadRequest("Los datos de la consulta son requeridos");

            try
            {
                // var cita = _citasRepository.CitaObtenerPorId(citaId);
                // if (cita == null)
                //     return new ServiceResult().NotFound("La cita especificada no existe.");

                // if (cita.MedicoId != medicoIdLogueado)
                //     return new ServiceResult().Conflict("No está autorizado para registrar la consulta de este médico.");

                var response = _consultasRepository.ConsultaInsertar(consulta);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al registrar la consulta: {ex.Message}");
            }
        }

        // public ServiceResult ConsultaActualizar(int citaId, int medicoIdLogueado, ConsultaDTO consulta)
        public ServiceResult ConsultaActualizar(ConsultaDto consulta)
        {
            // if (citaId <= 0)
            //     return new ServiceResult().BadRequest("El id de la cita es requerido");
        
            try
            {
                // var cita = _citasRepository.CitaObtenerPorId(citaId);
                // if (cita == null || cita.MedicoId != medicoIdLogueado)
                //     return new ServiceResult().Conflict("No autorizado para editar esta consulta.");
        
                var response = _consultasRepository.ConsultaActualizar(consulta);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al actualizar la consulta: {ex.Message}");
            }
        }
        
        public ServiceResult ConsultaObtenerPorCita(int citaId)
        {
            if (citaId <= 0)
                return new ServiceResult().BadRequest("El id de la cita es requerido");
        
            try
            {
                var consulta = _consultasRepository.ObtenerPorCita(citaId);
                if (consulta == null)
                    return new ServiceResult().NotFound("No se encontró registro clínico para esta cita.");
        
                return new ServiceResult().Ok(consulta);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al obtener la consulta: {ex.Message}");
            }
        }
        
        public ServiceResult HistorialObtenerPorPaciente(int pacienteId)
        {
            if (pacienteId <= 0)
                return new ServiceResult().BadRequest("El id del paciente es requerido");
        
            try
            {
                var historial = _consultasRepository.ObtenerHistorialPaciente(pacienteId);
                return new ServiceResult().Ok(historial);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al obtener el historial clínico: {ex.Message}");
            }
        }
        
        public ServiceResult ObtenerTodasLasConsultas()
        {
            try
            {
                var consultas = _consultasRepository.GetAllConsultasAsync();
                return new ServiceResult().Ok(consultas);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al recuperar el listado de consultas: {ex.Message}");
            }
        }

        #endregion
    }
}