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

        public string CodigoSala { get; set; }

        public string NombreSala { get; set; }

        public string Ubicacion { get; set; }

        public bool Activo { get; set; }
    }
}