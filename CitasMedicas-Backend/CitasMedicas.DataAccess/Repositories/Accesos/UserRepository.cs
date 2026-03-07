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
    public class UserRepository
    {
        public IEnumerable<UsuariosDTO> Listar()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.Query<UsuariosDTO>(
                ScriptDatabase.SP_Usuarios_Listar,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }


        public RequestStatus Insertar(UsuariosDTO usuario)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@NombreUsuario", usuario.NombreUsuario);
            parameter.Add("@Correo", usuario.Correo);
            parameter.Add("@Telefono", usuario.Telefono);
            parameter.Add("@ClaveHash", usuario.ClaveHash, DbType.Binary);
            parameter.Add("@RolId", usuario.RolId);
            parameter.Add("@Activo", usuario.Activo);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Usuarios_Insertar,
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

        public RequestStatus Editar(UsuariosDTO usuario)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@UsuarioId", usuario.UsuarioId);
            parameter.Add("@NombreUsuario", usuario.NombreUsuario);
            parameter.Add("@Correo", usuario.Correo);
            parameter.Add("@Telefono", usuario.Telefono);
            parameter.Add("@ClaveHash", usuario.ClaveHash, DbType.Binary);
            parameter.Add("@RolId", usuario.RolId);
            parameter.Add("@Activo", usuario.Activo);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Usuarios_Editar,
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

        public RequestStatus Eliminar(int usuarioId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@UsuarioId", usuarioId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Usuarios_Eliminar,
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
