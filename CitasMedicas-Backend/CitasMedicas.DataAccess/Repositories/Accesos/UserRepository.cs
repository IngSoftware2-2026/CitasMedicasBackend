using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.DataAccess.Repositories.Accesos
{
    internal class UserRepository
    {

    }
}
CREATE TABLE Accesos.tbUsuarios(
    UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario VARCHAR(60) NULL,
    Correo VARCHAR(150) NULL,
    Telefono VARCHAR(25) NULL,
    ClaveHash VARBINARY(256) NOT NULL,
    RolId INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_Usuarios_Rol
        FOREIGN KEY (RolId) REFERENCES Accesos.tbRoles(RolId)
);
