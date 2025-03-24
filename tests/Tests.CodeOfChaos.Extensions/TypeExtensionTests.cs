// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TypeExtensionTests {
    [Test]
    public async Task MatchesGenericType_ShouldReturnTrue() {
        // Arrange
        var type = typeof(List<>);
        var genericType = typeof(List<>);
        
        // Act
        var result = type.MatchesGenericType(genericType);

        // Assert
        await Assert.That(result).IsTrue();
    }
}
