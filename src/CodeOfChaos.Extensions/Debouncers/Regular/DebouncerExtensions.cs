// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

// ReSharper disable once CheckNamespace
namespace System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class DebouncerExtensions {
    public static Debouncer<T> GetDebouncer<T>(this Action<T> callback, int debounceMs) => Debouncer<T>.FromDelegate(callback, debounceMs);
    public static Debouncer GetDebouncer(this Action callback, int debounceMs) => Debouncer.FromDelegate(callback, debounceMs);
    
    public static Debouncer<T> GetDebouncer<T>(this Func<T, Task> callback, int debounceMs) => Debouncer<T>.FromDelegate(callback, debounceMs);
    public static Debouncer GetDebouncer(this Func<Task> callback, int debounceMs) => Debouncer.FromDelegate(callback, debounceMs);
    public static Debouncer<T> GetDebouncer<T>(this Func<T, CancellationToken, Task> callback, int debounceMs) => Debouncer<T>.FromDelegate(callback, debounceMs);
    public static Debouncer GetDebouncer(this Func<CancellationToken, Task> callback, int debounceMs) => Debouncer.FromDelegate(callback, debounceMs);
}
