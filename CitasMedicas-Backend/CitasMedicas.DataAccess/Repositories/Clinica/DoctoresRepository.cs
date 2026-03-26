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

            var doctores = db.Query<DoctoresDTO>(
                ScriptDatabase.SP_Doctores_Listar,
                parametros,
                commandType: CommandType.StoredProcedure
            ).ToList();

            // Mapeamos las especialidades manualmente para ignorar si el SP trae o no la columna correcta
            var sqlSpecs = @"
                SELECT de.MedicoId, e.Nombre as NombreEspecialidad
                FROM Clinica.tbDoctorEspecialidades de
                INNER JOIN Catalogos.tbEspecialidades e ON e.EspecialidadId = de.EspecialidadId
                ORDER BY de.Principal DESC, e.Nombre
            ";
            var todasEspecialidades = db.Query(sqlSpecs).ToList();

            foreach (var doc in doctores)
            {
                var specsForDoc = todasEspecialidades
                    .Where(s => s.MedicoId == doc.MedicoId)
                    .Select(s => (string)s.NombreEspecialidad)
                    .ToList();

                if (specsForDoc.Any())
                {
                    // Unimos las especialidades con coma
                    doc.NombreEspecialidad = string.Join(", ", specsForDoc);
                }
            }

            return doctores;
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

            if (result != null)
            {
                var sqlSpecs = @"
                    SELECT e.Nombre as NombreEspecialidad
                    FROM Clinica.tbDoctorEspecialidades de
                    INNER JOIN Catalogos.tbEspecialidades e ON e.EspecialidadId = de.EspecialidadId
                    WHERE de.MedicoId = @MedicoId
                    ORDER BY de.Principal DESC, e.Nombre
                ";
                var specs = db.Query<string>(sqlSpecs, new { MedicoId = id }).ToList();

                if (specs.Any())
                {
                    result.NombreEspecialidad = string.Join(", ", specs);
                }
            }

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

            var parametros = new
            {
                UsuarioId = doctor.UsuarioId,
                NombrePublico = doctor.NombrePublico,
                SalaPredeterminadaId = doctor.SalaPredeterminadaId,
                DuracionIntervaloMinutos = doctor.DuracionIntervaloMinutos,
                DuracionDefaultMinutos = doctor.DuracionDefaultMinutos,
                MinutosBuffer = doctor.MinutosBuffer,
                Imagen = doctor.Imagen
            };

            var newId = db.QuerySingle<int>(
                "Clinica.sp_CrearDoctor",
                parametros,
                commandType: CommandType.StoredProcedure
            );
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
                MinutosBuffer = doctor.MinutosBuffer,
                Imagen = doctor.Imagen
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
        /// <summary>
        /// Lista todas las salas activas.
        /// </summary>
        public IEnumerable<SalasDTO> ListarSalas()
        {
            using var db = new SqlConnection(CitasMedicasContext.ConnectionString);

            var sql = @"
                SELECT SalaId, ISNULL(CodigoSala, '') as CodigoSala, NombreSala, ISNULL(Ubicacion, '') as Ubicacion, Activo 
                FROM Catalogos.tbSalas 
                WHERE Activo = 1
                ORDER BY NombreSala
            ";

            return db.Query<SalasDTO>(sql).ToList();
        }
    }
}