// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class StringExtensionTests {
    [Test]
    [Arguments(null, true)]
    [Arguments("", true)]
    [Arguments(" ", false)]
    [Arguments("a", false)]
    public async Task IsNullOrEmpty_ShouldWork(string? input, bool expected) {
        // Arrange

        // Act
        var output = input.IsNullOrEmpty();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }
    
    [Test]
    [Arguments(null, false)]
    [Arguments("", false)]
    [Arguments(" ", true)]
    [Arguments("a", true)]
    public async Task IsNotNullOrEmpty_ShouldWork(string? input, bool expected) {
        // Arrange

        // Act
        var output = input.IsNotNullOrEmpty();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }
    
    [Test]
    [Arguments(null, true)]
    [Arguments("", true)]
    [Arguments(" ", true)]
    [Arguments("a", false)]
    public async Task IsNullOrWhitespace_ShouldWork(string? input, bool expected) {
        // Arrange

        // Act
        var output = input.IsNullOrWhiteSpace();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }
    
    [Test]
    [Arguments(null, false)]
    [Arguments("", false)]
    [Arguments(" ", false)]
    [Arguments("a", true)]
    public async Task IsNotNullOrWhitespace_ShouldWork(string? input, bool expected) {
        // Arrange

        // Act
        bool output = input.IsNotNullOrWhiteSpace();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }
    
    [Test]
    [Arguments("test", 10, "test")]
    [Arguments("test", 4, "test")]
    [Arguments("testing", 4, "test")]
    [Arguments("hello world", 5, "hello")]
    public async Task Truncate_ShouldWork(string input, int maxLength, string expected) {
        // Arrange

        // Act
        string output = input.Truncate(maxLength);

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }
    
    [Test]
    [Arguments("d3cd4cfa-1cdf-4711-8d41-7e33a8d749fb", "d3cd4cfa-1cdf-4711-8d41-7e33a8d749fb")]
    [Arguments("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
    public async Task ToGuid_ShouldReturnGuid_WhenInputIsValid(string input, string expectedGuidString) {
        // Arrange
        Guid expected = Guid.Parse(expectedGuidString);

        // Act
        var output = input.ToGuid();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("InvalidGuidFormat")]
    [Arguments("1234")]
    public async Task ToGuid_ShouldThrowException_WhenInputIsInvalid(string input) {
        // Arrange

        // Act
        Func<Guid> act = input.ToGuid;

        // Assert
        await Assert.That(act).Throws<FormatException>();
    }
}
