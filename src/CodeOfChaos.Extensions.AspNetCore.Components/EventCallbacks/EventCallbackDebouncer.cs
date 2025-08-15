// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EventCallbackDebouncer(EventCallback callback, int debounceMs = DebouncerBase.DefaultDebounceMs)
    : DebouncerBase(debounceMs) {
    
    protected async override ValueTask InvokeCallbackAsync(CancellationToken ct = default)
        => await callback.InvokeAsync();
    
    public Task InvokeDebouncedAsync(CancellationToken ct = default) 
        => DebouncerLogicAsync(default, ct);
}

public class EventCallbackDebouncer<T>(EventCallback<T> callback, int debounceMs = EventCallbackDebouncer<T>.DefaultDebounceMs)
    : DebouncerBase<T>(debounceMs) {

    protected async override ValueTask InvokeCallbackAsync(T item, CancellationToken ct = default) 
        => await callback.InvokeAsync(item);
    
    public Task InvokeDebouncedAsync(CancellationToken ct = default) 
        => DebouncerLogicAsync(default, ct);

    public Task InvokeDebouncedAsync(T item, CancellationToken ct = default) 
        => DebouncerLogicAsync(item, ct);
}