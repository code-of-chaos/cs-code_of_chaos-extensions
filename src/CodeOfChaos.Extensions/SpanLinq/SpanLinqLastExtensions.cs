// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class SpanLinqLastExtensions {
    
    public static T Last<T>(this in Span<T> span, Func<T, bool> predicate) 
        => Last((ReadOnlySpan<T>)span, predicate);
    
    public static T Last<T>(this in ReadOnlySpan<T> span, Func<T, bool> predicate) {
        for (int i = span.Length - 1; i >= 0; i--) {
            T element = span[i];
            if (predicate(element)) return element;
        }

        throw new InvalidOperationException("Sequence contains no elements");
    }

    public static T? LastOrDefault<T>(this in Span<T> span, Func<T, bool> predicate) 
        => LastOrDefault((ReadOnlySpan<T>)span, predicate);

    public static T? LastOrDefault<T>(this in ReadOnlySpan<T> span, Func<T, bool> predicate) {
        for (int i = span.Length - 1; i >= 0; i--) {
            T element = span[i];
            if (predicate(element)) return element;
        }

        return default;
    }

}
