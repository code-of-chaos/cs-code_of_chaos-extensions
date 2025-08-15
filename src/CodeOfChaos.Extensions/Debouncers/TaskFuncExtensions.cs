// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class TaskFuncExtensions {
    public static TaskFuncDebouncer<T> GetDebouncer<T>(this Func<T, Task> callback, int debounceMs) => new(callback, debounceMs);
    public static TaskFuncDebouncer GetDebouncer(this Func<Task> callback, int debounceMs) => new(callback, debounceMs);
    public static TaskFuncCancellableDebouncer<T> GetDebouncer<T>(this Func<T, CancellationToken, Task> callback, int debounceMs) => new(callback, debounceMs);
    public static TaskFuncCancellableDebouncer GetDebouncer(this Func<CancellationToken, Task> callback, int debounceMs) => new(callback, debounceMs);

}
