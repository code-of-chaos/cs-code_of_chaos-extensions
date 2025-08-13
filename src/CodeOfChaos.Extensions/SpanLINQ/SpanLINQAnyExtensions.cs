// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public static class SpanLINQAnyExtensions {
    public static bool Any<T>(this in Span<T> span) 
        => Any((ReadOnlySpan<T>)span);
    
    public static bool Any<T>(this in ReadOnlySpan<T> span) {
        for (int i = span.Length - 1; i >= 0; i--)
            if (span[i] != null) return true;
        return false;
    }
    
    public static bool Any<T>(this in Span<T> span, Func<T, bool> predicate) 
        => Any((ReadOnlySpan<T>)span, predicate);
    
    public static bool Any<T>(this in ReadOnlySpan<T> span, Func<T, bool> predicate) {
        for (int i = span.Length - 1; i >= 0; i--)
            if (predicate(span[i])) return true;
        return false;
    }
}
