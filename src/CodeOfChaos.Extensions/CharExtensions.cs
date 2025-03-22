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
    
}
