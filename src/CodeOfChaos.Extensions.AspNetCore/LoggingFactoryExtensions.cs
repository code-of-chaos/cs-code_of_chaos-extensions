// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using Serilog;

namespace CodeOfChaos.Extensions.AspNetCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Provides extension methods for creating and configuring <see cref="ILoggerFactory" /> instances with Serilog.
/// </summary>
public static class LoggingFactoryExtensions {
    /// <summary>
    ///     Creates an <see cref="ILoggerFactory" /> configured to use Serilog as the logging provider.
    /// </summary>
    /// <returns>An instance of <see cref="ILoggerFactory" /> configured with Serilog.</returns>
    public static ILoggerFactory CreateWithSerilog()
        => LoggerFactory.Create(builder => builder.AddSerilog(Log.Logger));
    /// <summary>
    ///     Creates a new instance of <see cref="ILoggerFactory" /> configured with Serilog.
    /// </summary>
    /// <returns>A configured <see cref="ILoggerFactory" /> instance.</returns>
    public static ILoggerFactory CreateWithSerilog(string sectionName)
        => LoggerFactory.Create(builder => builder.AddSerilog(Log.Logger.ForSectionProperty(sectionName)));
}
