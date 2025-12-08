// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

namespace Tests.CodeOfChaos.Extensions.Debouncers.Regular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable ConvertToLocalFunction
public class ActionDebouncerTests {
    private Lock Lock { get; } = new();
    
    [Test]
    public async Task DefaultDebounceMs_ShouldBe100() {
        // Arrange
        int callCount = 0;
        
        // ReSharper disable once ConvertToLocalFunction
        Action callback = () => {
            Interlocked.Increment(ref callCount);
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
    public async Task CustomDebounceMs_ShouldRespectSpecifiedTime_Action() {
        // Arrange
        int callCount = 0;
        
        // ReSharper disable once ConvertToLocalFunction
        Action callback = () => {
            Interlocked.Increment(ref callCount);
        };

        const int customDebounceMs = 200;

        // Act
        await using Debouncer debouncer = Debouncer.FromDelegate(callback, customDebounceMs);
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(50);// Less than debouncing time

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
        
        // ReSharper disable once ConvertToLocalFunction
        Action callback = () => {
            Interlocked.Increment(ref callCount);
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
        Action callback = () => {
            Interlocked.Increment(ref callCount);
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
        Action callback = () => { };
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
        Action callback = () => { };
        Debouncer debouncer = Debouncer.FromDelegate(callback);

        // Act & Assert
        await debouncer.DisposeAsync();
        await debouncer.DisposeAsync();// Should not throw
    }

    [Test]
    public async Task InvocationDuringDebounce_ShouldCancelPrevious_Action() {
        // Arrange
        var executionTimes = new List<DateTime>();
        Action callback = () => {
            lock (Lock) {
                executionTimes.Add(DateTime.UtcNow);
            }
        };

        // Act
        await using Debouncer debouncer = Debouncer.FromDelegate(callback, debounceMs:100);
        await debouncer.InvokeDebouncedAsync();
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(executionTimes).Count().IsEqualTo(1);
    }
}
