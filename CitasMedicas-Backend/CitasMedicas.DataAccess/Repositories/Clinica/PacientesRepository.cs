using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.DataAccess.Repositories.Clinica
{
    public class PacientesRepository
    {
        public IEnumerable<PacientesDTO> Listar()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.Query<PacientesDTO>(
                ScriptDatabase.SP_Pacientes_Listar,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public PacientesDTO ObtenerPorId(int pacienteId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@PacienteId", pacienteId);

            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            return db.QueryFirstOrDefault<PacientesDTO>(
                ScriptDatabase.SP_Pacientes_ObtenerPorId,
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        public RequestStatus PacienteInsertar(PacientesDTO paciente)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@UsuarioId", paciente.UsuarioId);
            parameter.Add("@Nombres", paciente.Nombres);
            parameter.Add("@Apellidos", paciente.Apellidos);
            parameter.Add("@Telefono", paciente.Telefono);
            parameter.Add("@Correo", paciente.Correo);
            parameter.Add("@FechaNacimiento", paciente.FechaNacimiento);
            parameter.Add("@NumeroIdentidad", paciente.NumeroIdentidad);
            parameter.Add("@Activo", paciente.Activo);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Pacientes_Insertar,
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

        public RequestStatus PacienteEditar(PacientesDTO paciente)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@PacienteId", paciente.PacienteId);
            parameter.Add("@UsuarioId", paciente.UsuarioId);
            parameter.Add("@Nombres", paciente.Nombres);
            parameter.Add("@Apellidos", paciente.Apellidos);
            parameter.Add("@Telefono", paciente.Telefono);
            parameter.Add("@Correo", paciente.Correo);
            parameter.Add("@FechaNacimiento", paciente.FechaNacimiento);
            parameter.Add("@NumeroIdentidad", paciente.NumeroIdentidad);
            parameter.Add("@Activo", paciente.Activo);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Pacientes_Editar,
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

        public RequestStatus PacienteEliminar(int pacienteId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@PacienteId", pacienteId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Pacientes_Eliminar,
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
