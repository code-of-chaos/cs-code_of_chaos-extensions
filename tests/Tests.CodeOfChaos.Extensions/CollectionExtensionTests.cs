// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CollectionExtensionTests {
    [Test]
    [Arguments(new string[] { }, true)]
    [Arguments(new[] { "a" }, false)]
    [Arguments(new[] { "a", "b" }, false)]
    public async Task IsEmpty_Array_ShouldWork(string[] input, bool expected) {
        // Arrange

        // Act
        var output = input.IsEmpty();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }
    
    [Test]
    [Arguments(new string[] { }, true)]
    [Arguments(new[] { "a" }, false)]
    [Arguments(new[] { "a", "b" }, false)]
    public async Task IsEmpty_Enumerable_ShouldWork(IEnumerable<string> input, bool expected) {
        // Arrange

        // Act
        bool output = input.IsEmpty();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }
}
