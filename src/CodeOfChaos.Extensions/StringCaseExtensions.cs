// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class StringCaseExtensions {
    [GeneratedRegex("(?<=[a-z])(?=[A-Z0-9])|(?<=[0-9])(?=[a-zA-Z])|[^a-zA-Z0-9]+")]
    private static partial Regex NonAlphanumericRegex { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // Convert to PascalCase
    [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
    public static string ToPascalCase(this string input) {
        if (input.IsNullOrWhiteSpace()) return input;

        ReadOnlySpan<string> words = NonAlphanumericRegex.Split(input);

        Span<char> result = stackalloc char[input.Length];
        int position = 0;

        foreach (string word in words) {
            if (word.IsNullOrEmpty()) continue;

            ReadOnlySpan<char> wordSpan = word.AsSpan();

            // Uppercase first letter
            result[position++] = wordSpan[0].ToUpper();

            // Append the rest of the word in lowercase, if applicable
            for (int i = 1; i < wordSpan.Length; i++) {
                result[position++] = wordSpan[i].ToLower();
            }
        }
        return new string(result[..position]);
    }
    
    // Convert to camelCase
    [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
    public static string ToCamelCase(this string input) {
        if (input.IsNullOrWhiteSpace()) return input;

        ReadOnlySpan<string> words = NonAlphanumericRegex.Split(input);

        Span<char> result = stackalloc char[input.Length];
        int position = 0;

        for (int i = 0; i < words.Length; i++) {
            if (words[i].IsNullOrEmpty()) continue;

            ReadOnlySpan<char> wordSpan = words[i].AsSpan();

            result[position++] = i == 0
                ? wordSpan[0].ToLower()// Lowercase first character for the first word
                : wordSpan[0].ToUpper();// Uppercase first character for subsequent words

            // Append the rest of the word as-is
            for (int j = 1; j < wordSpan.Length; j++) {
                result[position++] = wordSpan[j];
            }
        }
        return new string(result[..position]);
    }

    // Convert to kebab-case
    [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
    public static string ToKebabCase(this string input) {
        if (input.IsNullOrWhiteSpace()) return input;

        ReadOnlySpan<string> words = NonAlphanumericRegex.Split(input);

        Span<char> result = stackalloc char[input.Length * 2]; // Overallocate to accommodate separators
        int position = 0;

        for (int i = 0; i < words.Length; i++) {
            if (words[i].IsNullOrEmpty()) continue;

            ReadOnlySpan<char> wordSpan = words[i].AsSpan();

            // Add separator for kebab-case
            if (position > 0) result[position++] = '-';

            // Append the word in lowercase
            for (int j = 0; j < wordSpan.Length; j++) {
                result[position++] = wordSpan[j].ToLower();
            }
        }

        return new string(result[..position]);
    }

    // Convert to snake_case
    [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
    public static string ToSnakeCase(this string input) {
        if (input.IsNullOrWhiteSpace()) return input;

        ReadOnlySpan<string> words = NonAlphanumericRegex.Split(input);

        Span<char> result = stackalloc char[input.Length * 2]; // Overallocate to accommodate separators
        int position = 0;

        for (int i = 0; i < words.Length; i++) {
            if (words[i].IsNullOrEmpty()) continue;

            ReadOnlySpan<char> wordSpan = words[i].AsSpan();

            // Add separator for snake_case
            if (position > 0) result[position++] = '_';

            // Append the word in lowercase
            for (int j = 0; j < wordSpan.Length; j++) {
                result[position++] = wordSpan[j].ToLower();
            }
        }

        return new string(result[..position]);
    }


    // Convert to period.separated.case
    [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
    public static string ToPeriodSeparatedCase(this string input) {
        if (input.IsNullOrWhiteSpace()) return input;

        ReadOnlySpan<string> words = NonAlphanumericRegex.Split(input);

        Span<char> result = stackalloc char[input.Length * 2]; // Overallocate to accommodate separators
        int position = 0;

        for (int i = 0; i < words.Length; i++) {
            if (words[i].IsNullOrEmpty()) continue;

            ReadOnlySpan<char> wordSpan = words[i].AsSpan();

            // Add separator for period.separated.case
            if (position > 0) result[position++] = '.';

            // Append the word in lowercase
            for (int j = 0; j < wordSpan.Length; j++) {
                result[position++] = char.ToLower(wordSpan[j]);
            }
        }

        return new string(result[..position]);
    }

}
