// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class SpanLinqCountExtensions {
    public static int Count<T>(this in Span<T> span, Func<T, bool> predicate) {
        int count = 0;
        for (int index = span.Length - 1; index >= 0; index--) {
            if (predicate(span[index])) count++;
        }
        return count;
    }
    
    public static int Count<T>(this in ReadOnlySpan<T> span, Func<T, bool> predicate) {
        int count = 0;
        for (int index = span.Length - 1; index >= 0; index--) {
            if (predicate(span[index])) count++;
        }
        return count;
    }
}
