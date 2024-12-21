// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using Serilog;
using Serilog.Core;
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Extensions.Serilog;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <summary>
///     Provides extension methods for the <see cref="ILogger" /> interface.
/// </summary>
public static class LoggerExtensions {
    /// <summary>
    ///     Throws a Error log message, logs the exception, and throws it.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template.</param>
    /// <param name="propertyValues">The property values.</param>
    /// <exception cref="Exception">Thrown exception.</exception>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static Exception ThrowableError(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        var exception = (Exception)Activator.CreateInstance(typeof(Exception), messageTemplate)!;
        logger.Error(exception, messageTemplate, propertyValues);
        return exception;
    }

    /// <summary>
    ///     Throws a Error exception and logs it using the logger. The exception is created with the specified message template
    ///     and property values.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template for the exception.</param>
    /// <param name="propertyValues">The property values for the exception.</param>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static TException ThrowableError<TException>(this ILogger logger, string messageTemplate, params object?[]? propertyValues) where TException : Exception, new() {
        var exception = (TException)Activator.CreateInstance(typeof(TException), messageTemplate)!;
        logger.Error(exception, messageTemplate, propertyValues);
        return exception;
    }
    
    /// <summary>
    ///     Throws a fatal log message, logs the exception, and throws it.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template.</param>
    /// <param name="propertyValues">The property values.</param>
    /// <exception cref="Exception">Thrown exception.</exception>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static Exception ThrowableFatal(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        var exception = (Exception)Activator.CreateInstance(typeof(Exception), messageTemplate)!;
        logger.Fatal(exception, messageTemplate, propertyValues);
        return exception;
    }

    /// <summary>
    ///     Throws a fatal exception and logs it using the logger. The exception is created with the specified message template
    ///     and property values.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template for the exception.</param>
    /// <param name="propertyValues">The property values for the exception.</param>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static TException ThrowableFatal<TException>(this ILogger logger, string messageTemplate, params object?[]? propertyValues) where TException : Exception, new() {
        var exception = (TException)Activator.CreateInstance(typeof(TException), messageTemplate)!;
        logger.Fatal(exception, messageTemplate, propertyValues);
        return exception;
    }

    /// <summary>
    ///     Throws a fatal exception with the specified message template and property values.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to be used for the exception.</param>
    /// <param name="exception">The type of expection to be thrown.</param>
    /// <param name="propertyValues">The property values to be used for formatting the message.</param>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static TException ThrowableFatal<TException>(this ILogger logger, TException exception, string messageTemplate, params object?[]? propertyValues) where TException : Exception {
        logger.Fatal(exception, messageTemplate, propertyValues);
        return exception;
    }

    /// <summary>
    ///     Writes a fatal log message and exits the application with the specified exit code.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="exitCode">The exit code.</param>
    /// <param name="messageTemplate">The message template.</param>
    /// <param name="propertyValues">The values to be included in the log message.</param>
    /// <remarks>
    ///     This method writes a fatal log message using the specified <paramref name="logger" /> and
    ///     <paramref name="messageTemplate" />.
    ///     It then exits the application with the specified <paramref name="exitCode" />.
    /// </remarks>
    [MessageTemplateFormatMethod("messageTemplate")]
    [DoesNotReturn, AssertionMethod]
    public static void ExitFatal(this ILogger logger, int exitCode, string messageTemplate, params object?[]? propertyValues) {
        logger.Fatal(messageTemplate, propertyValues);
        throw new ExitApplicationException(exitCode, messageTemplate);
    }
}
