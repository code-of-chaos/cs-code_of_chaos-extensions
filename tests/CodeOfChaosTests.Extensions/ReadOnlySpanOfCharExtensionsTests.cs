// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaosTests.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ReadOnlySpanOfCharExtensionsTests {

    [Test]
    [Arguments("", 0, true)]
    [Arguments("a", 0, true)]
    [Arguments(@"a\", 0, true)]
    [Arguments(@"a\b", 2, false)]
    [Arguments(@"\a", 1, false)]
    public async Task IsNotEscapedCharacterAtIndex_ShouldWork(string input, int index, bool expected) {
        // Arrange
        ReadOnlySpan<char> span = input.AsSpan();
        
        // Act
        bool result = span.IsNotEscapedCharacterAtIndex(index);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }
    
    [Test]
    [Arguments("", 0, false)]
    [Arguments("a", 0, false)]
    [Arguments(@"a\", 0, false)]
    [Arguments(@"a\b", 2, true)]
    [Arguments(@"\a", 1, true)]
    public async Task IsEscapedCharacterAtIndex_ShouldWork(string input, int index, bool expected) {
        // Arrange
        ReadOnlySpan<char> span = input.AsSpan();
        
        // Act
        bool result = span.IsEscapedCharacterAtIndex(index);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }
}
