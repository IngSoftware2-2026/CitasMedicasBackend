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
    public class UserRepository : IUserRepository
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
            parameter.Add("@Clave", System.Text.Encoding.UTF8.GetBytes(usuario.Clave ?? string.Empty), DbType.Binary);
            parameter.Add("@RolId", usuario.RolId);

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

        public UsuariosDTO? ObtenerPorId(int usuarioId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            return db.QueryFirstOrDefault<UsuariosDTO>(
                "SELECT TOP 1 * FROM Accesos.tbUsuarios WHERE UsuarioId = @UsuarioId",
                new { UsuarioId = usuarioId }
            );
        }

        public RequestStatus Editar(UsuariosDTO usuario)
        {
            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                string sql = @"UPDATE Accesos.tbUsuarios 
                               SET NombreUsuario = @NombreUsuario, 
                                   Correo = @Correo, 
                                   Telefono = @Telefono, 
                                   RolId = @RolId, 
                                   Activo = @Activo
                               WHERE UsuarioId = @UsuarioId";

                var rowsAffected = db.Execute(sql, new
                {
                    usuario.UsuarioId,
                    usuario.NombreUsuario,
                    usuario.Correo,
                    usuario.Telefono,
                    usuario.RolId,
                    usuario.Activo
                });

                return new RequestStatus
                {
                    CodeStatus = rowsAffected > 0 ? 1 : 0,
                    MessageStatus = rowsAffected > 0 ? "Usuario actualizado" : "No se encontró el usuario"
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
