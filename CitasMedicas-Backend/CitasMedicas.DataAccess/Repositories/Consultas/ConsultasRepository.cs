using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.DataAccess.Repositories.Consultas
{
    public abstract class ConsultasRepository
    {
        public RequestStatus ConsultaInsertar(ConsultaDto consulta)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@CitaId", consulta.CitaId);
            parameter.Add("@Motivo", consulta.Motivo);
            parameter.Add("@Notas", consulta.Notas);
            parameter.Add("@Tratamiento", consulta.Tratamiento);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Consulta_Crear,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus { CodeStatus = 0, MessageStatus = "Error desconocido al crear" };
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
        
        public RequestStatus ConsultaActualizar(ConsultaDto consulta)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@ConsultaId", consulta.CitaId); 
            parameter.Add("@Motivo", consulta.Motivo);
            parameter.Add("@Notas", consulta.Notas);
            parameter.Add("@Tratamiento", consulta.Tratamiento);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Consulta_Actualizar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus { CodeStatus = 0, MessageStatus = "Error al actualizar" };
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

        public ConsultaDetalleDto? ObtenerPorCita(int citaId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@CitaId", citaId);

            
            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<ConsultaDetalleDto>(
                    ScriptDatabase.SP_Consulta_Obtener_PorCita,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch
            {
                return new ConsultaDetalleDto();
            }
        }

        public IEnumerable<HistorialClinicoDto> ObtenerHistorialPaciente(int pacienteId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@PacienteId", pacienteId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                return db.Query<HistorialClinicoDto>(
                    ScriptDatabase.SP_Consulta_Obtener_PorPaciente, 
                    parameter,
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
            catch (Exception)
            {
                return Enumerable.Empty<HistorialClinicoDto>();
            }
        }
            
        public abstract IEnumerable<ConsultaDto> GetAllConsultasAsync();
        
    }
}

       