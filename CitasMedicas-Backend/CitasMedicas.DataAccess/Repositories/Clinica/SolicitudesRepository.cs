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

        public IEnumerable<SolicitudesPublicasListadoDTO> SolicitudesPublicasListar(SolicitudesFiltroDTO filtro)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@EstadoSolicitudId", filtro.EstadoId);
            parameter.Add("@MedicoId", filtro.MedicoId);
            parameter.Add("@Desde", filtro.Desde);
            parameter.Add("@Hasta", filtro.Hasta);

            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            return db.Query<SolicitudesPublicasListadoDTO>(
                ScriptDatabase.SP_SolicitudesPublicas_Listar,
                parameter,
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public SolicitudesPublicasListadoDTO? SolicitudesPublicasObtenerPorId(int solicitudId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@SolicitudId", solicitudId);

            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            return db.QueryFirstOrDefault<SolicitudesPublicasListadoDTO>(
                ScriptDatabase.SP_SolicitudesPublicas_ObtenerPorId,
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        public RequestStatus SolicitudesPublicasCambiarEstado(CambiarEstadoSolicitudDTO cambio)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@SolicitudId", cambio.SolicitudId);
            parameter.Add("@CodigoEstado", cambio.CodigoEstado);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_SolicitudesPublicas_CambiarEstado,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );
                return result ?? new RequestStatus { CodeStatus = 0, MessageStatus = "Error desconocido al cambiar estado" };
            }
            catch (Exception ex)
            {
                return new RequestStatus { CodeStatus = 0, MessageStatus = $"Error inesperado: {ex.Message}" };
            }
        }

        public IEnumerable<SolicitudesCitaListadoDTO> SolicitudesCitaListar(SolicitudesFiltroDTO filtro)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@EstadoId", filtro.EstadoId);
            parameter.Add("@MedicoId", filtro.MedicoId);
            parameter.Add("@PacienteId", filtro.PacienteId);
            parameter.Add("@Desde", filtro.Desde);
            parameter.Add("@Hasta", filtro.Hasta);

            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            return db.Query<SolicitudesCitaListadoDTO>(
                ScriptDatabase.SP_SolicitudesCita_Listar,
                parameter,
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public SolicitudesCitaListadoDTO? SolicitudesCitaObtenerPorId(int solicitudId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@SolicitudId", solicitudId);

            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
            return db.QueryFirstOrDefault<SolicitudesCitaListadoDTO>(
                ScriptDatabase.SP_SolicitudesCita_ObtenerPorId,
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        public RequestStatus SolicitudesCitaCambiarEstado(CambiarEstadoSolicitudDTO cambio)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@SolicitudId", cambio.SolicitudId);
            parameter.Add("@CodigoEstado", cambio.CodigoEstado);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_SolicitudesCita_CambiarEstado,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );
                return result ?? new RequestStatus { CodeStatus = 0, MessageStatus = "Error desconocido al cambiar estado" };
            }
            catch (Exception ex)
            {
                return new RequestStatus { CodeStatus = 0, MessageStatus = $"Error inesperado: {ex.Message}" };
            }
        }
    }
}
