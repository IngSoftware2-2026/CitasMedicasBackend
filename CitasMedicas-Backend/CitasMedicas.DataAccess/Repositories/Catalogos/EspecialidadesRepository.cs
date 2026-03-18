using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.DataAccess.Repositories.Catalogos
{
    public class EspecialidadesRepository : IEspecialidadesRepository
    {
        public IEnumerable<EspecialidadesDTO> Listar()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.Query<EspecialidadesDTO>(
                ScriptDatabase.SP_Especialidades_Listar,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public RequestStatus EspecialidadInsertar(EspecialidadesDTO especialidad)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Nombre", especialidad.Nombre);
            parameter.Add("@Activo", especialidad.Activo);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Especialidades_Insertar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al insertar"
                };
            }
            catch (Exception ex)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = $"Error inesperado: {ex.Message}"
                };
            }
        }

        public RequestStatus EspecialidadEditar(EspecialidadesDTO especialidad)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@EspecialidadId", especialidad.EspecialidadId);
            parameter.Add("@Nombre", especialidad.Nombre);
            parameter.Add("@Activo", especialidad.Activo);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Especialidades_Editar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al actualizar"
                };
            }
            catch (Exception ex)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = $"Error inesperado: {ex.Message}"
                };
            }
        }


        public RequestStatus EspecialidadEliminar(int especialidadId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@EspecialidadId", especialidadId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Especialidades_Eliminar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al eliminar"
                };
            }
            catch (Exception ex)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = $"Error inesperado: {ex.Message}"
                };
            }
        }
    }
}
