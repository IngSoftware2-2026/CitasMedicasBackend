using CitasMedicas.BusinessLogic;
using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Accesos;
using CitasMedicas.DataAccess.Repositories.Clinica;
using CitasMedicas.Models.Models;
using Moq;

namespace CitasMedicas.Tests
{
    public class AccesoServiceTests
    {
        private readonly Mock<IAuthRepository> _mockAuthRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly AccesoService _service;

        public AccesoServiceTests()
        {
            _mockAuthRepository = new Mock<IAuthRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            var pacientesRepo = new PacientesRepository();
            _service = new AccesoService(_mockAuthRepository.Object, _mockUserRepository.Object, pacientesRepo);
        }

        [Fact]
        public void Login_WithNullRequest_ShouldReturnBadRequest()
        {
            var result = _service.Login(null!);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("requeridas", result.Message);
        }

        [Fact]
        public void Login_WithEmptyUsername_ShouldReturnBadRequest()
        {
            var loginRequest = new LoginRequest
            {
                NombreUsuario = "",
                Clave = "password123"
            };

            var result = _service.Login(loginRequest);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("requeridos", result.Message);
        }

        [Fact]
        public void Login_WithEmptyPassword_ShouldReturnBadRequest()
        {
            var loginRequest = new LoginRequest
            {
                NombreUsuario = "testuser",
                Clave = ""
            };

            var result = _service.Login(loginRequest);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("requeridos", result.Message);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            var loginRequest = new LoginRequest
            {
                NombreUsuario = "invaliduser",
                Clave = "wrongpassword"
            };

            _mockAuthRepository.Setup(r => r.ValidarUsuario(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((UsuariosDTO?)null);

            var result = _service.Login(loginRequest);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Unauthorized, result.Type);
            Assert.Contains("incorrectos", result.Message);
        }

        [Fact]
        public void Login_WithInactiveUser_ShouldReturnUnauthorized()
        {
            var loginRequest = new LoginRequest
            {
                NombreUsuario = "testuser",
                Clave = "password123"
            };

            var inactiveUser = new UsuariosDTO
            {
                UsuarioId = 1,
                NombreUsuario = "testuser",
                Correo = "test@test.com",
                Activo = false,
                RolId = 1
            };

            _mockAuthRepository.Setup(r => r.ValidarUsuario(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(inactiveUser);

            var result = _service.Login(loginRequest);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Unauthorized, result.Type);
            Assert.Contains("inactivo", result.Message);
        }

        [Fact]
        public void LoginDebug_WithNullRequest_ShouldReturnBadRequest()
        {
            var result = _service.LoginDebug(null!);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
        }

        [Fact]
        public void ListarRoles_ShouldReturnRoles()
        {
            var roles = new List<RolDTO>
            {
                new RolDTO { RolId = 1, CodigoRol = "ADMIN", NombreRol = "Administrador" },
                new RolDTO { RolId = 2, CodigoRol = "USER", NombreRol = "Usuario" }
            };

            _mockAuthRepository.Setup(r => r.Listar()).Returns(roles);

            var result = _service.ListarRoles();

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void RolesInsertar_WithNullRol_ShouldReturnBadRequest()
        {
            var result = _service.RolesInsertar(null!);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("requeridos", result.Message);
        }

        [Fact]
        public void RolesInsertar_WithValidRol_ShouldCallRepository()
        {
            var rol = new RolDTO
            {
                RolId = 0,
                CodigoRol = "TEST",
                NombreRol = "Rol de Prueba"
            };

            _mockAuthRepository.Setup(r => r.Insertar(It.IsAny<RolDTO>()))
                .Returns(new RequestStatus { CodeStatus = 1, MessageStatus = "Insertado" });

            var result = _service.RolesInsertar(rol);

            Assert.True(result.Success);
            _mockAuthRepository.Verify(r => r.Insertar(It.IsAny<RolDTO>()), Times.Once);
        }

        [Fact]
        public void RolesEditar_WithValidRol_ShouldCallRepository()
        {
            var rol = new RolDTO
            {
                RolId = 1,
                CodigoRol = "TEST",
                NombreRol = "Rol de Prueba"
            };

            _mockAuthRepository.Setup(r => r.Editar(It.IsAny<RolDTO>()))
                .Returns(new RequestStatus { CodeStatus = 1, MessageStatus = "Editado" });

            var result = _service.RolesEditar(rol);

            Assert.True(result.Success);
            _mockAuthRepository.Verify(r => r.Editar(It.IsAny<RolDTO>()), Times.Once);
        }

        [Fact]
        public void RolesEliminar_WithZeroId_ShouldReturnBadRequest()
        {
            var result = _service.RolesEliminar(0);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
        }

        [Fact]
        public void RolesEliminar_WithNegativeId_ShouldReturnBadRequest()
        {
            var result = _service.RolesEliminar(-1);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
        }

        [Fact]
        public void ListarUsuarios_ShouldReturnUsers()
        {
            var usuarios = new List<UsuariosDTO>
            {
                new UsuariosDTO { UsuarioId = 1, NombreUsuario = "user1" },
                new UsuariosDTO { UsuarioId = 2, NombreUsuario = "user2" }
            };

            _mockUserRepository.Setup(r => r.Listar()).Returns(usuarios);

            var result = _service.ListarUsuarios();

            Assert.True(result.Success);
        }

        [Fact]
        public void UsuariosInsertar_WithValidUser_ShouldCallRepository()
        {
            var usuario = new UsuariosDTO
            {
                UsuarioId = 0,
                NombreUsuario = "nuevousuario",
                Correo = "test@test.com"
            };

            _mockUserRepository.Setup(r => r.Insertar(It.IsAny<UsuariosDTO>()))
                .Returns(new RequestStatus { CodeStatus = 1, MessageStatus = "Insertado" });

            var result = _service.UsuariosInsertar(usuario);

            Assert.True(result.Success);
            _mockUserRepository.Verify(r => r.Insertar(It.IsAny<UsuariosDTO>()), Times.Once);
        }

        [Fact]
        public void UsuariosEliminar_WithZeroId_ShouldReturnBadRequest()
        {
            var result = _service.UsuariosEliminar(0);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
        }

        [Fact]
        public void UsuariosEliminar_WithValidId_ShouldCallRepository()
        {
            _mockUserRepository.Setup(r => r.Eliminar(It.IsAny<int>()))
                .Returns(new RequestStatus { CodeStatus = 1, MessageStatus = "Eliminado" });

            var result = _service.UsuariosEliminar(1);

            Assert.True(result.Success);
            _mockUserRepository.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Once);
        }
    }
}
