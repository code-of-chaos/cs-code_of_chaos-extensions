// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators.Helpers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StringExtensions {
    private static readonly FrozenSet<string> ValidLifeTimes = new[] {"Singleton","Scoped","Transient"}.ToFrozenSet();
    public static bool TryGetAsServiceLifetimeString(this MemberAccessExpressionSyntax input, [NotNullWhen(true)] out string? output) {
        output = input.Name.Identifier.Text;
        return ValidLifeTimes.Contains(output);
    }
}
