using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CitasMedicas.DataAccess.Repositories.Accesos
{
    public class AuthRepository
    {
        public virtual UsuarioDTO Login(string nombreUsuario, string clave)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@NombreUsuario", nombreUsuario, DbType.String);
            parameters.Add("@Clave", clave, DbType.String);

            return db.QueryFirstOrDefault<UsuarioDTO>(
                ScriptDatabase.SP_Usuarios_Login,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
