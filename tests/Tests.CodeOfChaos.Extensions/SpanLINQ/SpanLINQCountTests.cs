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
public class SpanLINQCountTests {
    private record TestItem(int Value) {
        public bool IsEven => Value % 2 == 0;
    }
    
    [Test]
    public async Task Count_Span_WithPredicate_ShouldCountMatchingElements() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4, 5];
        
        // Act
        int result = numbers.Count(x => x > 2);
        
        // Assert
        await Assert.That(result).IsEqualTo(3); // 3, 4, 5
    }
    
    [Test]
    public async Task Count_Span_AllElementsMatch_ShouldReturnTotalCount() {
        // Arrange
        Span<int> numbers = [2, 4, 6];
        
        // Act
        int result = numbers.Count(x => x % 2 == 0);
        
        // Assert
        await Assert.That(result).IsEqualTo(3);
    }
    
    [Test]
    public async Task Count_Span_NoElementsMatch_ShouldReturnZero() {
        // Arrange
        Span<int> numbers = [1, 3, 5];
        
        // Act
        int result = numbers.Count(x => x % 2 == 0);
        
        // Assert
        await Assert.That(result).IsEqualTo(0);
    }
    
    [Test]
    public async Task Count_ReadOnlySpan_WithPredicate_ShouldCountMatchingElements() {
        // Arrange
        ReadOnlySpan<int> numbers = [1, 2, 3, 4, 5];
        
        // Act
        int result = numbers.Count(x => x > 2);
        
        // Assert
        await Assert.That(result).IsEqualTo(3);
    }
    
    [Test]
    public async Task Count_WithCustomType_ShouldCountSelectedValues() {
        // Arrange
        var items = new TestItem[] {
            new(1),
            new(2),
            new(3),
            new(4)
        };
        Span<TestItem> span = items;
        
        // Act
        int result = span.Count(x => x.IsEven);
        
        // Assert
        await Assert.That(result).IsEqualTo(2);
    }
    
    [Test]
    public async Task Count_EmptySpan_ShouldReturnZero() {
        // Arrange
        Span<int> emptySpan = [];
        
        // Act
        int result = emptySpan.Count(x => x > 0);
        
        // Assert
        await Assert.That(result).IsEqualTo(0);
    }
}