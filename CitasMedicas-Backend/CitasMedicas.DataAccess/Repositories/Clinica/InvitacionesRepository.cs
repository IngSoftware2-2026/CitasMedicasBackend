using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CitasMedicas.DataAccess.Repositories.Clinica
{
    public class InvitacionesRepository
    {
        // Genera invitación — invalida las pendientes anteriores del mismo paciente (lo hace el SP)
        public RequestStatus GenerarInvitacion(int pacienteId, string hashToken)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@PacienteId", pacienteId, DbType.Int32);
            parameters.Add("@HashToken",  hashToken,  DbType.String);

            return db.QueryFirstOrDefault<RequestStatus>(
                ScriptDatabase.SP_GenerarInvitacion,
                parameters,
                commandType: CommandType.StoredProcedure
            )!;
        }

        // Valida token — retorna datos del paciente si es válido, o CodeStatus = -1 si expiró/fue usado
        public InvitacionDTO ValidarInvitacion(string hashToken)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@HashToken", hashToken, DbType.String);

            return db.QueryFirstOrDefault<InvitacionDTO>(
                ScriptDatabase.SP_ValidarInvitacion,
                parameters,
                commandType: CommandType.StoredProcedure
            )!;
        }

        // Marca invitación como usada con timestamp actual
        public RequestStatus UsarInvitacion(string hashToken)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@HashToken", hashToken, DbType.String);

            return db.QueryFirstOrDefault<RequestStatus>(
                ScriptDatabase.SP_UsarInvitacion,
                parameters,
                commandType: CommandType.StoredProcedure
            )!;
        }

        // Lista todas las invitaciones de un paciente con su estado
        public IEnumerable<InvitacionDTO> ObtenerPorPaciente(int pacienteId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@PacienteId", pacienteId, DbType.Int32);

            return db.Query<InvitacionDTO>(
                ScriptDatabase.SP_ObtenerInvitacionesPorPaciente,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}