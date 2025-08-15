// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class ThrottledDebouncerBase(int debounceMs) : ThrottledDebouncerBase<ThrottledDebouncerBase.EmptyUnit>(debounceMs) {
    public readonly struct EmptyUnit;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected abstract ValueTask InvokeCallbackAsync(CancellationToken ct = default);
    protected override ValueTask InvokeCallbackAsync(EmptyUnit unit, CancellationToken ct = default) => InvokeCallbackAsync(ct);
}

public abstract class ThrottledDebouncerBase<T>(int debounceMs) : IAsyncDisposable {
    protected const int DefaultDebounceMs = 100;
    
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private CancellationTokenSource? _cts;
    private Task? _debounceTask;
    private bool _isDisposed;
    private T? _latestValue;
    private DateTime _lastExecuteTime = DateTime.MinValue;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected abstract ValueTask InvokeCallbackAsync(T item, CancellationToken ct = default);
    
    // ReSharper disable twice PossiblyMistakenUseOfCancellationToken
    protected async Task DebouncerLogicAsync(T? value = default, CancellationToken ct = default) {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        await _semaphore.WaitAsync(ct);
        try {
            _latestValue = value;

            // Cancel any existing debounce task
            if (_cts is not null) {
                await _cts.CancelAsync();
                _cts.Dispose();
            }

            _cts = new CancellationTokenSource();
            CancellationTokenSource? localCts = _cts;
            CancellationToken debounceToken = localCts.Token;
            
            _debounceTask = Task.Run(async () => {
                try {
                    DateTime now = DateTime.UtcNow;
                    double timeSinceLastExecute = (now - _lastExecuteTime).TotalMilliseconds;
                    
                    // If we haven't executed recently, execute immediately
                    if (timeSinceLastExecute >= debounceMs) {
                        _lastExecuteTime = now;
                        await InvokeCallbackAsync(_latestValue!, ct);
                    }
                    else {
                        // Wait for the remaining time, then execute
                        int remainingDelay = (int)(debounceMs - timeSinceLastExecute);
                        await Task.Delay(remainingDelay, debounceToken);
                        
                        if (!debounceToken.IsCancellationRequested) {
                            _lastExecuteTime = DateTime.UtcNow;
                            await InvokeCallbackAsync(_latestValue!, ct);
                        }
                    }
                }
                catch (OperationCanceledException) {
                    // Ignore cancellation
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
