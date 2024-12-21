# CodeOfChaos.Extensions.Serilog

`CodeOfChaos.Extensions.Serilog` extends the capabilities of Serilog, providing additional methods to simplify logging and exception management. It is particularly useful for elegantly handling fatal and error conditions, enhancing both logging clarity and application lifecycle management.

---

## Features

### Enhanced Logging and Exception Management
- **Throwable Errors**:
    - Log an error and throw an exception in a single call.
    - Supports both generic exceptions and custom exception types.
- **Throwable Fatals**:
    - Log a fatal error and throw an exception simultaneously.
    - Supports both generic and custom exceptions.
- **Exit Logging**:
    - Log a fatal error and immediately terminate the application with a specified exit code.

---

## Installation

This library targets `.NET 9.0` and requires C# 13.0. Ensure your project meets these requirements before using.

Add the dependency to your project via NuGet:
```bash
dotnet add package CodeOfChaos.Extensions.Serilog
```

---

## Usage

Here's how you can use the library to enhance logging in your applications:

### Throwable Errors
Log an error and throw an exception:
```csharp
using Serilog;

ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

try {
    throw logger.ThrowableError("An error occurred: {Error}", "SampleError");
} catch (Exception ex) {
    Console.WriteLine($"Caught exception: {ex.Message}");
}
```

Or, use a specific exception type:
```csharp
try {
    throw logger.ThrowableError<InvalidOperationException>("Invalid operation: {Details}", "SampleDetails");
} catch (Exception ex) {
    Console.WriteLine($"Caught exception: {ex.Message}");
}
```

### Throwable Fatals
Log a fatal error and throw an exception:
```csharp
try {
    throw logger.ThrowableFatal("A fatal error occurred: {Error}", "CriticalError");
} catch (Exception ex) {
    Console.WriteLine($"Caught fatal exception: {ex.Message}");
}
```

Specify a custom exception type:
```csharp
try {
    throw logger.ThrowableFatal<InvalidOperationException>("A critical failure: {Details}", "CriticalDetails");
} catch (Exception ex) {
    Console.WriteLine($"Caught fatal exception: {ex.Message}");
}
```

### Exit Logging
Log a fatal error and immediately terminate the application with an exit code:
```csharp
logger.ExitFatal(1, "The application encountered a critical error and will exit: {Reason}", "CriticalIssue");
```

---

## Features in Detail

### Method: `ThrowableError`
- Logs an **error**-level message and throws an exception.
- Supports custom exception types via the generic overload `ThrowableError<TException>()`.

### Method: `ThrowableFatal`
- Logs a **fatal**-level message and throws an exception.
- Supports custom exception types as well.

### Method: `ExitFatal`
- Logs a **fatal**-level message and terminates the application.
- Takes an **exit code** to ensure proper command-line application management.

---

## Contributing

Feel free to fork and contribute to the project by submitting pull requests. When contributing, ensure your changes align with the project’s coding standards.
