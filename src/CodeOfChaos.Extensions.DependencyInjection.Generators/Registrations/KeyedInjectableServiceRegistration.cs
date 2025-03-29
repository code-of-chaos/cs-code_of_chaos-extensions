// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection.Generators.Helpers;
using CodeOfChaos.GeneratorTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators.Registrations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once StructCanBeMadeReadOnly
public record struct KeyedInjectableServiceRegistration(
    INamedTypeSymbol ServiceTypeName,
    INamedTypeSymbol ImplementationTypeName,
    string LifeTime,
    string Key
) : IServiceRegistration {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void FormatText(GeneratorStringBuilder builder, string _) => builder
        .AppendLine($"services.AddKeyed{LifeTime}<{ServiceTypeName.ToDisplayString()}, {ImplementationTypeName.ToDisplayString()}>({Key.ToQuotedString()});");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateFromModel(
        INamedTypeSymbol implementationTypeSymbol,
        AttributeSyntax attribute,
        ISymbolResolver resolver,
        out KeyedInjectableServiceRegistration registration
    ) {
        registration = default;

        GenericNameSyntax? genericNameSyntax = attribute switch {
            { Name: QualifiedNameSyntax { Right: GenericNameSyntax genericInQualifiedNameSyntax } } => genericInQualifiedNameSyntax,
            { Name: GenericNameSyntax genericNameSyntaxByItself } => genericNameSyntaxByItself,
            _ => null
        };
        
        ImmutableArray<AttributeData> attributes = implementationTypeSymbol.GetAttributes();

        
        if (genericNameSyntax?.TypeArgumentList.Arguments.FirstOrDefault() is not {} serviceTypeSyntax) return false;
        if (resolver.ResolveSymbol(serviceTypeSyntax) is not INamedTypeSymbol serviceNamedTypeSymbol) return false;
        
        AttributeData? keyedServiceAttribute =    attributes.FirstOrDefault(attr => attr.AttributeClass?.ToDisplayString().Contains("KeyedInjectableServiceAttribute") ?? false);
        string key = (string)(keyedServiceAttribute?.ConstructorArguments.ElementAtOrDefault(0).Value ?? string.Empty);
        int lifeTime = (int)(keyedServiceAttribute?.ConstructorArguments.ElementAtOrDefault(1).Value ?? -1);
        
        registration = new KeyedInjectableServiceRegistration(
            serviceNamedTypeSymbol,
            implementationTypeSymbol,
            lifeTime switch {
                0 => "Singleton",
                1 => "Scoped",
                2 => "Transient",
                _ => "Transient"
            },
            key
        );

        return true;
    }
}
