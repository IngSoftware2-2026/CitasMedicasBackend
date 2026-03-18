using CitasMedicas.Models.Models;
using System.Collections.Generic;

namespace CitasMedicas.DataAccess.Repositories.Accesos
{
    public interface IUserRepository
    {
        IEnumerable<UsuariosDTO> Listar();
        RequestStatus Insertar(UsuariosDTO usuario);
        RequestStatus Editar(UsuariosDTO usuario);
        RequestStatus Eliminar(int usuarioId);
    }
}
