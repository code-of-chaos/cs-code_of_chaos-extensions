// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LoggerExtensions {
    
    #region AsFalse
    /// <summary>
    /// Logs an informational message and returns false.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to log.</param>
    /// <param name="propertyValues">The property values for the message template.</param>
    /// <returns>Always returns false.</returns>
    [StringFormatMethod("messageTemplate")]
    public static bool InformationAsFalse(this ILogger logger, string messageTemplate, params object?[] propertyValues) {
        logger.Information(messageTemplate, propertyValues);
        return false;
    }

    /// <summary>
    /// Logs a debug-level message and always returns false.
    /// </summary>
    /// <param name="logger">The logger instance used to log the message.</param>
    /// <param name="messageTemplate">The message template that describes the log message.</param>
    /// <param name="propertyValues">Optional property values for formatting the message template.</param>
    /// <returns>Always returns false.</returns>
    [StringFormatMethod("messageTemplate")]
    public static bool DebugAsFalse(this ILogger logger, string messageTemplate, params object?[] propertyValues) {
        logger.Debug(messageTemplate, propertyValues);
        return false;
    }

    /// <summary>
    /// Logs a warning message and always returns false.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to log.</param>
    /// <param name="propertyValues">Values to format into the template.</param>
    /// <returns>Always returns false.</returns>
    [StringFormatMethod("messageTemplate")]
    public static bool WarningAsFalse(this ILogger logger, string messageTemplate, params object?[] propertyValues) {
        logger.Warning(messageTemplate, propertyValues);
        return false;
    }

    /// <summary>
    /// Logs an error message with the specified message template and property values,
    /// then always returns false.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template describing the log message format.</param>
    /// <param name="propertyValues">An array of objects to format into the message template.</param>
    /// <returns>Always returns false.</returns>
    [StringFormatMethod("messageTemplate")]
    public static bool ErrorAsFalse(this ILogger logger, string messageTemplate, params object?[] propertyValues) {
        logger.Error(messageTemplate, propertyValues);
        return false;
    }

    #endregion
    #region AsTrue
    /// <summary>
    /// Logs a debug-level message and returns true.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template for the log entry.</param>
    /// <param name="propertyValues">The property values corresponding to the message template.</param>
    /// <returns>Always returns true.</returns>
    [StringFormatMethod("messageTemplate")]
    public static bool DebugAsTrue(this ILogger logger, string messageTemplate, params object?[] propertyValues) {
        logger.LogDebug(messageTemplate, propertyValues);
        return true;
    }

    /// <summary>
    /// Logs an information-level message and returns true.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to log.</param>
    /// <param name="propertyValues">An array of property values to format and include in the message.</param>
    /// <returns>Returns true after logging the message.</returns>
    [StringFormatMethod("messageTemplate")]
    public static bool InformationAsTrue(this ILogger logger, string messageTemplate, params object?[] propertyValues) {
        logger.LogInformation(messageTemplate, propertyValues);
        return true;
    }

    /// <summary>
    /// Logs a warning level message and returns true.
    /// </summary>
    /// <param name="logger">The logger instance used for logging.</param>
    /// <param name="messageTemplate">The message template describing the log event.</param>
    /// <param name="propertyValues">The values to format the message template.</param>
    /// <returns>Always returns true.</returns>
    [StringFormatMethod("messageTemplate")]
    public static bool WarningAsTrue(this ILogger logger, string messageTemplate, params object?[] propertyValues) {
        logger.LogWarning(messageTemplate, propertyValues);
        return true;
    }

    /// <summary>
    /// Logs an error message at the Error level and returns a boolean value of true.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to be logged.</param>
    /// <param name="propertyValues">The property values for the message template.</param>
    /// <returns>True, indicating that the operation was successful.</returns>
    [StringFormatMethod("messageTemplate")]
    public static bool ErrorAsTrue(this ILogger logger, string messageTemplate, params object?[] propertyValues) {
        logger.LogError(messageTemplate, propertyValues);
        return true;
    }
    #endregion
    
    [StringFormatMethod("message")]
    public static void Debug(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
        => logger.LogDebug(eventId, exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Debug(this ILogger logger, EventId eventId, string? message, params object?[] args)
        => logger.LogDebug(eventId, message, args);
    
    [StringFormatMethod("message")]
    public static void Debug(this ILogger logger, Exception? exception, string? message, params object?[] args)
        => logger.LogDebug(exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Debug(this ILogger logger, string? message, params object?[] args)
        => logger.LogDebug(message, args);
    
    [StringFormatMethod("message")]
    public static void Trace(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
        => logger.LogTrace(eventId, exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Trace(this ILogger logger, EventId eventId, string? message, params object?[] args)
        => logger.LogTrace(eventId, message, args);
    
    [StringFormatMethod("message")]
    public static void Trace(this ILogger logger, Exception? exception, string? message, params object?[] args)
        => logger.LogTrace(exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Trace(this ILogger logger, string? message, params object?[] args)
        => logger.LogTrace(message, args);
    
    [StringFormatMethod("message")]
    public static void Information(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
        => logger.LogInformation(eventId, exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Information(this ILogger logger, EventId eventId, string? message, params object?[] args)
        => logger.LogInformation(eventId, message, args);
    
    [StringFormatMethod("message")]
    public static void Information(this ILogger logger, Exception? exception, string? message, params object?[] args)
        => logger.LogInformation(exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Information(this ILogger logger, string? message, params object?[] args)
        => logger.LogInformation(message, args);
    
    [StringFormatMethod("message")]
    public static void Warning(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
        => logger.LogWarning(eventId, exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Warning(this ILogger logger, EventId eventId, string? message, params object?[] args)
        => logger.LogWarning(eventId, message, args);
    
    [StringFormatMethod("message")]
    public static void Warning(this ILogger logger, Exception? exception, string? message, params object?[] args)
        => logger.LogWarning(exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Warning(this ILogger logger, string? message, params object?[] args)
        => logger.LogWarning(message, args);
    
    [StringFormatMethod("message")]
    public static void Error(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
        => logger.LogError(eventId, exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Error(this ILogger logger, EventId eventId, string? message, params object?[] args)
        => logger.LogError(eventId, message, args);
    
    [StringFormatMethod("message")]
    public static void Error(this ILogger logger, Exception? exception, string? message, params object?[] args)
        => logger.LogError(exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Error(this ILogger logger, string? message, params object?[] args)
        => logger.LogError(message, args);
    
    [StringFormatMethod("message")]
    public static void Critical(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
        => logger.LogCritical(eventId, exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Critical(this ILogger logger, EventId eventId, string? message, params object?[] args)
        => logger.LogCritical(eventId, message, args);
    
    [StringFormatMethod("message")]
    public static void Critical(this ILogger logger, Exception? exception, string? message, params object?[] args)
        => logger.LogCritical(exception, message, args);
    
    [StringFormatMethod("message")]
    public static void Critical(this ILogger logger, string? message, params object?[] args)
        => logger.LogCritical(message, args);
}
