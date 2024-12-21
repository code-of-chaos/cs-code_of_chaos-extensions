// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace CodeOfChaos.Extensions.AspNetCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LoggingOverrideExtensions {

    private static readonly LoggingLevelSwitch LoggingLevelSwitch = new();

    /// <summary>
    ///     Configures Serilog as the logging provider for the web application builder.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder" /> to configure.</param>
    /// <param name="configure">Optional delegate to override the default <see cref="LoggerConfiguration" />.</param>
    /// <returns>The modified <see cref="WebApplicationBuilder" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the builder is null.</exception>
    public static WebApplicationBuilder OverrideLoggingWithSerilog(
        this WebApplicationBuilder builder,
        Action<LoggerConfiguration>? configure = null
    ) {
        LoggerConfiguration loggerConfig = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LoggingLevelSwitch);

        configure?.Invoke(loggerConfig);

        Log.Logger = loggerConfig.CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger);
        builder.Services.AddSingleton(Log.Logger);
        builder.Services.AddSingleton<IHostedService, ApplicationShutdownLoggerCleanup>();// Ensure cleanup

        return builder;
    }
    
    public class ApplicationShutdownLoggerCleanup : IHostedService {
        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task StopAsync(CancellationToken cancellationToken) {
            await Log.CloseAndFlushAsync();
        }
    }
}


