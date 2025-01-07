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
        _ => !source.Any()
    };
    
    public static bool IsCollectionEmpty<T>(this ICollection<T> collection) => collection.Count == 0;
}
