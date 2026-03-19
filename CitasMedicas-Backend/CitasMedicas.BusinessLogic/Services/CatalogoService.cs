using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Catalogos;
using CitasMedicas.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.BusinessLogic.Services
{
    public class CatalogoService
    {
        private readonly EspecialidadesRepository _especialidadesRepository;
        private readonly SalasRepository _salasRepository;
        private readonly PacientesRepository _pacientesRepository;
        private readonly DoctoresRepository _doctoresRepository;
        private readonly EstadosRepository _estadosRepository;

       public CatalogoService(
            EspecialidadesRepository especialidadesRepository,
            SalasRepository salasRepository,
            PacientesRepository pacientesRepository,
            DoctoresRepository doctoresRepository,
            EstadosRepository estadosRepository)
        {
            _especialidadesRepository = especialidadesRepository;
            _salasRepository = salasRepository;
            _pacientesRepository = pacientesRepository;
            _doctoresRepository = doctoresRepository;
            _estadosRepository = estadosRepository;
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
                    return result.Conflict(response.MessageStatus!, response);

                case -2:
                    return result.Conflict(response.MessageStatus!, response);

                case 0:
                    return result.Error(response.MessageStatus!);

                default:
                    return result.Error("Ocurrió un error desconocido.");
            }
        }
        #endregion

        #region Especialidades
        public ServiceResult ListarEspecialidades()
        {
            var result = new ServiceResult();
            try
            {
                var response = _especialidadesRepository.Listar();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error inesperado al listar especialidades: {ex.Message}");
            }
        }

        public ServiceResult EspecialidadesInsertar(EspecialidadesDTO especialidad)
        {
            if (especialidad == null)
                return new ServiceResult().BadRequest("Los datos de la especialidad son requeridos");

            if (string.IsNullOrWhiteSpace(especialidad.Nombre))
                return new ServiceResult().BadRequest("Nombre de la especialidad es requerido");

            try
            {
                var response = _especialidadesRepository.EspecialidadInsertar(especialidad);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la inserción: {ex.Message}");
            }
        }

        public ServiceResult EspecialidadesEditar(EspecialidadesDTO especialidad)
        {
            if (especialidad == null)
                return new ServiceResult().BadRequest("Los datos de la especialidad son requeridos");

            if (especialidad.EspecialidadId <= 0)
                return new ServiceResult().BadRequest("El id de la especialidad es requerido");

            if (string.IsNullOrWhiteSpace(especialidad.Nombre))
                return new ServiceResult().BadRequest("Nombre de la especialidad es requerido");

            try
            {
                var response = _especialidadesRepository.EspecialidadEditar(especialidad);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la edición: {ex.Message}");
            }
        }

        public ServiceResult EspecialidadesEliminar(int especialidadId)
        {
            if (especialidadId <= 0)
                return new ServiceResult().BadRequest("El id de la especialidad es requerido");

            try
            {
                var response = _especialidadesRepository.EspecialidadEliminar(especialidadId);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la eliminación: {ex.Message}");
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

        #endregion

        #region Doctores

        public ServiceResult ListarDoctores()
        {
            var result = new ServiceResult();

            try
            {
                var response = _doctoresRepository.Listar();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error inesperado al listar doctores: {ex.Message}");
            }
        }

        #endregion

        #region Salas

        public ServiceResult ListarSalas()
        {
            var result = new ServiceResult();

            if (_salasRepository == null)
                return result.Error("Repositorio de salas no disponible");

            try
            {
                var response = _salasRepository.Listar();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error inesperado al listar salas: {ex.Message}");
            }
        }

        public ServiceResult SalaInsertar(SalasDTO sala)
        {
            if (sala == null)
                return new ServiceResult().BadRequest("Los datos de la sala son requeridos");

            if (string.IsNullOrWhiteSpace(sala.NombreSala))
                return new ServiceResult().BadRequest("El nombre de la sala es requerido");

            if (string.IsNullOrWhiteSpace(sala.CodigoSala))
                return new ServiceResult().BadRequest("El código de la sala es requerido");

            try
            {
                var response = _salasRepository!.Crear(sala);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la inserción: {ex.Message}");
            }
        }

        public ServiceResult SalaEditar(SalasDTO sala)
        {
            if (sala == null)
                return new ServiceResult().BadRequest("Los datos de la sala son requeridos");

            if (sala.SalaId <= 0)
                return new ServiceResult().BadRequest("El id de la sala es requerido");

            try
            {
                var response = _salasRepository!.Editar(sala);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la edición: {ex.Message}");
            }
        }

        public ServiceResult SalaCambiarEstado(int salaId)
        {
            if (salaId <= 0)
                return new ServiceResult().BadRequest("El id de la sala es requerido");

            try
            {
                var response = _salasRepository!.CambiarEstado(salaId);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al cambiar estado: {ex.Message}");
            }
        }

        #endregion

        #region Estados
        public ServiceResult ListarEstadosCita()
        {
            var result = new ServiceResult();

            if (_estadosRepository == null)
                return result.Error("Repositorio de estados no disponible");

            try
            {
                var response = _estadosRepository.ListarEstadosCita();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error al listar estados de cita: {ex.Message}");
            }
        }

        public ServiceResult ListarEstadosSolicitud()
        {
            var result = new ServiceResult();

            if (_estadosRepository == null)
                return result.Error("Repositorio de estados no disponible");

            try
            {
                var response = _estadosRepository.ListarEstadosSolicitud();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error al listar estados de solicitud: {ex.Message}");
            }
        }

        #endregion
    }
}
