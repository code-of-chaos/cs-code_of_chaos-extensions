// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public static class SpanLINQWhereExtensions {
    public static void Where<T>(this scoped Span<T> span, Func<T, bool> predicate, scoped Span<int> indices, out int count) 
        => Where((ReadOnlySpan<T>)span, predicate, indices, out count);
    
    public static void Where<T>(this scoped ReadOnlySpan<T> span, Func<T, bool> predicate, scoped Span<int> indices, out int count) {
        if (indices.Length < span.Length) throw new ArgumentException("Indices span is too small");
        
        count = 0;
        for (int i = 0; i < span.Length; i++) {
            if (predicate(span[i])) indices[count++] = i;
        }
    }
    
    public static int Where<T>(this scoped Span<T> span, Func<T, bool> predicate, scoped Span<T> destination) 
        => Where((ReadOnlySpan<T>)span, predicate, destination);
    
    public static int Where<T>(this scoped ReadOnlySpan<T> span, Func<T, bool> predicate, scoped Span<T> destination)  {
        if (destination.Length < span.Length) throw new ArgumentException("Destination span is too small");
        
        int count = 0;
        for (int i = 0; i < span.Length; i++) {
            T item = span[i];
            if (predicate(item)) destination[count++] = item;
        }
        
        return count;
    }
}
