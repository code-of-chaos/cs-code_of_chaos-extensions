// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections;

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class CollectionExtensions {
    public static bool IsEmpty<T>(this IEnumerable<T> source) => source switch {
        ICollection<T> collection => collection.Count == 0,
        ICollection collection => collection.Count == 0,
        IReadOnlyCollection<T> collection => collection.Count == 0,
        string str => str.Length == 0,
        null => true,
        _ => !source.Any()
    };
    
    public static bool IsCollectionEmpty<T>(this ICollection<T> collection) 
        => collection.Count == 0;
}
