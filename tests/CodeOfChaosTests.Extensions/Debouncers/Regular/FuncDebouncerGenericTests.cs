// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

namespace CodeOfChaosTests.Extensions.Debouncers.Regular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable ConvertToLocalFunction
public class FuncDebouncerGenericTests {
    [Test]
    public async Task GenericDebouncer_ShouldPassValueToCallback() {
        // Arrange
        string? receivedValue = null;
        Func<string, Task> callback = value => {
            receivedValue = value;
            return Task.CompletedTask;
        };

        // Act
        await using Debouncer<string> debouncer = Debouncer<string>.FromDelegate(callback);
        await debouncer.InvokeDebouncedAsync("test");
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(receivedValue).IsEqualTo("test");
    }

    [Test]
    public async Task GenericDebouncer_MultipleValues_ShouldUseLastValue() {
        // Arrange
        string? receivedValue = null;
        Func<string, Task> callback = value => {
            receivedValue = value;
            return Task.CompletedTask;
        };

        // Act
        await using Debouncer<string> debouncer = Debouncer<string>.FromDelegate(callback);
        await debouncer.InvokeDebouncedAsync("first");
        await debouncer.InvokeDebouncedAsync("second");
        await debouncer.InvokeDebouncedAsync("third");
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(receivedValue).IsEqualTo("third");
    }

    [Test]
    public async Task GenericDebouncer_ConcurrentValues_ShouldBeThreadSafe() {
        // Arrange
        var receivedValues = new List<string>();
        Func<string, Task> callback = value => {
            receivedValues.Add(value);
            return Task.CompletedTask;
        };

        // Act
        Debouncer<string> debouncer = Debouncer<string>.FromDelegate(callback);
        IEnumerable<Task> tasks = Enumerable.Range(0, 10)
            .Select(i => debouncer.InvokeDebouncedAsync($"value{i}"));

        await Task.WhenAll(tasks);
        await Task.Delay(150);
        await debouncer.FlushAsync();

        // Assert
        await Assert.That(receivedValues).Count().IsEqualTo(1);
        await debouncer.DisposeAsync();
    }

    [Test]
    public async Task GenericDebouncer_AfterDispose_ShouldThrowObjectDisposedException() {
        // Arrange
        Func<string, Task> callback = _ => Task.CompletedTask;
        Debouncer<string> debouncer = Debouncer<string>.FromDelegate(callback);

        // Act
        await debouncer.DisposeAsync();

        // Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
            await debouncer.InvokeDebouncedAsync("test")
        );
    }

    [Test]
    public async Task GenericDebouncer_MultipleDispose_ShouldBeIdempotent() {
        // Arrange
        Func<string, Task> callback = _ => Task.CompletedTask;
        Debouncer<string> debouncer = Debouncer<string>.FromDelegate(callback);

        // Act & Assert
        await debouncer.DisposeAsync();
        await debouncer.DisposeAsync(); // Should not throw
    }
}
