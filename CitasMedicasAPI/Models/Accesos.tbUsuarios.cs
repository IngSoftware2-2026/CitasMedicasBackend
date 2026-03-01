namespace CitasMedicasAPI.Models;

public class Accesos_tbUsuarios
{
    public int UsuarioId { get; set; }
    public string? NombreUsuario { get; set; }
    public string? Correo { get; set; }
    public string? Telefono { get; set; }
    public byte[] ClaveHash { get; set; } = null!;
    public int RolId { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
}