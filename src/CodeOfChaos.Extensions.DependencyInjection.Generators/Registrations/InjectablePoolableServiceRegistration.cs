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
public record struct InjectablePoolableServiceRegistration(
    INamedTypeSymbol ServiceTypeName,
    INamedTypeSymbol ImplementationTypeName
) : IServiceRegistration {
    public string LifeTime { get; set; } = "Transient";

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void FormatText(StringBuilder builder, string assemblyName) {
        builder
            .IndentLine(2, $"services.Add{LifeTime}<{ServiceTypeName.ToDisplayString()}>(")
            .IndentLine(3, $"(provider) => provider.GetRequiredService<{assemblyName}.AutoPooledServices>().{ImplementationTypeName.Name}Pool.Get()")
            .IndentLine(2, ");")
            ;
    }

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

        var serviceTypeSyntax = typeArgumentsList[0];
        var implementationTypeSyntax = typeArgumentsList[1];

        var serviceNamedTypeSymbol = resolver.ResolveSymbol(serviceTypeSyntax) as INamedTypeSymbol;
        var implementationTypeSymbol = resolver.ResolveSymbol(implementationTypeSyntax) as INamedTypeSymbol;

        if (serviceNamedTypeSymbol is null || implementationTypeSymbol is null) return false;

        registration = new InjectablePoolableServiceRegistration(
            serviceNamedTypeSymbol,
            implementationTypeSymbol
        );

        return true;
    }


    public void FormatPoolText(StringBuilder builder) {
        builder.IndentLine(1, $"public ObjectPool<{ImplementationTypeName.ToDisplayString()}> {ImplementationTypeName.Name}Pool {{ get; }} = _objectPoolProvider")
            .IndentLine(2, $".Create(new CodeOfChaos.Extensions.DependencyInjection.PooledInjectableServiceObjectPolicy<{ImplementationTypeName.ToDisplayString()}>());");
    }
}
