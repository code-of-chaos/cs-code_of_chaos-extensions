// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System;

namespace Tests.CodeOfChaos.Extensions.SpanLinq;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SpanLinqAverageTests {
    private record TestItem(double Value);

    [Test]
    public async Task Average_Span_WithPositiveNumbers_ShouldReturnCorrectAverage() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4, 5];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(3.0);
    }

    [Test]
    public async Task Average_Span_WithNegativeNumbers_ShouldReturnCorrectAverage() {
        // Arrange
        Span<int> numbers = [-2, -4, -6];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(-4.0);
    }

    [Test]
    public async Task Average_Span_WithMixedNumbers_ShouldReturnCorrectAverage() {
        // Arrange
        Span<int> numbers = [-10, 0, 10];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(0.0);
    }

    [Test]
    public async Task Average_Span_WithSingleElement_ShouldReturnThatElement() {
        // Arrange
        Span<int> numbers = [42];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(42.0);
    }

    [Test]
    public async Task Average_Span_WithDecimalValues_ShouldReturnCorrectAverage() {
        // Arrange
        Span<double> numbers = [1.5, 2.5, 3.5];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(2.5);
    }

    [Test]
    public async Task Average_Span_EmptySpan_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                Span<int> emptySpan = [];
                return emptySpan.Average(x => x);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }

    [Test]
    public async Task Average_ReadOnlySpan_WithPositiveNumbers_ShouldReturnCorrectAverage() {
        // Arrange
        ReadOnlySpan<int> numbers = [2, 4, 6, 8];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(5.0);
    }

    [Test]
    public async Task Average_ReadOnlySpan_WithNegativeNumbers_ShouldReturnCorrectAverage() {
        // Arrange
        ReadOnlySpan<int> numbers = [-1, -3, -5];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(-3.0);
    }

    [Test]
    public async Task Average_ReadOnlySpan_EmptySpan_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                ReadOnlySpan<int> emptySpan = [];
                return emptySpan.Average(x => x);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }

    [Test]
    public async Task Average_WithCustomType_ShouldReturnCorrectAverage() {
        // Arrange
        var items = new TestItem[] {
            new(10.0),
            new(20.0),
            new(30.0)
        };
        Span<TestItem> span = items;

        // Act
        double result = span.Average(x => x.Value);

        // Assert
        await Assert.That(result).IsEqualTo(20.0);
    }

    [Test]
    public async Task Average_WithStringLength_ShouldReturnCorrectAverage() {
        // Arrange
        Span<string> words = ["hi", "world", "test"];

        // Act
        double result = words.Average(x => x.Length);

        // Assert
        await Assert.That(result).IsEqualTo(3.6666666666666665);// (2 + 5 + 4) / 3
    }

    [Test]
    public async Task Average_WithComplexSelector_ShouldReturnCorrectAverage() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4];

        // Act
        double result = numbers.Average(x => x * x);// Average of squares

        // Assert
        await Assert.That(result).IsEqualTo(7.5);// (1 + 4 + 9 + 16) / 4 = 30 / 4
    }

    [Test]
    public async Task Average_WithLargeNumbers_ShouldHandleCorrectly() {
        // Arrange
        Span<long> numbers = [1000000L, 2000000L, 3000000L];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(2000000.0);
    }

    [Test]
    public async Task Average_WithFloatingPointPrecision_ShouldHandleCorrectly() {
        // Arrange
        Span<double> numbers = [0.1, 0.2, 0.3];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsBetween(0.2 - 0.0000001, 0.2 + 0.0000001);
    }

    [Test]
    public async Task Average_WithZeros_ShouldReturnZero() {
        // Arrange
        Span<int> numbers = [0, 0, 0, 0];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(0.0);
    }

    [Test]
    public async Task Average_WithVerySmallNumbers_ShouldHandleCorrectly() {
        // Arrange
        Span<double> numbers = [0.001, 0.002, 0.003];

        // Act
        double result = numbers.Average(x => x);

        // Assert
        await Assert.That(result).IsBetween(0.002 - 0.0000001, 0.002 + 0.0000001);

    }

    [Test]
    public async Task Average_WithSelectorReturningNegative_ShouldReturnCorrectAverage() {
        // Arrange
        Span<int> numbers = [1, 2, 3];

        // Act
        double result = numbers.Average(x => -x);// Negate all values

        // Assert
        await Assert.That(result).IsEqualTo(-2.0);// (-1 + -2 + -3) / 3
    }

    [Test]
    public async Task Average_WithCustomTypeComplexSelector_ShouldReturnCorrectAverage() {
        // Arrange
        var items = new TestItem[] {
            new(5.0),
            new(15.0),
            new(25.0)
        };
        Span<TestItem> span = items;

        // Act
        double result = span.Average(x => x.Value * 2);// Double each value

        // Assert
        await Assert.That(result).IsEqualTo(30.0);// (10 + 30 + 50) / 3
    }
}
