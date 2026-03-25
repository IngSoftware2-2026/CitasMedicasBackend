namespace CitasMedicas.Models.Models
{ 
    public class ConsultaDto
    {
        public int ConsultaId { get; set; }
        public int CitaId { get; set; }
        public string? Motivo { get; set; }
        public string? Notas { get; set; }
        public string? Tratamiento { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class ConsultaDetalleDto
    {
        public int ConsultaId { get; set; }
        public int CitaId { get; set; }
        public string? Motivo { get; set; }
        public string? Notas { get; set; }
        public string? Tratamiento { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class HistorialClinicoDto
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