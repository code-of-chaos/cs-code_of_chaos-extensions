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
    private int _debounceVersion;

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

            ReplaceCancellationTokenSource();
            lock (_stateLock) {
                if (_firstCallTime == DateTime.MinValue) _firstCallTime = DateTime.UtcNow;
                _debounceVersion++;
                _debounceTask = ExecuteDebounceTaskAsync(_latestValue, _debounceVersion, _cts.Token);
            }
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

    private async Task ExecuteDebounceTaskAsync(T? capturedValue, int scheduledVersion, CancellationToken externalCt) {
        try {
            DateTime taskStartTime = DateTime.UtcNow;

            DateTime referenceTime;
            lock (_stateLock) {
                referenceTime = _lastExecuteTime != DateTime.MinValue ? _lastExecuteTime : _firstCallTime;
            }

            double timeSinceReference = (taskStartTime - referenceTime).TotalMilliseconds;

            if (timeSinceReference >= ThrottleMs) {
                await HandleDebounceExecution(capturedValue, taskStartTime, externalCt);
                return;
            }

            await Task.Delay(DebounceMs, externalCt);

            if (externalCt.IsCancellationRequested) return;
            await HandleDebounceExecution(capturedValue, taskStartTime, externalCt);
        }
        catch (OperationCanceledException) {
            // Ignore cancellation
        }

        lock (_stateLock) {
            if (scheduledVersion == _debounceVersion) _debounceTask = null;
        }
    }
    private async Task HandleDebounceExecution(T? capturedValue, DateTime taskStartTime, CancellationToken externalCt) {
        lock (_stateLock) {
            _lastExecuteTime = taskStartTime;
        }
        if (capturedValue is null) return;

        await InvokeCallbackAsync(capturedValue, externalCt);
    }
    
    public Task FlushAsync(CancellationToken ct = default) {
        EnsureNotDisposed();
        lock (_stateLock) {
            return _debounceTask ?? Task.CompletedTask;
        }
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

                _debounceTask = null;
            }
        }
        finally {
            _semaphore.Release();
            _semaphore.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
