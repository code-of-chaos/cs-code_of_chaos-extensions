// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class TypeExtensions {
    public static bool MatchesGenericType(this Type type, Type genericType) {
        if (!type.IsGenericType) return false;
        return type.GetGenericTypeDefinition() == genericType;
    }
}
