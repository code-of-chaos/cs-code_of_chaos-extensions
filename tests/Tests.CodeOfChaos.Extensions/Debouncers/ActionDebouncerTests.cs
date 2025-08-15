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
    
    [Test]
    public async Task SustainedInvocations_ShouldExecuteEveryDebounceInterval() {
        // Arrange
        List<DateTime> executionTimes = new();
        Action callback = () => { executionTimes.Add(DateTime.UtcNow); };

        const int debounceMs = 100; // Define debounce interval
        const int totalDurationMs = 500; // Total time to sustain invocations

        await using var debouncer = new ActionDebouncer(callback, debounceMs);

        // Act
        var startTime = DateTime.UtcNow;
        var tasks = new List<Task>();

        // Perform sustained invocations for `totalDurationMs`
        while ((DateTime.UtcNow - startTime).TotalMilliseconds < totalDurationMs) {
            tasks.Add(debouncer.InvokeDebouncedAsync());
            await Task.Delay(50); // Simulate frequent invocations faster than debounce interval
        }

        // Wait just beyond the last debounce interval to ensure final execution
        await Task.Delay(debounceMs + 50);

        // Assert
        await Task.WhenAll(tasks);
        const int expectedExecutions = totalDurationMs / debounceMs;

        // Each execution should occur roughly at debounceMs intervals
        await Assert.That(executionTimes.Count).IsEqualTo((int)Math.Floor((double)expectedExecutions));
        for (int i = 1; i < executionTimes.Count; i++) {
            TimeSpan elapsed = executionTimes[i] - executionTimes[i - 1];
            await Assert.That(elapsed.TotalMilliseconds).IsGreaterThanOrEqualTo(debounceMs).Because("Execution is too frequent.");
        }
    }

}
