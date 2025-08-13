// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public static class SpanLINQMinExtensions {
    
    public static TResult Min<T, TResult>(this scoped Span<T> span, Func<T, TResult> selector) where TResult : IComparable<TResult> 
        => Min((ReadOnlySpan<T>)span, selector);
    
    public static TResult Min<T, TResult>(this scoped ReadOnlySpan<T> span, Func<T, TResult> selector)
        where TResult : IComparable<TResult> {
        if (span.Length == 0) throw new InvalidOperationException("Sequence contains no elements");

        TResult min = selector(span[0]);
        for (int i = span.Length - 1; i >= 1; i--) {
            TResult current = selector(span[i]);
            if (current.CompareTo(min) < 0) min = current;
        }

        return min;
    }

}
