// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ActionDebouncer(Action action, int debounceMs = DebouncerBase.DefaultDebounceMs) : DebouncerBase(debounceMs) {
    protected override ValueTask InvokeCallbackAsync(CancellationToken ct = default) {
        if (ct.IsCancellationRequested) return ValueTask.CompletedTask;
        
        action.Invoke();
        return ValueTask.CompletedTask;
    }
}

public class ActionDebouncer<T>(Action<T> action, int debounceMs = DebouncerBase<T>.DefaultDebounceMs) : DebouncerBase<T>(debounceMs) {
    protected override ValueTask InvokeCallbackAsync(T item, CancellationToken ct = default) {
        if (ct.IsCancellationRequested) return ValueTask.CompletedTask;
        
        action.Invoke(item);
        return ValueTask.CompletedTask;
    }
}




