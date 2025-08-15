// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class ThrottledDebouncerBase<T> : IAsyncDisposable {
    protected const int DefaultDebounceMs = 100;
    protected const int DefaultThrottleMs = 100;

    protected int DebounceMs { get; init; } = DefaultDebounceMs;
    protected int ThrottleMs { get; init; } = DefaultThrottleMs;
    
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private CancellationTokenSource? _cts;
    private Task? _debounceTask;
    private bool _isDisposed;
    private T? _latestValue;
    private DateTime _lastExecuteTime = DateTime.MinValue;
    private DateTime _firstCallTime = DateTime.MinValue; 


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
            if (_firstCallTime == DateTime.MinValue) {
                _firstCallTime = DateTime.UtcNow;
            }

            if (_cts is not null) {
                await _cts.CancelAsync();
                _cts.Dispose();
            }

            _cts = new CancellationTokenSource();
            CancellationTokenSource? localCts = _cts;
            CancellationToken debounceToken = localCts.Token;
            
            _debounceTask = Task.Run(async () => {
                try {
                    DateTime taskStartTime = DateTime.UtcNow;
                    
                    DateTime referenceTime = _lastExecuteTime != DateTime.MinValue ? _lastExecuteTime : _firstCallTime;
                    double timeSinceReference = (taskStartTime - referenceTime).TotalMilliseconds;

                    // Execute immediately due to throttling
                    if (timeSinceReference >= ThrottleMs) {
                        _lastExecuteTime = taskStartTime;
                        await InvokeCallbackAsync(_latestValue!, ct);
                        return;
                    }

                    await Task.Delay(DebounceMs, debounceToken);

                    if (debounceToken.IsCancellationRequested) return;
                    _lastExecuteTime = taskStartTime;
                    await InvokeCallbackAsync(_latestValue!, ct);
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
            if (_debounceTask is not null) {
                try { await _debounceTask; }
                catch {
                    // Ignore
                }
            }
            
            if (_cts is not null) {
                await _cts.CancelAsync();
                _cts.Dispose();
            }
        }
        finally {
            _semaphore.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
