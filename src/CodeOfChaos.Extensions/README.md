# CodeOfChaos.Extensions

`CodeOfChaos.Extensions` is a collection of utility extension methods designed to enhance and simplify everyday programming tasks in .NET projects. This library offers extensions for tasks, strings, enums, LINQ queries, collections, and dictionaries, enabling more readable, efficient, and intuitive code.

---

## Features

### Task Extensions
Enhance task handling with support for **timeouts** and **cancellation tokens**:
- `.WithCancellation(CancellationToken)` – Cancel a task when the token is triggered.
- `.WithTimeout(TimeSpan)` – Enforce a timeout on task execution.
- Available for both `Task` and `Task<T>`.

---

### String Extensions
Simplify string management with useful utilities:
- `IsNullOrEmpty()` / `IsNotNullOrEmpty()` – Check for null or empty strings.
- `IsNullOrWhiteSpace()` / `IsNotNullOrWhiteSpace()` – Check for null, empty, or whitespace strings.
- `Truncate(int maxLength)` – Shorten a string to a maximum length.
- `ToGuid()` – Parse a string into a `Guid`.

---

### Enum Extensions
Work with enums more efficiently:
- `.GetFlags()` – Retrieve all flagged values from an enum.
- `.GetFlagsAsArray()` / `.GetFlagsAsList()` – Retrieve flagged values as arrays or lists.
- Caches enums for better performance.

---

### LINQ Extensions
Add conditional logic to LINQ operations:
- Apply operations conditionally:
    - `ConditionalWhere`
    - `ConditionalTake`
    - `ConditionalOrderBy`
    - `ConditionalDistinct` and more.
- Chain LINQ operations dynamically based on runtime conditions.

---

### Collection Extensions
Extensions to handle collections and arrays more effectively:
- `IsEmpty()` – Check if a `string[]` or `IEnumerable<string>` is empty.

---

### Dictionary Extensions
Enhance dictionary management:
- `AddOrUpdate()` – Add a key-value pair or update the existing value.
- `TryAddToOrCreateCollection()` – Add values to a collection within a dictionary or create a new collection.

---

## Installation

This library targets `.NET 9.0` and requires C# 13.0. Ensure your project meets these requirements before using.

Add the dependency to your project via NuGet:
```bash
dotnet add package CodeOfChaos.Extensions
```

---

## Usage

Here’s how you can leverage the `CodeOfChaos.Extensions` library:

### Task Example
```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

var tokenSource = new CancellationTokenSource();
var task = Task.Delay(5000); // Example task
await task.WithCancellation(tokenSource.Token);
```

### String Example
```csharp
using System;

string? input = "Hello, World!";
if (input.IsNotNullOrWhiteSpace()) {
    Console.WriteLine(input.Truncate(5)); // Output: Hello
}
```

### Enum Example
```csharp
[Flags]
enum AccessLevel { None = 0, Read = 1, Write = 2, Execute = 4 }

var access = AccessLevel.Read | AccessLevel.Execute;
var flags = access.GetFlagsAsList(); // Output: [Read, Execute]
```

### LINQ Example
```csharp
var items = new List<int> { 1, 2, 3, 4, 5 }.AsQueryable();
var filtered = items.ConditionalWhere(true, x => x > 2); // Applies condition
```

---

## Contributing

Feel free to fork and contribute to the project by submitting pull requests. When contributing, ensure your changes align with the project’s coding standards.
