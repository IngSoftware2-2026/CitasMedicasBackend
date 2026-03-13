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
│   │   │   └── UserRepository.cs        → CRUD Usuarios
│   │   └── Catalogos/
│   │       └── EspecialidadesRepository.cs
│   ├── CitasMedicasContext.cs
│   └── ScriptDatabase.cs
│
└── CitasMedicas.Models/                 → Modelos/DTOs
    └── Models/
        ├── UsuariosDTO.cs
        ├── RolDTO.cs
        ├── LoginRequest.cs
        └── LoginResponse.cs
```
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