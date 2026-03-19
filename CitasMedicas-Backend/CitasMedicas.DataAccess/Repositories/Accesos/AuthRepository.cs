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
        public UsuariosDTO ValidarUsuario(string nombreUsuario, string clave)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@NombreUsuario", nombreUsuario);
            parameter.Add("@Clave", clave);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<dynamic>(
                    ScriptDatabase.SP_Usuarios_Login,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                    return null;

                return new UsuariosDTO
                {
                    UsuarioId = (int)result.UsuarioId,
                    NombreUsuario = result.NombreUsuario?.ToString(),
                    Correo = result.Correo?.ToString(),
                    Telefono = result.Telefono?.ToString(),
                    Clave = result.ClaveHash?.ToString(),
                    RolId = (int)result.RolId,
                    Activo = (bool)result.Activo,
                    FechaCreacion = result.FechaCreacion != null ? (DateTime)result.FechaCreacion : null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ValidarUsuario: {ex.Message}");
                return null;
            }
        }

        public dynamic ValidarUsuarioDebug(string nombreUsuario, string clave)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@NombreUsuario", nombreUsuario);
            parameter.Add("@Clave", clave);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<dynamic>(
                    ScriptDatabase.SP_Usuarios_Login,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message };
            }
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
