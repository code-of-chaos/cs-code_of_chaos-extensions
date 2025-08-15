// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

namespace Tests.CodeOfChaos.Extensions.Debouncers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable ConvertToLocalFunction
public class ActionDebouncerGenericTests {
    [Test]
    public async Task GenericDebouncer_ShouldPassValueToCallback() {
        // Arrange
        string? receivedValue = null;
        Action<string> callback = value => {
            receivedValue = value;
        };

        // Act
        await using var debouncer = new ActionDebouncer<string>(callback);
        await debouncer.InvokeDebouncedAsync("test");
        await Task.Delay(150);

        // Assert
        await Assert.That(receivedValue).IsEqualTo("test");
    }

    [Test]
    public async Task GenericDebouncer_MultipleValues_ShouldUseLastValue() {
        // Arrange
        string? receivedValue = null;
        Action<string> callback = value => {
            receivedValue = value;
        };

        // Act
        await using var debouncer = new ActionDebouncer<string>(callback);
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
        Action<string> callback = value => {
            receivedValues.Add(value);
        };

        // Act
        var debouncer = new ActionDebouncer<string>(callback);
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
        Action<string> callback = _ => {};
        var debouncer = new ActionDebouncer<string>(callback);

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
        Action<string> callback = _ => {};
        var debouncer = new ActionDebouncer<string>(callback);

        // Act & Assert
        await debouncer.DisposeAsync();
        await debouncer.DisposeAsync(); // Should not throw
    }
}
