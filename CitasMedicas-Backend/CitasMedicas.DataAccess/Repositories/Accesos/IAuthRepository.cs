using CitasMedicas.Models.Models;
using System.Collections.Generic;

namespace CitasMedicas.DataAccess.Repositories.Accesos
{
    public interface IAuthRepository
    {
        UsuariosDTO? ValidarUsuario(string nombreUsuario, string clave);
        IEnumerable<RolDTO> Listar();
        RequestStatus Insertar(RolDTO rol);
        RequestStatus Editar(RolDTO rol);
        RequestStatus Eliminar(int rolId);
    }
}
