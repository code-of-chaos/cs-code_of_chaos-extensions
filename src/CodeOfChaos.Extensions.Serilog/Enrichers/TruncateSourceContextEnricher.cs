// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog.Core;
using Serilog.Events;

namespace CodeOfChaos.Extensions.Serilog.Enrichers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TruncateSourceContextEnricher : ILogEventEnricher {
    public int MaxLength { get; } = 8;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public TruncateSourceContextEnricher() { }
    public TruncateSourceContextEnricher(int maxLength) => MaxLength = maxLength;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
        if (!logEvent.Properties.TryGetValue("SourceContext", out LogEventPropertyValue? sourceContextValue)
            || sourceContextValue is not ScalarValue { Value: string sourceContext }) return;

        string truncatedSourceContext = sourceContext.Length > MaxLength + 3
            ? string.Concat("...", sourceContext.AsSpan(sourceContext.Length - MaxLength, MaxLength))
            : sourceContext;

        var truncatedProperty = new LogEventProperty("SourceContext", new ScalarValue(truncatedSourceContext));
        logEvent.AddOrUpdateProperty(truncatedProperty);
    }
}
