// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class DictionaryExtensions {
    public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        TValue value
    ) where TKey : notnull {
        if (dictionary.TryAdd(key, value)) return dictionary;

        dictionary[key] = value;
        return dictionary;
    }
    
    public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> valueFactory
    ) where TKey : notnull {
        if (dictionary.TryAdd(key, valueFactory(key))) return dictionary;
        dictionary[key] = valueFactory(key);
        return dictionary;
    }
    
    public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> addValueFactory,
        Func<TKey, TValue, TValue> updateValueFactory
    ) where TKey : notnull {
        if (dictionary.TryGetValue(key, out TValue? value)) {
            value = updateValueFactory(key, value);
            dictionary[key] = value;
            return dictionary;
        }
        value = addValueFactory(key);
        dictionary.Add(key, value);
        return dictionary;
    }

    public static bool TryAddToOrCreateCollection<TKey, TValue, TCollection>(
        this Dictionary<TKey, TCollection> dictionary,
        TKey key,
        TValue value
    ) where TCollection : ICollection<TValue>, new() where TKey : notnull {
        if (!dictionary.TryGetValue(key, out TCollection? collection)) return dictionary.TryAdd(key, [value]);
        if (collection.Contains(value)) return false;

        collection.Add(value);
        return true;

    }

    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> valueFactory
    ) where TKey : notnull {
        if (dictionary.TryGetValue(key, out TValue? value)) return value;

        value = valueFactory(key);
        dictionary.Add(key, value);
        return value;
    }

    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        TValue value
    ) where TKey : notnull {
        if (dictionary.TryGetValue(key, out TValue? existingValue)) return existingValue;

        dictionary.Add(key, value);
        return value;
    }

    public static TValue GetOrAdd<TKey, TValue, TArg>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TArg, TValue> valueFactory,
        TArg factoryArgument
    ) where TKey : notnull {
        if (dictionary.TryGetValue(key, out TValue? value)) return value;

        value = valueFactory(key, factoryArgument);
        dictionary.Add(key, value);
        return value;
    }
}
