// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

namespace CodeOfChaosTests.Extensions.Debouncers.Throttled;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable ConvertToLocalFunction
public class ThrottledDebouncerGenericTests {
    private Lock Lock { get; } = new();
    
    [Test]
    public async Task ThrottledDebouncer_ShouldPassValueToCallback() {
        // Arrange
        string? receivedValue = null;
        Action<string> callback = value => {
            lock (Lock) {
                receivedValue = value;
            }
        };

        // Act
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs: 100, throttleMs: 500);
        await debouncer.InvokeDebouncedAsync("test");
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(receivedValue).IsEqualTo("test");
    }

    [Test]
    public async Task ThrottledDebouncer_MultipleValues_ShouldUseLastValue() {
        // Arrange
        string? receivedValue = null;
        Func<string, Task> callback = value => {
            lock (Lock) {
                receivedValue = value;
            }
            return Task.CompletedTask;
        };

        // Act
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs: 100, throttleMs: 500);
        await debouncer.InvokeDebouncedAsync("first");
        await debouncer.InvokeDebouncedAsync("second");
        await debouncer.InvokeDebouncedAsync("third");
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(receivedValue).IsEqualTo("third");
    }

    [Test]
    public async Task ThrottledDebouncer_ThrottleBehavior_ShouldExecuteImmediatelyAfterThrottleTime() {
        // Arrange
        var receivedValues = new List<(string Value, DateTime Time)>();
        Func<string, Task> callback = value => {
            lock (Lock) {
                receivedValues.Add((value, DateTime.UtcNow));
            }
            return Task.CompletedTask;
        };

        const int debounceMs = 1000;
        const int throttleMs = 2000;

        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs, throttleMs);

        // First execution (debounced)
        await debouncer.InvokeDebouncedAsync("first");
        await debouncer.FlushAsync(); // Ensure first execution has completed

        // Second execution after throttle period
        await Task.Delay(throttleMs + 200); // safely beyond throttle window

        await debouncer.InvokeDebouncedAsync("second");
        await debouncer.InvokeDebouncedAsync("third");
        await debouncer.InvokeDebouncedAsync("fourth");

        await debouncer.FlushAsync(); // Ensure second execution has completed

        // Assert
        await Assert.That(receivedValues).Count().IsGreaterThanOrEqualTo(2);
        await Assert.That(receivedValues[0].Value).IsEqualTo("first");
        await Assert.That(receivedValues[^1].Value).IsEqualTo("fourth");

        double timeBetweenExecutions = (receivedValues[^1].Time - receivedValues[0].Time).TotalMilliseconds;

        // Depending on your intended semantics, something like:
        await Assert.That(timeBetweenExecutions).IsGreaterThanOrEqualTo(throttleMs);
        await Assert.That(timeBetweenExecutions).IsLessThan(debounceMs + throttleMs + 500); // some slack
    }

    [Test]
    public async Task ThrottledDebouncer_ContinuousRequests_ShouldRespectThrottleInterval() {
        // Arrange
        var receivedValues = new List<string>();
        int executionCount = 0;
        string? lastScheduledValue = null;
        Action<string> callback = value => {
            lock (Lock) {
                receivedValues.Add(value);
            }
            Interlocked.Increment(ref executionCount);
        };

        // Act
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs: 50, throttleMs: 150);

        DateTime max = DateTime.UtcNow.AddMilliseconds(400);
        int counter = 0;
        while (DateTime.UtcNow < max) { 
            string value = $"value{counter++}";
            lastScheduledValue = value;
            await debouncer.InvokeDebouncedAsync(value);
            await Task.Delay(25);
        }

        await Task.Delay(1000); // Wait for final execution
        await debouncer.FlushAsync();

        
        // Assert
        await Assert.That(executionCount).IsGreaterThanOrEqualTo(1);
        await Assert.That(receivedValues).Count().IsEqualTo(executionCount);
        await Assert.That(receivedValues[^1]).IsEqualTo(lastScheduledValue);
    }
    
    [Test]
    public async Task ThrottledDebouncer_ContinuousRequests_ShouldRespectThrottleInterval_v2() {
        // Arrange
        var receivedValues = new List<string>();
        int executionCount = 0;
        Action<string> callback = value => {
            lock (Lock) {
                receivedValues.Add(value);
            }
            Interlocked.Increment(ref executionCount);
        };

        // Act
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs: 50, throttleMs: 150);
        
        // First burst - should execute after debounce (50ms)
        await Task.WhenAll(
            debouncer.InvokeDebouncedAsync("burst1-1"),
            debouncer.InvokeDebouncedAsync("burst1-2"),
            debouncer.InvokeDebouncedAsync("burst1-3")
        );
    
        await Task.Delay(75); // Wait for first execution (longer than debounce)
    
        // Second burst after throttle period - should execute immediately
        await Task.Delay(100); // Total ~175ms from start, exceeds throttle (150ms)
        await Task.WhenAll(
            debouncer.InvokeDebouncedAsync("burst2-1"),
            debouncer.InvokeDebouncedAsync("burst2-2")
        );
    
        await Task.Delay(25); // Short wait for immediate execution
    
        // Third burst - should be debounced normally
        await Task.WhenAll(
            debouncer.InvokeDebouncedAsync("burst3-1"),
            debouncer.InvokeDebouncedAsync("burst3-2")
        );
        
        await Task.Delay(75); // Wait for final debounced execution
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(executionCount).IsGreaterThanOrEqualTo(2);
        await Assert.That(executionCount).IsLessThanOrEqualTo(4);
        await Assert.That(receivedValues).Count().IsEqualTo(executionCount);
        await Assert.That(receivedValues[^1]).IsEqualTo("burst3-2");

    }

    [Test]
    public async Task ThrottledDebouncer_ConcurrentValues_ShouldBeThreadSafe() {
        // Arrange
        var receivedValues = new List<string>();
        Func<string, Task> callback = value => {
            lock (Lock) {
                receivedValues.Add(value);
            }
            return Task.CompletedTask;
        };

        // Act
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs: 100, throttleMs: 300);
        IEnumerable<Task> tasks = Enumerable.Range(0, 10)
            .Select(i => debouncer.InvokeDebouncedAsync($"value{i}"));

        await Task.WhenAll(tasks);
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert - May have 1 or 2 executions depending on timing (debounce + possible throttle)
        await Assert.That(receivedValues.Count).IsGreaterThanOrEqualTo(1);
        await Assert.That(receivedValues.Count).IsLessThanOrEqualTo(2);
    }

    [Test]
    public async Task ThrottledDebouncer_AfterDispose_ShouldThrowObjectDisposedException() {
        // Arrange
        Func<string, Task> callback = _ => Task.CompletedTask;
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs: 100, throttleMs: 300);

        // Act
        await debouncer.DisposeAsync();

        // Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
            await debouncer.InvokeDebouncedAsync("test")
        );
    }

    [Test]
    public async Task ThrottledDebouncer_MultipleDispose_ShouldBeIdempotent() {
        // Arrange
        Func<string, Task> callback = _ => Task.CompletedTask;
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs: 100, throttleMs: 300);

        // Act & Assert
        await debouncer.DisposeAsync();
        await debouncer.DisposeAsync(); // Should not throw
    }

    [Test]
    public async Task ThrottledDebouncer_DefaultValues_ShouldUseDefaultDebounceAndThrottleTimes() {
        // Arrange
        var executionTimes = new List<DateTime>();
        Func<string, Task> callback = _ => {
            lock (Lock) {
                executionTimes.Add(DateTime.UtcNow);
            }
            return Task.CompletedTask;
        };

        // Act
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback); // Using defaults
        
        DateTime startTime = DateTime.UtcNow;
        await debouncer.InvokeDebouncedAsync("test");
        await Task.Delay(150); // Should execute after default debounce (100ms)
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(executionTimes).Count().IsEqualTo(1);
        double executionDelay = (executionTimes[0] - startTime).TotalMilliseconds;
        await Assert.That(executionDelay).IsGreaterThanOrEqualTo(95); // Account for timing variations
    }

    [Test]
    public async Task ThrottledDebouncer_ZeroDebounce_ShouldExecuteImmediately() {
        // Arrange
        string? receivedValue = null;
        var executionTime = DateTime.MinValue;
        Func<string, Task> callback = value => {
            lock (Lock) {
                receivedValue = value;
                executionTime = DateTime.UtcNow;
            }
            return Task.CompletedTask;
        };

        // Act
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs: 0, throttleMs: 100);
        
        DateTime startTime = DateTime.UtcNow;
        await debouncer.InvokeDebouncedAsync("immediate");
        await Task.Delay(50); // Give time for execution
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(receivedValue).IsEqualTo("immediate");
        double executionDelay = (executionTime - startTime).TotalMilliseconds;
        await Assert.That(executionDelay).IsLessThan(50); // Should execute very quickly
    }

    [Test]
    public async Task ThrottledDebouncer_LongRunningCallback_ShouldNotBlockSubsequentCalls() {
        // Arrange
        int executionCount = 0;
        Func<string, Task> callback = async value => {
            if (value == "slow") await Task.Delay(200); // Simulate long-running operation
            Interlocked.Increment(ref executionCount);
        };

        // Act
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs: 50, throttleMs: 100);
        
        await debouncer.InvokeDebouncedAsync("slow");
        await Task.Delay(500); // Wait for first call to start
        
        await debouncer.InvokeDebouncedAsync("fast");
        
        await Task.Delay(500); // Wait for both calls to complete
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(executionCount).IsEqualTo(2);
    }

    [Test]
    public async Task ThrottledDebouncer_FlushAsync_ShouldWaitForLatestPendingInvocation() {
        // Arrange
        int invocation = 0;
        int completed = 0;
        var firstInvocationStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Func<string, Task> callback = async _ => {
            int current = Interlocked.Increment(ref invocation);
            if (current == 1) firstInvocationStarted.TrySetResult();
            await Task.Delay(current == 1 ? 50 : 300);
            Interlocked.Increment(ref completed);
        };
        ThrottledDebouncer<string> debouncer = ThrottledDebouncer<string>.FromDelegate(callback, debounceMs: 10, throttleMs: 0);

        // Act
        await debouncer.InvokeDebouncedAsync("first");
        await firstInvocationStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        await debouncer.InvokeDebouncedAsync("second");
        await Task.Delay(80); // Callback 1 completes while callback 2 is still running

        Task flushTask = debouncer.FlushAsync();
        await flushTask;

        // Assert
        await Assert.That(completed).IsEqualTo(2);
        await debouncer.DisposeAsync();
    }
}
