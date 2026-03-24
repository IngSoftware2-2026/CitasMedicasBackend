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
    public class CitasRepository
    {
        public RequestStatus CitaInsertar(CitasInsertarDTO cita)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@SolicitudId", cita.SolicitudId);
            parameter.Add("@PacienteId", cita.PacienteId);
            parameter.Add("@MedicoId", cita.MedicoId);
            parameter.Add("@SalaId", cita.SalaId);
            parameter.Add("@Inicio", cita.Inicio);
            parameter.Add("@Fin", cita.Fin);
            parameter.Add("@DuracionMinutos", cita.DuracionMinutos);
            parameter.Add("@CreadaPorUsuarioId", cita.CreadaPorUsuarioId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Citas_Insertar,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al crear la cita"
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

        public IEnumerable<CitasListadoDTO> CitasObtenerPorFiltro(CitasFiltroDTO filtro)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@MedicoId", filtro.MedicoId);
            parameter.Add("@PacienteId", filtro.PacienteId);
            parameter.Add("@SalaId", filtro.SalaId);
            parameter.Add("@EstadoId", filtro.EstadoId);
            parameter.Add("@Desde", filtro.Desde);
            parameter.Add("@Hasta", filtro.Hasta);

            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.Query<CitasListadoDTO>(
                ScriptDatabase.SP_Citas_ObtenerPorFiltro,
                parameter,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public CitasDetalleDTO? CitaObtenerPorId(int citaId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@CitaId", citaId);

            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var result = db.QueryFirstOrDefault<CitasDetalleDTO>(
                ScriptDatabase.SP_Citas_ObtenerPorId,
                parameter,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public RequestStatus CitaCambiarEstado(CitasCambiarEstadoDTO cambioEstado)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@CitaId", cambioEstado.CitaId);
            parameter.Add("@CodigoEstado", cambioEstado.CodigoEstado);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Citas_CambiarEstado,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al cambiar el estado de la cita"
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

        public RequestStatus CitaActualizarSala(CitasActualizarSalaDTO actualizacionSala)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@CitaId", actualizacionSala.CitaId);
            parameter.Add("@SalaId", actualizacionSala.SalaId);

            try
            {
                using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Citas_ActualizarSala,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Error desconocido al actualizar la sala de la cita"
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
