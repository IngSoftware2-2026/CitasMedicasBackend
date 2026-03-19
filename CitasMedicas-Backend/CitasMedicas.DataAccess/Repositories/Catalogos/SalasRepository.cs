using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CitasMedicas.DataAccess.Repositories.Catalogos
{
    public class SalasRepository
    {
        public IEnumerable<SalasDTO> Listar()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.Query<SalasDTO>(
                ScriptDatabase.SP_Sala_Listar,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public RequestStatus Crear(SalasDTO sala)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@CodigoSala", sala.CodigoSala);
            parameter.Add("@NombreSala", sala.NombreSala);
            parameter.Add("@Ubicacion", sala.Ubicacion);

            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.QueryFirstOrDefault<RequestStatus>(
                ScriptDatabase.SP_Sala_Crear,
                parameter,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public RequestStatus Editar(SalasDTO sala)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@SalaId", sala.SalaId);
            parameter.Add("@CodigoSala", sala.CodigoSala);
            parameter.Add("@NombreSala", sala.NombreSala);
            parameter.Add("@Ubicacion", sala.Ubicacion);

            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.QueryFirstOrDefault<RequestStatus>(
                ScriptDatabase.SP_Sala_Editar,
                parameter,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public RequestStatus CambiarEstado(int salaId)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@SalaId", salaId);

            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.QueryFirstOrDefault<RequestStatus>(
                ScriptDatabase.SP_Sala_CambiarEstado,
                parameter,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}