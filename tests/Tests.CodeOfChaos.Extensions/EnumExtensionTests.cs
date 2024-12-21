// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace Tests.CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(EnumExtensions))]
public class EnumExtensionsTest {

    [Flags]
    private enum TestFlags {
        None = 0,
        Flag1 = 1 << 0,   // 1
        Flag2 = 1 << 1,   // 2
        Flag3 = 1 << 2,   // 4
        Flag4 = 1 << 3    // 8
    }

    // ReSharper disable UnusedMember.Local
    private enum NonFlagsEnum {
        Value1 = 0,
        Value2 = 1,
        Value3 = 2
    }

    [Test]
    public async Task GetFlags_ShouldReturnCorrectFlags_WhenExcludeZeroValueIsTrue() {
        // Arrange
        const TestFlags enumValue = TestFlags.Flag1 | TestFlags.Flag3;

        // Act
        IEnumerable<TestFlags> result = enumValue.GetFlags();

        // Assert
        var expected = new[] { TestFlags.Flag1, TestFlags.Flag3 };
        await Assert.That(result).IsEquivalentTo(expected);
    }

    [Test]
    public async Task GetFlags_ShouldReturnCorrectFlagsIncludingZero_WhenExcludeZeroValueIsFalse() {
        // Arrange
        const TestFlags enumValue = TestFlags.None | TestFlags.Flag2;

        // Act
        IEnumerable<TestFlags> result = enumValue.GetFlags(false);

        // Assert
        var expected = new[] { TestFlags.None, TestFlags.Flag2 };
        await Assert.That(result).IsEquivalentTo(expected);
    }

    [Test]
    public async Task GetFlags_ShouldReturnAllFlags_WhenAllFlagsAreSet() {
        // Arrange
        const TestFlags enumValue = TestFlags.Flag1 | TestFlags.Flag2 | TestFlags.Flag3 | TestFlags.Flag4;

        // Act
        IEnumerable<TestFlags> result = enumValue.GetFlags();

        // Assert
        var expected = new[] { TestFlags.Flag1, TestFlags.Flag2, TestFlags.Flag3, TestFlags.Flag4 };
        await Assert.That(result).IsEquivalentTo(expected);
    }

    [Test]
    public async Task GetFlags_ShouldReturnEmpty_WhenNoFlagIsSetAndZeroIsExcluded() {
        // Arrange
        const TestFlags enumValue = TestFlags.None;

        // Act
        IEnumerable<TestFlags> result = enumValue.GetFlags();

        // Assert
        TestFlags[] expected = Array.Empty<TestFlags>();
        await Assert.That(result).IsEquivalentTo(expected);
    }

    [Test]
    public async Task GetFlags_ShouldThrowArgumentException_WhenEnumTypeIsNotFlags() {
        // Arrange
        const NonFlagsEnum nonFlagsEnumValue = NonFlagsEnum.Value2;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => Task.Run(() => nonFlagsEnumValue.GetFlags()));
    }

    [Test]
    public async Task GetFlagsAsArray_ShouldReturnArrayOfFlags_WhenCalled() {
        // Arrange
        const TestFlags enumValue = TestFlags.Flag2 | TestFlags.Flag4;

        // Act
        TestFlags[] result = enumValue.GetFlagsAsArray();

        // Assert
        var expected = new[] { TestFlags.Flag2, TestFlags.Flag4 };
        await Assert.That(result).IsEquivalentTo(expected);
    }

    [Test]
    public async Task GetFlagsAsList_ShouldReturnListOfFlags_WhenCalled() {
        // Arrange
        const TestFlags enumValue = TestFlags.Flag1 | TestFlags.Flag3;

        // Act
        List<TestFlags> result = enumValue.GetFlagsAsList();

        // Assert
        var expected = new List<TestFlags> { TestFlags.Flag1, TestFlags.Flag3 };
        await Assert.That(result).IsEquivalentTo(expected);
    }

    [Test]
    public async Task GetFlagsAsArray_ShouldIncludeZero_WhenExcludeZeroValueIsFalse() {
        // Arrange
        const TestFlags enumValue = TestFlags.Flag3 | TestFlags.None;

        // Act
        TestFlags[] result = enumValue.GetFlagsAsArray(false);

        // Assert
        var expected = new[] { TestFlags.None, TestFlags.Flag3 };
        await Assert.That(result).IsEquivalentTo(expected);
    }

    [Test]
    public async Task GetFlagsAsList_ShouldExcludeZero_WhenExcludeZeroValueIsTrue() {
        // Arrange
        TestFlags enumValue = TestFlags.Flag1 | TestFlags.None;

        // Act
        List<TestFlags> result = enumValue.GetFlagsAsList();

        // Assert
        var expected = new List<TestFlags> { TestFlags.Flag1 };
        await Assert.That(result).IsEquivalentTo(expected);
    }
}