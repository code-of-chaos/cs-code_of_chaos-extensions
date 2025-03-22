// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.CodeOfChaos.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CollectionExtensionTests {
    [Test]
    [Arguments(new string[] {}, true)]
    [Arguments(new[] { "a" }, false)]
    [Arguments(new[] { "a", "b" }, false)]
    public async Task IsEmpty_Array_ShouldWork(string[] input, bool expected) {
        // Arrange

        // Act
        bool output = input.IsEmpty();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }

    [Test]
    [Arguments(new string[] {}, true)]
    [Arguments(new[] { "a" }, false)]
    [Arguments(new[] { "a", "b" }, false)]
    public async Task IsEmpty_Enumerable_ShouldWork(IEnumerable<string> input, bool expected) {
        // Arrange

        // Act
        bool output = input.IsEmpty();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }

    [Test]
    [Arguments(new string[] {}, true)]
    [Arguments(new[] { "a" }, false)]
    [Arguments(new[] { "a", "b" }, false)]
    public async Task IsCollectionEmpty_Array_ShouldWork(string[] input, bool expected) {
        // Arrange

        // Act
        bool output = input.IsCollectionEmpty();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }

    [Test]
    [Arguments(new string[] {}, true)]
    [Arguments(new[] { "a" }, false)]
    [Arguments(new[] { "a", "b" }, false)]
    public async Task IsCollectionEmpty_Enumerable_ShouldWork(ICollection<string> input, bool expected) {
        // Arrange

        // Act
        bool output = input.IsCollectionEmpty();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }

    [Test]
    [Arguments(new string[] {}, true)]
    [Arguments(new[] { "a" }, false)]
    public async Task IsCollectionEmpty_ShouldWork_List(IEnumerable<string> input, bool expected) {
        // Arrange
        List<string> collection = input.ToList();

        // Act
        bool output = collection.IsCollectionEmpty();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }

    [Test]
    [Arguments(new string[] {}, true)]
    [Arguments(new[] { "a" }, false)]
    public async Task IsCollectionEmpty_ShouldWork_Dictionary(IEnumerable<string> input, bool expected) {
        // Arrange
        Dictionary<string, string> collection = input.ToDictionary(keySelector: s => s, elementSelector: s => s);

        // Act
        bool output = collection.IsCollectionEmpty();

        // Assert
        await Assert.That(output).IsEqualTo(expected);
    }
}
