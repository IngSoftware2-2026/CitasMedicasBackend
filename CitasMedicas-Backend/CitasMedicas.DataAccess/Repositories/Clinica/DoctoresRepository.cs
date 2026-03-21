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

        /// <summary>
        /// Crea un nuevo doctor y retorna el MedicoId generado.
        /// Usa una consulta directa con SCOPE_IDENTITY() para obtener el ID.
        /// </summary>
        public int Crear(DoctoresDTO doctor)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            // Intentar con el SP primero, pero necesitamos capturar el ID
            // Usamos INSERT directo para obtener SCOPE_IDENTITY()
            var sql = @"
                INSERT INTO Clinica.tbDoctores (UsuarioId, NombrePublico, SalaPredeterminadaId, 
                    DuracionIntervaloMinutos, DuracionDefaultMinutos, MinutosBuffer)
                VALUES (@UsuarioId, @NombrePublico, @SalaPredeterminadaId, 
                    @DuracionIntervaloMinutos, @DuracionDefaultMinutos, @MinutosBuffer);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            var parametros = new
            {
                NombrePublico = doctor.NombrePublico,
                UsuarioId = doctor.UsuarioId,
                SalaPredeterminadaId = doctor.SalaPredeterminadaId,
                DuracionIntervaloMinutos = doctor.DuracionIntervaloMinutos,
                DuracionDefaultMinutos = doctor.DuracionDefaultMinutos,
                MinutosBuffer = doctor.MinutosBuffer
            };

            var newId = db.QuerySingle<int>(sql, parametros);
            return newId;
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

        /// <summary>
        /// Asigna una especialidad a un doctor.
        /// Usa MERGE/IF NOT EXISTS para evitar violación de PK si ya existe.
        /// </summary>
        public void AsignarEspecialidad(int medicoId, int especialidadId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var sql = @"
                IF NOT EXISTS (
                    SELECT 1 FROM Clinica.tbDoctorEspecialidades 
                    WHERE MedicoId = @MedicoId AND EspecialidadId = @EspecialidadId
                )
                BEGIN
                    INSERT INTO Clinica.tbDoctorEspecialidades (MedicoId, EspecialidadId, Principal)
                    VALUES (@MedicoId, @EspecialidadId, @Principal)
                END
            ";

            var parametros = new
            {
                MedicoId = medicoId,
                EspecialidadId = especialidadId,
                Principal = false
            };

            db.Execute(sql, parametros);
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

        /// <summary>
        /// Lista todas las especialidades asignadas a un doctor.
        /// Hace JOIN con tbEspecialidades para obtener el nombre.
        /// </summary>
        public IEnumerable<DoctorEspecialidadDTO> ListarEspecialidades(int medicoId)
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var sql = @"
                SELECT de.MedicoId, de.EspecialidadId, e.Nombre AS NombreEspecialidad, de.Principal
                FROM Clinica.tbDoctorEspecialidades de
                INNER JOIN Catalogos.tbEspecialidades e ON e.EspecialidadId = de.EspecialidadId
                WHERE de.MedicoId = @MedicoId
                ORDER BY de.Principal DESC, e.Nombre
            ";

            return db.Query<DoctorEspecialidadDTO>(sql, new { MedicoId = medicoId }).ToList();
        }
    }
}