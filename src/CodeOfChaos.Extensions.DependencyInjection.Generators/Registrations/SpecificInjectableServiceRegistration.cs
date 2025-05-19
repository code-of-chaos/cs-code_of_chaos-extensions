// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection.Generators.Helpers;
using CodeOfChaos.GeneratorTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators.Registrations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once StructCanBeMadeReadOnly
public record struct SpecificInjectableServiceRegistration(
    INamedTypeSymbol ServiceTypeName,
    INamedTypeSymbol ImplementationTypeName,
    string LifeTime,
    string? Key = null
) : IServiceRegistration {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void FormatText(GeneratorStringBuilder builder, string _) {
        if (!string.IsNullOrWhiteSpace(Key)) {
            builder.AppendLine($"services.AddKeyed{LifeTime}<{ServiceTypeName.ToDisplayString()}, {ImplementationTypeName.ToDisplayString()}>({Key!.ToQuotedString()});");
            return;
        }
        builder.AppendLine($"services.Add{LifeTime}<{ServiceTypeName.ToDisplayString()}, {ImplementationTypeName.ToDisplayString()}>();");
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateFromModel(
        INamedTypeSymbol implementationTypeSymbol,
        AttributeSyntax attribute,
        ISymbolResolver resolver,
        out SpecificInjectableServiceRegistration registration
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
        
        AttributeData? keyedServiceAttribute = attributes.FirstOrDefault(attr => attr.AttributeClass?.ToDisplayString().Contains("Injectable") ?? false);
        string? key = (string?)keyedServiceAttribute?.ConstructorArguments.ElementAtOrDefault(0).Value;
        string lifeTimeName = attribute.Name.ToFullString();
        
        string lifeTime = "Transient";
        if (lifeTimeName.Contains("Singleton")) lifeTime = "Singleton";
        if (lifeTimeName.Contains("Scoped")) lifeTime = "Scoped";
        if (lifeTimeName.Contains("Transient")) lifeTime = "Transient";
        
        registration = new SpecificInjectableServiceRegistration(
            serviceNamedTypeSymbol,
            implementationTypeSymbol,
            lifeTime,
            key
        );

        return true;
    }
}
