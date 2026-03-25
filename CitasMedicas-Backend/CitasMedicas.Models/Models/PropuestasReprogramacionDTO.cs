using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.Models.Models
{
    public class PropuestasReprogramacionDTO
    {
        public int? SolicitudCitaId { get; set; }
        public int? SolicitudPublicaId { get; set; }
        public DateTime OpcionInicio { get; set; }
        public int UsuarioProponeId { get; set; }
    }

    public class AceptarPropuestaReprogramacionDTO
    {
        public int PropuestaId { get; set; }
        public int UsuarioId { get; set; }
    }
}
