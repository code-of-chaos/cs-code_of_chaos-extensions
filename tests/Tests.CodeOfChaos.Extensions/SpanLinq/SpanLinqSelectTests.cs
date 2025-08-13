// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.SpanLINQ;

namespace Tests.CodeOfChaos.Extensions.SpanLinq;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SpanLinqSelectTests {
    // ReSharper disable once NotAccessedPositionalProperty.Local
    private record Person(string Name, int Age);
    
    #region Select tests
    
    [Test]
    public async Task Select_Span_WithIntegerTransformation_ShouldTransformAllElements() {
        // Arrange
        Span<int> source = [1, 2, 3, 4, 5];
        Span<int> destination = new int[5];
        
        // Act
        source.Select(x => x * 2, destination);
        
        // Assert
        await Assert.That(destination.ToArray()).IsEquivalentTo([2, 4, 6, 8, 10]);
    }
    
    [Test]
    public async Task Select_ReadOnlySpan_WithStringTransformation_ShouldTransformAllElements() {
        // Arrange
        ReadOnlySpan<string> source = ["hello", "world", "test"];
        Span<int> destination = new int[3];
        
        // Act
        source.Select(s => s.Length, destination);
        
        // Assert
        await Assert.That(destination.ToArray()).IsEquivalentTo([5, 5, 4]);
    }
    
    [Test]
    public async Task Select_WithCustomTypes_ShouldTransformCorrectly() {
        // Arrange
        var people = new Person[] {
            new("Alice", 25),
            new("Bob", 30),
            new("Charlie", 35)
        };
        Span<Person> source = people;
        Span<string> destination = new string[3];
        
        // Act
        source.Select(p => p.Name, destination);
        
        // Assert
        await Assert.That(destination.ToArray()).IsEquivalentTo(["Alice", "Bob", "Charlie"]);
    }
    
    [Test]
    public async Task Select_EmptySpan_ShouldNotModifyDestination() {
        // Arrange
        Span<int> source = [];
        Span<string> destination = [];
        
        // Act & Assert (should not throw)
        source.Select(x => x.ToString(), destination);
        
        await Assert.That(destination.Length).IsEquivalentTo(0);
    }
    
    [Test]
    public async Task Select_SingleElement_ShouldTransformSingleElement() {
        // Arrange
        Span<double> source = [3.14];
        Span<int> destination = new int[1];
        
        // Act
        source.Select(x => (int)Math.Round(x), destination);
        
        // Assert
        await Assert.That(destination[0]).IsEquivalentTo(3);
    }
    
    [Test]
    public async Task Select_DestinationTooSmall_ShouldThrowArgumentException() {
        // Arrange
        
        // Act & Assert
        await Assert.That(() => {
                Span<int> source = [1, 2, 3];
                Span<int> destination = new int[2]; // Too small
                source.Select(x => x * 2, destination);
            })
            .Throws<ArgumentException>()
            .WithMessage("Destination span is too small");
    }
    
