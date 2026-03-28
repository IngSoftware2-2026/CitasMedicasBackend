using CitasMedicas.BusinessLogic;
using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Clinica;
using CitasMedicas.Models.Models;
using FluentAssertions;
using Moq;

namespace CitasMedicas.Tests.Services
{
    public class ClinicaServiceTests
    {
        private readonly Mock<PacientesRepository> _mockRepo;
        private readonly ClinicaService _service;

        public ClinicaServiceTests()
        {
            _mockRepo = new Mock<PacientesRepository>();
            _service = new ClinicaService(null!, null!, null!, _mockRepo.Object);
        }

        #region ListarPacientes

        [Fact]
        public void ListarPacientes_RetornaOk_CuandoHayPacientes()
        {
            // Arrange
            var pacientes = new List<PacientesDTO>
            {
                new() { PacienteId = 1, Nombres = "Juan", Apellidos = "Pérez", UsuarioId = 9 },
                new() { PacienteId = 2, Nombres = "María", Apellidos = "López", UsuarioId = 10 }
            };
            _mockRepo.Setup(r => r.Listar()).Returns(pacientes);

            // Act
            var result = _service.ListarPacientes();

            // Assert
            result.Success.Should().BeTrue();
            result.Code.Should().Be(200);
            ((IEnumerable<PacientesDTO>)result.Data).Should().HaveCount(2);
        }

        [Fact]
        public void ListarPacientes_RetornaOk_CuandoListaVacia()
        {
            // Arrange
            _mockRepo.Setup(r => r.Listar()).Returns(new List<PacientesDTO>());

            // Act
            var result = _service.ListarPacientes();

            // Assert
            result.Success.Should().BeTrue();
            result.Code.Should().Be(200);
        }

        [Fact]
        public void ListarPacientes_RetornaError_CuandoExcepcion()
        {
            // Arrange
            _mockRepo.Setup(r => r.Listar()).Throws(new Exception("DB error"));

            // Act
            var result = _service.ListarPacientes();

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(500);
            result.Message.Should().Contain("Error inesperado");
        }

        #endregion

        #region ObtenerPacientePorId

        [Fact]
        public void ObtenerPacientePorId_RetornaOk_CuandoExiste()
        {
            // Arrange
            var paciente = new PacientesDTO { PacienteId = 1, Nombres = "Juan", UsuarioId = 9 };
            _mockRepo.Setup(r => r.ObtenerPorId(1)).Returns(paciente);

            // Act
            var result = _service.ObtenerPacientePorId(1);

            // Assert
            result.Success.Should().BeTrue();
            result.Code.Should().Be(200);
            ((PacientesDTO)result.Data).PacienteId.Should().Be(1);
        }

        [Fact]
        public void ObtenerPacientePorId_RetornaNotFound_CuandoNoExiste()
        {
            // Arrange
            _mockRepo.Setup(r => r.ObtenerPorId(999)).Returns((PacientesDTO)null!);

            // Act
            var result = _service.ObtenerPacientePorId(999);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(404);
            result.Message.Should().Contain("no encontrado");
        }

        [Fact]
        public void ObtenerPacientePorId_RetornaBadRequest_CuandoIdCero()
        {
            // Act
            var result = _service.ObtenerPacientePorId(0);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("mayor que cero");
        }

        [Fact]
        public void ObtenerPacientePorId_RetornaBadRequest_CuandoIdNegativo()
        {
            // Act
            var result = _service.ObtenerPacientePorId(-5);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
        }

        [Fact]
        public void ObtenerPacientePorId_RetornaError_CuandoExcepcion()
        {
            // Arrange
            _mockRepo.Setup(r => r.ObtenerPorId(1)).Throws(new Exception("DB error"));

            // Act
            var result = _service.ObtenerPacientePorId(1);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(500);
            result.Message.Should().Contain("Error inesperado");
        }

        #endregion

        #region PacientesInsertar

        [Fact]
        public void PacientesInsertar_RetornaOk_CuandoDatosValidos()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez",
                NumeroIdentidad = "0801199900001"
            };
            var response = new RequestStatus { CodeStatus = 1, MessageStatus = "Paciente insertado correctamente" };
            _mockRepo.Setup(r => r.PacienteInsertar(paciente)).Returns(response);

