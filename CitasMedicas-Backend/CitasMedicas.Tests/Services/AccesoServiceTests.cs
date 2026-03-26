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
        private readonly Mock<IAuthRepository> _mockAuthRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly AccesoService _service;

        public AccesoServiceTests()
        {
            _mockAuthRepo = new Mock<IAuthRepository>();
            _mockUserRepo = new Mock<IUserRepository>();

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
            _service = new AccesoService(_mockAuthRepo.Object, _mockUserRepo.Object);
        }

        public void Dispose() { }

        [Fact]
        public void Login_RetornaToken_CuandoCredencialesValidas()
        {
            // Arrange
            var login = new LoginRequest { NombreUsuario = "admin", Clave = "admin123" };
            var usuario = new UsuariosDTO
            {
                UsuarioId = 1,
                NombreUsuario = "admin",
                Correo = "admin@test.com",
                RolId = 1,
                Activo = true
            };
            var roles = new List<RolDTO> { new() { RolId = 1, CodigoRol = "ADMIN", NombreRol = "Administrador" } };
            _mockAuthRepo.Setup(r => r.ValidarUsuario("admin", "admin123")).Returns(usuario);
            _mockAuthRepo.Setup(r => r.Listar()).Returns(roles);

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeTrue();
            result.Code.Should().Be(200);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void Login_RetornaUnauthorized_CuandoCredencialesIncorrectas()
        {
            // Arrange
            var login = new LoginRequest { NombreUsuario = "admin", Clave = "clavemal" };
            _mockAuthRepo.Setup(r => r.ValidarUsuario("admin", "clavemal")).Returns((UsuariosDTO)null!);

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(401);
            result.Message.Should().Contain("incorrectos");
        }

        [Fact]
        public void Login_RetornaUnauthorized_CuandoUsuarioInactivo()
        {
            // Arrange
            var login = new LoginRequest { NombreUsuario = "inactivo", Clave = "clave123" };
            var usuario = new UsuariosDTO
            {
                UsuarioId = 2,
                NombreUsuario = "inactivo",
                RolId = 2,
                Activo = false
            };
            _mockAuthRepo.Setup(r => r.ValidarUsuario("inactivo", "clave123")).Returns(usuario);

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
            var login = new LoginRequest { NombreUsuario = "", Clave = "clave123" };

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
        }

        [Fact]
        public void Login_RetornaBadRequest_CuandoClaveVacia()
        {
            // Arrange
            var login = new LoginRequest { NombreUsuario = "admin", Clave = "" };

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
        }

        [Fact]
        public void Login_RetornaError_CuandoExcepcion()
        {
            // Arrange
            var login = new LoginRequest { NombreUsuario = "admin", Clave = "admin123" };
            _mockAuthRepo.Setup(r => r.ValidarUsuario("admin", "admin123")).Throws(new Exception("DB error"));

            // Act
            var result = _service.Login(login);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(500);
        }
    }
}
