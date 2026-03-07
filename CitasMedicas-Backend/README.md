# CitasMedicasBackend

Resumen corto

Proyecto backend para gestión de citas médicas (.NET 8, C# 12). API REST organizada en capas: `CitasMedicas.API`, `CitasMedicas.BusinessLogic`, `CitasMedicas.DataAccess`, `CitasMedicas.Models`.

Estado actual

- Rama activa para desarrollo: `julio` (ya creada y subida a `origin/julio`).
- Compila correctamente después de cambios recientes (repositorios y controladores añadidos para `Accesos`).

Estructura principal del repositorio

- `CitasMedicas.API/` — proyecto web (controladores, middleware, Swagger).
  - `Controllers/Accesos/AccesosController.cs` — endpoints para Roles y Usuarios.
- `CitasMedicas.BusinessLogic/` — servicios de negocio e configuración de DI.
  - `ServiceConfiguration.cs` — registra repositorios y servicios en DI.
  - `Services/AccesoService.cs` — lógica de negocio para Roles y Usuarios (mapeo a `ServiceResult`).
  - `Services/CatalogoService.cs` — ejemplo: Especialidades.
- `CitasMedicas.DataAccess/` — acceso a datos con Dapper.
  - `Repositories/Accesos/AuthRepository.cs` — CRUD Roles.
  - `Repositories/Accesos/UserRepository.cs` — CRUD Usuarios (Insertar, Editar, Listar, Eliminar).
  - `ScriptDatabase.cs` — nombres de stored procedures.
  - `RequestStatus.cs` — modelo de respuesta desde los procedimientos almacenados.
  - `CitasMedicasContext` (configuración de conexión centralizada).
- `CitasMedicas.Models/` — DTOs usados entre capas.
  - `UsuariosDTO.cs`, `RolDTO.cs`, etc.

Puntos críticos (revisar antes de desplegar)

1. Stored procedures
   - Asegurarse de que los SP listados en `ScriptDatabase.cs` existen en la base de datos y tienen la firma esperada:
     - `Accesos.SP_Roles_Listar`, `Accesos.SP_Roles_Insertar`, `Accesos.SP_Roles_Editar`, `Accesos.SP_Roles_Eliminar`
     - `Accesos.SP_Usuarios_Listar`, `Accesos.SP_Usuarios_Insertar`, `Accesos.SP_Usuarios_Editar`, `Accesos.SP_Usuarios_Eliminar`

2. Seguridad y manejo de contraseñas
   - `UsuariosDTO.ClaveHash` contiene el hash de la clave: nunca enviar/almacenar contraseñas en texto plano.
   - Generar y verificar hashes de forma segura (por ejemplo PBKDF2/argon2/bcrypt) en la capa correspondiente — actualmente el repositorio solo recibe el `byte[]`.

3. Configuración sensible
   - Mantener `ConnectionString` y `Jwt:Key` fuera del control de versiones (usar `appsettings.Development.json`, `dotnet user-secrets` o variables de entorno).
   - Revisar `JwtSettings.Initialize` y valores en `appsettings`.

4. Autenticación y autorización
   - `ApiKeyMiddleware` requiere cabecera `XApiKey` (ver `CitasMedicas.API/Middleware/ApiKeyMiddleware.cs`).
   - JWT está configurado — asegurar que `Issuer`, `Audience`, `Key` estén definidos.

5. Inyección de dependencias
   - Verificar que todos los repositorios/servicios están registrados en `ServiceConfiguration.DataAccess` y `.BusinessLogic`:
     - `AuthRepository`, `UserRepository`, `EspecialidadesRepository`, `CatalogoService`, `AccesoService`, etc.

6. Visibilidad de DTOs
   - `UsuariosDTO` debe ser `public` para que las capas superiores (API/DataAccess) lo consuman. (Ya ajustado en la rama actual.)

7. CORS y origenes
   - Política `AllowAngularApp` permite `http://localhost:4200` — revisar antes de producción.

Cómo ejecutar en local

1. Configurar variables sensibles:
   - `ConnectionString` en `CitasMedicas.API/appsettings.json` o usar `dotnet user-secrets`.
   - `Jwt:Issuer`, `Jwt:Audience`, `Jwt:Key` en `appsettings` o variables de entorno.

2. Restaurar, compilar y ejecutar:
   - `dotnet restore` (en la solución raíz)
   - `dotnet build`
   - `dotnet run --project CitasMedicas.API/CitasMedicas.API.csproj`

3. Abrir Swagger: `https://localhost:{port}/swagger` (el `Program.cs` redirige `/` a `/swagger`).

Endpoints relevantes (resumen)

- Roles
  - `GET  /Accesos/Roles/Listar`
  - `POST /Accesos/Roles/Insertar`  (body: `RolDTO`)
  - `POST /Accesos/Roles/Editar`    (body: `RolDTO`)
  - `DELETE /Accesos/Roles/Eliminar?rolId={id}`

- Usuarios
  - `GET  /Accesos/Usuarios/Listar`
  - `POST /Accesos/Usuarios/Insertar` (body: `UsuariosDTO`)
  - `POST /Accesos/Usuarios/Editar`   (body: `UsuariosDTO`)
  - `DELETE /Accesos/Usuarios/Eliminar?usuarioId={id}`

Ejemplo rápido (curl)

- Listar roles:
  - `curl -H "XApiKey: <tu_apikey>" https://localhost:5001/Accesos/Roles/Listar`

Contribuir / Pull Requests

- Trabajar en una rama por feature/nombre (`feature/mi-cambio` o usar `julio` si corresponde).
- Hacer commits claros y descriptivos.
- Crear PR hacia `main` o la rama de integración del equipo y describir los cambios críticos (stored procedures, cambios en DTOs, configuración requerida).

Checklist antes de merge a main

- [ ] SP en BD actualizados y probados
- [ ] Variables sensibles configuradas en entorno de CI/CD
- [ ] Pruebas manuales de endpoints principales
- [ ] Revisión de seguridad: manejo de claves, cabeceras y CORS

Contacto / Notas finales

- Esta README es una guía técnica rápida para subir el repositorio y desplegar localmente. Ajustar las secciones de configuración y seguridad según políticas de la organización.

- Archivo principal para revisar cambios recientes: `CitasMedicas.BusinessLogic/ServiceConfiguration.cs` (registro DI) y `CitasMedicas.API/Controllers/Accesos/AccesosController.cs` (endpoints añadidos).