            // Act
            var result = _service.PacientesInsertar(paciente);

            // Assert
            result.Success.Should().BeTrue();
            result.Code.Should().Be(200);
        }

        [Fact]
        public void PacientesInsertar_RetornaBadRequest_CuandoPacienteNull()
        {
            // Act
            var result = _service.PacientesInsertar(null!);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("requeridos");
        }

        [Fact]
        public void PacientesInsertar_RetornaBadRequest_CuandoNombreVacio()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                UsuarioId = 9,
                Nombres = "",
                Apellidos = "Pérez",
                NumeroIdentidad = "0801199900001"
            };

            // Act
            var result = _service.PacientesInsertar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("nombre");
        }

        [Fact]
        public void PacientesInsertar_RetornaBadRequest_CuandoApellidosVacios()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "",
                NumeroIdentidad = "0801199900001"
            };

            // Act
            var result = _service.PacientesInsertar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("apellidos");
        }

        [Fact]
        public void PacientesInsertar_RetornaBadRequest_CuandoUsuarioIdCero()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                UsuarioId = 0,
                Nombres = "Juan",
                Apellidos = "Pérez",
                NumeroIdentidad = "0801199900001"
            };

            // Act
            var result = _service.PacientesInsertar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("UsuarioId");
        }

        [Fact]
        public void PacientesInsertar_RetornaBadRequest_CuandoUsuarioIdNull()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                UsuarioId = null,
                Nombres = "Juan",
                Apellidos = "Pérez",
                NumeroIdentidad = "0801199900001"
            };

            // Act
            var result = _service.PacientesInsertar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("UsuarioId");
        }

        [Fact]
        public void PacientesInsertar_RetornaBadRequest_CuandoIdentidadVacia()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez",
                NumeroIdentidad = ""
            };

            // Act
            var result = _service.PacientesInsertar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("identidad");
        }

        [Fact]
        public void PacientesInsertar_RetornaConflict_CuandoSPRetornaMenosUno()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez",
                NumeroIdentidad = "0801199900001"
            };
            var response = new RequestStatus { CodeStatus = -1, MessageStatus = "El paciente ya existe" };
            _mockRepo.Setup(r => r.PacienteInsertar(paciente)).Returns(response);

            // Act
            var result = _service.PacientesInsertar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(409);
        }

        [Fact]
        public void PacientesInsertar_RetornaError_CuandoExcepcion()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez",
                NumeroIdentidad = "0801199900001"
            };
            _mockRepo.Setup(r => r.PacienteInsertar(paciente)).Throws(new Exception("DB error"));

            // Act
            var result = _service.PacientesInsertar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(500);
            result.Message.Should().Contain("Error inesperado");
        }

        [Fact]
        public void PacientesInsertar_RetornaError_CuandoSPRetornaNull()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez",
                NumeroIdentidad = "0801199900001"
            };
            _mockRepo.Setup(r => r.PacienteInsertar(paciente)).Returns((RequestStatus)null!);

            // Act
            var result = _service.PacientesInsertar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(500);
        }

        #endregion

        #region PacientesEditar

        [Fact]
        public void PacientesEditar_RetornaOk_CuandoDatosValidos()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                PacienteId = 1,
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez Actualizado"
            };
            var response = new RequestStatus { CodeStatus = 1, MessageStatus = "Paciente editado correctamente" };
            _mockRepo.Setup(r => r.PacienteEditar(paciente)).Returns(response);

            // Act
            var result = _service.PacientesEditar(paciente);

            // Assert
            result.Success.Should().BeTrue();
            result.Code.Should().Be(200);
        }

        [Fact]
        public void PacientesEditar_RetornaBadRequest_CuandoPacienteNull()
        {
            // Act
            var result = _service.PacientesEditar(null!);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
        }

        [Fact]
        public void PacientesEditar_RetornaBadRequest_CuandoPacienteIdCero()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                PacienteId = 0,
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez"
            };

            // Act
            var result = _service.PacientesEditar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("ID del paciente");
        }

        [Fact]
        public void PacientesEditar_RetornaBadRequest_CuandoUsuarioIdCero()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                PacienteId = 1,
                UsuarioId = 0,
                Nombres = "Juan",
                Apellidos = "Pérez"
            };

            // Act
            var result = _service.PacientesEditar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("UsuarioId");
        }

        [Fact]
        public void PacientesEditar_RetornaBadRequest_CuandoNombreVacio()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                PacienteId = 1,
                UsuarioId = 9,
                Nombres = "",
                Apellidos = "Pérez"
            };

            // Act
            var result = _service.PacientesEditar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("nombre");
        }

        [Fact]
        public void PacientesEditar_RetornaBadRequest_CuandoApellidosVacios()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                PacienteId = 1,
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = ""
            };

            // Act
            var result = _service.PacientesEditar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
            result.Message.Should().Contain("apellidos");
        }

        [Fact]
        public void PacientesEditar_RetornaError_CuandoExcepcion()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                PacienteId = 1,
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez"
            };
            _mockRepo.Setup(r => r.PacienteEditar(paciente)).Throws(new Exception("DB error"));

            // Act
            var result = _service.PacientesEditar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(500);
            result.Message.Should().Contain("Error inesperado");
        }

        [Fact]
        public void PacientesEditar_RetornaConflict_CuandoSPRetornaMenosUno()
        {
            // Arrange
            var paciente = new PacientesDTO
            {
                PacienteId = 1,
                UsuarioId = 9,
                Nombres = "Juan",
                Apellidos = "Pérez"
            };
            var response = new RequestStatus { CodeStatus = -1, MessageStatus = "Conflicto al editar" };
            _mockRepo.Setup(r => r.PacienteEditar(paciente)).Returns(response);

            // Act
            var result = _service.PacientesEditar(paciente);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(409);
        }

        #endregion

        #region PacientesEliminar

        [Fact]
        public void PacientesEliminar_RetornaOk_CuandoIdValido()
        {
            // Arrange
            var response = new RequestStatus { CodeStatus = 1, MessageStatus = "Paciente eliminado correctamente" };
            _mockRepo.Setup(r => r.PacienteEliminar(1)).Returns(response);

            // Act
            var result = _service.PacientesEliminar(1);

            // Assert
            result.Success.Should().BeTrue();
            result.Code.Should().Be(200);
        }

        [Fact]
        public void PacientesEliminar_RetornaBadRequest_CuandoIdCero()
        {
            // Act
            var result = _service.PacientesEliminar(0);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
        }

        [Fact]
        public void PacientesEliminar_RetornaBadRequest_CuandoIdNegativo()
        {
            // Act
            var result = _service.PacientesEliminar(-1);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(400);
        }

        [Fact]
        public void PacientesEliminar_RetornaError_CuandoSPRetornaCero()
        {
            // Arrange
            var response = new RequestStatus { CodeStatus = 0, MessageStatus = "Error al eliminar" };
            _mockRepo.Setup(r => r.PacienteEliminar(1)).Returns(response);

            // Act
            var result = _service.PacientesEliminar(1);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(500);
        }

        [Fact]
        public void PacientesEliminar_RetornaError_CuandoExcepcion()
        {
            // Arrange
            _mockRepo.Setup(r => r.PacienteEliminar(1)).Throws(new Exception("DB error"));

            // Act
            var result = _service.PacientesEliminar(1);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(500);
            result.Message.Should().Contain("Error inesperado");
        }

        [Fact]
        public void PacientesEliminar_RetornaConflict_CuandoSPRetornaMenosUno()
        {
            // Arrange
            var response = new RequestStatus { CodeStatus = -1, MessageStatus = "No se puede eliminar" };
            _mockRepo.Setup(r => r.PacienteEliminar(1)).Returns(response);

            // Act
            var result = _service.PacientesEliminar(1);

            // Assert
            result.Success.Should().BeFalse();
            result.Code.Should().Be(409);
        }

        #endregion
    }
}
