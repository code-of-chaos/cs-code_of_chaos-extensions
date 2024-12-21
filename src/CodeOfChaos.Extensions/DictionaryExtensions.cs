// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class DictionaryExtensions {
    public static IDictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull {
        if (dictionary.TryAdd(key, value)) return dictionary;
        dictionary[key] = value;
        return dictionary;
    }
    
    public static bool TryAddToOrCreateCollection<TKey, TValue, TCollection>(
        this IDictionary<TKey, TCollection> dictionary,
        TKey key,
        TValue value
    ) where TCollection : ICollection<TValue>, new() {
        if (!dictionary.TryGetValue(key, out TCollection? collection)) return dictionary.TryAdd(key, [value]);
        if (collection.Contains(value)) return false;
        
        collection.Add(value);
        return true;
        
    }
}
