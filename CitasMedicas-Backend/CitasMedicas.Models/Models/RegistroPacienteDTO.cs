namespace CitasMedicas.Models.Models
{
    public class RegistroPacienteDTO
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string NumeroIdentidad { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
    }
}
