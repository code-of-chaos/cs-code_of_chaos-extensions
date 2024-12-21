# CodeOfChaos.Extensions.AspNetCore

`CodeOfChaos.Extensions.AspNetCore` is a lightweight library that simplifies some aspects of ASP.NET Core applications.

---

## Features

### Simplified Serilog Integration
- Easily replaces the default ASP.NET Core logging with **Serilog**.
- Supports **custom configuration** via `LoggerConfiguration` for full control over your logging setup.
- Automatically manages Serilog's lifecycle, ensuring proper cleanup on application shutdown.

---

## Installation

This library targets `.NET 9.0` and requires C# 13.0. Ensure your project meets these requirements before using.

Add the dependency to your project via NuGet:
```bash
dotnet add package CodeOfChaos.Extensions.AspNetCore
```

---

## Usage

Here's how you can use the library to configure Serilog in your ASP.NET Core application:

### Example Setup
1. Install the `CodeOfChaos.Extensions.AspNetCore` package in your project.
2. Modify your `Program.cs` or `Startup.cs` file:
   ```csharp
   using CodeOfChaos.Extensions.AspNetCore;
   using Serilog;

   var builder = WebApplication.CreateBuilder(args);

   // Override logging with Serilog
   builder.OverrideLoggingWithSerilog(configure: config =>
   {
       config.WriteTo.Console(); // Example: Log to console
   });

   var app = builder.Build();

   app.Run();
   ```

3. Run your application. Serilog will now be the active logging provider.

---

## Features in Detail

### Method: `OverrideLoggingWithSerilog`
This method replaces the default logging system with Serilog. It provides several key capabilities:
- Accepts an **optional configuration delegate** to customize `LoggerConfiguration` (e.g., specifying sinks like Console, File, etc.).
- Integrates all required services and clears default logging providers automatically.
- Ensures proper flushing and cleanup of logs using an internally managed hosted service on application shutdown.

---

## Contributing

Feel free to fork and contribute to the project by submitting pull requests. When contributing, ensure your changes align with the project’s coding standards.