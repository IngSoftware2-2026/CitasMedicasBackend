using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.Models.Models
{
    public class PacientesDTO
    {
        public int PacienteId { get; set; }

        public int UsuarioId { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public string Correo { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string NumeroIdentidad { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string? Paciente { get; set; }
    }
}
