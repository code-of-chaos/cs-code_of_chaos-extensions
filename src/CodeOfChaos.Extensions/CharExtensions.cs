// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class CharExtensions {
    public static char ToUpper(this char c) => char.ToUpper(c);
    public static char ToLower(this char c) => char.ToLower(c);
    public static char ToUpperInvariant(this char c) => char.ToUpperInvariant(c);
    public static char ToLowerInvariant(this char c) => char.ToLowerInvariant(c);
    public static bool IsUpper(this char c) => char.IsUpper(c);
    public static bool IsLower(this char c) => char.IsLower(c);
    public static bool IsLetter(this char c) => char.IsLetter(c);
    public static bool IsDigit(this char c) => char.IsDigit(c);
    public static bool IsWhiteSpace(this char c) => char.IsWhiteSpace(c);
    public static bool IsPunctuation(this char c) => char.IsPunctuation(c);
    public static bool IsSymbol(this char c) => char.IsSymbol(c);
    public static bool IsControl(this char c) => char.IsControl(c);
    public static bool IsSeparator(this char c) => char.IsSeparator(c);
    public static bool IsHighSurrogate(this char c) => char.IsHighSurrogate(c);
    public static bool IsLowSurrogate(this char c) => char.IsLowSurrogate(c);
    public static bool IsSurrogate(this char c) => char.IsSurrogate(c);
    public static bool IsAscii(this char c) => char.IsAscii(c);
    public static bool IsAsciiDigit(this char c) => char.IsAsciiDigit(c);
    public static bool IsAsciiLetter(this char c) => char.IsAsciiLetter(c);
    
    // Checks if the character is a hexadecimal digit (0-9, A-F, a-f)
    public static bool IsHexDigit(this char c) => char.IsDigit(c)
        || c is >= 'A' and <= 'F' 
        || c is >= 'a' and <= 'f';

    // Converts a character to its ASCII code.
    public static int ToAsciiCode(this char c) => c;

    // Converts an ASCII code (integer) back to a character.
    public static char FromAsciiCode(this int code) => (char)code;

    // Checks if the character is a vowel (a, e, i, o, u, both upper and lower case).
    #pragma warning disable CA2249
    public static bool IsVowel(this char c) => "aeiouAEIOU".IndexOf(c) >= 0;
    #pragma warning restore CA2249

    // Checks if the character is a consonant (not a vowel, and is a letter).
    public static bool IsConsonant(this char c) => char.IsLetter(c) && !c.IsVowel();

    // Checks if the character is part of the Roman numeral set (I, V, X, L, C, D, M).
    #pragma warning disable CA2249
    public static bool IsRomanNumeral(this char c) => "IVXLCDM".IndexOf(c) >= 0;
    #pragma warning restore CA2249

    // Repeats a character n times and returns a string.
    public static string Repeat(this char c, int times) => new(c, times);

    // Checks if the character is a valid identifier start (e.g., for variable names in C#).
    public static bool IsIdentifierStart(this char c) => char.IsLetter(c) || c == '_';

    // Checks if the character is a valid part of an identifier (letter, digit, or underscore).
    public static bool IsIdentifierPart(this char c) => char.IsLetterOrDigit(c) || c == '_';

    // Checks if the character is a valid mathematical operator (+, -, *, /, ^, etc.)
    #pragma warning disable CA2249
    public static bool IsMathOperator(this char c) => "+-*/^%=".IndexOf(c) >= 0;
    #pragma warning restore CA2249

    // Checks if the character is a whitespace but NOT a newline.
    public static bool IsNonNewlineWhiteSpace(this char c) => c != '\n' && c != '\r' && char.IsWhiteSpace(c);
}
