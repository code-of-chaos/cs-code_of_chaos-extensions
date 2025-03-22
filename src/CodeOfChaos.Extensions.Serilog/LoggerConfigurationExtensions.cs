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

    public static LoggerConfiguration WithPaddedSectionEnricher(this LoggerConfiguration loggerConfiguration, int maxLength = 12)
        => loggerConfiguration.Enrich.With(new PaddedSectionEnricher(maxLength));

    public static LoggerConfiguration WithTruncateSourceContextEnricher(this LoggerConfiguration loggerConfiguration, int maxLength = 12)
        => loggerConfiguration.Enrich.With(new TruncateSourceContextEnricher(maxLength));

    // -----------------------------------------------------------------------------------------------------------------
    // Opinionated configurations
    // -----------------------------------------------------------------------------------------------------------------
    public static LoggerConfiguration AsAnnaSasDevServerConsole(
        this LoggerConfiguration loggerConfiguration,
        int sectionMaxLength = 12,
        Action<AsyncConsoleConfig>? configure = null
    ) {
        var asyncConsoleConfig = new AsyncConsoleConfig();
        configure?.Invoke(asyncConsoleConfig);

        return loggerConfiguration
            .Enrich.FromLogContext()
            .WithPaddedSectionEnricher(maxLength: sectionMaxLength)
            .WriteToAsyncConsole(
                asyncConsoleConfig.RestrictedToMinimumLevel,
                asyncConsoleConfig.OutputTemplate,
                asyncConsoleConfig.FormatProvider,
                asyncConsoleConfig.LevelSwitch,
                asyncConsoleConfig.StandardErrorFromLevel,
                asyncConsoleConfig.Theme,
                asyncConsoleConfig.ApplyThemeToRedirectedOutput,
                asyncConsoleConfig.SyncRoot
            );
    }

    public class AsyncConsoleConfig {
        public LogEventLevel RestrictedToMinimumLevel { get; set; } = LogEventLevel.Verbose;
        public string OutputTemplate { get; set; } = ConsoleOutputTemplates.AnnaSasDevServer;
        public IFormatProvider? FormatProvider { get; set; } = null;
        public LoggingLevelSwitch? LevelSwitch { get; set; } = null;
        public LogEventLevel? StandardErrorFromLevel { get; set; } = null;
        public ConsoleTheme? Theme { get; set; } = ConsoleThemes.AnnaSasDevTheme;
        public bool ApplyThemeToRedirectedOutput { get; set; } = false;
        public object? SyncRoot { get; set; } = null;
    }
}
