namespace CitasMedicas.Models.Models
{
    public class CitasInsertarDTO
    {
        public int? SolicitudId { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int SalaId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public int DuracionMinutos { get; set; }
        public int? CreadaPorUsuarioId { get; set; }
    }

    public class CitasFiltroDTO
    {
        public int? MedicoId { get; set; }
        public int? PacienteId { get; set; }
        public int? SalaId { get; set; }
        public int? EstadoId { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
    }

    public class CitasListadoDTO
    {
        public int CitaId { get; set; }
        public int? SolicitudId { get; set; }
        public int PacienteId { get; set; }
        public string? Paciente { get; set; }
        public int MedicoId { get; set; }
        public string? Medico { get; set; }
        public int SalaId { get; set; }
        public string? Sala { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public int DuracionMinutos { get; set; }
        public int EstadoId { get; set; }
        public string? CodigoEstado { get; set; }
        public string? Estado { get; set; }
        public int? CreadaPorUsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CitasDetalleDTO
    {
        public int CitaId { get; set; }
        public int? SolicitudId { get; set; }
        public int PacienteId { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Paciente { get; set; }
        public int MedicoId { get; set; }
        public int MedicoUsuarioId { get; set; }
        public string? Medico { get; set; }
        public int? SalaPredeterminadaId { get; set; }
        public int? DuracionIntervaloMinutos { get; set; }
        public int? DuracionDefaultMinutos { get; set; }
        public int? MinutosBuffer { get; set; }
        public int SalaId { get; set; }
        public string? CodigoSala { get; set; }
        public string? Sala { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public int DuracionMinutos { get; set; }
        public int EstadoId { get; set; }
        public string? CodigoEstado { get; set; }
        public string? Estado { get; set; }
        public int? CreadaPorUsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CitasCambiarEstadoDTO
    {
        public int CitaId { get; set; }
        public string CodigoEstado { get; set; } = string.Empty;
    }

    public class CitasActualizarSalaDTO
    {
        public int CitaId { get; set; }
        public int SalaId { get; set; }
    }
}
