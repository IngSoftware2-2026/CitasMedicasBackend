using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.Models.Models
{
    public class UsuariosDTO
    {
        public int UsuarioId { get; set; }

        public string? NombreUsuario { get; set; }

        public string? Correo { get; set; }

        public string? Telefono { get; set; }

        public string? Clave { get; set; }

        public byte[]? ClaveHash { get; set; }

        public int? RolId { get; set; }

        public bool? Activo { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }
}
