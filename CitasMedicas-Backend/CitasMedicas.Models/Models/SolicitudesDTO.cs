using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.Models.Models
{
    public class SolicitudesDTO
    {
        public int PacienteId { get; set; }

        public int MedicoId { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public int DuracionMinutos { get; set; }

        public string? Motivo { get; set; }
        

    }

    public class SolicitudesPublicasDTO
    {
        public string? NombrePaciente { get; set; }

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        public int MedicoId { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public string? Motivo { get; set; }
    }

    public class SolicitudesPublicasListadoDTO
    {
        public int SolicitudId { get; set; }
        public string? NombrePaciente { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public int MedicoId { get; set; }
        public string? Medico { get; set; }
        public int? DuracionDefaultMinutos { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public string? Motivo { get; set; }
        public int EstadoId { get; set; }
        public string? CodigoEstado { get; set; }
        public string? Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class SolicitudesCitaListadoDTO
    {
        public int SolicitudId { get; set; }
        public int PacienteId { get; set; }
        public string? NombrePaciente { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public int MedicoId { get; set; }
        public string? Medico { get; set; }
        public int? DuracionDefaultMinutos { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public int DuracionMinutos { get; set; }
        public string? Motivo { get; set; }
        public int EstadoId { get; set; }
        public string? CodigoEstado { get; set; }
        public string? Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class SolicitudesFiltroDTO
    {
        public int? EstadoId { get; set; }
        public int? MedicoId { get; set; }
        public int? PacienteId { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
    }

    public class CambiarEstadoSolicitudDTO
    {
        public int SolicitudId { get; set; }
        public string CodigoEstado { get; set; } = string.Empty;
    }
}
