using CitasMedicas.BusinessLogic;
using CitasMedicas.BusinessLogic.Configuration;
using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.DataAccess.Repositories.Accesos;
using CitasMedicas.Models.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CitasMedicas.Tests.Services
{
    public class AccesoServiceTests : IDisposable
    {
        private readonly Mock<AuthRepository> _mockRepo;
        private readonly AccesoService _service;

        public AccesoServiceTests()
        {
            _mockRepo = new Mock<AuthRepository>();

            // Configurar JwtSettings con valores de prueba
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "TestKeyMuySeguraDeAlMenos32CaracteresParaPruebas123" },
                    { "Jwt:Issuer", "TestIssuer" },
                    { "Jwt:Audience", "TestAudience" },
                    { "Jwt:ExpirationHours", "1" }
                })
                .Build();

            JwtSettings.Initialize(config);
            _service = new AccesoService(_mockRepo.Object);
        }

        public void Dispose() { }

        [Fact]
        public void Login_RetornaToken_CuandoCredencialesValidas()
        {
            // Arrange
            var login = new LoginDTO { NombreUsuario = "admin", Clave = "admin123" };
            var usuario = new UsuarioDTO
            {
                UsuarioId = 1,
                NombreUsuario = "admin",
                Correo = "admin@test.com",
                RolId = 1,
                CodigoRol = "ADMIN",
                NombreRol = "Administrador",
                Activo = true
            };
            _mockRepo.Setup(r => r.Login("admin", "admin123")).Returns(usuario);

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeTrue();
            result.Code.Should().Be(200);
            Assert.NotNull(result.Data);

            // Verificar propiedades vía reflection (tipos anónimos son internos)
            object data = result.Data;
            var tokenProp = data.GetType().GetProperty("token");
            Assert.NotNull(tokenProp);
            var tokenValue = tokenProp!.GetValue(data) as string;
            Assert.False(string.IsNullOrEmpty(tokenValue));

            var usuarioIdProp = data.GetType().GetProperty("UsuarioId");
            Assert.NotNull(usuarioIdProp);
            Assert.Equal(1, (int)usuarioIdProp!.GetValue(data)!);
        }

        [Fact]
        public void Login_RetornaUnauthorized_CuandoCredencialesIncorrectas()
        {
            // Arrange
            var login = new LoginDTO { NombreUsuario = "admin", Clave = "clavemal" };
            _mockRepo.Setup(r => r.Login("admin", "clavemal")).Returns((UsuarioDTO)null!);

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(401);
            result.Message.Should().Contain("incorrectas");
        }

        [Fact]
        public void Login_RetornaUnauthorized_CuandoUsuarioInactivo()
        {
            // Arrange
            var login = new LoginDTO { NombreUsuario = "inactivo", Clave = "clave123" };
            var usuario = new UsuarioDTO
            {
                UsuarioId = 2,
                NombreUsuario = "inactivo",
                CodigoRol = "PACIENTE",
                Activo = false
            };
            _mockRepo.Setup(r => r.Login("inactivo", "clave123")).Returns(usuario);

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(401);
            result.Message.Should().Contain("inactivo");
        }

        [Fact]
        public void Login_RetornaBadRequest_CuandoLoginNull()
        {
            // Act
            var result = _service.Login(null!);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
        }

        [Fact]
        public void Login_RetornaBadRequest_CuandoNombreUsuarioVacio()
        {
            // Arrange
            var login = new LoginDTO { NombreUsuario = "", Clave = "clave123" };

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("usuario");
        }

        [Fact]
        public void Login_RetornaBadRequest_CuandoClaveVacia()
        {
            // Arrange
            var login = new LoginDTO { NombreUsuario = "admin", Clave = "" };

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("clave");
        }

        [Fact]
        public void Login_RetornaError_CuandoExcepcion()
        {
            // Arrange
            var login = new LoginDTO { NombreUsuario = "admin", Clave = "admin123" };
            _mockRepo.Setup(r => r.Login("admin", "admin123")).Throws(new Exception("DB error"));

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(500);
            result.Message.Should().Contain("Error inesperado");
        }
    }
}
