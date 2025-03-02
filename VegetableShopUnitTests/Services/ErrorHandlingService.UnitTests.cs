using Microsoft.Extensions.Logging;
using Moq;
using VegetableShop.Models.Error_Handling;
using VegetableShop.Services;

namespace VegetableShop.UnitTests.Services
{
    public class ErrorHandlerServiceUnitTests
    {
        private readonly ErrorHandlerService _errorHandlerService;
        private readonly Mock<ILogger<ErrorHandlerService>> _mockLogger;

        public ErrorHandlerServiceUnitTests()
        {
            _mockLogger = new Mock<ILogger<ErrorHandlerService>>();
            _errorHandlerService = new ErrorHandlerService(_mockLogger.Object);
        }

        [Fact]
        public void HandleException_LogsFileNotFoundException()
        {
            // Arrange
            var exception = new FileNotFoundException("Test file not found");

            // Act
            _errorHandlerService.HandleException(exception);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("File Error: Test file not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ),
                Times.Once);
        }

        [Fact]
        public void HandleException_LogsFormatException()
        {
            // Arrange
            var exception = new FormatException("Test format issue");

            // Act
            _errorHandlerService.HandleException(exception);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Invalid Data Format: Test format issue")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ),
                Times.Once);
        }

        [Fact]
        public void HandleException_LogsArgumentException()
        {
            // Arrange
            var exception = new ArgumentException("Test invalid argument");

            // Act
            _errorHandlerService.HandleException(exception);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Invalid Argument Format: Test invalid argument")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ),
                Times.Once);
        }

        [Fact]
        public void HandleException_LogsInvalidOfferException()
        {
            // Arrange
            var exception = new InvalidOfferException("Test invalid offer");

            // Act
            _errorHandlerService.HandleException(exception);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Invalid Offer: Test invalid offer")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ),
                Times.Once);
        }

        [Fact]
        public void HandleException_LogsGenericException()
        {
            // Arrange
            var exception = new Exception("Test unexpected error");

            // Act
            _errorHandlerService.HandleException(exception);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("An unexpected error occurred: Test unexpected error")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ),
                Times.Once);
        }
    }
}
