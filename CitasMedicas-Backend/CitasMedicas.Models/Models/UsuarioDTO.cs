namespace CitasMedicas.Models.Models
{
    public class UsuarioDTO
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public int RolId { get; set; }
        public string CodigoRol { get; set; }
        public string NombreRol { get; set; }
        public bool Activo { get; set; }
    }
}
