// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public static class SpanLINQAverageExtensions {
    
    public static double Average<T>(this in Span<T> span, Func<T, double> selector)
        => Average((ReadOnlySpan<T>)span, selector);

    public static double Average<T>(this in ReadOnlySpan<T> span, Func<T, double> selector) {
        int length = span.Length;
        if (length == 0) throw new InvalidOperationException("Sequence contains no elements");

        double sum = 0;
        for (int i = span.Length - 1; i >= 0; i--) {
            sum += selector(span[i]);
        }

        return sum / length;
    }
}
