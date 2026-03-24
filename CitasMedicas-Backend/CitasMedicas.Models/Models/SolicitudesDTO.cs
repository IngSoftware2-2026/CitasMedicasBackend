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
}
