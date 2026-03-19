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
    public class PropuestasReprogramacionRepository
    {
        public RequestStatus CrearPropuesta(PropuestasReprogramacionDTO propuesta)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@SolicitudCitaId", propuesta.SolicitudCitaId);
            parameter.Add("@SolicitudPublicaId", propuesta.SolicitudPublicaId);
            parameter.Add("@OpcionInicio", propuesta.OpcionInicio);
            parameter.Add("@UsuarioProponeId", propuesta.UsuarioProponeId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_CrearPropuestaReprogramacion,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );
                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al crear la propuesta"
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

        public RequestStatus AceptarPropuesta(AceptarPropuestaReprogramacionDTO propuesta)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@PropuestaId", propuesta.PropuestaId);
            parameter.Add("@UsuarioId", propuesta.UsuarioId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_AceptarPropuestaReprogramacion,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );
                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al aceptar la propuesta"
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

        public RequestStatus RechazarPropuesta(int propuestaId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@PropuestaId", propuestaId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_RechazarPropuestaReprogramacion,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );
                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al rechazar la propuesta"
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
