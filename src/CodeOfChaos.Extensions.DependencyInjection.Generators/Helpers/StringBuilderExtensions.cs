// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators.Helpers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StringBuilderExtensions {
    private static string IndentString(int amount) => string.Empty.PadLeft(amount * 4, ' ');

    public static StringBuilder AppendWithIndentation(this StringBuilder builder, int amount) => builder.Append(IndentString(amount));
    public static StringBuilder AppendWithIndentation(this StringBuilder builder, int amount, string text) => builder.Append(IndentString(amount)).Append(text);

    public static StringBuilder AppendLineWithIndentation(this StringBuilder builder, int amount) => builder.Append(IndentString(amount)).AppendLine();
    public static StringBuilder AppendLineWithIndentation(this StringBuilder builder, int amount, string text) => builder.Append(IndentString(amount)).AppendLine(text);

    public static StringBuilder Indent(this StringBuilder builder, int amount) => builder.AppendWithIndentation(amount);
    public static StringBuilder Indent(this StringBuilder builder, int amount, string text) => builder.AppendWithIndentation(amount, text);

    public static StringBuilder IndentLine(this StringBuilder builder, int amount) => builder.AppendLineWithIndentation(amount);
    public static StringBuilder IndentLine(this StringBuilder builder, int amount, string text) => builder.AppendLineWithIndentation(amount, text);
}
