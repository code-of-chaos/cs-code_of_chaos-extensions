// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.CodeOfChaos.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class StringColorExtensionTests {
    public static IEnumerable<Func<(string, string)>> ValidDataSources() {
        yield return () => ("#FF0000", "255, 0, 0");  
        yield return () => ("FF0000", "255, 0, 0");
        yield return () => ("#00FF00", "0, 255, 0");
        yield return () => ("#0000FF", "0, 0, 255");
        yield return () => ("#F00", "15, 0, 0");
        yield return () => ("#FFF", "15, 15, 15");
        
        // Common Colors
        yield return () => ("#FFFFFF", "255, 255, 255"); 
        yield return () => ("#000000", "0, 0, 0"); 
        yield return () => ("#808080", "128, 128, 128"); 
        yield return () => ("#FFF", "15, 15, 15"); 
        yield return () => ("#000", "0, 0, 0"); 
        
        yield return () => ("#5BCEFA", "91, 206, 250");
        yield return () => ("#F5A9B8", "245, 169, 184");
        yield return () => ("#FFFFFF", "255, 255, 255");
        yield return () => ("#F5A9B8", "245, 169, 184");
        yield return () => ("#5BCEFA", "91, 206, 250");
    }

    // ReSharper disable StringLiteralTypo
    public static IEnumerable<Func<string?>> InvalidDataSources() {
        yield return () => null;
        yield return () => "";
        yield return () => " ";
        yield return () => "#";
        yield return () => "#F";
        yield return () => "#FF";
        yield return () => "#FFFF";
        yield return () => "#FFFFF";
        yield return () => "#FFFFFFF";
        yield return () => "#GG0000";
        yield return () => "GGGGGG";
    }
    // ReSharper enable StringLiteralTypo
    
    [Test]
    [MethodDataSource<StringColorExtensionTests>(nameof(ValidDataSources))]
    public async Task ConvertToRgbValues_ValidHexColors_ReturnsRgbString(string input, string expected) {
        // Act
        string result = input.ConvertToRgbValues();

        // Assert
        await Assert.That(result).IsNotNull().And.IsEquatableOrEqualTo(expected);
    }

    [Test]
    [MethodDataSource<StringColorExtensionTests>(nameof(InvalidDataSources))]
    public Task ConvertToRgbValues_InvalidInput_ThrowsArgumentException(string? input) {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => input.ConvertToRgbValues());
        return Task.CompletedTask;
    }
    
    [Test]
    [MethodDataSource<StringColorExtensionTests>(nameof(ValidDataSources))]
    public async Task TryConvertToRgbValues_ValidHexColors_ReturnsTrue(string input, string expected) {
        // Act
        bool result = input.TryConvertToRgbValues(out string? rgb);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(rgb).IsNotNull().And.IsEquatableOrEqualTo(expected);
    }

    // ReSharper disable StringLiteralTypo
    [Test]
    [MethodDataSource<StringColorExtensionTests>(nameof(InvalidDataSources))]
    public async Task TryConvertToRgbValues_InvalidInput_ReturnsFalse(string? input) {
        // Act
        bool result = input.TryConvertToRgbValues(out string? rgb);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(rgb).IsNull();
    }
}
