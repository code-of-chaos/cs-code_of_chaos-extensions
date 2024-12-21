// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace Tests.CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(DictionaryExtensions))]
public class DictionaryExtensionsTest {

    [Test]
    public async Task AddOrUpdate_ShouldAddNewKey_WhenKeyDoesNotExist() {
        // Arrange
        var dictionary = new Dictionary<string, string>();

        // Act
        var result = dictionary.AddOrUpdate("key1", "value1");

        // Assert
        await Assert.That(result).IsEqualTo(dictionary);
        await Assert.That(dictionary.ContainsKey("key1")).IsTrue();
        await Assert.That(dictionary["key1"]).IsEqualTo("value1");
    }
    
    [Test]
    public async Task AddOrUpdate_ShouldUpdateValue_WhenKeyAlreadyExists() {
        // Arrange
        var dictionary = new Dictionary<string, string> { { "key1", "value1" } };

        // Act
        var result = dictionary.AddOrUpdate("key1", "value2");

        // Assert
        await Assert.That(result).IsEqualTo(dictionary);
        await Assert.That(dictionary.ContainsKey("key1")).IsTrue();
        await Assert.That(dictionary["key1"]).IsEqualTo("value2");
    }

    [Test]
    public async Task TryAddToOrCreateCollection_ShouldAddValueToNewCollection_WhenKeyDoesNotExist() {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();

        // Act
        var result = dictionary.TryAddToOrCreateCollection("key1", 1);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(dictionary.ContainsKey("key1")).IsTrue();
        await Assert.That(dictionary["key1"]).IsEquivalentTo(new List<int> { 1 });
    }

    [Test]
    public async Task TryAddToOrCreateCollection_ShouldAddValueToExistingCollection_WhenKeyExistsAndValueIsNew() {
        // Arrange
        var dictionary = new Dictionary<string, List<int>> { { "key1", [1] } };

        // Act
        var result = dictionary.TryAddToOrCreateCollection("key1", 2);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(dictionary["key1"]).IsEquivalentTo(new List<int> { 1, 2 });
    }
    
    [Test]
    public async Task TryAddToOrCreateCollection_ShouldNotAddValue_WhenValueAlreadyExistsInCollection() {
        // Arrange
        var dictionary = new Dictionary<string, List<int>> { { "key1", [1] } };

        // Act
        var result = dictionary.TryAddToOrCreateCollection("key1", 1);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(dictionary["key1"]).IsEquivalentTo(new List<int> { 1 });
    }
}