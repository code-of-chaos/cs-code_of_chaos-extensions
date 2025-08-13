// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TaskFuncDebouncer(Func<Task> func, int debounceMs = DebouncerBase.DefaultDebounceMs) : DebouncerBase(debounceMs) {
    protected async override ValueTask InvokeCallbackAsync(CancellationToken ct = default) {
        if (ct.IsCancellationRequested) return;
        await func.Invoke();
    }
}

public class TaskFuncCancellableDebouncer(Func<CancellationToken, Task> func, int debounceMs = DebouncerBase.DefaultDebounceMs) : DebouncerBase(debounceMs) {
    protected async override ValueTask InvokeCallbackAsync(CancellationToken ct = default) {
        if (ct.IsCancellationRequested) return;
        await func.Invoke(ct);
    }
}

public class TaskFuncDebouncer<T>(Func<T, Task> func, int debounceMs = DebouncerBase<T>.DefaultDebounceMs) : DebouncerBase<T>(debounceMs) {
    protected async override ValueTask InvokeCallbackAsync(T item, CancellationToken ct = default) {
        if (ct.IsCancellationRequested) return;
        await func.Invoke(item);
    }
}

public class TaskFuncCancellableDebouncer<T>(Func<T, CancellationToken, Task> func, int debounceMs = DebouncerBase<T>.DefaultDebounceMs) : DebouncerBase<T>(debounceMs) {
    protected async override ValueTask InvokeCallbackAsync(T item, CancellationToken ct = default) {
        if (ct.IsCancellationRequested) return;
        await func.Invoke(item, ct);
    }
}
