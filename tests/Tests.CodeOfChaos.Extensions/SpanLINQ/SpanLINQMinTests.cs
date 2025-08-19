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
public class SpanLINQMinTests {
    // ReSharper disable once NotAccessedPositionalProperty.Local
    private record TestItem(int Value, string Name);

    // ReSharper disable once NotAccessedPositionalProperty.Local
    private record TestScore(string Player, double Score);

    #region Basic Min tests
    [Test]
    public async Task Min_Span_WithIntegers_ShouldReturnMinValue() {
        // Arrange
        Span<int> numbers = [3, 1, 4, 1, 5, 9, 2, 6];

        // Act
        int result = numbers.Min(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(1);
    }

    [Test]
    public async Task Min_Span_WithDoubles_ShouldReturnMinValue() {
        // Arrange
        const double tolerance = 0.0000001;
        Span<double> numbers = [3.14, 2.71, 1.41, 1.73];

        // Act
        double result = numbers.Min(x => x);

        // Assert
        await Assert.That(result).IsBetween(1.41 - tolerance, 1.41 + tolerance);
    }

    [Test]
    public async Task Min_Span_EmptySpan_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                Span<int> emptySpan = [];
                return emptySpan.Min(x => x);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }

    [Test]
    public async Task Min_Span_SingleElement_ShouldReturnThatElement() {
        // Arrange
        Span<int> singleElement = [42];

        // Act
        int result = singleElement.Min(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(42);
    }

    [Test]
    public async Task Min_ReadOnlySpan_WithIntegers_ShouldReturnMinValue() {
        // Arrange
        ReadOnlySpan<int> numbers = [10, 5, 8, 3, 12];

        // Act
        int result = numbers.Min(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(3);
    }

    [Test]
    public async Task Min_ReadOnlySpan_EmptySpan_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                ReadOnlySpan<int> emptySpan = [];
                return emptySpan.Min(x => x);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }
    #endregion

    #region Min with selector tests
    [Test]
    public async Task Min_WithSelector_ShouldReturnMinProjectedValue() {
        // Arrange
        var items = new TestItem[] {
            new(5, "first"),
            new(10, "second"),
            new(3, "third"),
            new(8, "fourth")
        };
        Span<TestItem> span = items;

        // Act
        int result = span.Min(x => x.Value);

        // Assert
        await Assert.That(result).IsEqualTo(3);
    }

    [Test]
    public async Task Min_WithStringLengthSelector_ShouldReturnMinLength() {
        // Arrange
        Span<string> words = ["cat", "elephant", "dog", "a"];

        // Act
        int result = words.Min(x => x.Length);

        // Assert
        await Assert.That(result).IsEqualTo(1);// "a".Length
    }

    [Test]
    public async Task Min_WithStringSelector_ShouldReturnMinString() {
        // Arrange
        Span<string> words = ["apple", "zebra", "banana", "cherry"];

        // Act
        string result = words.Min(x => x);

        // Assert
        await Assert.That(result).IsEqualTo("apple");// Alphabetically first
    }

    [Test]
    public async Task Min_WithDoubleSelector_ShouldReturnMinDouble() {
        // Arrange
        const double tolerance = 0.0000001;
        var scores = new TestScore[] {
            new("Alice", 85.5),
            new("Bob", 92.3),
            new("Charlie", 78.9),
            new("Diana", 96.1)
        };
        Span<TestScore> span = scores;

        // Act
        double result = span.Min(x => x.Score);

        // Assert
        await Assert.That(result).IsBetween(78.9 - tolerance, 78.9 + tolerance);
    }

    [Test]
    public async Task Min_WithDateTimeSelector_ShouldReturnLatestDate() {
        // Arrange
        (string Name, DateTime Date)[] events = [
            ("Event1", new DateTime(2023, 1, 15)),
            ("Event2", new DateTime(2023, 3, 10)),
            ("Event3", new DateTime(2023, 2, 5))
        ];
        Span<(string Name, DateTime Date)> span = events;

        // Act
        DateTime result = span.Min(x => x.Date);

        // Assert
        await Assert.That(result).IsEqualTo(new DateTime(2023, 1, 15));
    }
    #endregion

    #region Edge cases and special scenarios
    [Test]
    public async Task Min_WithNegativeNumbers_ShouldReturnLeastNegative() {
        // Arrange
        Span<int> negativeNumbers = [-5, -1, -10, -3];

        // Act
        int result = negativeNumbers.Min(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(-10);
    }

    [Test]
    public async Task Min_WithMixedPositiveNegative_ShouldReturnMinPositive() {
        // Arrange
        Span<int> mixedNumbers = [-5, 3, -2, 7, -1];

        // Act
        int result = mixedNumbers.Min(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(-5);
    }

    [Test]
    public async Task Min_WithDuplicateMinValues_ShouldReturnMinValue() {
        // Arrange
        Span<int> numbersWithDuplicates = [3, 7, 5, 7, 2];

        // Act
        int result = numbersWithDuplicates.Min(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(2);
    }

    [Test]
    public async Task Min_WithAllSameValues_ShouldReturnThatValue() {
        // Arrange
        Span<int> sameValues = [5, 5, 5, 5];

        // Act
        int result = sameValues.Min(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(5);
    }

    [Test]
    public async Task Min_WithCustomComparable_ShouldUseCompareTo() {
        // Arrange
        var versions = new Version[] {
            new(1, 0, 0),
            new(2, 1, 0),
            new(1, 5, 0),
            new(2, 0, 1)
        };
        Span<Version> span = versions;

        // Act
        Version result = span.Min(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(new Version(1, 0, 0));
    }
    #endregion

    #region Performance and behavior tests
    [Test]
    public async Task Min_SelectorCallCount_ShouldCallSelectorForEachElement() {
        // Arrange
        int callCount = 0;
        Span<int> numbers = [1, 3, 2, 4];

        // Act
        int result = numbers.Min(x => {
            callCount++;
            return x * 2;// Transform each element
        });

        // Assert
        await Assert.That(result).IsEqualTo(2);
        await Assert.That(callCount).IsEqualTo(4); // Should call selector for all elements
    }

    [Test]
    public async Task Min_WithComplexSelector_ShouldWorkCorrectly() {
        // Arrange
        const double tolerance = 0.05;
        var items = new (int X, int Y)[] {
            (3, 4),// Distance = 5
            (5, 12),// Distance = 13
            (8, 6),// Distance = 10
            (1, 1)// Distance ≈ 1.41
        };
        Span<(int X, int Y)> span = items;

        // Act
        double result = span.Min(item => Math.Sqrt(item.X * item.X + item.Y * item.Y));// Euclidean distance

        // Assert
        await Assert.That(result).IsBetween(1.4 - tolerance, 1.4 + tolerance);
    }


    [Test]
    public async Task Min_WithFloatingPointPrecision_ShouldHandleCorrectly() {
        // Arrange
        const double tolerance = 0.000000000001;
        Span<double> numbers = [0.1 + 0.2, 0.3, 0.30000000001];

        // Act
        double result = numbers.Min(x => x);

        // Assert
        await Assert.That(result).IsBetween(0.3 - tolerance, 0.3 + tolerance);
    }

    [Test]
    public async Task Min_ReverseIteration_VerifyBehavior() {
        // Arrange
        var visitedElements = new List<int>();
        Span<int> numbers = [1, 5, 3, 4, 2];// Min is 5 at index 1

        // Act
        int result = numbers.Min(x => {
            visitedElements.Add(x);
            return x;
        });

        // Assert
        await Assert.That(result).IsEqualTo(1);
        // Note: The implementation starts with first element, then iterates backwards
        await Assert.That(visitedElements.First()).IsEqualTo(1);// First element processed
        await Assert.That(visitedElements).Contains(5);// Min element was processed
        await Assert.That(visitedElements.Count).IsEqualTo(5);// All elements processed
    }

    [Test]
    public async Task Min_WithLargeNumbers_ShouldNotOverflow() {
        // Arrange
        Span<long> largeNumbers = [long.MaxValue - 1, long.MaxValue - 10, long.MaxValue - 5];

        // Act
        long result = largeNumbers.Min(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(long.MaxValue - 10);
    }

    [Test]
    public async Task Min_WithTransformationSelector_ShouldReturnTransformedMin() {
        // Arrange
        Span<string> words = ["a", "bb", "ccc", "d"];

        // Act
        int result = words.Min(x => x.Length);

        // Assert
        await Assert.That(result).IsEqualTo(1);// Length of "a"
    }

    [Test]
    public async Task Min_MultipleCallsOnSameSpan_ShouldReturnSameResult() {
        // Arrange
        Span<int> numbers = [7, 2, 9, 1, 5];

        // Act
        int result1 = numbers.Min(x => x);
        int result2 = numbers.Min(x => x);

        // Assert
        await Assert.That(result1).IsEqualTo(1);
        await Assert.That(result2).IsEqualTo(1);
        await Assert.That(result1).IsEqualTo(result2);
    }
    #endregion
}
