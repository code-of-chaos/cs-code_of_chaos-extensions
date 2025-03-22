// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace CodeOfChaos.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class StringCaseExtensions {
    [GeneratedRegex("(?<=[a-z])(?=[A-Z])|[^a-zA-Z0-9]+|(?<=[^\\d])(?=\\d)|(?<=\\d)(?=[^\\d])")]
    // [GeneratedRegex("(?<=[a-z])(?=[A-Z0-9])|(?<=[0-9])(?=[a-zA-Z])|[^a-zA-Z0-9]+")]
    private static partial Regex NonAlphanumericRegex { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // Convert to PascalCase
    public static string ToPascalCase(this string input) {
        if (string.IsNullOrEmpty(input))
            return input;

        string[] words = NonAlphanumericRegex.Split(input);

        for (int i = 0; i < words.Length; i++) {
            string word = words[i];
            if (word.Length > 0) {
                words[i] = char.ToUpper(word[0]) + word[1..].ToLower();
            }
        }

        return string.Concat(words);
    }

    // Convert to camelCase
    public static string ToCamelCase(this string input) {
        if (string.IsNullOrEmpty(input))
            return input;


        string[] words = NonAlphanumericRegex.Split(input);

        for (int i = 0; i < words.Length; i++) {
            string word = words[i];
            if (string.IsNullOrEmpty(word))
                continue;

            words[i] = i == 0 
                ? char.ToLower(word[0]) + word[1..]
                : char.ToUpper(word[0]) + word[1..];
        }

        return string.Concat(words);

    }


    // Convert to kebab-case
    public static string ToKebabCase(this string input) {
        if (string.IsNullOrEmpty(input))
            return input;

        string[] words = NonAlphanumericRegex.Split(input);
        for (int i = 0; i < words.Length; i++) {
            words[i] = words[i].ToLower();
        }

        return string.Join("-", words.Where(word => word.Length > 0));
    }

    // Convert to snake_case
    public static string ToSnakeCase(this string input) {
        if (string.IsNullOrEmpty(input))
            return input;

        string[] words = NonAlphanumericRegex.Split(input);
        for (int i = 0; i < words.Length; i++) {
            words[i] = words[i].ToLower();
        }

        return string.Join("_", words.Where(word => word.Length > 0));
    }

    // Convert to period.separated.case
    public static string ToPeriodSeparatedCase(this string input) {
        if (string.IsNullOrEmpty(input))
            return input;

        string[] words = NonAlphanumericRegex.Split(input);
        for (int i = 0; i < words.Length; i++) {
            words[i] = words[i].ToLower();
        }

        return string.Join(".", words.Where(word => word.Length > 0));
    }
}
