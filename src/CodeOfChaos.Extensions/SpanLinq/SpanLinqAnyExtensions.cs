// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class SpanLinqAnyExtensions {
    public static bool Any<T>(this in ReadOnlySpan<T> span, Func<T, bool> predicate) {
        for (int i = span.Length - 1; i >= 0; i--)
            if (predicate(span[i])) return true;
        return false;
    }
}
