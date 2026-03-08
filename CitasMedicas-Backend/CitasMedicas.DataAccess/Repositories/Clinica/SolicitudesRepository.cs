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
    public class SolicitudesRepository
    {
        public RequestStatus SolicitudPublicaInsertar(SolicitudesPublicasDTO solicitud)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@NombrePaciente", solicitud.NombrePaciente);
            parameter.Add("@Telefono", solicitud.Telefono);
            parameter.Add("@Email", solicitud.Email);
            parameter.Add("@MedicoId", solicitud.MedicoId);
            parameter.Add("@FechaHoraInicio", solicitud.FechaHoraInicio);
            parameter.Add("@Motivo", solicitud.Motivo);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_SolicitudesPublicas_Insertar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al crear la solicitud"
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


        public RequestStatus SolicitudCitaInsertar(SolicitudesDTO solicitud)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@PacienteId", solicitud.PacienteId);
            parameter.Add("@MedicoId", solicitud.MedicoId);
            parameter.Add("@FechaHoraInicio", solicitud.FechaHoraInicio);
            parameter.Add("@DuracionMinutos", solicitud.DuracionMinutos);
            parameter.Add("@Motivo", solicitud.Motivo);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_SolicitudesCita_Insertar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al crear la solicitud"
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
