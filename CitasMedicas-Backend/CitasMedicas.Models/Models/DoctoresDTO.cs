using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Campos opcionales que el SP puede retornar via JOIN
        public string? NombreEspecialidad { get; set; }
        public string? NombreSala { get; set; }

        public int? MedicoUsuarioId { get; set; }
        public string? Medico { get; set; }
    }
}