// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace CodeOfChaos.Extensions.Debouncers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
public sealed class EventCallbackDebouncer : DebouncerBase<EventCallbackDebouncer.EmptyUnit>, IDebouncer {
    public readonly struct EmptyUnit;
    private EventCallback Callback { get; init; }
    
    private bool _isEmpty;
    public override bool IsEmpty => _isEmpty;
    public static EventCallbackDebouncer Empty => new() {
        Callback = default,
        _isEmpty = true
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static EventCallbackDebouncer FromEventCallback(EventCallback callback, int debounceMs = DefaultDebounceMs)
        => new() {
            Callback = callback,
            DebounceMs = debounceMs
        };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Task InvokeDebouncedAsync(CancellationToken ct = default) 
        => DebouncerLogicAsync(default, ct);

    protected async override ValueTask InvokeCallbackAsync(EmptyUnit item, CancellationToken ct = default) {
        if (ct.IsCancellationRequested) return;
        await Callback.InvokeAsync();
    }
}

// ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
public sealed class EventCallbackDebouncer<T> : DebouncerBase<T>, IDebouncer, IDebouncer<T> {
    private EventCallback<T> Callback { get; init; }
    
    private bool _isEmpty;
    public override bool IsEmpty => _isEmpty;
    public static EventCallbackDebouncer<T> Empty => new() {
        Callback = default,
        _isEmpty = true
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static EventCallbackDebouncer<T> FromEventCallback(EventCallback<T> callback, int debounceMs = DefaultDebounceMs)
        => new() {
            Callback = callback,
            DebounceMs = debounceMs
        };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // EventCallbacks can also be invoked without the T parameter
    public Task InvokeDebouncedAsync(CancellationToken ct = default)
        => DebouncerLogicAsync(default, ct);
        
    public Task InvokeDebouncedAsync(T item, CancellationToken ct = default) 
        => DebouncerLogicAsync(item, ct);
    
    protected async override ValueTask InvokeCallbackAsync(T item, CancellationToken ct = default) {
        if (ct.IsCancellationRequested) return;
        await Callback.InvokeAsync(item);
    }
}