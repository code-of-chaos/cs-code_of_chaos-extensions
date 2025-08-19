// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.SpanLINQ;

namespace Tests.CodeOfChaos.Extensions.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Category("SpanLINQ")]
// ReSharper disable once InconsistentNaming
public class SpanLINQAllTests {
    private record TestItem(int Value);

    [Test]
    public async Task All() {
        // Arrange
        Span<int> span = [2, 4, 6, 8];

        // Act
        bool result = span.All(static x => x % 2 == 0);

        // Assert
        await Assert.That(result).IsTrue();;
    }

    [Test]
    public async Task All_Span_SomeElementsDoNotMatchPredicate_ShouldReturnFalse() {
        // Arrange
        Span<int> numbers = [2, 4, 5, 8];

        // Act
        bool result = numbers.All(x => x % 2 == 0);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task All_Span_EmptySpan_ShouldReturnTrue() {
        // Arrange
        Span<int> emptySpan = [];

        // Act
        bool result = emptySpan.All(x => x > 0);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task All_Span_SingleElementMatches_ShouldReturnTrue() {
        // Arrange
        Span<int> singleElement = [10];

        // Act
        bool result = singleElement.All(x => x > 5);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task All_Span_SingleElementDoesNotMatch_ShouldReturnFalse() {
        // Arrange
        Span<int> singleElement = [3];

        // Act
        bool result = singleElement.All(x => x > 5);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task All_ReadOnlySpan_AllElementsMatchPredicate_ShouldReturnTrue() {
        // Arrange
        ReadOnlySpan<int> numbers = [1, 3, 5, 7];

        // Act
        bool result = numbers.All(x => x % 2 == 1);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task All_ReadOnlySpan_SomeElementsDoNotMatchPredicate_ShouldReturnFalse() {
        // Arrange
        ReadOnlySpan<int> numbers = [1, 3, 4, 7];

        // Act
        bool result = numbers.All(x => x % 2 == 1);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task All_ReadOnlySpan_EmptySpan_ShouldReturnTrue() {
        // Arrange
        ReadOnlySpan<int> emptySpan = [];

        // Act
        bool result = emptySpan.All(x => x < 0);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task All_WithCustomType_AllElementsMatchPredicate_ShouldReturnTrue() {
        // Arrange
        var items = new TestItem[] {
            new(10),
            new(20),
            new(30)
        };
        Span<TestItem> span = items;

        // Act
        bool result = span.All(x => x.Value >= 10);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task All_WithCustomType_SomeElementsDoNotMatchPredicate_ShouldReturnFalse() {
        // Arrange
        var items = new TestItem[] {
            new(5),
            new(20),
            new(30)
        };
        Span<TestItem> span = items;

        // Act
        bool result = span.All(x => x.Value >= 10);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task All_WithStringType_AllElementsMatchPredicate_ShouldReturnTrue() {
        // Arrange
        Span<string> words = ["hello", "world", "test"];

        // Act
        bool result = words.All(x => x.Length >= 4);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task All_WithStringType_SomeElementsDoNotMatchPredicate_ShouldReturnFalse() {
        // Arrange
        Span<string> words = ["hi", "world", "test"];

        // Act
        bool result = words.All(x => x.Length >= 4);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task All_WithNegativeNumbers_AllElementsMatchPredicate_ShouldReturnTrue() {
        // Arrange
        Span<int> numbers = [-5, -10, -15];

        // Act
        bool result = numbers.All(x => x < 0);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task All_ShortCircuitEvaluation_ShouldStopOnFirstFalse() {
        // Arrange
        int callCount = 0;
        Span<int> numbers = [1, 2, 3, 4, 5];

        // Act
        bool result = numbers.All(x => {
            callCount++;
            return x < 3;// Will be false for elements 3, 4, 5
        });

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(callCount).IsLessThanOrEqualTo(3);// Should stop early
    }

}
