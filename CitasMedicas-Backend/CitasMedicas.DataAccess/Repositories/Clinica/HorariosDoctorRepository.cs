using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CitasMedicas.DataAccess.Repositories.Clinica
{
    public class HorariosDoctorRepository
    {
        // 1. Obtener Horarios (GET)
        public IEnumerable<HorarioDoctorDTO> ObtenerHorarios(int doctorId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            var parameter = new DynamicParameters();
            
            parameter.Add("@DoctorId", doctorId);

            var result = db.Query<HorarioDoctorDTO>(
                ScriptDatabase.SP_HorariosDoctor_Obtener,
                parameter,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        // 2. Crear Horario (POST)
        public RequestStatus CrearHorario(HorarioDoctorDTO horario)
        {
            var parameter = new DynamicParameters();
            
            parameter.Add("@DoctorId", horario.DoctorId);
            parameter.Add("@DiaSemana", horario.DiaSemana);
            parameter.Add("@HoraInicio", horario.HoraInicio);
            parameter.Add("@HoraFin", horario.HoraFin);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_HorariosDoctor_Crear,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus { CodeStatus = 0, MessageStatus = "Error desconocido al crear" };
            }
            catch (Exception ex)
            {
                return new RequestStatus { CodeStatus = 0, MessageStatus = $"Error inesperado: {ex.Message}" };
            }
        }

        // 3. Actualizar Horario (PUT)
        public RequestStatus ActualizarHorario(HorarioDoctorDTO horario)
        {
            var parameter = new DynamicParameters();
            
            parameter.Add("@HorarioId", horario.HorarioId);
            parameter.Add("@DiaSemana", horario.DiaSemana);
            parameter.Add("@HoraInicio", horario.HoraInicio);
            parameter.Add("@HoraFin", horario.HoraFin);
            parameter.Add("@Activo", horario.Activo);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_HorariosDoctor_Actualizar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus { CodeStatus = 0, MessageStatus = "Error desconocido al actualizar" };
            }
            catch (Exception ex)
            {
                return new RequestStatus { CodeStatus = 0, MessageStatus = $"Error inesperado: {ex.Message}" };
            }
        }

        // 4. Eliminar Horario (DELETE)
        public RequestStatus EliminarHorario(int horarioId)
        {
            var parameter = new DynamicParameters();
            
            parameter.Add("@HorarioId", horarioId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_HorariosDoctor_Eliminar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus { CodeStatus = 0, MessageStatus = "Error desconocido al eliminar" };
            }
            catch (Exception ex)
            {
                return new RequestStatus { CodeStatus = 0, MessageStatus = $"Error inesperado: {ex.Message}" };
            }
        }
    }
}