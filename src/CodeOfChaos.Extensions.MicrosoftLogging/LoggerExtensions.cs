// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

#pragma warning disable CA2254
public static class LoggerExtensions {
    #region AsFalse
    public static bool InformationAsFalse(this ILogger logger, [StructuredMessageTemplate] string messageTemplate, params object?[] propertyValues) {
        logger.Information(messageTemplate, propertyValues);
        return false;
    }

    public static bool DebugAsFalse(this ILogger logger, [StructuredMessageTemplate] string messageTemplate, params object?[] propertyValues) {
        logger.Debug(messageTemplate, propertyValues);
        return false;
    }

    public static bool WarningAsFalse(this ILogger logger, [StructuredMessageTemplate] string messageTemplate, params object?[] propertyValues) {
        logger.Warning(messageTemplate, propertyValues);
        return false;
    }

    public static bool ErrorAsFalse(this ILogger logger, [StructuredMessageTemplate] string messageTemplate, params object?[] propertyValues) {
        logger.Error(messageTemplate, propertyValues);
        return false;
    }

    #endregion
    #region AsTrue
    public static bool DebugAsTrue(this ILogger logger, [StructuredMessageTemplate] string messageTemplate, params object?[] propertyValues) {
        logger.LogDebug(messageTemplate, propertyValues);
        return true;
    }

    public static bool InformationAsTrue(this ILogger logger, [StructuredMessageTemplate] string messageTemplate, params object?[] propertyValues) {
        logger.LogInformation(messageTemplate, propertyValues);
        return true;
    }

    public static bool WarningAsTrue(this ILogger logger, [StructuredMessageTemplate] string messageTemplate, params object?[] propertyValues) {
        logger.LogWarning(messageTemplate, propertyValues);
        return true;
    }

    public static bool ErrorAsTrue(this ILogger logger, [StructuredMessageTemplate] string messageTemplate, params object?[] propertyValues) {
        logger.LogError(messageTemplate, propertyValues);
        return true;
    }
    #endregion

    public static void Debug(this ILogger logger, EventId eventId, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogDebug(eventId, exception, message, args);

    public static void Debug(this ILogger logger, EventId eventId, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogDebug(eventId, message, args);

    public static void Debug(this ILogger logger, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogDebug(exception, message, args);

    public static void Debug(this ILogger logger, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogDebug(message, args);

    public static void Trace(this ILogger logger, EventId eventId, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogTrace(eventId, exception, message, args);

    public static void Trace(this ILogger logger, EventId eventId, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogTrace(eventId, message, args);

    public static void Trace(this ILogger logger, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogTrace(exception, message, args);

    public static void Trace(this ILogger logger, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogTrace(message, args);

    public static void Information(this ILogger logger, EventId eventId, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogInformation(eventId, exception, message, args);

    public static void Information(this ILogger logger, EventId eventId, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogInformation(eventId, message, args);

    public static void Information(this ILogger logger, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogInformation(exception, message, args);

    public static void Information(this ILogger logger, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogInformation(message, args);

    public static void Warning(this ILogger logger, EventId eventId, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogWarning(eventId, exception, message, args);

    public static void Warning(this ILogger logger, EventId eventId, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogWarning(eventId, message, args);

    public static void Warning(this ILogger logger, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogWarning(exception, message, args);

    public static void Warning(this ILogger logger, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogWarning(message, args);

    public static void Error(this ILogger logger, EventId eventId, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogError(eventId, exception, message, args);

    public static void Error(this ILogger logger, EventId eventId, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogError(eventId, message, args);

    public static void Error(this ILogger logger, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogError(exception, message, args);

    public static void Error(this ILogger logger, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogError(message, args);

    public static void Critical(this ILogger logger, EventId eventId, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogCritical(eventId, exception, message, args);

    public static void Critical(this ILogger logger, EventId eventId, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogCritical(eventId, message, args);

    public static void Critical(this ILogger logger, Exception? exception, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogCritical(exception, message, args);

    public static void Critical(this ILogger logger, [StructuredMessageTemplate] string message, params object?[] args)
        => logger.LogCritical(message, args);
}
#pragma warning restore CA2254