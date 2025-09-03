// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ReadOnlySpanOfCharExtensions {
    public static bool IsNotEscapedCharacterAtIndex(this scoped ReadOnlySpan<char> source, int atIndex) {
        int backslashCount = 0;
        for (int i = atIndex - 1; i >= 0 && source[i] is '\\'; i--) {
            backslashCount++;
        }
        return backslashCount % 2 == 0;
    }

    public static bool IsEscapedCharacterAtIndex(this scoped ReadOnlySpan<char> source, int atIndex) {
        return !IsNotEscapedCharacterAtIndex(source, atIndex);
    }
    
}
