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
public class SpanLINQFirstTests {
    private record TestItem(int Value, string Name);

    #region First tests
    [Test]
    public async Task First_Span_WithMatchingElements_ShouldReturnFirstMatch() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4, 5];

        // Act
        int result = numbers.First(x => x > 2);

        // Assert
        await Assert.That(result).IsEqualTo(3);
    }

    [Test]
    public async Task First_Span_WithNoMatchingElements_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                Span<int> numbers = [1, 2, 3];
                return numbers.First(x => x > 10);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }

    [Test]
    public async Task First_Span_EmptySpan_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                Span<int> emptySpan = [];
                return emptySpan.First(x => x > 0);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }

    [Test]
    public async Task First_Span_SingleElementMatches_ShouldReturnThatElement() {
        // Arrange
        Span<int> singleElement = [42];

        // Act
        int result = singleElement.First(x => x > 40);

        // Assert
        await Assert.That(result).IsEqualTo(42);
    }

    [Test]
    public async Task First_Span_SingleElementDoesNotMatch_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                Span<int> singleElement = [5];
                return singleElement.First(x => x > 10);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }

    [Test]
    public async Task First_Span_MultipleMatches_ShouldReturnFirstMatch() {
        // Arrange
        Span<int> numbers = [2, 4, 6, 8];

        // Act
        int result = numbers.First(x => x % 2 == 0);

        // Assert
        await Assert.That(result).IsEqualTo(2);// First even number
    }

    [Test]
    public async Task First_ReadOnlySpan_WithMatchingElements_ShouldReturnFirstMatch() {
        // Arrange
        ReadOnlySpan<int> numbers = [10, 20, 30];

        // Act
        int result = numbers.First(x => x >= 20);

        // Assert
        await Assert.That(result).IsEqualTo(20);
    }

    [Test]
    public async Task First_ReadOnlySpan_WithNoMatchingElements_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                ReadOnlySpan<int> numbers = [1, 2, 3];
                return numbers.First(x => x > 5);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }

    [Test]
    public async Task First_WithCustomType_ShouldReturnFirstMatch() {
        // Arrange
        var items = new TestItem[] {
            new(5, "first"),
            new(10, "second"),
            new(15, "third")
        };
        Span<TestItem> span = items;

        // Act
        TestItem result = span.First(x => x.Value >= 10);

        // Assert
        await Assert.That(result.Value).IsEqualTo(10);
        await Assert.That(result.Name).IsEqualTo("second");
    }

    [Test]
    public async Task First_WithStringType_ShouldReturnFirstMatch() {
        // Arrange
        Span<string> words = ["hi", "world", "test"];

        // Act
        string result = words.First(x => x.Length > 3);

        // Assert
        await Assert.That(result).IsEqualTo("world");
    }
    #endregion

    #region FirstOrDefault tests
    [Test]
    public async Task FirstOrDefault_Span_WithMatchingElements_ShouldReturnFirstMatch() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4, 5];

        // Act
        int? result = numbers.FirstOrDefault(x => x > 3);

        // Assert
        await Assert.That(result).IsEqualTo(4);
    }

    [Test]
    public async Task FirstOrDefault_Span_WithNoMatchingElements_ShouldReturnDefault() {
        // Arrange
        Span<int> numbers = [1, 2, 3];

        // Act
        int? result = numbers.FirstOrDefault(x => x > 10);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task FirstOrDefault_Span_EmptySpan_ShouldReturnDefault() {
        // Arrange
        Span<int> emptySpan = [];

        // Act
        int? result = emptySpan.FirstOrDefault(x => x > 0);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task FirstOrDefault_Span_SingleElementMatches_ShouldReturnThatElement() {
        // Arrange
        Span<int> singleElement = [7];

        // Act
        int? result = singleElement.FirstOrDefault(x => x < 10);

        // Assert
        await Assert.That(result).IsEqualTo(7);
    }

    [Test]
    public async Task FirstOrDefault_Span_SingleElementDoesNotMatch_ShouldReturnDefault() {
        // Arrange
        Span<int> singleElement = [5];

        // Act
        int? result = singleElement.FirstOrDefault(x => x > 10);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task FirstOrDefault_ReadOnlySpan_WithMatchingElements_ShouldReturnFirstMatch() {
        // Arrange
        ReadOnlySpan<int> numbers = [100, 200, 300];

        // Act
        int? result = numbers.FirstOrDefault(x => x >= 150);

        // Assert
        await Assert.That(result).IsEqualTo(200);
    }

    [Test]
    public async Task FirstOrDefault_ReadOnlySpan_WithNoMatchingElements_ShouldReturnDefault() {
        // Arrange
        ReadOnlySpan<int> numbers = [1, 2, 3];

        // Act
        int? result = numbers.FirstOrDefault(x => x > 5);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task FirstOrDefault_WithCustomType_ShouldReturnFirstMatch() {
        // Arrange
        var items = new TestItem[] {
            new(3, "small"),
            new(8, "medium"),
            new(12, "large")
        };
        Span<TestItem> span = items;

        // Act
        TestItem? result = span.FirstOrDefault(x => x.Value > 5);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Value).IsEqualTo(8);
        await Assert.That(result.Name).IsEqualTo("medium");
    }

    [Test]
    public async Task FirstOrDefault_WithCustomType_NoMatch_ShouldReturnNull() {
        // Arrange
        var items = new TestItem[] {
            new(1, "one"),
            new(2, "two")
        };
        Span<TestItem> span = items;

        // Act
        TestItem? result = span.FirstOrDefault(x => x.Value > 10);

        // Assert
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task FirstOrDefault_WithStringType_ShouldReturnFirstMatch() {
        // Arrange
        Span<string> words = ["cat", "elephant", "dog"];

        // Act
        string? result = words.FirstOrDefault(x => x.Length > 5);

        // Assert
        await Assert.That(result).IsEqualTo("elephant");
    }

    [Test]
    public async Task FirstOrDefault_WithStringType_NoMatch_ShouldReturnNull() {
        // Arrange
        Span<string> words = ["a", "hi", "go"];

        // Act
        string? result = words.FirstOrDefault(x => x.Length > 10);

        // Assert
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task FirstOrDefault_WithValueType_NoMatch_ShouldReturnDefaultValue() {
        // Arrange
        Span<double> numbers = [1.1, 2.2, 3.3];

        // Act
        double? result = numbers.FirstOrDefault(x => x > 5.0);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task FirstOrDefault_WithNullableValueType_ShouldHandleNulls() {
        // Arrange
        Span<int?> numbers = [null, 5, null, 10];

        // Act
        int? result = numbers.FirstOrDefault(x => x is > 3);

        // Assert
        await Assert.That(result).IsEqualTo(5);
    }

    [Test]
    public async Task First_ShortCircuitEvaluation_ShouldStopOnFirstMatch() {
        // Arrange
        int callCount = 0;
        Span<int> numbers = [1, 2, 3, 4, 5];

        // Act
        int result = numbers.First(x => {
            callCount++;
            return x > 2;// Will match on element 3 (third call)
        });

        // Assert
        await Assert.That(result).IsEqualTo(3);
        await Assert.That(callCount).IsEqualTo(3);// Should have called predicate 3 times
    }

    [Test]
    public async Task FirstOrDefault_ShortCircuitEvaluation_ShouldStopOnFirstMatch() {
        // Arrange
        var callCount = 0;
        Span<int> numbers = [5, 10, 15, 20];

        // Act
        int? result = numbers.FirstOrDefault(x => {
            callCount++;
            return x >= 10;// Will match on the second element
        });

        // Assert
        await Assert.That(result).IsEqualTo(10);
        await Assert.That(callCount).IsEqualTo(2);// Should have called predicate 2 times
    }
    #endregion
}
