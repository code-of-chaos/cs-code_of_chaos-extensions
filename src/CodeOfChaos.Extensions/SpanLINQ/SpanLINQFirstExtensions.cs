// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public static class SpanLINQFirstExtensions {
    
    public static T First<T>(this in Span<T> span, Func<T, bool> predicate)
        => First((ReadOnlySpan<T>)span, predicate);

    public static T First<T>(this in ReadOnlySpan<T> span, Func<T, bool> predicate) {
        // ReSharper disable once ForCanBeConvertedToForeach
        for (int i = 0; i < span.Length; i++) {
            T element = span[i];
            if (predicate(element)) return element;
        }

        throw new InvalidOperationException("Sequence contains no elements");
    }

    public static T? FirstOrDefault<T>(this in Span<T> span, Func<T, bool> predicate) 
        => FirstOrDefault((ReadOnlySpan<T>)span, predicate);

    public static T? FirstOrDefault<T>(this in ReadOnlySpan<T> span, Func<T, bool> predicate) {
        // ReSharper disable once ForCanBeConvertedToForeach
        for (int i = 0; i < span.Length; i++) {
            T element = span[i];
            if (predicate(element)) return element;
        }
        
        return default;
    }

}
