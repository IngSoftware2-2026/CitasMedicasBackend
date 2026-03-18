using CitasMedicas.Models.Models;
using System.Collections.Generic;

namespace CitasMedicas.DataAccess.Repositories.Accesos
{
    public interface IUserRepository
    {
        IEnumerable<UsuariosDTO> Listar();
        UsuariosDTO? ObtenerPorId(int usuarioId);
        RequestStatus Insertar(UsuariosDTO usuario);
        RequestStatus Editar(UsuariosDTO usuario);
        RequestStatus Eliminar(int usuarioId);
    }
}
