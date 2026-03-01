namespace CitasMedicasAPI.Models;

public class Catalogos_tbSalas
{
    public int SalaId { get; set; }
    public string? CodigoSala { get; set; }
    public string? NombreSala { get; set; }
    public string? Ubicacion { get; set; }
    public bool Activo { get; set; }
}