using CitasMedicas.Models.Models;
using System.Collections.Generic;

namespace CitasMedicas.DataAccess.Repositories.Catalogos
{
    public interface IEspecialidadesRepository
    {
        IEnumerable<EspecialidadesDTO> Listar();
        RequestStatus EspecialidadInsertar(EspecialidadesDTO especialidad);
        RequestStatus EspecialidadEditar(EspecialidadesDTO especialidad);
        RequestStatus EspecialidadEliminar(int especialidadId);
    }
}
