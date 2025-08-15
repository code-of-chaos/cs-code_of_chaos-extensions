// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.Debouncers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class DebouncerBase(int debounceMs) : DebouncerBase<DebouncerBase.EmptyUnit>(debounceMs) {
    public readonly struct EmptyUnit;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected abstract ValueTask InvokeCallbackAsync(CancellationToken ct = default);
    protected override ValueTask InvokeCallbackAsync(EmptyUnit unit, CancellationToken ct = default) => InvokeCallbackAsync(ct);
}

public abstract class DebouncerBase<T>(int debounceMs) : IAsyncDisposable {
    protected const int DefaultDebounceMs = 100;
    
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private CancellationTokenSource? _cts;
    private Task? _debounceTask;
    private bool _isDisposed;
    private T? _latestValue;
    private DateTime _lastInvokeTime = DateTime.MinValue;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected abstract ValueTask InvokeCallbackAsync(T item, CancellationToken ct = default);
    
    // ReSharper disable once PossiblyMistakenUseOfCancellationToken
    protected async Task DebouncerLogicAsync(T? value = default, CancellationToken ct = default) {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        await _semaphore.WaitAsync(ct);
        try {
            _latestValue = value;

            if (_cts is not null) {
                await _cts.CancelAsync();
                _cts.Dispose();
            }

            _cts = new CancellationTokenSource();
            CancellationTokenSource? localCts = _cts;
            CancellationToken debounceToken = localCts.Token;
            
            _debounceTask = Task.Run(function: async () => {
                try {
                    await Task.Delay(debounceMs, debounceToken);

                    if (debounceToken.IsCancellationRequested) return;

                    DateTime now = DateTime.UtcNow;
                    if (!((now - _lastInvokeTime).TotalMilliseconds >= debounceMs)) return;

                    _lastInvokeTime = now;
                    await InvokeCallbackAsync(_latestValue!, ct);
                }
                catch (OperationCanceledException) {
                    // Ignore
                }
            }, debounceToken);

        }
        finally {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync() {
        if (_isDisposed) return;

        _isDisposed = true;

        await _semaphore.WaitAsync();
        try {
            if (_cts is not null) {
                await _cts.CancelAsync();
                _cts.Dispose();
            }

            if (_debounceTask is not null) {
                try { await _debounceTask; }
                catch {
                    // Ignore
                }
            }
        }
        finally {
            _semaphore.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
