// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StringColorExtensions {
    public static string ConvertToRgbValues(this string? hexColor) 
        => hexColor.TryConvertToRgbValues(out string? rgbString) ? rgbString : throw new ArgumentException("Invalid hex color");

    public static bool TryConvertToRgbValues(this string? hexColor, [NotNullWhen(true)] out string? rgbString) {
        if (hexColor.IsNullOrEmpty()) {
            rgbString = null;
            return false;
        }

        ReadOnlySpan<char> hexSpan = hexColor.AsSpan();
        int offset = hexSpan.StartsWith('#') ? 1 : 0;

        Range rRange;
        Range gRange;
        Range bRange;

        switch (hexSpan.Length - offset) {
            case 3:
                rRange = offset..(offset + 1);
                gRange = (offset + 1)..(offset + 2);
                bRange = (offset + 2)..(offset + 3);
                break;
            case 6:
                rRange = offset..(offset + 2);
                gRange = (offset + 2)..(offset + 4);
                bRange = (offset + 4)..(offset + 6);
                break;
            default:
                rgbString = null;
                return false;
        }

        if (!int.TryParse(hexSpan[rRange], Globalization.NumberStyles.HexNumber, null, out int r)
            || !int.TryParse(hexSpan[gRange], Globalization.NumberStyles.HexNumber, null, out int g)
            || !int.TryParse(hexSpan[bRange], Globalization.NumberStyles.HexNumber, null, out int b)) {
            rgbString = null;
            return false;
        }

        rgbString = $"{r}, {g}, {b}";
        return true;
    }
}
