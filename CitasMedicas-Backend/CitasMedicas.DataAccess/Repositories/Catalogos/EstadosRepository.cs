using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CitasMedicas.DataAccess.Repositories.Catalogos
{
    public class EstadosRepository
    {
        public IEnumerable<EstadosDTO> ListarEstadosCita()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            return db.Query<EstadosDTO>(
                ScriptDatabase.SP_EstadosCita_Listar,
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<EstadosDTO> ListarEstadosSolicitud()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            return db.Query<EstadosDTO>(
                ScriptDatabase.SP_EstadosSolicitud_Listar,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}