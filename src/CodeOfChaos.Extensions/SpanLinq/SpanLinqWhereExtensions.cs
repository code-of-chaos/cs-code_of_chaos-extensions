// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class SpanLinqWhereExtensions {
    public static void Where<T>(this in Span<T> span, Func<T, bool> predicate, in Span<int> indices, out int count) {
        if (indices.Length < span.Length) throw new ArgumentException("Indices span is too small");
        
        count = 0;
        for (int i = 0; i < span.Length && count < indices.Length; i++) {
            if (predicate(span[i])) indices[count++] = i;
        }
    }
    
    public static void Where<T>(this in ReadOnlySpan<T> span, Func<T, bool> predicate, in Span<int> indices, out int count) {
        if (indices.Length < span.Length) throw new ArgumentException("Indices span is too small");
        
        count = 0;
        for (int i = 0; i < span.Length && count < indices.Length; i++) {
            if (predicate(span[i])) indices[count++] = i;
        }
    }
}
