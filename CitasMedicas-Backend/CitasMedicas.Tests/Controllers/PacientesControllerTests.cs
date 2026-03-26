using CitasMedicas.API.Controllers.Clinica;
using CitasMedicas.BusinessLogic;
using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Clinica;
using CitasMedicas.Models.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace CitasMedicas.Tests.Controllers
{
    public class PacientesControllerTests
    {
        private readonly Mock<PacientesRepository> _mockRepo;
        private readonly ClinicaService _service;
        private readonly PacientesController _controller;

        public PacientesControllerTests()
        {
            _mockRepo = new Mock<PacientesRepository>();
            _service = new ClinicaService(null!, null!, null!, _mockRepo.Object);
            _controller = new PacientesController(_service);
        }

        private void SetUserClaims(int usuarioId, string rol)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                new(ClaimTypes.Role, rol)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        #region Editar - Restricción PACIENTE

        [Fact]
        public void Editar_Paciente_PuedeEditarSusPropiosDatos()
        {
            // Arrange
            SetUserClaims(usuarioId: 9, rol: "PACIENTE");
            var paciente = new PacientesDTO
            {
                PacienteId = 1,
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez"
            };
            var response = new RequestStatus { CodeStatus = 1, MessageStatus = "Editado" };
            _mockRepo.Setup(r => r.PacienteEditar(paciente)).Returns(response);

            // Act
            var result = _controller.Editar(paciente) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        [Fact]
        public void Editar_Paciente_NoPuedeEditarDatosDeOtro()
        {
            // Arrange
            SetUserClaims(usuarioId: 9, rol: "PACIENTE");
            var paciente = new PacientesDTO
            {
                PacienteId = 2,
                UsuarioId = 10,
                Nombres = "Otro",
                Apellidos = "Paciente"
            };

            // Act
            var result = _controller.Editar(paciente) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(203); // Forbidden
        }

        [Fact]
        public void Editar_Admin_PuedeEditarCualquierPaciente()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");
            var paciente = new PacientesDTO
            {
                PacienteId = 1,
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez"
            };
            var response = new RequestStatus { CodeStatus = 1, MessageStatus = "Editado" };
            _mockRepo.Setup(r => r.PacienteEditar(paciente)).Returns(response);

            // Act
            var result = _controller.Editar(paciente) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        [Fact]
        public void Editar_Recepcionista_PuedeEditarCualquierPaciente()
        {
            // Arrange
            SetUserClaims(usuarioId: 5, rol: "RECEP");
            var paciente = new PacientesDTO
            {
                PacienteId = 1,
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez"
            };
            var response = new RequestStatus { CodeStatus = 1, MessageStatus = "Editado" };
            _mockRepo.Setup(r => r.PacienteEditar(paciente)).Returns(response);

            // Act
            var result = _controller.Editar(paciente) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        [Fact]
        public void Editar_RetornaBadRequest_CuandoDatosInvalidos()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");
            var paciente = new PacientesDTO
            {
                PacienteId = 0, // ID inválido
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez"
            };

            // Act
            var result = _controller.Editar(paciente) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(400);
        }

        #endregion

        #region ObtenerPorId - Restricción PACIENTE

        [Fact]
        public void ObtenerPorId_Paciente_PuedeVerSusPropiosDatos()
        {
            // Arrange
            SetUserClaims(usuarioId: 9, rol: "PACIENTE");
            var paciente = new PacientesDTO { PacienteId = 1, UsuarioId = 9, Nombres = "Juan" };
            _mockRepo.Setup(r => r.ObtenerPorId(1)).Returns(paciente);

            // Act
            var result = _controller.ObtenerPorId(1) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        [Fact]
        public void ObtenerPorId_Paciente_NoPuedeVerDatosDeOtro()
        {
            // Arrange
            SetUserClaims(usuarioId: 9, rol: "PACIENTE");
            var paciente = new PacientesDTO { PacienteId = 2, UsuarioId = 10, Nombres = "Otro" };
            _mockRepo.Setup(r => r.ObtenerPorId(2)).Returns(paciente);

            // Act
            var result = _controller.ObtenerPorId(2) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(203); // Forbidden
        }

        [Fact]
        public void ObtenerPorId_Admin_PuedeVerCualquierPaciente()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");
            var paciente = new PacientesDTO { PacienteId = 2, UsuarioId = 10, Nombres = "Otro" };
            _mockRepo.Setup(r => r.ObtenerPorId(2)).Returns(paciente);

            // Act
            var result = _controller.ObtenerPorId(2) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        [Fact]
        public void ObtenerPorId_RetornaNotFound_CuandoNoExiste()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");
            _mockRepo.Setup(r => r.ObtenerPorId(999)).Returns((PacientesDTO)null!);

            // Act
            var result = _controller.ObtenerPorId(999) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(404);
        }

        [Fact]
        public void ObtenerPorId_RetornaBadRequest_CuandoIdInvalido()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");

            // Act
            var result = _controller.ObtenerPorId(0) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(400);
        }

        #endregion

        #region Listar

        [Fact]
        public void Listar_RetornaOk_ConPacientes()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");
            var pacientes = new List<PacientesDTO>
            {
                new() { PacienteId = 1, Nombres = "Juan" },
                new() { PacienteId = 2, Nombres = "María" }
            };
            _mockRepo.Setup(r => r.Listar()).Returns(pacientes);

            // Act
            var result = _controller.Listar() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        [Fact]
        public void Listar_RetornaOk_SinPacientes()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");
            _mockRepo.Setup(r => r.Listar()).Returns(new List<PacientesDTO>());

            // Act
            var result = _controller.Listar() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        #endregion

        #region Insertar

        [Fact]
        public void Insertar_RetornaOk_CuandoDatosValidos()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");
            var paciente = new PacientesDTO
            {
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez",
                NumeroIdentidad = "0801199900001"
            };
            var response = new RequestStatus { CodeStatus = 1, MessageStatus = "Insertado" };
            _mockRepo.Setup(r => r.PacienteInsertar(paciente)).Returns(response);

            // Act
            var result = _controller.Insertar(paciente) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        [Fact]
        public void Insertar_RetornaBadRequest_CuandoDatosInvalidos()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");
            var paciente = new PacientesDTO
            {
                UsuarioId = 0,
                Nombres = "",
                Apellidos = "",
                NumeroIdentidad = ""
            };

            // Act
            var result = _controller.Insertar(paciente) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(400);
        }

        #endregion

        #region Eliminar

        [Fact]
        public void Eliminar_RetornaOk_CuandoIdValido()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");
            var response = new RequestStatus { CodeStatus = 1, MessageStatus = "Eliminado" };
            _mockRepo.Setup(r => r.PacienteEliminar(1)).Returns(response);

            // Act
            var result = _controller.Eliminar(1) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        [Fact]
        public void Eliminar_RetornaBadRequest_CuandoIdInvalido()
        {
            // Arrange
            SetUserClaims(usuarioId: 1, rol: "ADMIN");

            // Act
            var result = _controller.Eliminar(0) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(400);
        }

        #endregion
    }
}
