using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.Models.Models
{
    public class LoginResponse
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Token { get; set; }
        public RolDTO Rol { get; set; }
    }
}