    [Test]
    public async Task Select_SelectorCallCount_ShouldCallSelectorForEachElement() {
        // Arrange
        int callCount = 0;
        Span<int> source = [1, 2, 3];
        Span<int> destination = new int[3];
        
        // Act
        source.Select(x => {
            callCount++;
            return x * 2;
        }, destination);
        int[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(callCount).IsEquivalentTo(3);
        await Assert.That(destinationResult).IsEquivalentTo([2, 4, 6]);
    }
    
    [Test]
    public async Task Select_WithComplexTransformation_ShouldWorkCorrectly() {
        // Arrange
        var points = new (int X, int Y)[] {
            (3, 4),   // Distance = 5
            (0, 0),   // Distance = 0
            (1, 1)    // Distance ≈ 1.41
        };
        Span<(int X, int Y)> source = points;
        Span<double> destination = new double[3];
        
        // Act
        source.Select(p => Math.Sqrt(p.X * p.X + p.Y * p.Y), destination);
        double[] destinationResult = destination.ToArray();
        
        // Assert
        const double tolerance = 0.0000001;
        await Assert.That(Math.Abs(destinationResult[0] - 5.0)).IsLessThan(tolerance);
        await Assert.That(Math.Abs(destinationResult[1] - 0.0)).IsLessThan(tolerance);
        await Assert.That(Math.Abs(destinationResult[2] - 1.41421356)).IsLessThan(tolerance);
    }
    
    #endregion
    
    #region SelectMany tests
    
    [Test]
    public async Task SelectMany_WithArrays_ShouldFlattenCorrectly() {
        // Arrange
        int[][] arrays = new int[][] {
            [1, 2],
            [3, 4, 5],
            [6]
        };
        Span<int[]> source = arrays;
        Span<int> destination = new int[10];
        
        // Act
        source.SelectMany(arr => arr.AsSpan(), destination, out int totalCount);
        int[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(6);
        await Assert.That(destinationResult[..totalCount].ToArray()).IsEquivalentTo([1, 2, 3, 4, 5, 6]);
    }
    
    [Test]
    public async Task SelectMany_WithStrings_ShouldFlattenCharacters() {
        // Arrange
        Span<string> source = ["hi", "world"];
        Span<char> destination = new char[10];
        
        // Act
        source.SelectMany(s => s.AsSpan(), destination, out int totalCount);
        char[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(7);
        await Assert.That(destinationResult[..totalCount].ToArray()).IsEquivalentTo(['h', 'i', 'w', 'o', 'r', 'l', 'd']);
    }
    
    [Test]
    public async Task SelectMany_EmptySource_ShouldReturnZeroCount() {
        // Arrange
        Span<int[]> source = [];
        Span<int> destination = new int[5];
        
        // Act
        source.SelectMany(arr => arr.AsSpan(), destination, out int totalCount);
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(0);
    }
    
    [Test]
    public async Task SelectMany_WithEmptyNestedSpans_ShouldHandleCorrectly() {
        // Arrange
        int[][] arrays = new int[][] {
            [1, 2],
            [], // Empty array
            [3, 4]
        };
        Span<int[]> source = arrays;
        Span<int> destination = new int[10];
        
        // Act
        source.SelectMany(arr => arr.AsSpan(), destination, out int totalCount);
        int[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(4);
        await Assert.That(destinationResult[..totalCount].ToArray()).IsEquivalentTo([1, 2, 3, 4]);
    }
    
    [Test]
    public async Task SelectMany_SingleElementWithMultipleNested_ShouldFlattenCorrectly() {
        // Arrange
        Span<string> source = ["hello"];
        Span<char> destination = new char[10];
        
        // Act
        source.SelectMany(s => s.AsSpan(), destination, out int totalCount);
        char[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(5);
        await Assert.That(destinationResult[..totalCount].ToArray()).IsEquivalentTo(['h', 'e', 'l', 'l', 'o']);
    }
    
    [Test]
    public async Task SelectMany_DestinationTooSmallForSource_ShouldThrowArgumentException() {
        // Arrange
        int[][] arrays = new int[][] { [1, 2, 3], [4, 5] };
        
        // Act & Assert
        await Assert.That(() => {
                Span<int[]> source = arrays;
                Span<int> destination = new int[1]; // Too small for source.Length
                source.SelectMany(arr => arr.AsSpan(), destination, out _);
            })
            .Throws<ArgumentException>()
            .WithMessage("Destination span is too small");
    }
    
    [Test]
    public async Task SelectMany_DestinationTooSmallForFlattened_ShouldThrowArgumentException() {
        // Arrange
        int[][] arrays = new int[][] { [1, 2], [3, 4, 5] };
        
        // Act & Assert
        await Assert.That(() => {
                Span<int[]> source = arrays;
                Span<int> destination = new int[4]; // Too small for flattened result (5 elements)
                source.SelectMany(arr => arr.AsSpan(), destination, out _);
            })
            .Throws<ArgumentException>()
            .WithMessage("Destination span is too small");
    }
    
    [Test]
    public async Task SelectMany_WithCustomSelector_ShouldWorkCorrectly() {
        // Arrange
        var people = new Person[] {
            new("Alice", 25),
            new("Bob", 30)
        };
        Span<Person> source = people;
        Span<char> destination = new char[20];
        
        // Act
        source.SelectMany(p => p.Name.AsSpan(), destination, out int totalCount);
        char[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(8); // "Alice" + "Bob" = 8 characters
        await Assert.That(destinationResult[..totalCount].ToArray()).IsEquivalentTo(['A', 'l', 'i', 'c', 'e', 'B', 'o', 'b']);
    }
    
    [Test]
    public async Task SelectMany_WithNumberRanges_ShouldFlattenCorrectly() {
        // Arrange
        Span<int> source = [2, 3, 1]; // Generate ranges of this size
        Span<int> destination = new int[10];
        
        // Act
        source.SelectMany(n => Enumerable.Range(1, n).ToArray().AsSpan(), destination, out int totalCount);
        int[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(6); // 2 + 3 + 1 = 6 elements
        await Assert.That(destinationResult[..totalCount].ToArray()).IsEquivalentTo([1, 2, 1, 2, 3, 1]);
    }
    
    [Test]
    public async Task SelectMany_SelectorCallCount_ShouldCallSelectorForEachElement() {
        // Arrange
        int callCount = 0;
        int[][] arrays = new int[][] { [1], [2, 3] };
        Span<int[]> source = arrays;
        Span<int> destination = new int[5];
        
        // Act
        source.SelectMany(arr => {
            callCount++;
            return arr.AsSpan();
        }, destination, out int totalCount);
        int[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(callCount).IsEquivalentTo(2);
        await Assert.That(totalCount).IsEquivalentTo(3);
        await Assert.That(destinationResult[..totalCount].ToArray()).IsEquivalentTo([1, 2, 3]);
    }
    
    [Test]
    public async Task SelectMany_WithAllEmptyNestedSpans_ShouldReturnZeroCount() {
        // Arrange
        int[][] emptyArrays = new int[][] { [], [], [] };
        Span<int[]> source = emptyArrays;
        Span<int> destination = new int[5];
        
        // Act
        source.SelectMany(arr => arr.AsSpan(), destination, out int totalCount);
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(0);
    }
    
    [Test]
    public async Task SelectMany_ExactDestinationSize_ShouldWorkCorrectly() {
        // Arrange
        int[][] arrays = new int[][] { [1, 2], [3] };
        Span<int[]> source = arrays;
        Span<int> destination = new int[3]; // Exact size needed
        
        // Act
        source.SelectMany(arr => arr.AsSpan(), destination, out int totalCount);
        int[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(3);
        await Assert.That(destinationResult).IsEquivalentTo([1, 2, 3]);
    }
    
    [Test]
    public async Task SelectMany_WithReadOnlySpan_ShouldWorkCorrectly() {
        // Arrange
        int[][] arrays = new int[][] { [10, 20], [30, 40, 50] };
        ReadOnlySpan<int[]> source = arrays;
        Span<int> destination = new int[10];
        
        // Act
        source.SelectMany(arr => arr.AsSpan(), destination, out int totalCount);
        int[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(5);
        await Assert.That(destinationResult[..totalCount].ToArray()).IsEquivalentTo([10, 20, 30, 40, 50]);
    }
    
    [Test]
    public async Task SelectMany_PartialFill_ShouldStopAtCorrectPosition() {
        // Arrange
        int[][] arrays = new int[][] { [1, 2, 3], [4, 5, 6], [7, 8, 9] };
        Span<int[]> source = arrays;
        Span<int> destination = new int[15]; // Larger than needed
        
        // Act
        source.SelectMany(arr => arr.AsSpan(), destination, out int totalCount);
        int[] destinationResult = destination.ToArray();
        
        // Assert
        await Assert.That(totalCount).IsEquivalentTo(9);
        // Only check the filled portion
        await Assert.That(destinationResult[..totalCount].ToArray()).IsEquivalentTo([1, 2, 3, 4, 5, 6, 7, 8, 9]);
        // Verify the rest remains uninitialized (should be 0 for int)
        await Assert.That(destinationResult[totalCount]).IsEquivalentTo(0);
    }
    
    #endregion
}