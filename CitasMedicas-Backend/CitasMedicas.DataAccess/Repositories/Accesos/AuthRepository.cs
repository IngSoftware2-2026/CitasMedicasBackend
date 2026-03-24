using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CitasMedicas.DataAccess.Repositories.Accesos
{
    public class AuthRepository : IAuthRepository
    {
        public virtual UsuarioDTO Login(string nombreUsuario, string clave)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.Query<RolDTO>(
                ScriptDatabase.SP_Roles_Listar,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return null; // Ajusta el retorno según la implementación real de dev
        }

        public IEnumerable<RolDTO> Listar()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.Query<RolDTO>(
                ScriptDatabase.SP_Roles_Listar,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public RequestStatus Eliminar(int rolId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@RolId", rolId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Roles_Eliminar,
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



        public RequestStatus Insertar(RolDTO rol)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@CodigoRol", rol.CodigoRol);
            parameter.Add("@NombreRol", rol.NombreRol);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Roles_Insertar,
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
        public RequestStatus Editar(RolDTO rol)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@RolId", rol.RolId);
            parameter.Add("@CodigoRol", rol.CodigoRol);
            parameter.Add("@NombreRol", rol.NombreRol);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Roles_Editar,
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

    }
}
