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


        public CatalogoService(EspecialidadesRepository especialidadesRepository)
        {
            _especialidadesRepository = especialidadesRepository;
        }


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
                return result.Error(ex.Message);
            }
        }


        public ServiceResult EspecialidadesInsertar(EspecialidadesDTO especialidad)
        {
            var result = new ServiceResult();

            if (especialidad == null)
                return result.Error("Los datos de la especialidad son requeridos");

            if (string.IsNullOrWhiteSpace(especialidad.Nombre))
                return result.Error("Nombre de la especialidad es requerido");

            try
            {
                var response = _especialidadesRepository.EspecialidadInsertar(especialidad);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Especialidad inserting: {ex.Message}");
            }
        }


        public ServiceResult EspecialidadesEditar(EspecialidadesDTO especialidad)
        {
            var result = new ServiceResult();

            if (especialidad == null)
                return result.Error("Los datos de la especialidad son requeridos");

            if (especialidad.EspecialidadId <= 0)
                return result.Error("El id de la especialidad es requerido");

            if (string.IsNullOrWhiteSpace(especialidad.Nombre))
                return result.Error("Nombre de la especialidad es requerido");

            try
            {
                var response = _especialidadesRepository.EspecialidadEditar(especialidad);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error inesperado al editar especialidad: {ex.Message}");
            }
        }


        public ServiceResult EspecialidadesEliminar(int especialidadId)
        {
            var result = new ServiceResult();

            if (especialidadId <= 0)
                return result.Error("El id de la especialidad es requerido");

            try
            {
                var response = _especialidadesRepository.EspecialidadEliminar(especialidadId);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error inesperado durante la eliminación: {ex.Message}");
            }
        }

        #endregion
    }
}
