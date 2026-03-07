using CitasMedicas.Models.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CitasMedicas.DataAccess.Repositories.Clinica
{
    public class DoctoresRepository
    {
        public IEnumerable<DoctoresDTO> Listar(bool? activo, int? especialidadId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var parametros = new
            {
                Activo = activo,
                EspecialidadId = especialidadId
            };

            var result = db.Query<DoctoresDTO>(
                ScriptDatabase.SP_Doctores_Listar,
                parametros,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public DoctoresDTO ObtenerPorId(int id)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var parametros = new
            {
                MedicoId = id
            };

            var result = db.QueryFirstOrDefault<DoctoresDTO>(
                ScriptDatabase.SP_Doctor_ObtenerPorId,
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public void CambiarActivo(int id, bool activo)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var parametros = new
            {
                MedicoId = id,
                Activo = activo
            };

            db.Execute(
                "Clinica.sp_ToggleActivoDoctor",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public void Crear(DoctoresDTO doctor)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var parametros = new
            {
                NombrePublico = doctor.NombrePublico,
                UsuarioId = doctor.UsuarioId,
                SalaPredeterminadaId = doctor.SalaPredeterminadaId,
                DuracionIntervaloMinutos = doctor.DuracionIntervaloMinutos,
                DuracionDefaultMinutos = doctor.DuracionDefaultMinutos,
                MinutosBuffer = doctor.MinutosBuffer
            };

            db.Execute(
                "Clinica.sp_CrearDoctor",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public void Editar(int id, DoctoresDTO doctor)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var parametros = new
            {
                MedicoId = id,
                NombrePublico = doctor.NombrePublico,
                SalaPredeterminadaId = doctor.SalaPredeterminadaId,
                DuracionIntervaloMinutos = doctor.DuracionIntervaloMinutos,
                DuracionDefaultMinutos = doctor.DuracionDefaultMinutos,
                MinutosBuffer = doctor.MinutosBuffer
            };

            db.Execute(
                "Clinica.sp_ActualizarDoctor",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public void AsignarEspecialidad(int medicoId, int especialidadId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var parametros = new
            {
                MedicoId = medicoId,
                EspecialidadId = especialidadId,
                Principal = false
            };

            db.Execute(
                "Clinica.sp_AsignarEspecialidadDoctor",
                parametros,
                commandType: CommandType.StoredProcedure
            );

        }

        public void RemoverEspecialidad(int medicoId, int especialidadId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var parametros = new
            {
                MedicoId = medicoId,
                EspecialidadId = especialidadId
            };

            db.Execute(
                "Clinica.sp_RemoverEspecialidadDoctor",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public void SetEspecialidadPrincipal(int medicoId, int especialidadId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var parametros = new
            {
                MedicoId = medicoId,
                EspecialidadId = especialidadId
            };

            db.Execute(
                "Clinica.sp_SetEspecialidadPrincipal",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}