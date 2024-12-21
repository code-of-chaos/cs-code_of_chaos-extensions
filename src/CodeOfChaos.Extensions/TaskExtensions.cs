// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class TaskExtensions {
    public static async Task WithCancellation(this Task task, CancellationToken cancellationToken) {
        var tcs = new TaskCompletionSource();

        await using (cancellationToken.Register(() => tcs.TrySetResult())) {
            if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                throw new OperationCanceledException(cancellationToken);
        }

        await task.ConfigureAwait(false);
    }

    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken) {
        var tcs = new TaskCompletionSource();

        await using (cancellationToken.Register(() => tcs.TrySetResult())) {
            if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                throw new OperationCanceledException(cancellationToken);
        }

        return await task.ConfigureAwait(false);
    }

    public static async Task WithTimeout(this Task task, TimeSpan timeout) {
        var tcs = new TaskCompletionSource();

        using (var cts = new CancellationTokenSource(timeout))
        await using (cts.Token.Register(() => tcs.TrySetResult())) {
            if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                throw new TimeoutException("The operation has timed out.");
        }

        await task.ConfigureAwait(false);
    }

    public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout) {
        var tcs = new TaskCompletionSource();

        using (var cts = new CancellationTokenSource(timeout))
        await using (cts.Token.Register(() => tcs.TrySetResult())) {
            if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                throw new TimeoutException("The operation has timed out.");
        }

        return await task.ConfigureAwait(false);
    }
}
