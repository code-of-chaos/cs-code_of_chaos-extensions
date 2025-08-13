// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public static class SpanLINQCountExtensions {
    
    public static int Count<T>(this in Span<T> span, Func<T, bool> predicate) 
        => Count((ReadOnlySpan<T>)span, predicate);
    
    public static int Count<T>(this in ReadOnlySpan<T> span, Func<T, bool> predicate) {
        int count = 0;
        for (int index = span.Length - 1; index >= 0; index--) {
            if (predicate(span[index])) count++;
        }
        return count;
    }
}
