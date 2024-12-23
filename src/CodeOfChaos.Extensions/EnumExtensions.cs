// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EnumExtensions {
    private static readonly ConcurrentDictionary<Type, Array> EnumValuesCache = new();
    
    /// <summary>
    /// Retrieves all values of the specified enum type from the cache, falling back to reflection if uncached.
    /// </summary>
    private static IEnumerable<T> GetEnumValues<T>() where T : struct, Enum {
        if (EnumValuesCache.TryGetValue(typeof(T), out Array? values)) return (T[])values;

        values = Enum.GetValues<T>();
        EnumValuesCache[typeof(T)] = values;
        return (T[])values;
    }
    
    
    /// <summary>
    /// Retrieves all flagged values from the given Enum.
    /// </summary>
    /// <typeparam name="T">The type of the Enum.</typeparam>
    /// <param name="flagEnum">The enum value to inspect for flags.</param>
    /// <param name="excludeZeroValue">Indicates whether the zero value (if present) should be excluded.</param>
    /// <returns>An enumerable containing the flagged values.</returns>
    public static IEnumerable<T> GetFlags<T>(this T flagEnum, bool excludeZeroValue = true) where T : struct, Enum {
        if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
            throw new ArgumentException("The provided enum type must have the [Flags] attribute.", nameof(flagEnum));

        return GetEnumValues<T>()
            .Where(f => (!excludeZeroValue || !f.Equals(default(T))) && flagEnum.HasFlag(f));
    }

    /// <summary>
    /// Retrieves all flagged values from the given Enum as an array.
    /// </summary>
    public static T[] GetFlagsAsArray<T>(this T flagEnum, bool excludeZeroValue = true) where T : struct, Enum {
        return GetFlags(flagEnum, excludeZeroValue).ToArray();
    }

    /// <summary>
    /// Retrieves all flagged values from the given Enum as a list.
    /// </summary>
    public static List<T> GetFlagsAsList<T>(this T flagEnum, bool excludeZeroValue = true) where T : struct, Enum {
        return GetFlags(flagEnum, excludeZeroValue).ToList();
    }
}
