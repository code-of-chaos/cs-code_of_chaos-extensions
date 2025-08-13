// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class DebouncerBase(int debounceMs) : IAsyncDisposable {
    protected const int DefaultDebounceMs = 100;
    
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private CancellationTokenSource? _cts;
    private Task? _debounceTask;
    private bool _isDisposed;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected abstract ValueTask InvokeCallbackAsync(CancellationToken ct = default);
    
    public async Task InvokeDebouncedAsync(CancellationToken ct = default) {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        await _semaphore.WaitAsync(ct);
        try {
            if (_cts is not null) await _cts.CancelAsync();
            _cts?.Dispose();

            _cts = new CancellationTokenSource();
            CancellationTokenSource? localCts = _cts;
            CancellationToken debounceToken = localCts.Token;

            _debounceTask = Task.Run(function: async () => {
                try {
                    await Task.Delay(debounceMs, debounceToken);
                    if (!debounceToken.IsCancellationRequested) {
                        
                        // ReSharper disable once PossiblyMistakenUseOfCancellationToken
                        await InvokeCallbackAsync(ct);
                    }
                }
                catch (OperationCanceledException) {
                    // Ignore
                }
            }, debounceToken);
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
        }
        finally {
            _semaphore.Release();
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
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
                    /* Ignore */
                }
            }
        }
        finally {
            _semaphore.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}