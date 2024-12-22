// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StringExtensions {
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str) => string.IsNullOrEmpty(str);
    
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? str) => !string.IsNullOrEmpty(str);
    
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? str) => string.IsNullOrWhiteSpace(str);

    public static bool IsNotNullOrWhiteSpace([NotNullWhen(true)] this string? str) => !string.IsNullOrWhiteSpace(str);
    
    public static string Truncate(this string input, int maxLength) =>  input.Length <= maxLength ? input : input[..maxLength];
    
    public static Guid ToGuid(this string input) {
        #if DEBUG
        if (Guid.TryParse(input, out Guid output)) return output;
        Debug.Fail("Failed to parse Guid");
        return Guid.Empty;
        #else 
        // Because "testing" of the input is done during debug, we can just "blindly" parse during release.
        return Guid.Parse(input); 
        #endif
    }
}
