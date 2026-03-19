using CitasMedicas.BusinessLogic;

namespace CitasMedicas.Tests
{
    public class ServiceResultTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithErrorType()
        {
            var result = new ServiceResult();

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Error, result.Type);
        }

        [Fact]
        public void Ok_WithData_ShouldSetSuccessTrue()
        {
            var result = new ServiceResult().Ok("Test data");

            Assert.True(result.Success);
            Assert.Equal(ServiceResultType.Success, result.Type);
            Assert.Equal("Test data", result.Message);
        }

        [Fact]
        public void Ok_WithMessageAndData_ShouldSetCorrectValues()
        {
            var data = new { Id = 1, Name = "Test" };
            var result = new ServiceResult().Ok("Custom message", data);

            Assert.True(result.Success);
            Assert.Equal(ServiceResultType.Success, result.Type);
            Assert.Equal("Custom message", result.Message);
            Assert.Equal(data, result.Data);
        }

        [Fact]
        public void Error_WithoutMessage_ShouldUseDefaultMessage()
        {
            var result = new ServiceResult().Error();

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Error, result.Type);
            Assert.Contains("error", result.Message.ToLower());
        }

        [Fact]
        public void Error_WithMessage_ShouldSetCustomMessage()
        {
            var result = new ServiceResult().Error("Custom error message");

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Error, result.Type);
            Assert.Equal("Custom error message", result.Message);
        }

        [Fact]
        public void BadRequest_ShouldSetBadRequestType()
        {
            var result = new ServiceResult().BadRequest("Invalid input");

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.BadRequest, result.Type);
            Assert.Equal("Invalid input", result.Message);
        }

        [Fact]
        public void NotFound_ShouldSetNotFoundType()
        {
            var result = new ServiceResult().NotFound("Resource not found");

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.NotFound, result.Type);
            Assert.Equal("Resource not found", result.Message);
        }

        [Fact]
        public void Unauthorized_ShouldSetUnauthorizedType()
        {
            var result = new ServiceResult().Unauthorized("Access denied");

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Unauthorized, result.Type);
            Assert.Equal("Access denied", result.Message);
        }

        [Fact]
        public void Conflict_ShouldSetConflictType()
        {
            var result = new ServiceResult().Conflict("Duplicate entry");

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Conflict, result.Type);
            Assert.Equal("Duplicate entry", result.Message);
        }

        [Fact]
        public void Code_ShouldReturnCorrectStatusCode()
        {
            var successResult = new ServiceResult().Ok();
            var badRequestResult = new ServiceResult().BadRequest("test");
            var notFoundResult = new ServiceResult().NotFound("test");
            var errorResult = new ServiceResult().Error();

            Assert.Equal(200, successResult.Code);
            Assert.Equal(400, badRequestResult.Code);
            Assert.Equal(404, notFoundResult.Code);
            Assert.Equal(500, errorResult.Code);
        }

        [Fact]
        public void Info_ShouldSetInfoType()
        {
            var result = new ServiceResult().Info("Information message");

            Assert.True(result.Success);
            Assert.Equal(ServiceResultType.Info, result.Type);
            Assert.Equal("Information message", result.Message);
        }

        [Fact]
        public void Warning_ShouldSetWarningType()
        {
            var result = new ServiceResult().Warning("Warning message");

            Assert.True(result.Success);
            Assert.Equal(ServiceResultType.Warning, result.Type);
            Assert.Equal("Warning message", result.Message);
        }

        [Fact]
        public void Forbidden_ShouldSetForbiddenType()
        {
            var result = new ServiceResult().Forbidden("Access forbidden");

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Forbidden, result.Type);
            Assert.Equal("Access forbidden", result.Message);
        }

        [Fact]
        public void Disabled_ShouldSetDisabledType()
        {
            var result = new ServiceResult().Disabled("Resource disabled");

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Disabled, result.Type);
            Assert.Equal("Resource disabled", result.Message);
        }

        [Fact]
        public void SetMessage_ShouldSetMessageAndType()
        {
            var result = new ServiceResult();
            result.SetMessage("Custom message", ServiceResultType.NotAcceptable);

            Assert.Equal("Custom message", result.Message);
            Assert.Equal(ServiceResultType.NotAcceptable, result.Type);
        }
    }
}
