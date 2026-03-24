using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.Models.Models
{
    public class LoginRequest
    {
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
    }
}
