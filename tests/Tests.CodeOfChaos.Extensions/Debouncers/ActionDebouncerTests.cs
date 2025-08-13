// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

namespace Tests.CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable ConvertToLocalFunction
public class ActionDebouncerTests {
    [Test]
    public async Task DefaultDebounceMs_ShouldBe100() {
        // Arrange
        int callCount = 0;
        
        // ReSharper disable once ConvertToLocalFunction
        Action callback = () => {
            callCount++;
        };

        // Act
        await using var debouncer = new ActionDebouncer(callback);
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);

        // Assert
        await Assert.That(callCount).IsEqualTo(1);
    }

    [Test]
    public async Task CustomDebounceMs_ShouldRespectSpecifiedTime() {
        // Arrange
        int callCount = 0;
        
        // ReSharper disable once ConvertToLocalFunction
        Action callback = () => {
            callCount++;
        };

        const int customDebounceMs = 200;

        // Act
        await using var debouncer = new ActionDebouncer(callback, customDebounceMs);
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);// Less than debouncing time

        // Assert
        await Assert.That(callCount).IsEqualTo(0);

        // Wait for the remaining time
        await Task.Delay(100);
        await Assert.That(callCount).IsEqualTo(1);
    }

    [Test]
    public async Task MultipleInvocations_ShouldDebounce() {
        // Arrange
        int callCount = 0;
        
        // ReSharper disable once ConvertToLocalFunction
        Action callback = () => {
            callCount++;
        };

        // Act
        await using var debouncer = new ActionDebouncer(callback);
        await debouncer.InvokeDebouncedAsync();
        await debouncer.InvokeDebouncedAsync();
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);

        // Assert
        await Assert.That(callCount).IsEqualTo(1);
    }

    [Test]
    public async Task ConcurrentInvocations_ShouldBeThreadSafe() {
        // Arrange
        int callCount = 0;
        Action callback = () => {
            callCount++;
        };

        // Act
        var debouncer = new ActionDebouncer(callback);
        IEnumerable<Task> tasks = Enumerable.Range(0, 10)
            .Select(_ => debouncer.InvokeDebouncedAsync());

        await Task.WhenAll(tasks);
        await Task.Delay(150);

        // Assert
        await Assert.That(callCount).IsEqualTo(1);
        await debouncer.DisposeAsync();
    }

    [Test]
    public async Task AfterDispose_ShouldThrowObjectDisposedException() {
        // Arrange
        Action callback = () => { };
        var debouncer = new ActionDebouncer(callback);

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
        var debouncer = new ActionDebouncer(callback);

        // Act & Assert
        await debouncer.DisposeAsync();
        await debouncer.DisposeAsync();// Should not throw
    }

    [Test]
    public async Task InvocationDuringDebounce_ShouldCancelPrevious() {
        // Arrange
        var executionTimes = new List<DateTime>();
        Action callback = () => {
            executionTimes.Add(DateTime.UtcNow);
        };

        // Act
        await using var debouncer = new ActionDebouncer(callback);
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(50);// Wait half the debounced time
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);

        // Assert
        await Assert.That(executionTimes).HasCount().EqualToOne();
    }
}
