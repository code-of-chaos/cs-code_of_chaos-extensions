// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection.Generators.Helpers;
using CodeOfChaos.GeneratorTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators.Registrations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once StructCanBeMadeReadOnly
public record struct InjectablePoolableServiceRegistration(
    INamedTypeSymbol ServiceTypeName,
    INamedTypeSymbol ImplementationTypeName
) : IServiceRegistration {
    public string LifeTime { get; set; } = "Transient";

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void FormatText(GeneratorStringBuilder builder, string assemblyName) => builder
        .AppendLine($"services.Add{LifeTime}<{ServiceTypeName.ToDisplayString()}>(")
        .AppendLineIndented($"(provider) => provider.GetRequiredService<{assemblyName}.AutoPooledServices>().{ImplementationTypeName.Name}Pool.Get()")
        .AppendLine(");");

    public void FormatPoolText(GeneratorStringBuilder builder) => builder
        .AppendLine($"public ObjectPool<{ImplementationTypeName.ToDisplayString()}> {ImplementationTypeName.Name}Pool {{ get; }} = _objectPoolProvider")
        .AppendLineIndented($".Create(new CodeOfChaos.Extensions.DependencyInjection.PooledInjectableServiceObjectPolicy<{ImplementationTypeName.ToDisplayString()}>());");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static bool TryCreateFromModel(
        AttributeSyntax attribute,
        ISymbolResolver resolver,
        out InjectablePoolableServiceRegistration registration
    ) {
        registration = default;

        GenericNameSyntax? genericNameSyntax = attribute switch {
            { Name: QualifiedNameSyntax { Right: GenericNameSyntax genericInQualifiedNameSyntax } } => genericInQualifiedNameSyntax,
            { Name: GenericNameSyntax genericNameSyntaxByItself } => genericNameSyntaxByItself,
            _ => null
        };

        if (genericNameSyntax?.TypeArgumentList.Arguments is not { Count: 2 } typeArgumentsList) return false;

        TypeSyntax serviceTypeSyntax = typeArgumentsList[0];
        TypeSyntax implementationTypeSyntax = typeArgumentsList[1];

        var serviceNamedTypeSymbol = resolver.ResolveSymbol(serviceTypeSyntax) as INamedTypeSymbol;
        var implementationTypeSymbol = resolver.ResolveSymbol(implementationTypeSyntax) as INamedTypeSymbol;

        if (serviceNamedTypeSymbol is null || implementationTypeSymbol is null) return false;

        registration = new InjectablePoolableServiceRegistration(
            serviceNamedTypeSymbol,
            implementationTypeSymbol
        );

        return true;
    }
}
