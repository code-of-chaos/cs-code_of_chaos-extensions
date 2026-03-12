// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;
using Microsoft.AspNetCore.Components;

namespace Tests.CodeOfChaos.Extensions.AspNetCore.Components.EventCallbacks;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EventCallbackDebouncerTests {
    public static IEnumerable<Func<(IDebouncerBase, bool)>> GetEmptyDebouncers() {
        yield return () => (EventCallbackDebouncer.Empty, true);
        yield return () => (EventCallbackDebouncer<string>.Empty, true);
        yield return () => (EventCallbackDebouncer<int>.Empty, true);
        yield return () => (EventCallbackDebouncer.FromEventCallback(EventCallback.Factory.Create(string.Empty, callback: () => Task.CompletedTask)), false);
        yield return () => (EventCallbackDebouncer<string>.FromEventCallback(EventCallback.Factory.Create<string>(string.Empty, callback: () => Task.CompletedTask)), false);
        yield return () => (EventCallbackDebouncer<int>.FromEventCallback(EventCallback.Factory.Create<int>(string.Empty, callback: () => Task.CompletedTask)), false);
    }
    
    [Test]
    [MethodDataSource(nameof(GetEmptyDebouncers))]
    public async Task Empty_ShouldBeEmpty(IDebouncerBase debouncer, bool expectedIsEmpty) {
        // Arrange & Act
        bool result = debouncer.IsEmpty;

        // Assert
        await Assert.That(result).IsEqualTo(expectedIsEmpty);
    }
    

    [Test]
    public async Task DefaultDebounceMs_ShouldBe100() {
        // Arrange
        int callCount = 0;
        EventCallback callback = EventCallback.Factory.Create(this, callback: () => {
            callCount++;
            return Task.CompletedTask;
        });

        // Act
        await using EventCallbackDebouncer debouncer = EventCallbackDebouncer.FromEventCallback(callback);
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(callCount).IsEqualTo(1);
    }

    [Test]
    public async Task CustomDebounceMs_ShouldRespectSpecifiedTime_EventCallback() {
        // Arrange
        int callCount = 0;
        EventCallback callback = EventCallback.Factory.Create(this, callback: () => {
            callCount++;
            return Task.CompletedTask;
        });

        const int customDebounceMs = 200;

        // Act
        await using var debouncer = EventCallbackDebouncer.FromEventCallback(callback, customDebounceMs);
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
        EventCallback callback = EventCallback.Factory.Create(this, callback: () => {
            callCount++;
            return Task.CompletedTask;
        });

        // Act
        await using var debouncer = EventCallbackDebouncer.FromEventCallback(callback);
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
        EventCallback callback = EventCallback.Factory.Create(this, callback: () => {
            callCount++;
            return Task.CompletedTask;
        });

        // Act
        var debouncer = EventCallbackDebouncer.FromEventCallback(callback);
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
        EventCallback callback = EventCallback.Factory.Create(this, callback: () => Task.CompletedTask);
        var debouncer = EventCallbackDebouncer.FromEventCallback(callback);

        // Act
        await debouncer.DisposeAsync();

        // Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
            await debouncer.InvokeDebouncedAsync());
    }

    [Test]
    public async Task MultipleDispose_ShouldBeIdempotent() {
        // Arrange
        EventCallback callback = EventCallback.Factory.Create(this, callback: () => Task.CompletedTask);
        var debouncer = EventCallbackDebouncer.FromEventCallback(callback);

        // Act & Assert
        await debouncer.DisposeAsync();
        await debouncer.DisposeAsync();// Should not throw
    }

    [Test]
    public async Task InvocationDuringDebounce_ShouldCancelPrevious_EventCallback() {
        // Arrange
        var executionTimes = new List<DateTime>();
        EventCallback callback = EventCallback.Factory.Create(this, callback: () => {
            executionTimes.Add(DateTime.UtcNow);
            return Task.CompletedTask;
        });

        // Act
        await using var debouncer = EventCallbackDebouncer.FromEventCallback(callback);
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(50);// Wait half the debounced time
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(executionTimes).Count().IsEqualTo(1);
    }

    [Test]
    public async Task FlushAsync_ShouldWaitForLatestPendingInvocation_EventCallback() {
        // Arrange
        int invocation = 0;
        int completed = 0;
        EventCallback callback = EventCallback.Factory.Create(this, callback: async () => {
            int current = Interlocked.Increment(ref invocation);
            await Task.Delay(current == 1 ? 50 : 300);
            Interlocked.Increment(ref completed);
        });

        await using EventCallbackDebouncer debouncer = EventCallbackDebouncer.FromEventCallback(callback, debounceMs: 10);

        // Act
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(20); // Ensure callback 1 has started
        await debouncer.InvokeDebouncedAsync();
        await Task.Delay(80); // Callback 1 completes while callback 2 is still running

        Task flushTask = debouncer.FlushAsync();
        await flushTask;

        // Assert
        await Assert.That(completed).IsEqualTo(2);
    }
}
