// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

namespace Tests.CodeOfChaos.Extensions.Debouncers;

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
        await using var debouncer = new TaskFuncDebouncer<string>(callback);
        await debouncer.InvokeDebouncedAsync("test");
        await Task.Delay(150);

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
        await using var debouncer = new TaskFuncDebouncer<string>(callback);
        await debouncer.InvokeDebouncedAsync("first");
        await debouncer.InvokeDebouncedAsync("second");
        await debouncer.InvokeDebouncedAsync("third");
        await Task.Delay(150);

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
        var debouncer = new TaskFuncDebouncer<string>(callback);
        IEnumerable<Task> tasks = Enumerable.Range(0, 10)
            .Select(i => debouncer.InvokeDebouncedAsync($"value{i}"));

        await Task.WhenAll(tasks);
        await Task.Delay(150);

        // Assert
        await Assert.That(receivedValues).HasCount().EqualTo(1);
        await debouncer.DisposeAsync();
    }

    [Test]
    public async Task GenericDebouncer_AfterDispose_ShouldThrowObjectDisposedException() {
        // Arrange
        Func<string, Task> callback = _ => Task.CompletedTask;
        var debouncer = new TaskFuncDebouncer<string>(callback);

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
        var debouncer = new TaskFuncDebouncer<string>(callback);

        // Act & Assert
        await debouncer.DisposeAsync();
        await debouncer.DisposeAsync(); // Should not throw
    }
}
