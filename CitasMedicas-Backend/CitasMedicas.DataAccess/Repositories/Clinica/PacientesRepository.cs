using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CitasMedicas.DataAccess.Repositories.Clinica
{
    public class PacientesRepository
    {
        public virtual IEnumerable<PacientesDTO> Listar()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            return db.Query<PacientesDTO>(
                ScriptDatabase.SP_Pacientes_Listar,
                commandType: CommandType.StoredProcedure
            );
        }

        public virtual PacientesDTO ObtenerPorId(int pacienteId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@PacienteId", pacienteId, DbType.Int32);

            return db.QueryFirstOrDefault<PacientesDTO>(
                ScriptDatabase.SP_Pacientes_ObtenerPorId,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public virtual RequestStatus PacienteInsertar(PacientesDTO paciente)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@UsuarioId", paciente.UsuarioId, DbType.Int32);
            parameters.Add("@Nombres", paciente.Nombres, DbType.String);
            parameters.Add("@Apellidos", paciente.Apellidos, DbType.String);
            parameters.Add("@Telefono", paciente.Telefono, DbType.String);
            parameters.Add("@Correo", paciente.Correo, DbType.String);
            parameters.Add("@FechaNacimiento", paciente.FechaNacimiento, DbType.Date);
            parameters.Add("@NumeroIdentidad", paciente.NumeroIdentidad, DbType.String);

            return db.QueryFirstOrDefault<RequestStatus>(
                ScriptDatabase.SP_Pacientes_Insertar,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public virtual RequestStatus PacienteEditar(PacientesDTO paciente)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@PacienteId", paciente.PacienteId, DbType.Int32);
            parameters.Add("@UsuarioId", paciente.UsuarioId, DbType.Int32);
            parameters.Add("@Nombres", paciente.Nombres, DbType.String);
            parameters.Add("@Apellidos", paciente.Apellidos, DbType.String);
            parameters.Add("@Telefono", paciente.Telefono, DbType.String);
            parameters.Add("@Correo", paciente.Correo, DbType.String);
            parameters.Add("@FechaNacimiento", paciente.FechaNacimiento, DbType.Date);
            parameters.Add("@NumeroIdentidad", paciente.NumeroIdentidad, DbType.String);
            parameters.Add("@Activo", paciente.Activo, DbType.Boolean);

            return db.QueryFirstOrDefault<RequestStatus>(
                ScriptDatabase.SP_Pacientes_Editar,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public virtual RequestStatus PacienteEliminar(int pacienteId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@PacienteId", pacienteId, DbType.Int32);

            return db.QueryFirstOrDefault<RequestStatus>(
                ScriptDatabase.SP_Pacientes_Eliminar,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
