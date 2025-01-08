// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection.Generators.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators.Registrations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once StructCanBeMadeReadOnly
public record struct InjectableServiceRegistration(
    INamedTypeSymbol ServiceTypeName,
    INamedTypeSymbol ImplementationTypeName,
    string LifeTime
) : IServiceRegistration {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void FormatText(StringBuilder builder, string _) {
        builder.IndentLine(2, $"services.Add{LifeTime}<{ServiceTypeName.ToDisplayString()}, {ImplementationTypeName.ToDisplayString()}>();");
    }
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateFromModel(
        INamedTypeSymbol implementationTypeSymbol,
        AttributeSyntax attribute,
        ISymbolResolver resolver,
        out InjectableServiceRegistration registration
    ) {
        registration = default;

        GenericNameSyntax? genericNameSyntax = attribute switch {
            { Name: QualifiedNameSyntax { Right: GenericNameSyntax genericInQualifiedNameSyntax } } => genericInQualifiedNameSyntax,
            { Name: GenericNameSyntax genericNameSyntaxByItself } => genericNameSyntaxByItself,
            _ => null
        };

        if (genericNameSyntax?.TypeArgumentList.Arguments.FirstOrDefault() is not {} serviceTypeSyntax) return false;
        if (resolver.ResolveSymbol(serviceTypeSyntax) is not INamedTypeSymbol serviceNamedTypeSymbol) return false;
        if (attribute.ArgumentList?.Arguments.FirstOrDefault()?.Expression is not MemberAccessExpressionSyntax memberAccess) return false;
        if (!memberAccess.TryGetAsServiceLifetimeString(out string? lifeTime)) return false;

        registration = new InjectableServiceRegistration(
            serviceNamedTypeSymbol,
            implementationTypeSymbol,
            lifeTime
        );

        return true;
    }
}
