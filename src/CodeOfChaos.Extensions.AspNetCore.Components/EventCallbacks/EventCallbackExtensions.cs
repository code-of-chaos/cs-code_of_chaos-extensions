// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EventCallbackExtensions {
    public static EventCallbackDebouncer<T> GetDebouncer<T>(this EventCallback<T> callback, int debounceMs) => EventCallbackDebouncer<T>.FromEventCallback(callback, debounceMs);
    public static EventCallbackDebouncer GetDebouncer(this EventCallback callback, int debounceMs) => EventCallbackDebouncer.FromEventCallback(callback, debounceMs);
}
