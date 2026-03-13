using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.Models.Models
{
    public class SalasDTO
    {
        public int SalaId { get; set; }

        public string CodigoSala { get; set; } = null!;

        public string NombreSala { get; set; } = null!;

        public string Ubicacion { get; set; } = null!;

        public bool Activo { get; set; }
    }
}