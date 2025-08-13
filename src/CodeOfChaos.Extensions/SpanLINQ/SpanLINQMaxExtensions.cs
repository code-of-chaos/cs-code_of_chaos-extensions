// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public static class SpanLINQMaxExtensions {
    
    public static TResult Max<T, TResult>(this scoped Span<T> span, Func<T, TResult> selector) where TResult : IComparable<TResult> 
        => Max((ReadOnlySpan<T>)span, selector);

    public static TResult Max<T, TResult>(this scoped ReadOnlySpan<T> span, Func<T, TResult> selector)
        where TResult : IComparable<TResult> {
        if (span.Length == 0) throw new InvalidOperationException("Sequence contains no elements");

        TResult max = selector(span[0]);
        for (int i = span.Length - 1; i >= 1; i--) {
            TResult current = selector(span[i]);
            if (current.CompareTo(max) > 0) max = current;
        }

        return max;
    }

}
