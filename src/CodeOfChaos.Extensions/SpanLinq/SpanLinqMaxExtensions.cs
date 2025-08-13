// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class SpanLinqMaxExtensions {
    public static TResult Max<T, TResult>(this in Span<T> span, Func<T, TResult> selector)
        where TResult : IComparable<TResult> {
        if (span.Length == 0) throw new InvalidOperationException("Sequence contains no elements");

        TResult min = selector(span[0]);
        for (int i = span.Length - 1; i >= 1; i--) {
            TResult current = selector(span[i]);
            if (current.CompareTo(min) > 0) min = current;
        }

        return min;
    }

    public static TResult Max<T, TResult>(this in ReadOnlySpan<T> span, Func<T, TResult> selector)
        where TResult : IComparable<TResult> {
        if (span.Length == 0) throw new InvalidOperationException("Sequence contains no elements");

        TResult min = selector(span[0]);
        for (int i = span.Length - 1; i >= 1; i--) {
            TResult current = selector(span[i]);
            if (current.CompareTo(min) > 0) min = current;
        }

        return min;
    }

}
