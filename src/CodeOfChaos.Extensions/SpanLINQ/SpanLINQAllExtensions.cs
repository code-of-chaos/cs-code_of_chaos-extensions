// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace CodeOfChaos.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public static class SpanLINQAllExtensions {
    
    public static bool All<T>(this in Span<T> span, Func<T, bool> predicate) 
        => All((ReadOnlySpan<T>)span, predicate);
    
    public static bool All<T>(this in ReadOnlySpan<T> span, Func<T, bool> predicate) {
        for (int i = span.Length - 1; i >= 0; i--)
            if (!predicate(span[i])) return false;
        return true;
    }
}
