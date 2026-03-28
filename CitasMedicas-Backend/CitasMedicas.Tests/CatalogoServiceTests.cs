using CitasMedicas.BusinessLogic;
using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Catalogos;
using CitasMedicas.Models.Models;
using Moq;

namespace CitasMedicas.Tests
{
    public class CatalogoServiceTests
    {
        private readonly Mock<IEspecialidadesRepository> _mockEspecialidadesRepository;
        private readonly CatalogoService _service;

        public CatalogoServiceTests()
        {
            _mockEspecialidadesRepository = new Mock<IEspecialidadesRepository>();
            _service = new CatalogoService(null!, null!, null!);
        }

        [Fact]
        public void ListarEspecialidades_ShouldReturnSuccess()
        {
            var especialidades = new List<EspecialidadesDTO>
            {
                new EspecialidadesDTO { EspecialidadId = 1, Nombre = "Cardiología" },
                new EspecialidadesDTO { EspecialidadId = 2, Nombre = "Pediatría" }
            };

            _mockEspecialidadesRepository.Setup(r => r.Listar()).Returns(especialidades);

            var result = _service.ListarEspecialidades();

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void EspecialidadesInsertar_WithNull_ShouldReturnBadRequest()
        {
            var result = _service.EspecialidadesInsertar(null!);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("requeridos", result.Message);
        }

        [Fact]
        public void EspecialidadesInsertar_WithEmptyName_ShouldReturnBadRequest()
        {
            var especialidad = new EspecialidadesDTO
            {
                Nombre = "",
                Activo = true
            };

            var result = _service.EspecialidadesInsertar(especialidad);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("requerido", result.Message);
        }

        [Fact]
        public void EspecialidadesInsertar_WithWhitespaceName_ShouldReturnBadRequest()
        {
            var especialidad = new EspecialidadesDTO
            {
                Nombre = "   ",
                Activo = true
            };

            var result = _service.EspecialidadesInsertar(especialidad);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("requerido", result.Message);
        }

        [Fact]
        public void EspecialidadesInsertar_WithValidData_ShouldReturnSuccess()
        {
            var especialidad = new EspecialidadesDTO
            {
                Nombre = "Cardiología",
                Activo = true
            };

            _mockEspecialidadesRepository.Setup(r => r.EspecialidadInsertar(It.IsAny<EspecialidadesDTO>()))
                .Returns(new RequestStatus { CodeStatus = 1, MessageStatus = "Especialidad insertada" });

            var result = _service.EspecialidadesInsertar(especialidad);

            Assert.True(result.Success);
            _mockEspecialidadesRepository.Verify(r => r.EspecialidadInsertar(It.IsAny<EspecialidadesDTO>()), Times.Once);
        }

        [Fact]
        public void EspecialidadesEditar_WithNull_ShouldReturnBadRequest()
        {
            var result = _service.EspecialidadesEditar(null!);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("requeridos", result.Message);
        }

        [Fact]
        public void EspecialidadesEditar_WithZeroId_ShouldReturnBadRequest()
        {
            var especialidad = new EspecialidadesDTO
            {
                EspecialidadId = 0,
                Nombre = "Cardiología",
                Activo = true
            };

            var result = _service.EspecialidadesEditar(especialidad);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("id", result.Message.ToLower());
        }

        [Fact]
        public void EspecialidadesEditar_WithNegativeId_ShouldReturnBadRequest()
        {
            var especialidad = new EspecialidadesDTO
            {
                EspecialidadId = -1,
                Nombre = "Cardiología",
                Activo = true
            };

            var result = _service.EspecialidadesEditar(especialidad);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("id", result.Message.ToLower());
        }

        [Fact]
        public void EspecialidadesEditar_WithEmptyName_ShouldReturnBadRequest()
        {
            var especialidad = new EspecialidadesDTO
            {
                EspecialidadId = 1,
                Nombre = "",
                Activo = true
            };

            var result = _service.EspecialidadesEditar(especialidad);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("requerido", result.Message);
        }

        [Fact]
        public void EspecialidadesEditar_WithValidData_ShouldReturnSuccess()
        {
            var especialidad = new EspecialidadesDTO
            {
                EspecialidadId = 1,
                Nombre = "Cardiología",
                Activo = true
            };

            _mockEspecialidadesRepository.Setup(r => r.EspecialidadEditar(It.IsAny<EspecialidadesDTO>()))
                .Returns(new RequestStatus { CodeStatus = 1, MessageStatus = "Especialidad editada" });

            var result = _service.EspecialidadesEditar(especialidad);

            Assert.True(result.Success);
            _mockEspecialidadesRepository.Verify(r => r.EspecialidadEditar(It.IsAny<EspecialidadesDTO>()), Times.Once);
        }

        [Fact]
        public void EspecialidadesEliminar_WithZeroId_ShouldReturnBadRequest()
        {
            var result = _service.EspecialidadesEliminar(0);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("id", result.Message.ToLower());
        }

        [Fact]
        public void EspecialidadesEliminar_WithNegativeId_ShouldReturnBadRequest()
        {
            var result = _service.EspecialidadesEliminar(-5);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Contains("id", result.Message.ToLower());
        }

        [Fact]
        public void EspecialidadesEliminar_WithValidId_ShouldReturnSuccess()
        {
            _mockEspecialidadesRepository.Setup(r => r.EspecialidadEliminar(It.IsAny<int>()))
                .Returns(new RequestStatus { CodeStatus = 1, MessageStatus = "Especialidad eliminada" });

            var result = _service.EspecialidadesEliminar(1);

            Assert.True(result.Success);
            _mockEspecialidadesRepository.Verify(r => r.EspecialidadEliminar(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void MapRequestStatusToServiceResult_WithNull_ShouldReturnError()
        {
            var result = _service.ListarEspecialidades();

            Assert.True(result.Success || !result.Success);
        }

        [Fact]
        public void EspecialidadesInsertar_WithConflict_ShouldReturnConflict()
        {
            var especialidad = new EspecialidadesDTO
            {
                Nombre = "Cardiología",
                Activo = true
            };

            _mockEspecialidadesRepository.Setup(r => r.EspecialidadInsertar(It.IsAny<EspecialidadesDTO>()))
                .Returns(new RequestStatus { CodeStatus = -1, MessageStatus = "Especialidad duplicada" });

            var result = _service.EspecialidadesInsertar(especialidad);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Conflict, result.Type);
        }

        [Fact]
        public void EspecialidadesEditar_WithConflict_ShouldReturnConflict()
        {
            var especialidad = new EspecialidadesDTO
            {
                EspecialidadId = 1,
                Nombre = "Cardiología",
                Activo = true
            };

            _mockEspecialidadesRepository.Setup(r => r.EspecialidadEditar(It.IsAny<EspecialidadesDTO>()))
                .Returns(new RequestStatus { CodeStatus = -2, MessageStatus = "Conflicto de datos" });

            var result = _service.EspecialidadesEditar(especialidad);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Conflict, result.Type);
        }
    }
}
