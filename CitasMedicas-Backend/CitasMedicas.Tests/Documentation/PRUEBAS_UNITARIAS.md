# Documentación de Pruebas Unitarias

## Descripción General

Este proyecto contiene pruebas unitarias para el backend de Citas Médicas, utilizando **xUnit** como framework de pruebas y **Moq** para mockear dependencias.

## Estructura del Proyecto de Pruebas

```
CitasMedicas.Tests/
├── Documentation/
│   └── PRUEBAS_UNITARIAS.md
├── ServiceResultTests.cs
├── AccesoServiceTests.cs
├── CatalogoServiceTests.cs
└── CitasMedicas.Tests.csproj
```

## Requisitos

- .NET 8.0 SDK
- Paquetes NuGet:
  - xUnit (2.9.3)
  - Moq (4.20.72)
  - Microsoft.NET.Test.Sdk (17.14.1)
  - coverlet.collector (6.0.4)

## Ejecutar las Pruebas

### Todas las pruebas
```bash
dotnet test
```

### Pruebas específicas por archivo
```bash
dotnet test --filter "FullyQualifiedName~ServiceResultTests"
dotnet test --filter "FullyQualifiedName~AccesoServiceTests"
dotnet test --filter "FullyQualifiedName~CatalogoServiceTests"
```

### Con cobertura de código
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Archivos de Pruebas

### 1. ServiceResultTests.cs
Pruebas para la clase `ServiceResult` que verifica todos los métodos de respuesta del servicio.

**Métodos probados:**
- Constructor
- Ok()
- Error()
- BadRequest()
- NotFound()
- Unauthorized()
- Conflict()
- Info()
- Warning()
- Forbidden()
- Disabled()
- SetMessage()
- Code (propiedad)

### 2. AccesoServiceTests.cs
Pruebas para el servicio de acceso que maneja autenticación y gestión de usuarios/roles.

**Métodos probados:**
- Login() - validación de credenciales, usuario inactivo
- LoginDebug()
- ListarRoles()
- RolesInsertar()
- RolesEditar()
- RolesEliminar()
- ListarUsuarios()
- UsuariosInsertar()
- UsuariosEditar()
- UsuariosEliminar()

### 3. CatalogoServiceTests.cs
Pruebas para el servicio de catálogo que maneja especialidades médicas.

**Métodos probados:**
- ListarEspecialidades()
- EspecialidadesInsertar() - validación de datos, duplicados
- EspecialidadesEditar() - validación de ID y nombre
- EspecialidadesEliminar()

## Tips para Escribir Pruebas

### Buenas Prácticas

1. **Nombre descriptivo**: Los tests deben tener nombres que expliquen qué probamos
   ```csharp
   [Fact]
   public void Login_WithEmptyPassword_ShouldReturnBadRequest()
   ```

2. **Un solo assert por test**: Facilita el diagnóstico de errores

3. **Given-When-Then**: Estructura clara en cada prueba
   ```csharp
   [Fact]
   public void Login_WithInvalidCredentials_ShouldReturnUnauthorized()
   {
       // Given
       var loginRequest = new LoginRequest { ... };
       
       // When
       var result = _service.Login(loginRequest);
       
       // Then
       Assert.False(result.Success);
   }
   ```

4. **Usar Moq para dependencias**: No probar con datos reales de base de datos

### Ejemplo de Test

```csharp
[Fact]
public void Login_WithNullRequest_ShouldReturnBadRequest()
{
    var result = _service.Login(null!);

    Assert.False(result.Success);
    Assert.Equal(ServiceResultType.BadRequest, result.Type);
    Assert.Contains("requeridas", result.Message);
}
```

## Cobertura Esperada

| Servicio | Tests |
|----------|-------|
| ServiceResult | 16 tests |
| AccesoService | 18 tests |
| CatalogoService | 15 tests |
| **Total** | **49 tests** |

## Solución de Problemas

### Error: "The namespace 'ServiceResultType' could not be found"
Asegúrate de que el archivo de prueba tenga el import:
```csharp
using CitasMedicas.BusinessLogic;
```

### Error: "Could not load file or assembly"
Ejecuta:
```bash
dotnet restore
```

### Error: "Tests not found"
Verifica que el proyecto de tests esté en la solución:
```bash
dotnet sln add CitasMedicas.Tests/CitasMedicas.Tests.csproj
```
