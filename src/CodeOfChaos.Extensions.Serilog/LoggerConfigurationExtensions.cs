// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Serilog;
using CodeOfChaos.Extensions.Serilog.Enrichers;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

// ReSharper disable once CheckNamespace
namespace Serilog;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LoggerConfigurationExtensions {
    public static LoggerConfiguration WriteToAsyncConsole(
        this LoggerConfiguration loggerConfiguration,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
        string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        IFormatProvider? formatProvider = null,
        LoggingLevelSwitch? levelSwitch = null,
        LogEventLevel? standardErrorFromLevel = null,
        ConsoleTheme? theme = null,
        bool applyThemeToRedirectedOutput = false,
        object? syncRoot = null
        ) {
        loggerConfiguration.WriteTo.Async(lsc => lsc.Console(
            restrictedToMinimumLevel,
            outputTemplate,
            formatProvider,
            levelSwitch,
            standardErrorFromLevel,
            theme,
            applyThemeToRedirectedOutput,
            syncRoot
            ));
        return loggerConfiguration;
    }

    public static LoggerConfiguration WithPaddedSectionEnricher(this LoggerConfiguration loggerConfiguration, int maxLength = 8)
        => loggerConfiguration.Enrich.With(new PaddedSectionEnricher(maxLength));

    public static LoggerConfiguration WithTruncateSourceContextEnricher(this LoggerConfiguration loggerConfiguration, int maxLength = 8)
        => loggerConfiguration.Enrich.With(new TruncateSourceContextEnricher(maxLength));

    // -----------------------------------------------------------------------------------------------------------------
    // Opinionated configurations
    // -----------------------------------------------------------------------------------------------------------------
    public static LoggerConfiguration AsAnnaSasDevServerConsole(this LoggerConfiguration loggerConfiguration) {
        return loggerConfiguration
            .WithPaddedSectionEnricher()
            .WriteToAsyncConsole(
                outputTemplate: ConsoleOutputTemplates.AnnaSasDevServer,
                theme: ConsoleThemes.AnnaSasDevTheme
            );
    }
}
