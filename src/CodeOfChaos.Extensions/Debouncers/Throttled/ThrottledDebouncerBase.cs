// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Extensions.Debouncers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class ThrottledDebouncerBase<T> : IAsyncDisposable, IDebouncerBase {
    protected const int DefaultDebounceMs = 100;
    protected const int DefaultThrottleMs = 100;

    public required int DebounceMs { get; init; }
    public required int ThrottleMs { get; init; }

    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly Lock _stateLock = new();
    private CancellationTokenSource? _cts;
    private Task? _debounceTask;
    private bool _isDisposed;
    private T? _latestValue;
    private DateTime _lastExecuteTime = DateTime.MinValue;
    private DateTime _firstCallTime = DateTime.MinValue;

    public abstract bool IsEmpty { get; }
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected abstract ValueTask InvokeCallbackAsync(T item, CancellationToken ct = default);
    
    protected async Task DebouncerLogicAsync(T? value = default, CancellationToken ct = default) {
        EnsureNotDisposed();

        if (IsEmpty) return;

        await _semaphore.WaitAsync(ct);
        try {
            _latestValue = value;

            if (_firstCallTime == DateTime.MinValue) _firstCallTime = DateTime.UtcNow;

            ReplaceCancellationTokenSource();
            _debounceTask = ExecuteDebounceTaskAsync(_latestValue, _cts.Token);
        }
        finally {
            _semaphore.Release();
        }

        // Note: DebouncerLogicAsync doesn't await the task to avoid blocking; the task is managed internally.
    }
    
    [MemberNotNull(nameof(_cts))]
    private void ReplaceCancellationTokenSource() {
        lock (_stateLock) {
            if (_cts is not null) {
                _cts.Cancel();
                _cts.Dispose();
            }

            _cts = new CancellationTokenSource();
        }
    }

    private async Task ExecuteDebounceTaskAsync(T? capturedValue, CancellationToken externalCt) {
        try {
            DateTime taskStartTime = DateTime.UtcNow;

            DateTime referenceTime;
            lock (_stateLock) {
                referenceTime = _lastExecuteTime != DateTime.MinValue ? _lastExecuteTime : _firstCallTime;
            }

            double timeSinceReference = (taskStartTime - referenceTime).TotalMilliseconds;

            if (timeSinceReference >= ThrottleMs) {
                await HandleDebounceExecution(capturedValue, externalCt, taskStartTime);
                return;
            }

            await Task.Delay(DebounceMs, _cts!.Token);

            if (_cts.Token.IsCancellationRequested) return;
            await HandleDebounceExecution(capturedValue, externalCt, taskStartTime);
        }
        catch (OperationCanceledException) {
            // Ignore cancellation
        }
    }
    private async Task HandleDebounceExecution(T? capturedValue, CancellationToken externalCt, DateTime taskStartTime) {
        lock (_stateLock) {
            _lastExecuteTime = taskStartTime;
        }
        if (capturedValue is null) return;

        await InvokeCallbackAsync(capturedValue, externalCt);
        _debounceTask = null;

    }
    
    public Task FlushAsync(CancellationToken ct = default) {
        EnsureNotDisposed();
        return _debounceTask ?? Task.CompletedTask;
    }

    private void EnsureNotDisposed() {
        if (!_isDisposed) return;

        throw new ObjectDisposedException(nameof(DebouncerBase<T>));
    }
    public async ValueTask DisposeAsync() {
        if (_isDisposed) return;

        _isDisposed = true;

        await _semaphore.WaitAsync();
        try {
            if (_debounceTask is not null) {
                try {
                    await _debounceTask;
                }
                catch {
                    // Ignore task exceptions
                }
            }

            lock (_stateLock) {
                if (_cts is not null) {
                    _cts.Cancel();
                    _cts.Dispose();
                    _cts = null;
                }
            }
        }
        finally {
            _semaphore.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
