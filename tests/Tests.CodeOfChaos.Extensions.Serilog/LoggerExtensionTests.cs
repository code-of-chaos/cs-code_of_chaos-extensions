// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Serilog;
using Moq;
using Serilog;

namespace Tests.CodeOfChaos.Extensions.Serilog;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoggerExtensionsTests {
    private Mock<ILogger> _mockLogger = new();

    [Test]
    public async Task ThrowableError_ShouldLogErrorAndThrowException() {
        // Arrange
        const string messageTemplate = "Error occurred: {ErrorDetail}";
        object[] propertyValues = ["An error detail"];
        _mockLogger.Setup(logger => logger.Error(It.IsAny<Exception>(), messageTemplate, propertyValues));

        // Act 
        Exception exception = _mockLogger.Object.ThrowableError(messageTemplate, propertyValues);
        
        // Assert
        await Assert.That(exception).IsNotNull();
        await Assert.That(exception.Message).IsEqualTo(messageTemplate);
    }

    [Test]
    public async Task ThrowableError_TException_ShouldLogErrorAndThrowCustomException() {
        // Arrange
        const string messageTemplate = "Custom error occurred: {ErrorDetail}";
        object[] propertyValues = ["Custom exception property"];
        _mockLogger.Setup(logger => logger.Error(It.IsAny<InvalidOperationException>(), messageTemplate, propertyValues));

        // Act 
        Exception exception = _mockLogger.Object.ThrowableError<InvalidOperationException>(messageTemplate, propertyValues);
        
        // Assert
        await Assert.That(exception).IsNotNull()
            .And.IsTypeOf<InvalidOperationException>();
        await Assert.That(exception.Message).IsEqualTo(messageTemplate);
    }

    [Test]
    public async Task ThrowableFatal_ShouldLogFatalAndThrowException() {
        // Arrange
        string messageTemplate = "Fatal error occurred: {ErrorDetail}";
        object[] propertyValues = { "Fatal error detail" };
        _mockLogger.Setup(logger => logger.Fatal(It.IsAny<Exception>(), messageTemplate, propertyValues));

        // Act 
        Exception exception = _mockLogger.Object.ThrowableFatal(messageTemplate, propertyValues);
        
        // Assert
        await Assert.That(exception).IsNotNull();
        await Assert.That(exception.Message).IsEqualTo(messageTemplate);
    }

    [Test]
    public async Task ThrowableFatal_TException_ShouldLogFatalAndThrowCustomException() {
        // Arrange
        string messageTemplate = "Fatal custom error occurred: {ErrorDetail}";
        object[] propertyValues = { "Custom fatal detail" };
        _mockLogger.Setup(logger => logger.Fatal(It.IsAny<InvalidOperationException>(), messageTemplate, propertyValues));

        // Act 
        Exception exception = _mockLogger.Object.ThrowableError<InvalidOperationException>(messageTemplate, propertyValues);
        
        // Assert
        await Assert.That(exception).IsNotNull()
            .And.IsTypeOf<InvalidOperationException>();
        await Assert.That(exception.Message).IsEqualTo(messageTemplate);
    }

    [Test]
    public async Task ThrowableFatal_WithExistingException_ShouldLogFatalAndUseProvidedException() {
        // Arrange
        string messageTemplate = "An existing exception occurred: {ErrorDetail}";
        var providedException = new InvalidOperationException("Existing exception");
        object[] propertyValues = { "Extra detail" };
        _mockLogger.Setup(logger => logger.Fatal(providedException, messageTemplate, propertyValues));

        // Act
        var exception = _mockLogger.Object.ThrowableFatal(providedException, messageTemplate, propertyValues);

        // Assert
        await Assert.That(exception).IsNotNull();
        await Assert.That(exception.Message).IsEqualTo(providedException.Message);
        await Assert.That(exception).IsEqualTo(providedException);
    }

    [Test]
    public async Task ExitFatal_ShouldLogFatalAndThrowExitException() {
        // Arrange
        int exitCode = 1;
        string messageTemplate = "Critical failure - application exiting: {Reason}";
        object[] propertyValues = { "Unexpected failure" };

        _mockLogger.Setup(logger => logger.Fatal(messageTemplate, propertyValues));

        // Act
        var exception = Assert.Throws<ExitApplicationException>(() => 
            _mockLogger.Object.ExitFatal(exitCode, messageTemplate, propertyValues)
        );
        
        // Assert
        await Assert.That(exception).IsNotNull()
            .And.IsTypeOf<ExitApplicationException>();
        await Assert.That(exception.Message).IsEqualTo(messageTemplate);
    }
}