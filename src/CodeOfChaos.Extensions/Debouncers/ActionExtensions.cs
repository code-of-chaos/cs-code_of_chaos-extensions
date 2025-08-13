// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ActionExtensions {
    public static ActionDebouncer<T> GetDebouncer<T>(this Action<T> callback, int debounceMs) => new(callback, debounceMs);
    public static ActionDebouncer GetDebouncer(this Action callback, int debounceMs) => new(callback, debounceMs);
}
