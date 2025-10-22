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
        IDictionary<string, string> result = dictionary.AddOrUpdate("key1", "value1");

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
        IDictionary<string, string> result = dictionary.AddOrUpdate("key1", "value2");

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
        bool result = dictionary.TryAddToOrCreateCollection("key1", 1);

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
        bool result = dictionary.TryAddToOrCreateCollection("key1", 2);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(dictionary["key1"]).IsEquivalentTo(new List<int> { 1, 2 });
    }

    [Test]
    public async Task TryAddToOrCreateCollection_ShouldNotAddValue_WhenValueAlreadyExistsInCollection() {
        // Arrange
        var dictionary = new Dictionary<string, List<int>> { { "key1", [1] } };

        // Act
        bool result = dictionary.TryAddToOrCreateCollection("key1", 1);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(dictionary["key1"]).IsEquivalentTo(new List<int> { 1 });
    }

    [Test]
    public async Task GetOrAdd_ShouldAddValueToDictionary_WhenKeyDoesNotExist() {
        // Arrange
        var dictionary = new Dictionary<string, string>();
        string value = "value";
        Func<string, string> valueFactory = _ => value;

        // Act
        string newValue = dictionary.GetOrAdd("key1", valueFactory);

        // Assert
        await Assert.That(newValue).IsEqualTo(value);
        await Assert.That(dictionary.ContainsKey("key1")).IsTrue();
        await Assert.That(dictionary["key1"]).IsEqualTo(value);
    }

    [Test]
    public async Task GetOrAdd_ShouldAddValueToDictionary_WhenKeyDoesNotExist_Overload() {
        // Arrange
        var dictionary = new Dictionary<string, string>();
        string value = "value";
        Func<string, int, string> valueFactory = (_, i) => $"{value}{i}";

        // Act
        string newValue = dictionary.GetOrAdd("key1", valueFactory, 10);

        // Assert
        await Assert.That(newValue).IsEqualTo("value10");
        await Assert.That(dictionary.ContainsKey("key1")).IsTrue();
        await Assert.That(dictionary["key1"]).IsEqualTo("value10");
    }

    // ReSharper disable once UnusedParameter.Local
    // ReSharper disable once ConvertToLocalFunction
    [Test]
    public async Task AddOrUpdate_ShouldAddValueToDictionary_WhenKeyDoesNotExist_OverloadFactory() {
        // Arrange
        var dictionary = new Dictionary<string, string>();
        const string value = "value";
        Func<string, string> valueFactory = key => value;

        // Act
        dictionary.AddOrUpdate("key1", valueFactory);

        // Assert
        await Assert.That(dictionary.ContainsKey("key1")).IsTrue();
        await Assert.That(dictionary["key1"]).IsEqualTo(value);
    }

    // ReSharper disable once UnusedParameter.Local
    // ReSharper disable once ConvertToLocalFunction
    [Test]
    public async Task AddOrUpdate_ShouldAddValueToDictionary_WhenKeyDoesExist_OverloadFactory() {
        // Arrange
        var dictionary = new Dictionary<string, string> { ["key1"] = "oldValue" };

        const string value = "value";
        Func<string, string> valueFactory = key => $"{value}";

        // Act
        dictionary.AddOrUpdate("key1", valueFactory);

        // Assert
        await Assert.That(dictionary).ContainsKey("key1");
        await Assert.That(dictionary["key1"])
            .IsNotEqualTo("oldValue")
            .And.IsEqualTo("value");
    }
    
    // ReSharper disable once UnusedParameter.Local
    // ReSharper disable once ConvertToLocalFunction
    [Test]
    public async Task AddOrUpdate_ShouldAddValueToDictionary_WhenKeyDoesNotExist_OverloadFactoryAddAndUpdate() {
        // Arrange
        var dictionary = new Dictionary<string, string> { ["key1"] = "oldValue" };
        const string value = "value";
        Func<string, string> onAddFactory = key => value;
        Func<string, string, string> onUpdateFactory = (_, oldValue) => $"{oldValue};{value}";

        // Act
        dictionary.AddOrUpdate("key1", onAddFactory, onUpdateFactory);
        dictionary.AddOrUpdate("key2", onAddFactory, onUpdateFactory);

        // Assert
        await Assert.That(dictionary).ContainsKey("key1");
        await Assert.That(dictionary["key1"]).IsEqualTo("oldValue;value");
        
        await Assert.That(dictionary).ContainsKey("key2");
        await Assert.That(dictionary["key2"]).IsEqualTo(value);
    }
}
