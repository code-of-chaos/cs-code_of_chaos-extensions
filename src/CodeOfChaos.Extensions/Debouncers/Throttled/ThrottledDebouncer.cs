// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class ThrottledDebouncer: ThrottledDebouncerBase<ThrottledDebouncer.EmptyUnit> {
    public readonly struct EmptyUnit; // Workaround for reusing ThrottledDebouncerBase<T> instead of creating a fully new DebouncerBase without generics
    private object? Callback { get; init; }

    protected override bool IsEmpty => Callback is null;
    public static ThrottledDebouncer Empty => new() {
        Callback = null
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private ThrottledDebouncer() {}
    public static ThrottledDebouncer FromDelegate(Action action, int debounceMs = DefaultDebounceMs, int throttleMs = DefaultThrottleMs) 
        => new() {
            Callback = action,
            DebounceMs = debounceMs,
            ThrottleMs = throttleMs
        };
    
    public static ThrottledDebouncer FromDelegate(Func<CancellationToken, Task> func, int debounceMs = DefaultDebounceMs, int throttleMs = DefaultThrottleMs)
        => new() {
            Callback = func,
            DebounceMs = debounceMs,
            ThrottleMs = throttleMs
        };

    public static ThrottledDebouncer FromDelegate(Func<Task> func, int debounceMs = DefaultDebounceMs, int throttleMs = DefaultThrottleMs) 
        => new() {
            Callback = func,
            DebounceMs = debounceMs,
            ThrottleMs = throttleMs
        };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Task InvokeDebouncedAsync(CancellationToken ct = default) 
        => DebouncerLogicAsync(default, ct);

    protected async override ValueTask InvokeCallbackAsync(EmptyUnit item, CancellationToken ct = default) {
        if (ct.IsCancellationRequested) return;
        
        switch (Callback) {

            case Action action: {
                action.Invoke();
                break;
            }

            case Func<Task> func: {
                await func.Invoke();
                break;
            }

            case Func<CancellationToken, Task> func: {
                await func.Invoke(ct);
                break;
            }

            default: throw new InvalidOperationException("Invalid function type");
        }
    }
}

public sealed class ThrottledDebouncer<T> : ThrottledDebouncerBase<T> {
    private object? Callback { get; init; }
    protected override bool IsEmpty => Callback is null;
    public static ThrottledDebouncer<T> Empty => new() {
        Callback = null
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private ThrottledDebouncer() {}
    public static ThrottledDebouncer<T> FromDelegate(Action<T> action, int debounceMs = DefaultDebounceMs, int throttleMs = DefaultThrottleMs) 
        => new() {
            Callback = action,
            DebounceMs = debounceMs,
            ThrottleMs = throttleMs
        };
    
    public static ThrottledDebouncer<T> FromDelegate(Func<T, Task> func, int debounceMs = DefaultDebounceMs, int throttleMs = DefaultThrottleMs)
        => new() {
            Callback = func,
            DebounceMs = debounceMs,
            ThrottleMs = throttleMs
        };
    
    public static ThrottledDebouncer<T> FromDelegate(Func<T, CancellationToken, Task> func, int debounceMs = DefaultDebounceMs, int throttleMs = DefaultThrottleMs)
        => new() {
            Callback = func,
            DebounceMs = debounceMs,
            ThrottleMs = throttleMs
        };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Task InvokeDebouncedAsync(T item, CancellationToken ct = default) 
        => DebouncerLogicAsync(item, ct);

    protected async override ValueTask InvokeCallbackAsync(T item, CancellationToken ct = default) {
        if (ct.IsCancellationRequested) return;
        
        switch (Callback) {
            case Action<T> action: {
                action.Invoke(item);
                break;
            }

            case Func<T, Task> func: {
                await func.Invoke(item);
                break;
            }
            
            case Func<T, CancellationToken, Task> func: {
                await func.Invoke(item, ct);
                break;
            }

            default: throw new InvalidOperationException("Invalid function type");
        }
    }
}