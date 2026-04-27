// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.ObjectPool;

namespace CodeOfChaosTests.Extensions.ObjectPool;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CollectionPoolPolicyTests {
    private readonly CollectionPoolPolicy<List<int>, int> _policy = new();

    [Test]
    public async Task Create_ShouldReturnNewEmptyCollection() {
        // Arrange
        
        // Act
        List<int> result = _policy.Create();
        
        // Assert
        await Assert.That(result).IsNotNull()
            .And.IsEmpty();
    }

    [Test]
    public async Task Reset_ShouldClearCollection() {
        // Arrange
        List<int> collection = [1, 2, 3];
        
        // Act
        bool result = _policy.Return(collection);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(collection).IsEmpty();
    }
}
