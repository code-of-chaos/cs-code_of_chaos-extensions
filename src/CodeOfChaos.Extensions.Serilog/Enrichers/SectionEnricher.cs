// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog.Core;
using Serilog.Events;

namespace CodeOfChaos.Extensions.Serilog.Enrichers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// Define your section here (e.g., "auth" or "db")
public class SectionEnricher(string sectionName) : ILogEventEnricher {
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
        LogEventProperty sectionProperty = propertyFactory.CreateProperty("Section", sectionName);
        logEvent.AddPropertyIfAbsent(sectionProperty);
    }
}
