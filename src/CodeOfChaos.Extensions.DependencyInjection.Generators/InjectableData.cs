// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record InjectableData(
    string? ClassName,
    ServiceLifetime ServiceLifetime,
    string? ServiceTypeName,
    object? ServiceKey,
    bool ServiceKeyIsString
) {
    [MemberNotNullWhen(true, nameof(ServiceKey))] public bool HasServiceKey => ServiceKey is not null;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<InjectableData> FromInjectableAttribute(GeneratorAttributeSyntaxContext context) {
        if (context.TargetNode is not ClassDeclarationSyntax classDeclaration) yield break;
        if (context.SemanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol) yield break;

        foreach (AttributeData attributeData in context.Attributes) {
            int lifetime = attributeData.ConstructorArguments[0].Value as int? ?? -1;
            yield return ExtractServiceData(classSymbol, attributeData, ServiceLifetimeUtlities.ToLifetime(lifetime), firstArgumentIndex: 1);
        }
    }

    // ReSharper disable once DuplicatedSequentialIfBodies
    // ReSharper disable once ConvertIfStatementToReturnStatement
    public static IEnumerable<InjectableData> FromInjectableAttribute(GeneratorAttributeSyntaxContext context, ServiceLifetime lifetime, int firstArgumentIndex = 0) {
        if (context.TargetNode is not ClassDeclarationSyntax classDeclaration) yield break;
        if (context.SemanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol) yield break;

        foreach (AttributeData attributeData in context.Attributes) {
            yield return ExtractServiceData(classSymbol, attributeData, lifetime, firstArgumentIndex);
        }
    }

    // ReSharper disable once DuplicatedSequentialIfBodies
    // ReSharper disable once ConvertIfStatementToReturnStatement
    public static IEnumerable<InjectableData> FromSyntaxAnalyzer(SyntaxNodeAnalysisContext context) {
        if (context.IsGeneratedCode) yield break;
        if (context.Node is not ClassDeclarationSyntax classDeclaration) yield break;
        if (context.SemanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol) yield break;

        ImmutableArray<AttributeData> attributes = classSymbol.GetAttributes();
        foreach (AttributeData attributeData in attributes) {
            string? fullMetadataName = attributeData.AttributeClass?.OriginalDefinition.ToDisplayString().Replace("<T>", "`1");
            switch (fullMetadataName) {
                case SourceCodes.InjectableAttributeMetadataName: {
                    int lifetime = attributeData.ConstructorArguments[0].Value as int? ?? -1;
                    yield return ExtractServiceData(classSymbol, attributeData, ServiceLifetimeUtlities.ToLifetime(lifetime), 1);
                    break;
                }

                case SourceCodes.InjectableSingletonAttributeMetadataName: {
                    yield return ExtractServiceData(classSymbol, attributeData, ServiceLifetime.Singleton, 1);
                    break;
                }

                case SourceCodes.InjectableScopedAttributeMetadataName: {
                    yield return ExtractServiceData(classSymbol, attributeData, ServiceLifetime.Scoped, 1);
                    break;
                }

                case SourceCodes.InjectableTransientAttributeMetadataName: {
                    yield return ExtractServiceData(classSymbol, attributeData, ServiceLifetime.Transient, 1);
                    break;
                }
            }
        }
    }

    private static InjectableData ExtractServiceData(INamedTypeSymbol classSymbol, AttributeData attributeData, ServiceLifetime lifetime, int firstArgumentIndex) {
        TypedConstant keyArgument = attributeData.ConstructorArguments.ElementAtOrDefault(firstArgumentIndex);
        ITypeSymbol? keyType = keyArgument.Type;
        object? keyObject = keyArgument.Value;

        string? serviceTypeName = attributeData.AttributeClass?.TypeArguments.First().ToString();
        string className = classSymbol.ToDisplayString();

        return new InjectableData(
            className,
            lifetime,
            serviceTypeName,
            keyObject,
            keyType is { SpecialType: SpecialType.System_String }
        );
    }
}
