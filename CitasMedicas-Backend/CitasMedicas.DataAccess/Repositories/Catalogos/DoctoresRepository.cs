using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CitasMedicas.DataAccess.Repositories.Catalogos
{
    public class DoctoresRepository
    {
        public IEnumerable<DoctoresDTO> Listar()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            return db.Query<DoctoresDTO>(
                ScriptDatabase.SP_Doctores_Listar,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
