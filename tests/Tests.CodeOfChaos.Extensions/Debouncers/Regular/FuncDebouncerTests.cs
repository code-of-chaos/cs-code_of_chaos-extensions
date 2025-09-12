// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

namespace Tests.CodeOfChaos.Extensions.Debouncers.Regular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable ConvertToLocalFunction
public class FuncDebouncerTests {
    [Test]
    public async Task DefaultDebounceMs_ShouldBe100() {
        // Arrange
        int callCount = 0;
        Func<Task> callback = () => {
            callCount++;
            return Task.CompletedTask;
        };

        // Act
        await using Debouncer debouncer = Debouncer.FromDelegate(callback);
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(callCount).IsEqualTo(1);
    }

    [Test]
    public async Task CustomDebounceMs_ShouldRespectSpecifiedTime() {
        // Arrange
        int callCount = 0;
        Func<Task> callback = () => {
            callCount++;
            return Task.CompletedTask;
        };

        const int customDebounceMs = 200;

        // Act
        await using Debouncer debouncer = Debouncer.FromDelegate(callback, customDebounceMs);
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);// Less than debouncing time

        // Assert
        await Assert.That(callCount).IsEqualTo(0);

        // Wait for the remaining time
        await Task.Delay(100);
        await debouncer.FlushAsync();
        await Assert.That(callCount).IsEqualTo(1);
    }

    [Test]
    public async Task MultipleInvocations_ShouldDebounce() {
        // Arrange
        int callCount = 0;
        Func<Task> callback = () => {
            callCount++;
            return Task.CompletedTask;
        };

        // Act
        await using Debouncer debouncer = Debouncer.FromDelegate(callback);
        await debouncer.InvokeDebouncedAsync();
        await debouncer.InvokeDebouncedAsync();
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(callCount).IsEqualTo(1);
    }

    [Test]
    public async Task ConcurrentInvocations_ShouldBeThreadSafe() {
        // Arrange
        int callCount = 0;
        Func<Task> callback = () => {
            callCount++;
            return Task.CompletedTask;
        };

        // Act
        Debouncer debouncer = Debouncer.FromDelegate(callback);
        IEnumerable<Task> tasks = Enumerable.Range(0, 10)
            .Select(_ => debouncer.InvokeDebouncedAsync());

        await Task.WhenAll(tasks);
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(callCount).IsEqualTo(1);
        await debouncer.DisposeAsync();
    }

    [Test]
    public async Task AfterDispose_ShouldThrowObjectDisposedException() {
        // Arrange
        Func<Task> callback = () => Task.CompletedTask;
        Debouncer debouncer = Debouncer.FromDelegate(callback);

        // Act
        await debouncer.DisposeAsync();

        // Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
            await debouncer.InvokeDebouncedAsync());
    }

    [Test]
    public async Task MultipleDispose_ShouldBeIdempotent() {
        // Arrange
        Func<Task> callback = () => Task.CompletedTask;
        Debouncer debouncer = Debouncer.FromDelegate(callback);

        // Act & Assert
        await debouncer.DisposeAsync();
        await debouncer.DisposeAsync();// Should not throw
    }

    [Test]
    public async Task InvocationDuringDebounce_ShouldCancelPrevious() {
        // Arrange
        var executionTimes = new List<DateTime>();
        Func<Task> callback = () => {
            executionTimes.Add(DateTime.UtcNow);
            return Task.CompletedTask;
        };

        // Act
        await using Debouncer debouncer = Debouncer.FromDelegate(callback);
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(50);// Wait half the debounced time
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(executionTimes).HasCount().EqualToOne();
    }
    
    [Test]
    public async Task CancellationToken_OnCancellation_ShouldWork() {
        // Arrange
        int callCount = 0;
        CancellationTokenSource cts = new();
        CancellationToken token = cts.Token;
        
        Func<CancellationToken, Task> callback = async ct => {
            callCount++;
            await Task.Delay(150, ct);
        };

        // Act
        await using Debouncer debouncer = Debouncer.FromDelegate(callback);
        await Task.WhenAll(
            debouncer.InvokeDebouncedAsync(token), 
            cts.CancelAsync()
        );

        // Assert
        await Assert.That(callCount).IsEqualTo(0); // Operation should have been canceled
    }
}
