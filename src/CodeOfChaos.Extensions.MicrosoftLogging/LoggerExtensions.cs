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
