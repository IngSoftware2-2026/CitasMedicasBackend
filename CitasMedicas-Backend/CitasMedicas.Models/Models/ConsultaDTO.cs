namespace CitasMedicas.Models.Models
{ 
    public class ConsultaDTO
    {
        public int CitaId { get; set; }
        public string? Motivo { get; set; }
        public string? Notas { get; set; }
        public string? Tratamiento { get; set; }
    }

    public class ConsultaDetalleDTO
    {
        public int ConsultaId { get; set; }
        public int CitaId { get; set; }
        public string? Motivo { get; set; }
        public string? Notas { get; set; }
        public string? Tratamiento { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class HistorialClinicoDTO
    {
        public int ConsultaId { get; set; }
        public string? Motivo { get; set; }
        public string? Tratamiento { get; set; }
        public DateTime FechaConsulta { get; set; }

        public int CitaId { get; set; }
        public string? EstadoCita { get; set; }
        public string? NombreMedico { get; set; }
        public string? Especialidad { get; set; }
    }
}