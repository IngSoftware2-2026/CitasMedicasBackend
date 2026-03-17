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
        private readonly PacientesRepository _pacientesRepository;

        public ClinicaService(PacientesRepository pacientesRepository)
        {
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
                    return result.Ok(response.MessageStatus, response);

                case -1:
                    return result.Conflict(response.MessageStatus, response);

                case -2:
                    return result.Conflict(response.MessageStatus, response);

                case 0:
                    return result.Error(response.MessageStatus);

                default:
                    return result.Error("Ocurrió un error desconocido.");
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

        public ServiceResult PacientesInsertar(PacientesDTO paciente)
        {
            if (paciente == null)
                return new ServiceResult().BadRequest("Los datos del paciente son requeridos");

            if (paciente.UsuarioId <= 0)
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

            if (paciente.UsuarioId <= 0)
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
