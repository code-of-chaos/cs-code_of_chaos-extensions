// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

// ReSharper disable once CheckNamespace
namespace System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ThrottledDebouncerExtensions {
    public static ThrottledDebouncer<T> GetThrottledDebouncer<T>(this Action<T> callback, int debounceMs, int throttleMs) => ThrottledDebouncer<T>.FromDelegate(callback, debounceMs, throttleMs);
    public static ThrottledDebouncer GetThrottledDebouncer(this Action callback, int debounceMs, int throttleMs) => ThrottledDebouncer.FromDelegate(callback, debounceMs, throttleMs);
    
    public static ThrottledDebouncer<T> GetThrottledDebouncer<T>(this Func<T, Task> callback, int debounceMs, int throttleMs) => ThrottledDebouncer<T>.FromDelegate(callback, debounceMs, throttleMs);
    public static ThrottledDebouncer GetThrottledDebouncer(this Func<Task> callback, int debounceMs, int throttleMs) => ThrottledDebouncer.FromDelegate(callback, debounceMs, throttleMs);
    public static ThrottledDebouncer<T> GetThrottledDebouncer<T>(this Func<T, CancellationToken, Task> callback, int debounceMs, int throttleMs) => ThrottledDebouncer<T>.FromDelegate(callback, debounceMs, throttleMs);
    public static ThrottledDebouncer GetThrottledDebouncer(this Func<CancellationToken, Task> callback, int debounceMs, int throttleMs) => ThrottledDebouncer.FromDelegate(callback, debounceMs, throttleMs);
}
