namespace CitasMedicas.Models.Models
{
    public class DoctoresDTO
    {
        public int MedicoId { get; set; }

        public string NombrePublico { get; set; }

        public int UsuarioId { get; set; }

        public int? SalaPredeterminadaId { get; set; }

        public int DuracionIntervaloMinutos { get; set; }

        public int DuracionDefaultMinutos { get; set; }

        public int MinutosBuffer { get; set; }

        public bool Activo { get; set; }
        public int? MedicoUsuarioId { get; set; }

        public string? Medico { get; set; }
    }
}