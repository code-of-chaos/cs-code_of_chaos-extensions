// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.CodeOfChaos.Extensions.SpanLinq;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SpanLinqSumTests {
    private record TestItem(int Value);
    
    [Test]
    public async Task Sum_Span_WithDefaultLength_ShouldSumAllElements() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4, 5];
        
        // Act
        int result = numbers.Sum(x => x);
        
        // Assert
        await Assert.That(result).IsEqualTo(15);
    }
    
    [Test]
    public async Task Sum_Span_WithSpecificLength_ShouldSumSpecifiedElements() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4, 5];
        
        // Act
        int result = numbers.Sum(x => x, 3);
        
        // Assert
        await Assert.That(result).IsEqualTo(6); // Sum of first 3 elements: 1 + 2 + 3
    }
    
    [Test]
    public async Task Sum_Span_WithRange_ShouldSumElementsInRange() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4, 5];
        var range = 1..4; // Elements at index 1, 2, and 3
        
        // Act
        int result = numbers.Sum(x => x, range);
        
        // Assert
        await Assert.That(result).IsEqualTo(9); // Sum of elements 2 + 3 + 4
    }
    
    [Test]
    public async Task Sum_Span_WithSelector_ShouldApplySelector() {
        // Arrange
        Span<int> numbers = [1, 2, 3];
        
        // Act
        int result = numbers.Sum(x => x * 2);
        
        // Assert
        await Assert.That(result).IsEqualTo(12); // (1*2) + (2*2) + (3*2)
    }
    
    [Test]
    public async Task Sum_ReadOnlySpan_WithDefaultLength_ShouldSumAllElements() {
        // Arrange
        ReadOnlySpan<int> numbers = [1, 2, 3, 4, 5];
        
        // Act
        int result = numbers.Sum(x => x);
        
        // Assert
        await Assert.That(result).IsEqualTo(15);
    }
    
    [Test]
    public async Task Sum_ReadOnlySpan_WithSpecificLength_ShouldSumSpecifiedElements() {
        // Arrange
        ReadOnlySpan<int> numbers = [1, 2, 3, 4, 5];
        
        // Act
        int result = numbers.Sum(x => x, 3);
        
        // Assert
        await Assert.That(result).IsEqualTo(6); // Sum of first 3 elements
    }
    
    [Test]
    public async Task Sum_ReadOnlySpan_WithRange_ShouldSumElementsInRange() {
        // Arrange
        ReadOnlySpan<int> numbers = [1, 2, 3, 4, 5];
        Range range = 1..4;
        
        // Act
        int result = numbers.Sum(x => x, range);
        
        // Assert
        await Assert.That(result).IsEqualTo(9); // Sum of elements 2 + 3 + 4
    }
    
    [Test]
    public void Sum_InvalidLength_ShouldThrowArgumentOutOfRangeException() {
        Assert.Throws<ArgumentOutOfRangeException>(() => {
            Span<int> numbers = [1, 2, 3];
            numbers.Sum(x => x, -2);
        });
    }
    
    [Test]
    public async Task Sum_WithCustomType_ShouldSumSelectedValues() {
        // Arrange
        var items = new TestItem[] {
            new(1),
            new(2),
            new(3)
        };
        Span<TestItem> span = items;
        
        // Act
        int result = span.Sum(x => x.Value);
        
        // Assert
        await Assert.That(result).IsEqualTo(6);
    }
}
