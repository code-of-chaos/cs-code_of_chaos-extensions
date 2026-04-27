// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaosTests.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TypeExtensionTests {
    [Test]
    public async Task MatchesGenericType_ShouldReturnTrue() {
        // Arrange
        Type type = typeof(List<>);
        Type genericType = typeof(List<>);
        
        // Act
        bool result = type.MatchesGenericType(genericType);

        // Assert
        await Assert.That(result).IsTrue();
    }
}
