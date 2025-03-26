// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace System.Text.RegularExpressions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class GroupExtensions {
    public static bool TryGetValue(this Group group,[NotNullWhen(true)] out string? value) {
        if (group.Success) {
            value = group.Value;
            return true;
        }
        value = null;
        return false;
    }
    
    public static bool TryGetValueSpan(this Group group, out ReadOnlySpan<char> value) {
        if (group.Success) {
            value = group.ValueSpan;
            return true;
        }
        value = default;
        return false;
    }
}
