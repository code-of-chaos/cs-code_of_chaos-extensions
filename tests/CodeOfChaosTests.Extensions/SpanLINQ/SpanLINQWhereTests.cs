// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.SpanLINQ;

namespace CodeOfChaosTests.Extensions.SpanLINQ;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Category("SpanLINQ")]
// ReSharper disable once InconsistentNaming
public class SpanLINQWhereTests {
    // ReSharper disable once NotAccessedPositionalProperty.Local
    private record TestItem(int Value, string Name) {
        public bool IsEven => Value % 2 == 0;
    }
    
    #region Basic Where tests
    
    [Test]
    public async Task Where_Span_WithEvenNumbers_ShouldReturnCorrectIndices() {
        // Arrange
        int[] sourceArray = [1, 2, 3, 4, 5, 6];
        Span<int> source = sourceArray;
        Span<int> indices = new int[6];
        
        // Act
        source.Where(x => x % 2 == 0, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(3);
        await Assert.That(resultIndices).IsEquivalentTo([1, 3, 5]); // Indices of 2, 4, 6
    }
    
    [Test]
    public async Task Where_ReadOnlySpan_WithStringPredicate_ShouldReturnCorrectIndices() {
        // Arrange
        string[] words = ["apple", "banana", "cherry", "date"];
        ReadOnlySpan<string> source = words;
        Span<int> indices = new int[4];
        
        // Act
        source.Where(s => s.Length > 5, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(2);
        await Assert.That(resultIndices).IsEquivalentTo([1, 2]); // "banana" and "cherry"
    }
    
    [Test]
    public async Task Where_EmptySpan_ShouldReturnZeroCount() {
        // Arrange
        Span<int> source = [];
        Span<int> indices = new int[5];
        
        // Act
        source.Where(x => x > 0, indices, out int count);
        
        // Assert
        await Assert.That(count).IsEquivalentTo(0);
    }
    
    [Test]
    public async Task Where_NoMatches_ShouldReturnZeroCount() {
        // Arrange
        int[] sourceArray = [1, 3, 5, 7];
        Span<int> source = sourceArray;
        Span<int> indices = new int[4];
        
        // Act
        source.Where(x => x % 2 == 0, indices, out int count);
        
        // Assert
        await Assert.That(count).IsEquivalentTo(0);
    }
    
    [Test]
    public async Task Where_AllMatch_ShouldReturnAllIndices() {
        // Arrange
        int[] sourceArray = [2, 4, 6, 8];
        Span<int> source = sourceArray;
        Span<int> indices = new int[4];
        
        // Act
        source.Where(x => x % 2 == 0, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(4);
        await Assert.That(resultIndices).IsEquivalentTo([0, 1, 2, 3]);
    }
    
    [Test]
    public async Task Where_SingleElement_Match_ShouldReturnSingleIndex() {
        // Arrange
        int[] sourceArray = [42];
        Span<int> source = sourceArray;
        Span<int> indices = new int[1];
        
        // Act
        source.Where(x => x > 0, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(1);
        await Assert.That(resultIndices).IsEquivalentTo([0]);
    }
    
    [Test]
    public async Task Where_SingleElement_NoMatch_ShouldReturnZeroCount() {
        // Arrange
        int[] sourceArray = [42];
        Span<int> source = sourceArray;
        Span<int> indices = new int[1];
        
        // Act
        source.Where(x => x < 0, indices, out int count);
        
        // Assert
        await Assert.That(count).IsEquivalentTo(0);
    }
    
    #endregion
    
    #region Error handling tests
    
    [Test]
    public async Task Where_IndicesTooSmall_ShouldThrowArgumentException() {
        // Arrange
        int[] sourceArray = [1, 2, 3, 4];
        
        // Act & Assert
        await Assert.That(() => {
                Span<int> source = sourceArray;
                Span<int> indices = new int[3]; // Too small
                source.Where(x => x > 0, indices, out _);
            })
            .Throws<ArgumentException>()
            .WithMessage("Indices span is too small");
    }
    
    [Test]
    public async Task Where_IndicesExactSize_ShouldWorkCorrectly() {
        // Arrange
        int[] sourceArray = [1, 2, 3];
        Span<int> source = sourceArray;
        Span<int> indices = new int[3]; // Exact size
        
        // Act
        source.Where(x => x > 1, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(2);
        await Assert.That(resultIndices).IsEquivalentTo([1, 2]); // Indices of 2, 3
    }
    
    [Test]
    public async Task Where_IndicesLargerThanNeeded_ShouldWorkCorrectly() {
        // Arrange
        int[] sourceArray = [1, 2, 3];
        Span<int> source = sourceArray;
        Span<int> indices = new int[10]; // Larger than needed
        
        // Act
        source.Where(x => x > 1, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(2);
        await Assert.That(resultIndices).IsEquivalentTo([1, 2]); // Only the matched indices
    }
    
    #endregion
    
    #region Custom types and complex predicates
    
    [Test]
    public async Task Where_WithCustomType_ShouldReturnCorrectIndices() {
        // Arrange
        var items = new TestItem[] {
            new(1, "first"),
            new(2, "second"),
            new(3, "third"),
            new(4, "fourth")
        };
        Span<TestItem> source = items;
        Span<int> indices = new int[4];
        
        // Act
        source.Where(x => x.IsEven, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(2);
        await Assert.That(resultIndices).IsEquivalentTo([1, 3]); // Items with values 2 and 4
    }
    
    [Test]
    public async Task Where_WithStringLength_ShouldReturnCorrectIndices() {
        // Arrange
        string[] words = ["a", "hello", "hi", "world"];
        Span<string> source = words;
        Span<int> indices = new int[4];
        
        // Act
        source.Where(s => s.Length >= 4, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(2);
        await Assert.That(resultIndices).IsEquivalentTo([1, 3]); // "hello" and "world"
    }
    
    [Test]
    public async Task Where_WithComplexPredicate_ShouldReturnCorrectIndices() {
        // Arrange
        int[] numbers = [1, 5, 10, 15, 20, 25];
        Span<int> source = numbers;
        Span<int> indices = new int[6];
        
        // Act
        source.Where(x => x > 5 && x < 20, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(2);
        await Assert.That(resultIndices).IsEquivalentTo([2, 3]); // Indices of 10 and 15
    }
    
    #endregion
    
    #region Behavior and performance tests
    
    [Test]
    public async Task Where_PredicateCallCount_ShouldCallPredicateForEachElement() {
        // Arrange
        int callCount = 0;
        int[] sourceArray = [1, 2, 3, 4];
        Span<int> source = sourceArray;
        Span<int> indices = new int[4];
        
        // Act
        source.Where(x => {
            callCount++;
            return x % 2 == 0;
        }, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int actualCallCount = callCount;
        
        // Assert
        await Assert.That(actualCallCount).IsEquivalentTo(4); // Called for all elements
        await Assert.That(actualCount).IsEquivalentTo(2); // Only 2 matches
    }
    
    [Test]
    public async Task Where_WithNegativeNumbers_ShouldReturnCorrectIndices() {
        // Arrange
        int[] numbers = [-5, -2, 0, 3, -1];
        Span<int> source = numbers;
        Span<int> indices = new int[5];
        
        // Act
        source.Where(x => x < 0, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(3);
        await Assert.That(resultIndices).IsEquivalentTo([0, 1, 4]); // Indices of -5, -2, -1
    }
    
    [Test]
    public async Task Where_WithDuplicateValues_ShouldReturnAllMatchingIndices() {
        // Arrange
        int[] numbers = [1, 2, 2, 3, 2];
        Span<int> source = numbers;
        Span<int> indices = new int[5];
        
        // Act
        source.Where(x => x == 2, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(3);
        await Assert.That(resultIndices).IsEquivalentTo([1, 2, 4]); // All indices where value is 2
    }
    
    [Test]
    public async Task Where_WithFloatingPoint_ShouldHandlePrecisionCorrectly() {
        // Arrange
        double[] numbers = [1.0, 1.1, 1.01, 1.001];
        Span<double> source = numbers;
        Span<int> indices = new int[4];
        
        // Act
        source.Where(x => x > 1.0, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(3);
        await Assert.That(resultIndices).IsEquivalentTo([1, 2, 3]); // All except exact 1.0
    }
    
    [Test]
    public async Task Where_MultipleCallsOnSameSpan_ShouldReturnConsistentResults() {
        // Arrange
        int[] sourceArray = [1, 2, 3, 4, 5];
        Span<int> source = sourceArray;
        Span<int> indices1 = new int[5];
        Span<int> indices2 = new int[5];
        
        // Act
        source.Where(x => x % 2 == 0, indices1, out int count1);
        source.Where(x => x % 2 == 0, indices2, out int count2);
        
        // Extract results before await
        int actualCount1 = count1;
        int actualCount2 = count2;
        int[] resultIndices1 = indices1[..count1].ToArray();
        int[] resultIndices2 = indices2[..count2].ToArray();
        
        // Assert
        await Assert.That(actualCount1).IsEquivalentTo(actualCount2);
        await Assert.That(resultIndices1).IsEquivalentTo(resultIndices2);
    }
    
    [Test]
    public async Task Where_WithBooleanValues_ShouldReturnTrueIndices() {
        // Arrange
        bool[] flags = [true, false, true, false, true];
        Span<bool> source = flags;
        Span<int> indices = new int[5];
        
        // Act
        source.Where(x => x, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(3);
        await Assert.That(resultIndices).IsEquivalentTo([0, 2, 4]); // Indices of true values
    }
    
    [Test]
    public async Task Where_WithNullableTypes_ShouldHandleNullsCorrectly() {
        // Arrange
        int?[] numbers = [1, null, 3, null, 5];
        Span<int?> source = numbers;
        Span<int> indices = new int[5];
        
        // Act
        source.Where(x => x.HasValue, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(3);
        await Assert.That(resultIndices).IsEquivalentTo([0, 2, 4]); // Indices of non-null values
    }
    
    [Test]
    public async Task Where_WithLargeDataset_ShouldPerformCorrectly() {
        // Arrange
        int[] largeArray = Enumerable.Range(1, 1000).ToArray();
        Span<int> source = largeArray;
        Span<int> indices = new int[1000];
        
        // Act
        source.Where(x => x % 10 == 0, indices, out int count);
        
        // Extract results before await
        int actualCount = count;
        int[] resultIndices = indices[..count].ToArray();
        
        // Assert
        await Assert.That(actualCount).IsEquivalentTo(100); // Every 10th number (10, 20, 30, ..., 1000)
        
        // Verify first and last indices
        await Assert.That(resultIndices[0]).IsEquivalentTo(9);   // Index of 10 (0-based)
        await Assert.That(resultIndices[99]).IsEquivalentTo(999); // Index of 1000 (0-based)
    }
    
    #endregion
}