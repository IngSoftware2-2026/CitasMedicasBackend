namespace CitasMedicas.Models.Models
{
    public class SalasDTO
    {
        public int SalaId { get; set; }
        public string CodigoSala { get; set; } = string.Empty;
        public string NombreSala { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
