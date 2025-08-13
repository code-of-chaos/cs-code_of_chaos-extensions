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
public class SpanLINQMaxTests {
    // ReSharper disable once NotAccessedPositionalProperty.Local
    private record TestItem(int Value, string Name);

    // ReSharper disable once NotAccessedPositionalProperty.Local
    private record TestScore(string Player, double Score);

    #region Basic Max tests
    [Test]
    public async Task Max_Span_WithIntegers_ShouldReturnMaxValue() {
        // Arrange
        Span<int> numbers = [3, 1, 4, 1, 5, 9, 2, 6];

        // Act
        int result = numbers.Max(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(9);
    }

    [Test]
    public async Task Max_Span_WithDoubles_ShouldReturnMaxValue() {
        // Arrange
        const double tolerance = 0.0000001;
        Span<double> numbers = [3.14, 2.71, 1.41, 1.73];

        // Act
        double result = numbers.Max(x => x);

        // Assert
        await Assert.That(result).IsBetween(3.14 - tolerance, 3.14 + tolerance);
    }

    [Test]
    public async Task Max_Span_EmptySpan_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                Span<int> emptySpan = [];
                return emptySpan.Max(x => x);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }

    [Test]
    public async Task Max_Span_SingleElement_ShouldReturnThatElement() {
        // Arrange
        Span<int> singleElement = [42];

        // Act
        int result = singleElement.Max(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(42);
    }

    [Test]
    public async Task Max_ReadOnlySpan_WithIntegers_ShouldReturnMaxValue() {
        // Arrange
        ReadOnlySpan<int> numbers = [10, 5, 8, 3, 12];

        // Act
        int result = numbers.Max(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(12);
    }

    [Test]
    public async Task Max_ReadOnlySpan_EmptySpan_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                ReadOnlySpan<int> emptySpan = [];
                return emptySpan.Max(x => x);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }
    #endregion

    #region Max with selector tests
    [Test]
    public async Task Max_WithSelector_ShouldReturnMaxProjectedValue() {
        // Arrange
        var items = new TestItem[] {
            new(5, "first"),
            new(10, "second"),
            new(3, "third"),
            new(8, "fourth")
        };
        Span<TestItem> span = items;

        // Act
        int result = span.Max(x => x.Value);

        // Assert
        await Assert.That(result).IsEqualTo(10);
    }

    [Test]
    public async Task Max_WithStringLengthSelector_ShouldReturnMaxLength() {
        // Arrange
        Span<string> words = ["cat", "elephant", "dog", "a"];

        // Act
        int result = words.Max(x => x.Length);

        // Assert
        await Assert.That(result).IsEqualTo(8);// "elephant".Length
    }

    [Test]
    public async Task Max_WithStringSelector_ShouldReturnMaxString() {
        // Arrange
        Span<string> words = ["apple", "zebra", "banana", "cherry"];

        // Act
        string result = words.Max(x => x);

        // Assert
        await Assert.That(result).IsEqualTo("zebra");// Alphabetically last
    }

    [Test]
    public async Task Max_WithDoubleSelector_ShouldReturnMaxDouble() {
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
        double result = span.Max(x => x.Score);

        // Assert
        await Assert.That(result).IsBetween(96.1 - tolerance, 96.1 + tolerance);
    }

    [Test]
    public async Task Max_WithDateTimeSelector_ShouldReturnLatestDate() {
        // Arrange
        (string Name, DateTime Date)[] events = [
            ("Event1", new DateTime(2023, 1, 15)),
            ("Event2", new DateTime(2023, 3, 10)),
            ("Event3", new DateTime(2023, 2, 5))
        ];
        Span<(string Name, DateTime Date)> span = events;

        // Act
        DateTime result = span.Max(x => x.Date);

        // Assert
        await Assert.That(result).IsEqualTo(new DateTime(2023, 3, 10));
    }
    #endregion

    #region Edge cases and special scenarios
    [Test]
    public async Task Max_WithNegativeNumbers_ShouldReturnLeastNegative() {
        // Arrange
        Span<int> negativeNumbers = [-5, -1, -10, -3];

        // Act
        int result = negativeNumbers.Max(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(-1);// Least negative = maximum
    }

    [Test]
    public async Task Max_WithMixedPositiveNegative_ShouldReturnMaxPositive() {
        // Arrange
        Span<int> mixedNumbers = [-5, 3, -2, 7, -1];

        // Act
        int result = mixedNumbers.Max(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(7);
    }

    [Test]
    public async Task Max_WithDuplicateMaxValues_ShouldReturnMaxValue() {
        // Arrange
        Span<int> numbersWithDuplicates = [3, 7, 5, 7, 2];

        // Act
        int result = numbersWithDuplicates.Max(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(7);
    }

    [Test]
    public async Task Max_WithAllSameValues_ShouldReturnThatValue() {
        // Arrange
        Span<int> sameValues = [5, 5, 5, 5];

        // Act
        int result = sameValues.Max(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(5);
    }

    [Test]
    public async Task Max_WithCustomComparable_ShouldUseCompareTo() {
        // Arrange
        var versions = new Version[] {
            new(1, 0, 0),
            new(2, 1, 0),
            new(1, 5, 0),
            new(2, 0, 1)
        };
        Span<Version> span = versions;

        // Act
        Version result = span.Max(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(new Version(2, 1, 0));
    }
    #endregion

    #region Performance and behavior tests
    [Test]
    public async Task Max_SelectorCallCount_ShouldCallSelectorForEachElement() {
        // Arrange
        var callCount = 0;
        Span<int> numbers = [1, 3, 2, 4];

        // Act
        int result = numbers.Max(x => {
            callCount++;
            return x * 2;// Transform each element
        });

        // Assert
        await Assert.That(result).IsEqualTo(8);// 4 * 2 = 8
        await Assert.That(callCount).IsEqualTo(4);// Should call selector for all elements
    }

    [Test]
    public async Task Max_WithComplexSelector_ShouldWorkCorrectly() {
        // Arrange
        const double tolerance = 0.0000001;
        var items = new (int X, int Y)[] {
            (3, 4),// Distance = 5
            (5, 12),// Distance = 13
            (8, 6),// Distance = 10
            (1, 1)// Distance ≈ 1.41
        };
        Span<(int X, int Y)> span = items;

        // Act
        double result = span.Max(item => Math.Sqrt(item.X * item.X + item.Y * item.Y));// Euclidean distance

        // Assert
        await Assert.That(result).IsBetween(13.0 - tolerance, 13.0 + tolerance);
    }


    [Test]
    public async Task Max_WithFloatingPointPrecision_ShouldHandleCorrectly() {
        // Arrange
        const double tolerance = 0.0000000001;
        Span<double> numbers = [0.1 + 0.2, 0.3, 0.30000000001];

        // Act
        double result = numbers.Max(x => x);

        // Assert
        await Assert.That(result).IsBetween(0.3 - tolerance, 0.3 + tolerance);
    }

    [Test]
    public async Task Max_ReverseIteration_VerifyBehavior() {
        // Arrange
        var visitedElements = new List<int>();
        Span<int> numbers = [1, 5, 3, 4, 2];// Max is 5 at index 1

        // Act
        int result = numbers.Max(x => {
            visitedElements.Add(x);
            return x;
        });

        // Assert
        await Assert.That(result).IsEqualTo(5);
        // Note: The implementation starts with first element, then iterates backwards
        await Assert.That(visitedElements.First()).IsEqualTo(1);// First element processed
        await Assert.That(visitedElements).Contains(5);// Max element was processed
        await Assert.That(visitedElements.Count).IsEqualTo(5);// All elements processed
    }

    [Test]
    public async Task Max_WithLargeNumbers_ShouldNotOverflow() {
        // Arrange
        Span<long> largeNumbers = [long.MaxValue - 1, long.MaxValue - 10, long.MaxValue - 5];

        // Act
        long result = largeNumbers.Max(x => x);

        // Assert
        await Assert.That(result).IsEqualTo(long.MaxValue - 1);
    }

    [Test]
    public async Task Max_WithTransformationSelector_ShouldReturnTransformedMax() {
        // Arrange
        Span<string> words = ["a", "bb", "ccc", "d"];

        // Act
        int result = words.Max(x => x.Length);

        // Assert
        await Assert.That(result).IsEqualTo(3);// Length of "ccc"
    }

    [Test]
    public async Task Max_MultipleCallsOnSameSpan_ShouldReturnSameResult() {
        // Arrange
        Span<int> numbers = [7, 2, 9, 1, 5];

        // Act
        int result1 = numbers.Max(x => x);
        int result2 = numbers.Max(x => x);

        // Assert
        await Assert.That(result1).IsEqualTo(9);
        await Assert.That(result2).IsEqualTo(9);
        await Assert.That(result1).IsEqualTo(result2);
    }
    #endregion
}
