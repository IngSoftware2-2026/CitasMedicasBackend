namespace CitasMedicas.Models.Models
{
    /// <summary>
    /// DTO para representar la relación doctor-especialidad.
    /// Mapea los datos de Clinica.tbDoctorEspecialidades JOIN Catalogos.tbEspecialidades.
    /// </summary>
    public class DoctorEspecialidadDTO
    {
        public int MedicoId { get; set; }
        public int EspecialidadId { get; set; }
        public string NombreEspecialidad { get; set; } = string.Empty;
        public bool Principal { get; set; }
    }
}
