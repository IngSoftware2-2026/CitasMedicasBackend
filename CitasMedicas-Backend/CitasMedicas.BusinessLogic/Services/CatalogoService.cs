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
        private readonly IEspecialidadesRepository _especialidadesRepository;


        public CatalogoService(IEspecialidadesRepository especialidadesRepository)
        {
            _especialidadesRepository = especialidadesRepository;
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

        #region Estados Cita

        #endregion
    }
}
