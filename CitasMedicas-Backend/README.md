# CitasMedicas Backend

## Estructura del Proyecto

```
CitasMedicas-Backend/
├── CitasMedicas.API/                    → Controladores (EndPoints)
│   ├── Controllers/
│   │   ├── Accesos/
│   │   │   └── AccesosController.cs     → Login, Roles, Usuarios
│   │   └── Catalogos/
│   │       └── EspecialidadesController.cs
│   ├── Middleware/
│   │   └── ApiKeyMiddleware.cs
│   └── Program.cs
│
├── CitasMedicas.BusinessLogic/          → Lógica de negocio
│   ├── Services/
│   │   ├── AccesoService.cs             → Login, Roles, Usuarios
│   │   ├── CatalogoService.cs
│   │   └── ClinicaService.cs
│   └── Configuration/
│       └── JwtSettings.cs
│
├── CitasMedicas.DataAccess/             → Repositorios (DB)
│   ├── Repositories/
│   │   ├── Accesos/
│   │   │   ├── AuthRepository.cs        → Login
│   │   │   ├── IAuthRepository.cs       → Interfaz para pruebas
│   │   │   ├── UserRepository.cs        → CRUD Usuarios
│   │   │   └── IUserRepository.cs       → Interfaz para pruebas
│   │   └── Catalogos/
│   │       ├── EspecialidadesRepository.cs
│   │       └── IEspecialidadesRepository.cs
│   ├── CitasMedicasContext.cs
│   └── ScriptDatabase.cs
│
├── CitasMedicas.Models/                 → Modelos/DTOs
│   └── Models/
│       ├── UsuariosDTO.cs
│       ├── RolDTO.cs
│       ├── LoginRequest.cs
│       └── LoginResponse.cs
│
└── CitasMedicas.Tests/                  → Pruebas Unitarias
    ├── Documentation/
    │   └── PRUEBAS_UNITARIAS.md         → Documentación de pruebas
    ├── ServiceResultTests.cs            → 16 tests
    ├── AccesoServiceTests.cs            → 18 tests
    └── CatalogoServiceTests.cs          → 15 tests
```

---

## Pruebas Unitarias

El proyecto incluye un conjunto completo de pruebas unitarias utilizando **xUnit** y **Moq**.

### Ejecutar Pruebas

```bash
# Todas las pruebas
dotnet test

# Pruebas específicas
dotnet test --filter "FullyQualifiedName~ServiceResultTests"
dotnet test --filter "FullyQualifiedName~AccesoServiceTests"
dotnet test --filter "FullyQualifiedName~CatalogoServiceTests"
```

### Cobertura de Pruebas

| Archivo | Descripción | Tests |
|---------|-------------|-------|
| ServiceResultTests.cs | Pruebas de la clase ServiceResult | 16 |
| AccesoServiceTests.cs | Pruebas del servicio de acceso/login | 18 |
| CatalogoServiceTests.cs | Pruebas del servicio de catálogo | 15 |
| **Total** | | **47** |

### Detalles de Pruebas

#### ServiceResultTests.cs
Pruebas para la clase `ServiceResult`:
- Constructor initialization
- Ok() methods
- Error() methods
- BadRequest(), NotFound(), Unauthorized()
- Conflict(), Info(), Warning()
- Forbidden(), Disabled()
- SetMessage()
- Code property

#### AccesoServiceTests.cs
Pruebas para el servicio de acceso:
- Login() - null request, empty credentials, invalid credentials, inactive user
- LoginDebug()
- ListarRoles()
- RolesInsertar(), RolesEditar(), RolesEliminar()
- ListarUsuarios()
- UsuariosInsertar(), UsuariosEditar(), UsuariosEliminar()

#### CatalogoServiceTests.cs
Pruebas para el servicio de catálogo:
- ListarEspecialidades()
- EspecialidadesInsertar() - null, empty name, valid data, conflicts
- EspecialidadesEditar() - null, zero ID, negative ID, empty name, valid data
- EspecialidadesEliminar() - zero ID, negative ID, valid ID

### Documentación

Ver archivo `CitasMedicas.Tests/Documentation/PRUEBAS_UNITARIAS.md` para información detallada.

---

## Endpoints Login

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/Accesos/Login` | Login con JWT |
| POST | `/Accesos/LoginDebug` | Login sin JWT (debug) |

---

## Archivos modificados para Login

### 1. CitasMedicas.API/Controllers/Accesos/AccesosController.cs
- Agregado endpoint `POST /Accesos/Login`

### 2. CitasMedicas.BusinessLogic/Services/AccesoService.cs
- Método `Login()` - genera token JWT
- Método `LoginDebug()` - sin token

### 3. CitasMedicas.DataAccess/Repositories/Accesos/AuthRepository.cs
- Método `ValidarUsuario()` - valida credenciales

### 4. CitasMedicas.Models/Models/UsuariosDTO.cs
- Agregada propiedad `ClaveHash`

---

## Endpoints Roles

| Método | Endpoint |
|--------|----------|
| GET | `/Accesos/Roles/Listar` |
| POST | `/Accesos/Roles/Insertar` |
| POST | `/Accesos/Roles/Editar` |
| DELETE | `/Accesos/Roles/Eliminar` |

---

## Endpoints Usuarios

| Método | Endpoint |
|--------|----------|
| GET | `/Accesos/Usuarios/Listar` |
| POST | `/Accesos/Usuarios/Insertar` |
| POST | `/Accesos/Usuarios/Editar` |
| DELETE | `/Accesos/Usuarios/Eliminar` |
