// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class SpanLinqSelectExtensions {
    public static void Select<TSource, TResult>(this in Span<TSource> source, Func<TSource, TResult> selector, Span<TResult> destination)
        => Select((ReadOnlySpan<TSource>)source, selector, destination);
    
    public static void Select<TSource, TResult>(
        this in ReadOnlySpan<TSource> source,
        Func<TSource, TResult> selector,
        Span<TResult> destination
    ) {
        if (source.Length != destination.Length) throw new ArgumentException("Destination span is too small");

        for (int i = 0; i < source.Length; i++) {
            destination[i] = selector(source[i]);
        }
    }
    
    public static void SelectMany<TSource, TResult>(this in Span<TSource> source, Func<TSource, ReadOnlySpan<TResult>> selector, Span<TResult> destination, out int totalCount) 
        => SelectMany((ReadOnlySpan<TSource>)source, selector, destination, out totalCount);

    public static void SelectMany<TSource, TResult>(
        this in ReadOnlySpan<TSource> source,
        Func<TSource, ReadOnlySpan<TResult>> selector,
        Span<TResult> destination,
        out int totalCount
    ) {
        if (source.Length > destination.Length) throw new ArgumentException("Destination span is too small");

        int destinationMaxCount = destination.Length;
        totalCount = 0;
        
        // ReSharper disable once ForCanBeConvertedToForeach
        for (int i = 0; i < source.Length; i++) {
            ReadOnlySpan<TResult> nested = selector(source[i]);
            if (totalCount + nested.Length > destinationMaxCount) throw new ArgumentException("Destination span is too small");
            nested.CopyTo(destination[totalCount..]);
            totalCount += nested.Length;
        }
    }

}
