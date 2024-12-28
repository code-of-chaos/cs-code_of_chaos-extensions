// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog.Core;
using Serilog.Events;

namespace CodeOfChaos.Extensions.Serilog.Enrichers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PaddedSectionEnricher : ILogEventEnricher {

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PaddedSectionEnricher() {}
    public PaddedSectionEnricher(int maxLength) {
        MaxLength = maxLength;
    }
    public int MaxLength { get; } = 8;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
        if (!logEvent.Properties.TryGetValue("Section", out LogEventPropertyValue? sectionProperty)) {
            // If "Section" is not defined, fallback to default value
            sectionProperty = new ScalarValue(string.Empty);
        }

        string sectionValue = sectionProperty.ToString().Trim('"');// Remove quotes and trim

        // Left-pad as required to a max of 8 characters
        string paddedSection = sectionValue.PadLeft(MaxLength)[..MaxLength];

        LogEventProperty paddedProperty = propertyFactory.CreateProperty("Section", paddedSection);
        logEvent.AddOrUpdateProperty(paddedProperty);
    }
}
