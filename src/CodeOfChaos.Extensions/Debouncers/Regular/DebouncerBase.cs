// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class DebouncerBase<T> : IAsyncDisposable {
    protected const int DefaultDebounceMs = 100;
    protected int DebounceMs { get; init; }

    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly Lock _stateLock = new(); 
    private CancellationTokenSource? _cts;
    private Task? _debounceTask;
    private bool _isDisposed;
    private T? _latestValue;

    public abstract bool IsEmpty { get; }
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected abstract ValueTask InvokeCallbackAsync(T item, CancellationToken ct = default);

    // ReSharper disable once InconsistentlySynchronizedField
    protected async Task DebouncerLogicAsync(T? value = default, CancellationToken ct = default) {
        EnsureNotDisposed();

        if (IsEmpty) return;

        await _semaphore.WaitAsync(ct);
        try {
            _latestValue = value;

            ReplaceCancellationTokenSource();
            _debounceTask = ExecuteDebounceTaskAsync(_latestValue, _cts.Token);
        }
        finally {
            _semaphore.Release();
        }

        // Awaiting the debounce task is not done here to allow non-blocking execution of this method.
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

    private async Task ExecuteDebounceTaskAsync(T? capturedValue, CancellationToken debounceToken) {
        try {
            await Task.Delay(DebounceMs, debounceToken);

            if (!debounceToken.IsCancellationRequested && capturedValue is not null) {
                await InvokeCallbackAsync(capturedValue, debounceToken);
                _debounceTask = null;
            }
        }
        catch (OperationCanceledException) {
            // Ignored: Task was canceled before completion
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
            }
        }
        finally {
            _semaphore.Release();
            _semaphore.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}