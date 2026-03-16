using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CitasMedicas.DataAccess.Repositories.Catalogos
{
    public class PacientesRepository
    {
        public IEnumerable<PacientesDTO> Listar()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            return db.Query<PacientesDTO>(
                ScriptDatabase.SP_Pacientes_Listar,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
