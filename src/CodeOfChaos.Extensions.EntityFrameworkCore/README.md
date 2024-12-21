# CodeOfChaos.Extensions.EntityFrameworkCore

`CodeOfChaos.Extensions.EntityFrameworkCore` is a library that extends LINQ capabilities in Entity Framework Core. It introduces conditional extensions for operations like `Include`, `Where`, `OrderBy`, and others. These methods enable dynamic query building, improving flexibility and readability when working with Entity Framework Core.

---

## Features

### Conditional Query Building
Enhance `IQueryable` queries with conditional functionality:
- **Conditional Includes**:
    - Dynamically include related entities based on runtime conditions.
- **Conditional Filters**:
    - Apply `Where` clauses only when a condition is met.
- **Conditional Sorting**:
    - Apply `OrderBy` clauses conditionally or use optional order expressions.
- **Conditional Pagination**:
    - Apply `Take`, including support for ranges.

---

## Installation

This library targets `.NET 9.0` and requires C# 13.0. Ensure your project meets these requirements before using.

Add the dependency to your project via NuGet:
```bash
dotnet add package CodeOfChaos.Extensions.EntityFrameworkCore
```

---

## Usage

Here’s how you can leverage the library for dynamic query building in Entity Framework Core:

### Conditional `Include`
Dynamically include related entities only when needed:
```csharp
using Microsoft.EntityFrameworkCore;

var query = dbContext.Users.ConditionalInclude(isAdmin, u => u.Roles);
```

### Conditional `Where`
Apply a filter based on runtime conditions:
```csharp
var query = dbContext.Users.ConditionalWhere(filterByEmail, u => u.Email == searchEmail);
```

### Conditional `OrderBy`
Use conditional sorting:
```csharp
var query = dbContext.Users.ConditionalOrderBy(applySorting, u => u.LastName);
```

Or, with a comparer:
```csharp
var query = dbContext.Users.ConditionalOrderBy(applySorting, u => u.LastName, StringComparer.OrdinalIgnoreCase);
```

### Conditional `Take`
Limit the results dynamically:
```csharp
var query = dbContext.Users.ConditionalTake(applyPagination, 50);
```

Or, with a range:
```csharp
var query = dbContext.Users.ConditionalTake(applyPagination, ..10);
```

---

## Contributing

Feel free to fork and contribute to the project by submitting pull requests. When contributing, ensure your changes align with the project’s coding standards.