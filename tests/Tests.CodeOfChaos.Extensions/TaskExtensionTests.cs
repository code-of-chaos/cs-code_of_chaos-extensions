// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TaskExtensionsTest {

    [Test]
    public async Task WithCancellation_ShouldComplete_WhenTokenIsNotCanceled() {
        // Arrange
        var tokenSource = new CancellationTokenSource();
        Task task = DummyTask();

        // Act
        await task.WithCancellation(tokenSource.Token);

        // Assert
        await Assert.That(task.IsCompleted).IsTrue(); // Validate the task is completed
    }

    [Test]
    public async Task WithCancellation_ShouldThrowOperationCanceledException_WhenTokenIsCanceled() {
        // Arrange
        var tokenSource = new CancellationTokenSource();
        Task task = Task.Delay(1000, tokenSource.Token);

        // Act
        await tokenSource.CancelAsync(); // Cancel token immediately

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => task.WithCancellation(tokenSource.Token));
    }

    [Test]
    public async Task WithCancellation_Generic_ShouldComplete_WhenTokenIsNotCanceled() {
        // Arrange
        var tokenSource = new CancellationTokenSource();
        Task<string> task = DummyTaskWithResult();

        // Act
        string result = await task.WithCancellation(tokenSource.Token);

        // Assert
        await Assert.That(task.IsCompleted).IsTrue();
        await Assert.That(result).IsEqualTo("Completed"); 
    }

    [Test]
    public async Task WithCancellation_Generic_ShouldThrowOperationCanceledException_WhenTokenIsCanceled() {
        // Arrange
        var tokenSource = new CancellationTokenSource();
        Task<string> task = Task.Delay(1000, tokenSource.Token).ContinueWith(_ => "This will not complete", tokenSource.Token);

        // Act
        await tokenSource.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => task.WithCancellation(tokenSource.Token));
    }

    [Test]
    public async Task WithTimeout_ShouldComplete_WhenTaskFinishesBeforeTimeout() {
        // Arrange
        Task task = DummyTask();

        // Act
        await task.WithTimeout(TimeSpan.FromSeconds(1));

        // Assert
        await Assert.That(task.IsCompleted).IsTrue(); // Validate the task is completed
    }

    [Test]
    public async Task WithTimeout_ShouldThrowTimeoutException_WhenTaskExceedsTimeout() {
        // Arrange
        Task task = Task.Delay(2000); // Simulate a long-running task

        // Assert
        await Assert.ThrowsAsync<TimeoutException>(() => task.WithTimeout(TimeSpan.FromMilliseconds(500)));
    }

    [Test]
    public async Task WithTimeout_Generic_ShouldComplete_WhenTaskFinishesBeforeTimeout() {
        // Arrange
        Task<string> task = DummyTaskWithResult();

        // Act
        string result = await task.WithTimeout(TimeSpan.FromSeconds(2));

        // Assert
        await Assert.That(task.IsCompleted).IsTrue();
        await Assert.That(result).IsEqualTo("Completed");
    }

    [Test]
    public async Task WithTimeout_Generic_ShouldThrowTimeoutException_WhenTaskExceedsTimeout() {
        // Arrange
        Task<string> task = Task.Delay(2000).ContinueWith(_ => "This will timeout");

        // Assert
        await Assert.ThrowsAsync<TimeoutException>(() => task.WithTimeout(TimeSpan.FromMilliseconds(500)));
    }

    [Test]
    public async Task WithTimeout_AndCancellation_ShouldPrioritizeCancellationOverTimeout() {
        // Arrange
        var tokenSource = new CancellationTokenSource();
        Task task = Task.Delay(2000, tokenSource.Token); // Simulate a long-running task

        // Act
        await tokenSource.CancelAsync(); // Cancel the task before timeout

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => task.WithCancellation(tokenSource.Token).WithTimeout(TimeSpan.FromSeconds(5)));
    }

    [Test]
    public async Task WithTimeout_AndCancellation_ShouldCancelGenericTask() {
        // Arrange
        var tokenSource = new CancellationTokenSource();
        Task<string> task = Task.Delay(2000, tokenSource.Token).ContinueWith(_ => "This will be canceled", tokenSource.Token);

        // Act
        await tokenSource.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => task.WithCancellation(tokenSource.Token).WithTimeout(TimeSpan.FromSeconds(5)));
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------------------------------------------------------
    private async Task DummyTask() {
        await Task.Delay(100); // Simulate a short operation
    }

    private async Task<string> DummyTaskWithResult() {
        await Task.Delay(100); // Simulate a short operation
        return "Completed";
    }
}