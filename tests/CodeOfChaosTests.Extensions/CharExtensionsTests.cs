// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;

namespace CodeOfChaosTests.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CharExtensionsTests {

    [Test]
    [Arguments('A', true)]
    [Arguments('f', false)]
    public async Task IsUpper_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsUpper();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('a', true)]
    [Arguments('F', false)]
    public async Task IsLower_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsLower();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('a', 'A')]
    [Arguments('A', 'A')]
    public async Task ToUpper_ShouldWork(char input, char expected) {
        // Act
        char result = input.ToUpper();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('A', 'a')]
    [Arguments('a', 'a')]
    public async Task ToLower_ShouldWork(char input, char expected) {
        // Act
        char result = input.ToLower();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('9', true)]
    [Arguments('A', false)]
    public async Task IsDigit_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsDigit();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(' ', true)]
    [Arguments('A', false)]
    public async Task IsWhiteSpace_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsWhiteSpace();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('a', true)]
    [Arguments('1', false)]
    public async Task IsLetter_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsLetter();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(',', true)]
    [Arguments('A', false)]
    public async Task IsPunctuation_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsPunctuation();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('a', false)]
    [Arguments('\n', true)]
    public async Task IsControl_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsControl();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('F', true)]
    [Arguments('g', false)]
    [Arguments('5', true)]
    [Arguments(';', false)]
    public async Task IsHexDigit_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsHexDigit();

        // Assert
        await Assert.That(result).IsEqualTo(expected).Because($"'{input}' is not a hex digit");
    }

    [Test]
    [Arguments('A', 65)]
    [Arguments(' ', 32)]
    public async Task ToAsciiCode_ShouldWork(char input, int expected) {
        // Act
        int result = input.ToAsciiCode();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(65, 'A')]
    [Arguments(32, ' ')]
    public async Task FromAsciiCode_ShouldWork(int code, char expected) {
        // Act
        char result = code.FromAsciiCode();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('e', true)]
    [Arguments('Z', false)]
    public async Task IsVowel_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsVowel();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('c', true)]
    [Arguments('A', false)]
    [Arguments('e', false)]
    public async Task IsConsonant_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsConsonant();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('V', true)]
    [Arguments('A', false)]
    public async Task IsRomanNumeral_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsRomanNumeral();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('a', 3, "aaa")]
    [Arguments('*', 5, "*****")]
    public async Task Repeat_ShouldWork(char input, int count, string expected) {
        // Act
        string result = input.Repeat(count);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('_', true)]
    [Arguments('A', true)]
    [Arguments('1', false)]
    public async Task IsIdentifierStart_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsIdentifierStart();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('_', true)]
    [Arguments('A', true)]
    [Arguments('1', true)]
    [Arguments('-', false)]
    public async Task IsIdentifierPart_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsIdentifierPart();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments('a', false)]
    [Arguments('+', true)]
    [Arguments('1', false)]
    public async Task IsMathOperator_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsMathOperator();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(' ', true)]
    [Arguments('\n', false)]
    public async Task IsNonNewlineWhiteSpace_ShouldWork(char input, bool expected) {
        // Act
        bool result = input.IsNonNewlineWhiteSpace();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }
}
