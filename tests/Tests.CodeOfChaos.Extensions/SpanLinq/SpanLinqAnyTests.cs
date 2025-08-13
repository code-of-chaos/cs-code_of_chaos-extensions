// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.SpanLINQ;

namespace Tests.CodeOfChaos.Extensions.SpanLinq;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SpanLinqAnyTests {
    private record TestItem(int Value);

    #region Any() - without predicate tests
    [Test]
    public async Task Any_Span_WithNonNullElements_ShouldReturnTrue() {
        // Arrange
        Span<string> words = ["hello", "world"];

        // Act
        bool result = words.Any();

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Any_Span_WithSomeNullElements_ShouldReturnTrue() {
        // Arrange
        Span<string?> words = [null, "world", null];

        // Act
        bool result = words.Any();

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Any_Span_WithAllNullElements_ShouldReturnFalse() {
        // Arrange
        Span<string?> words = [null, null, null];

        // Act
        bool result = words.Any();

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Any_Span_EmptySpan_ShouldReturnFalse() {
        // Arrange
        Span<string> emptySpan = [];

        // Act
        bool result = emptySpan.Any();

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Any_ReadOnlySpan_WithNonNullElements_ShouldReturnTrue() {
        // Arrange
        ReadOnlySpan<string> words = ["test"];

        // Act
        bool result = words.Any();

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Any_ReadOnlySpan_WithAllNullElements_ShouldReturnFalse() {
        // Arrange
        ReadOnlySpan<string?> words = [null, null];

        // Act
        bool result = words.Any();

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Any_ValueTypes_ShouldAlwaysReturnTrueForNonEmpty() {
        // Arrange - value types are never null
        Span<int> numbers = [0, 1, 2];

        // Act
        bool result = numbers.Any();

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Any_ValueTypesWithZero_ShouldReturnTrue() {
        // Arrange - even zero is not null for value types
        Span<int> numbers = [0];

        // Act
        bool result = numbers.Any();

        // Assert
        await Assert.That(result).IsTrue();
    }
    #endregion

    #region Any(predicate) tests
    [Test]
    public async Task Any_Span_WithPredicate_SomeElementsMatch_ShouldReturnTrue() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4];

        // Act
        bool result = numbers.Any(x => x % 2 == 0);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Any_Span_WithPredicate_NoElementsMatch_ShouldReturnFalse() {
        // Arrange
        Span<int> numbers = [1, 3, 5, 7];

        // Act
        bool result = numbers.Any(x => x % 2 == 0);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Any_Span_WithPredicate_EmptySpan_ShouldReturnFalse() {
        // Arrange
        Span<int> emptySpan = [];

        // Act
        bool result = emptySpan.Any(x => x > 0);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Any_Span_WithPredicate_SingleElementMatches_ShouldReturnTrue() {
        // Arrange
        Span<int> singleElement = [10];

        // Act
        bool result = singleElement.Any(x => x > 5);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Any_Span_WithPredicate_SingleElementDoesNotMatch_ShouldReturnFalse() {
        // Arrange
        Span<int> singleElement = [3];

        // Act
        bool result = singleElement.Any(x => x > 5);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Any_ReadOnlySpan_WithPredicate_SomeElementsMatch_ShouldReturnTrue() {
        // Arrange
        ReadOnlySpan<int> numbers = [2, 4, 6];

        // Act
        bool result = numbers.Any(x => x > 3);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Any_ReadOnlySpan_WithPredicate_NoElementsMatch_ShouldReturnFalse() {
        // Arrange
        ReadOnlySpan<int> numbers = [1, 2, 3];

        // Act
        bool result = numbers.Any(x => x > 10);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Any_WithCustomType_SomeElementsMatch_ShouldReturnTrue() {
        // Arrange
        var items = new TestItem[] {
            new(5),
            new(10),
            new(15)
        };
        Span<TestItem> span = items;

        // Act
        bool result = span.Any(x => x.Value >= 10);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Any_WithCustomType_NoElementsMatch_ShouldReturnFalse() {
        // Arrange
        var items = new TestItem[] {
            new(1),
            new(2),
            new(3)
        };
        Span<TestItem> span = items;

        // Act
        bool result = span.Any(x => x.Value > 10);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Any_WithStringType_SomeElementsMatch_ShouldReturnTrue() {
        // Arrange
        Span<string> words = ["hi", "world", "test"];

        // Act
        bool result = words.Any(x => x.Length > 4);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Any_WithStringType_NoElementsMatch_ShouldReturnFalse() {
        // Arrange
        Span<string> words = ["hi", "go", "ok"];

        // Act
        bool result = words.Any(x => x.Length > 4);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Any_ShortCircuitEvaluation_ShouldStopOnFirstTrue() {
        // Arrange
        var callCount = 0;
        Span<int> numbers = [1, 2, 3, 4, 5];

        // Act
        bool result = numbers.Any(x => {
            callCount++;
            return x > 2;// Will be true for elements 3, 4, 5
        });

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(callCount).IsLessThanOrEqualTo(3);// Should stop early when finding 3
    }

    [Test]
    public async Task Any_WithNegativeNumbers_SomeElementsMatch_ShouldReturnTrue() {
        // Arrange
        Span<int> numbers = [-5, -10, 5];

        // Act
        bool result = numbers.Any(x => x > 0);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Any_WithNullableValueType_ShouldHandleNullsCorrectly() {
        // Arrange
        Span<int?> numbers = [null, 2, null, 4];

        // Act
        bool resultAny = numbers.Any();// This checks for non-null
        bool resultPredicate = numbers.Any(x => x.HasValue && x.Value > 3);

        // Assert
        await Assert.That(resultAny).IsTrue();// Has non-null elements
        await Assert.That(resultPredicate).IsTrue();// 4 > 3
    }
    #endregion
}
