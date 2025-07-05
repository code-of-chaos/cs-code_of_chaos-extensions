// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class SpanLinqSumExtensions {
    public static int Sum<T>(this in Span<T> span, Func<T, int> selector, int length = -1) {
        int totalGroups = 0;
        length = length switch {
            < -1 => throw new ArgumentOutOfRangeException(nameof(length)),
            -1 => span.Length,
            _ => length
        };

        for (int index = 0; index < length; index++) {
            totalGroups += selector(span[index]);
        }
        return totalGroups;
    }
    
    public static int Sum<T>(this in Span<T> span, Func<T, int> selector, Range range) {
        int totalGroups = 0;
        ReadOnlySpan<T> slice = span[range];
        int length = slice.Length;

        for (int index = 0; index < length; index++) {
            totalGroups += selector(slice[index]);
        }
        return totalGroups;
    }
    
    public static int Sum<T>(this in ReadOnlySpan<T> span, Func<T, int> selector, int length = -1) {
        int totalGroups = 0;
        length = length switch {
            < -1 => throw new ArgumentOutOfRangeException(nameof(length)),
            -1 => span.Length,
            _ => length
        };

        for (int index = 0; index < length; index++) {
            totalGroups += selector(span[index]);
        }
        return totalGroups;
    }
    
    public static int Sum<T>(this in ReadOnlySpan<T> span, Func<T, int> selector, Range range) {
        int totalGroups = 0;
        ReadOnlySpan<T> slice = span[range];
        int length = slice.Length;

        for (int index = 0; index < length; index++) {
            totalGroups += selector(slice[index]);
        }
        return totalGroups;
    }
}
