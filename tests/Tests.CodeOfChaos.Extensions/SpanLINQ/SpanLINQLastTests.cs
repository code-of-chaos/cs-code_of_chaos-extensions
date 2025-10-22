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
public class SpanLINQLastTests {
    private record TestItem(int Value, string Name);
    
    #region Last tests
    
    [Test]
    public async Task Last_Span_WithMatchingElements_ShouldReturnLastMatch() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4, 3, 5];
        
        // Act
        int result = numbers.Last(x => x == 3);
        
        // Assert
        await Assert.That(result).IsEqualTo(3); // Should return the last occurrence
    }
    
    [Test]
    public async Task Last_Span_WithNoMatchingElements_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                Span<int> numbers = [1, 2, 3];
                return numbers.Last(x => x > 10);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }
    
    [Test]
    public async Task Last_Span_EmptySpan_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                Span<int> emptySpan = [];
                return emptySpan.Last(x => x > 0);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }
    
    [Test]
    public async Task Last_Span_SingleElementMatches_ShouldReturnThatElement() {
        // Arrange
        Span<int> singleElement = [42];
        
        // Act
        int result = singleElement.Last(x => x > 40);
        
        // Assert
        await Assert.That(result).IsEqualTo(42);
    }
    
    [Test]
    public async Task Last_Span_SingleElementDoesNotMatch_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                Span<int> singleElement = [5];
                return singleElement.Last(x => x > 10);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }
    
    [Test]
    public async Task Last_Span_MultipleMatches_ShouldReturnLastMatch() {
        // Arrange
        Span<int> numbers = [2, 4, 6, 8, 10];
        
        // Act
        int result = numbers.Last(x => x % 2 == 0);
        
        // Assert
        await Assert.That(result).IsEqualTo(10); // Last even number
    }
    
    [Test]
    public async Task Last_Span_FirstAndLastElementMatch_ShouldReturnLastElement() {
        // Arrange
        Span<int> numbers = [5, 1, 2, 3, 5];
        
        // Act
        int result = numbers.Last(x => x == 5);
        
        // Assert
        await Assert.That(result).IsEqualTo(5); // Should return the last 5, not the first
    }
    
    [Test]
    public async Task Last_ReadOnlySpan_WithMatchingElements_ShouldReturnLastMatch() {
        // Arrange
        ReadOnlySpan<int> numbers = [10, 20, 30, 20];
        
        // Act
        int result = numbers.Last(x => x == 20);
        
        // Assert
        await Assert.That(result).IsEqualTo(20); // Should return the last occurrence
    }
    
    [Test]
    public async Task Last_ReadOnlySpan_WithNoMatchingElements_ShouldThrowInvalidOperationException() {
        await Assert.That(() => {
                ReadOnlySpan<int> numbers = [1, 2, 3];
                return numbers.Last(x => x > 5);
            })
            .Throws<InvalidOperationException>()
            .WithMessage("Sequence contains no elements");
    }
    
    [Test]
    public async Task Last_WithCustomType_ShouldReturnLastMatch() {
        // Arrange
        var items = new TestItem[] {
            new(5, "first"),
            new(10, "second"),
            new(15, "third"),
            new(10, "fourth")
        };
        Span<TestItem> span = items;
        
        // Act
        TestItem result = span.Last(x => x.Value == 10);
        
        // Assert
        await Assert.That(result.Value).IsEqualTo(10);
        await Assert.That(result.Name).IsEqualTo("fourth"); // Should be the last match
    }
    
    [Test]
    public async Task Last_WithStringType_ShouldReturnLastMatch() {
        // Arrange
        Span<string> words = ["hi", "world", "test", "world"];
        
        // Act
        string result = words.Last(x => x == "world");
        
        // Assert
        await Assert.That(result).IsEqualTo("world"); // Should return the last occurrence
    }
    
    #endregion
    
    #region LastOrDefault tests
    
    [Test]
    public async Task LastOrDefault_Span_WithMatchingElements_ShouldReturnLastMatch() {
        // Arrange
        Span<int> numbers = [1, 2, 3, 4, 3, 5];
        
        // Act
        int? result = numbers.LastOrDefault(x => x == 3);
        
        // Assert
        await Assert.That(result).IsEqualTo(3); // Should return the last occurrence
    }
    
    [Test]
    public async Task LastOrDefault_Span_WithNoMatchingElements_ShouldReturnDefault() {
        // Arrange
        Span<int> numbers = [1, 2, 3];
        
        // Act
        int result = numbers.LastOrDefault(x => x > 10);
        
        // Assert
        await Assert.That(result).IsEqualTo(0);
    }
    
    [Test]
    public async Task LastOrDefault_Span_EmptySpan_ShouldReturnDefault() {
        // Arrange
        Span<int> emptySpan = [];
        
        // Act
        int result = emptySpan.LastOrDefault(x => x > 0);
        
        // Assert
        await Assert.That(result).IsEqualTo(0);
    }
    
    [Test]
    public async Task LastOrDefault_Span_SingleElementMatches_ShouldReturnThatElement() {
        // Arrange
        Span<int> singleElement = [7];
        
        // Act
        int result = singleElement.LastOrDefault(x => x < 10);
        
        // Assert
        await Assert.That(result).IsEqualTo(7);
    }
    
    [Test]
    public async Task LastOrDefault_Span_SingleElementDoesNotMatch_ShouldReturnDefault() {
        // Arrange
        Span<int> singleElement = [5];
        
        // Act
        int result = singleElement.LastOrDefault(x => x > 10);
        
        // Assert
        await Assert.That(result).IsEqualTo(0);
    }
    
    [Test]
    public async Task LastOrDefault_ReadOnlySpan_WithMatchingElements_ShouldReturnLastMatch() {
        // Arrange
        ReadOnlySpan<int> numbers = [100, 200, 300, 200];
        
        // Act
        int result = numbers.LastOrDefault(x => x == 200);
        
        // Assert
        await Assert.That(result).IsEqualTo(200); // Should return the last occurrence
    }
    
    [Test]
    public async Task LastOrDefault_ReadOnlySpan_WithNoMatchingElements_ShouldReturnDefault() {
        // Arrange
        ReadOnlySpan<int> numbers = [1, 2, 3];
        
        // Act
        int? result = numbers.LastOrDefault(x => x > 5);
        
        // Assert
        await Assert.That(result).IsEqualTo(0);
    }
    
    [Test]
    public async Task LastOrDefault_WithCustomType_ShouldReturnLastMatch() {
        // Arrange
        var items = new TestItem[] {
            new(3, "small"),
            new(8, "medium"),
            new(12, "large"),
            new(8, "another")
        };
        Span<TestItem> span = items;
        
        // Act
        TestItem? result = span.LastOrDefault(x => x.Value == 8);
        
        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Value).IsEqualTo(8);
        await Assert.That(result.Name).IsEqualTo("another"); // Should be the last match
    }
    
    [Test]
    public async Task LastOrDefault_WithCustomType_NoMatch_ShouldReturnNull() {
        // Arrange
        var items = new TestItem[] {
            new(1, "one"),
            new(2, "two")
        };
        Span<TestItem> span = items;
        
        // Act
        TestItem? result = span.LastOrDefault(x => x.Value > 10);
        
        // Assert
        await Assert.That(result).IsNull();
    }
    
    [Test]
    public async Task LastOrDefault_WithStringType_ShouldReturnLastMatch() {
        // Arrange
        Span<string> words = ["cat", "elephant", "dog", "elephant"];
        
        // Act
        string? result = words.LastOrDefault(x => x == "elephant");
        
        // Assert
        await Assert.That(result).IsEqualTo("elephant"); // Should return the last occurrence
    }
    
    [Test]
    public async Task LastOrDefault_WithStringType_NoMatch_ShouldReturnNull() {
        // Arrange
        Span<string> words = ["a", "hi", "go"];
        
        // Act
        string? result = words.LastOrDefault(x => x.Length > 10);
        
        // Assert
        await Assert.That(result).IsNull();
    }
    
    [Test]
    public async Task LastOrDefault_WithValueType_NoMatch_ShouldReturnDefaultValue() {
        // Arrange
        Span<double> numbers = [1.1, 2.2, 3.3];
        
        // Act
        double result = numbers.LastOrDefault(x => x > 5.0);
        
        // Assert
        await Assert.That(result).IsEqualTo(0);
    }
    
    [Test]
    public async Task LastOrDefault_WithNullableValueType_ShouldHandleNulls() {
        // Arrange
        Span<int?> numbers = [null, 5, null, 10, 5];
        
        // Act
        int? result = numbers.LastOrDefault(x => x is 5);
        
        // Assert
        await Assert.That(result).IsEqualTo(5); // Should return the last occurrence of 5
    }
    
    [Test]
    public async Task Last_ShortCircuitEvaluation_ShouldStopOnFirstMatchFromEnd() {
        // Arrange
        int callCount = 0;
        Span<int> numbers = [1, 2, 3, 4, 5];
        
        // Act
        int result = numbers.Last(x => {
            callCount++;
            return x > 2; // Starting from end: 5 matches immediately
        });
        
        // Assert
        await Assert.That(result).IsEqualTo(5);
        await Assert.That(callCount).IsEqualTo(1); // Should have called predicate only once
    }
    
    [Test]
    public async Task LastOrDefault_ShortCircuitEvaluation_ShouldStopOnFirstMatchFromEnd() {
        // Arrange
        int callCount = 0;
        Span<int> numbers = [5, 10, 15, 20];
        
        // Act
        int? result = numbers.LastOrDefault(x => {
            callCount++;
            return x >= 15; // Starting from end: 20 matches immediately
        });
        
        // Assert
        await Assert.That(result).IsEqualTo(20);
        await Assert.That(callCount).IsEqualTo(1); // Should have called predicate only once
    }
    
    [Test]
    public async Task Last_ReverseIteration_VerifyOrder() {
        // Arrange
        var visitedElements = new List<int>();
        Span<int> numbers = [1, 2, 3, 4, 5];
        
        // Act
        int result = numbers.Last(x => {
            visitedElements.Add(x);
            return x == 2; // Will find 2, but should visit elements in reverse order
        });
        
        // Assert
        await Assert.That(result).IsEqualTo(2);
        await Assert.That(visitedElements).IsEquivalentTo(new List<int> { 5, 4, 3, 2 }); // Reverse order until match
    }
    
    [Test]
    public async Task LastOrDefault_ReverseIteration_VerifyOrder() {
        // Arrange
        var visitedElements = new List<int>();
        Span<int> numbers = [1, 2, 3, 4, 5];
        
        // Act
        int? result = numbers.LastOrDefault(x => {
            visitedElements.Add(x);
            return x == 3; // Will find 3, should visit 5, 4, 3
        });
        
        // Assert
        await Assert.That(result).IsEqualTo(3);
        await Assert.That(visitedElements).IsEquivalentTo(new List<int> { 5, 4, 3 }); // Reverse order until match
    }
    
    #endregion
}