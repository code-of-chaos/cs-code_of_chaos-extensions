// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Serilog;
using JetBrains.Annotations;
using Serilog.Core;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace Serilog;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Provides extension methods for the <see cref="ILogger" /> interface.
/// </summary>
public static class LoggerExtensions {
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
    [DoesNotReturn] [AssertionMethod]
    public static void ExitFatal(this ILogger logger, int exitCode, string messageTemplate, params object?[]? propertyValues) {
        logger.Fatal(messageTemplate, propertyValues);
        throw new ExitApplicationException(exitCode, messageTemplate);
    }

    public static ILogger ForSectionProperty(this ILogger logger, string sectionName) => logger.ForContext("Section", sectionName);
    #region AsFalse
    /// <summary>
    ///     Logs a verbose message using the specified message template and property values, and returns false.
    /// </summary>
    /// <param name="logger">The logger instance used for logging.</param>
    /// <param name="messageTemplate">The message template to format and log.</param>
    /// <param name="propertyValues">An array of property values to include in the log message.</param>
    /// <returns>Always returns false.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool VerboseAsFalse(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Verbose(messageTemplate, propertyValues);
        return false;
    }

    /// <summary>
    ///     Logs an informational message and returns false.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to log.</param>
    /// <param name="propertyValues">The property values for the message template.</param>
    /// <returns>Always returns false.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool InformationAsFalse(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Information(messageTemplate, propertyValues);
        return false;
    }

    /// <summary>
    ///     Logs a debug-level message and always returns false.
    /// </summary>
    /// <param name="logger">The logger instance used to log the message.</param>
    /// <param name="messageTemplate">The message template that describes the log message.</param>
    /// <param name="propertyValues">Optional property values for formatting the message template.</param>
    /// <returns>Always returns false.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool DebugAsFalse(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Debug(messageTemplate, propertyValues);
        return false;
    }

    /// <summary>
    ///     Logs a warning message and always returns false.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to log.</param>
    /// <param name="propertyValues">Values to format into the template.</param>
    /// <returns>Always returns false.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool WarningAsFalse(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Warning(messageTemplate, propertyValues);
        return false;
    }

    /// <summary>
    ///     Logs an error message with the specified message template and property values,
    ///     then always returns false.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template describing the log message format.</param>
    /// <param name="propertyValues">An array of objects to format into the message template.</param>
    /// <returns>Always returns false.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool ErrorAsFalse(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Error(messageTemplate, propertyValues);
        return false;
    }

    /// <summary>
    ///     Logs a fatal-level message and returns false.
    /// </summary>
    /// <param name="logger">The logger instance used to write the message.</param>
    /// <param name="messageTemplate">The message template that describes the log message.</param>
    /// <param name="propertyValues">The values to populate the message template.</param>
    /// <returns>Always returns false.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool FatalAsFalse(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Fatal(messageTemplate, propertyValues);
        return false;
    }
    #endregion
    #region AsTrue
    /// <summary>
    ///     Logs a verbose message and always returns true.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to be logged.</param>
    /// <param name="propertyValues">The property values to format the message template.</param>
    /// <returns>Always returns true.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool VerboseAsTrue(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Verbose(messageTemplate, propertyValues);
        return true;
    }

    /// <summary>
    ///     Logs a debug-level message and returns true.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template for the log entry.</param>
    /// <param name="propertyValues">The property values corresponding to the message template.</param>
    /// <returns>Always returns true.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool DebugAsTrue(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Debug(messageTemplate, propertyValues);
        return true;
    }

    /// <summary>
    ///     Logs an information-level message and returns true.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to log.</param>
    /// <param name="propertyValues">An array of property values to format and include in the message.</param>
    /// <returns>Returns true after logging the message.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool InformationAsTrue(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Information(messageTemplate, propertyValues);
        return true;
    }

    /// <summary>
    ///     Logs a warning level message and returns true.
    /// </summary>
    /// <param name="logger">The logger instance used for logging.</param>
    /// <param name="messageTemplate">The message template describing the log event.</param>
    /// <param name="propertyValues">The values to format the message template.</param>
    /// <returns>Always returns true.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool WarningAsTrue(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Warning(messageTemplate, propertyValues);
        return true;
    }

    /// <summary>
    ///     Logs an error message at the Error level and returns a boolean value of true.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to be logged.</param>
    /// <param name="propertyValues">The property values for the message template.</param>
    /// <returns>True, indicating that the operation was successful.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool ErrorAsTrue(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Error(messageTemplate, propertyValues);
        return true;
    }

    /// <summary>
    ///     Logs a fatal message and always returns true.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageTemplate">The message template to log.</param>
    /// <param name="propertyValues">The values to include in the message template.</param>
    /// <returns>Always returns true.</returns>
    [MessageTemplateFormatMethod("messageTemplate")]
    public static bool FatalAsTrue(this ILogger logger, string messageTemplate, params object?[]? propertyValues) {
        logger.Fatal(messageTemplate, propertyValues);
        return true;
    }
    #endregion

    #region Throwable
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
    #endregion
}
