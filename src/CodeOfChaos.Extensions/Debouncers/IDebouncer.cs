// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDebouncerBase {
    bool IsEmpty { get; }
    Task FlushAsync(CancellationToken ct = default);
}

public interface IDebouncer : IDebouncerBase {
    Task InvokeDebouncedAsync(CancellationToken ct = default);
}

public interface IDebouncer<in T> : IDebouncerBase {
    Task InvokeDebouncedAsync(T item, CancellationToken ct = default);
}