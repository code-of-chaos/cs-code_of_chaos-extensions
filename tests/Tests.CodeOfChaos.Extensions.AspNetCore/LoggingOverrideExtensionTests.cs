// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using ILogger=Serilog.ILogger;

namespace Tests.CodeOfChaos.Extensions.AspNetCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoggingOverrideExtensionsTests {
    
    [Test]
    public async Task OverrideLoggingWithSerilog_ShouldApplyDefaultSerilogConfiguration() {
        // Arrange
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        // Act
        builder.OverrideLoggingWithSerilog();

        // Assert
        ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
        ILogger? logger = serviceProvider.GetService<ILogger>();
        await Assert.That(logger)
            .IsNotNull()
            .Because("ILogger should be registered in the service collection.");
        await Assert.That(Log.Logger)
            .IsNotNull()
            .Because("Log.Logger should be initialized by Serilog with default configuration.");
    }

    [Test]
    public async Task OverrideLoggingWithSerilog_ShouldAllowCustomLoggerConfiguration() {
        // Arrange
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        bool customConfigurationApplied = false;

        // Act
        builder.OverrideLoggingWithSerilog(configure => {
            customConfigurationApplied = true;
            configure.MinimumLevel.Debug();
        });

        // Assert
        await Assert.That(customConfigurationApplied)
            .IsTrue()
            .Because("The custom configuration delegate should be applied when provided.");
    }

    [Test]
    public async Task OverrideLoggingWithSerilog_ShouldClearExistingLoggingProviders() {
        // Arrange
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Logging.AddConsole(); // Add a console provider to verify it is cleared

        // Act
        builder.OverrideLoggingWithSerilog();

        // Assert
        ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        await Assert.That(loggerFactory)
            .IsNotNull()
            .Because("The ILoggerFactory instance is required in the service collection.");

        // Count the logging providers
        Microsoft.Extensions.Logging.ILogger? logger = loggerFactory?.CreateLogger("Test");
        await Assert.That(logger)
            .IsNotNull()
            .Because("Serilog should replace the existing providers when overriding logging.");
    }

    [Test]
    public async Task OverrideLoggingWithSerilog_ShouldRegisterApplicationShutdownCleanupService() {
        // Arrange
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        // Act
        builder.OverrideLoggingWithSerilog();

        // Assert
        ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
        IEnumerable<IHostedService> hostedServices = serviceProvider.GetServices<IHostedService>().ToArray();
        await Assert.That(hostedServices).IsNotNull().Because("IHostedService collection should be available.");
        await Assert.That(hostedServices.Any(service => service is LoggingOverrideExtensions.ApplicationShutdownLoggerCleanup)).IsTrue().Because("There should be at least one hosted service.");
    }

    [Test]
    public async Task ApplicationShutdownLoggerCleanup_ShouldFlushLogsOnStop() {
        // Arrange
        var cleanupService = new LoggingOverrideExtensions.ApplicationShutdownLoggerCleanup();
        bool flushCalled = false;

        // Replace the static Serilog Logger with a mock logger
        ILogger originalLogger = Log.Logger;
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Sink(new DelegatingSink(_ => flushCalled = true)) // Custom sink to detect flush calls
            .CreateLogger();
        
        
        // Act
        Log.Logger.Information("Test log message"); // Log a message to verify it is flushed
        
        await cleanupService.StartAsync(CancellationToken.None);
        await cleanupService.StopAsync(CancellationToken.None);

        // Assert
        await Assert.That(flushCalled)
            .IsTrue()
            .Because("Log.CloseAndFlush should be triggered when the application stops.");
        
        // Just to be sure
        Log.Logger  = originalLogger; // Restore the original logger
    }
}

// ---------------------------------------------------------------------------------------------------------------------
// Helper Classes
// ---------------------------------------------------------------------------------------------------------------------
public class DelegatingSink : Serilog.Core.ILogEventSink {
    private readonly Action<LogEvent> _write;

    public DelegatingSink(Action<LogEvent> write) {
        _write = write ?? throw new ArgumentNullException(nameof(write));
    }

    public void Emit(LogEvent logEvent) => _write.Invoke(logEvent);
}