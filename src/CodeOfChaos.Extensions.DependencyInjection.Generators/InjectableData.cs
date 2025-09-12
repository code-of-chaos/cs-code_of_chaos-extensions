// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.GeneratorTools;
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
    string? ServiceLifetime,
    string? ServiceTypeName,
    object? ServiceKey,
    LocationInfo? LocationInfo
) {
    public const int Singleton = 0;
    public const int Scoped = 1;
    public const int Transient = 2;
    public bool IsEmpty { get; private set; }

    [MemberNotNullWhen(true, nameof(ServiceKey))] public bool HasServiceKey => ServiceKey is not null;

    private static readonly InjectableData Empty = new(null, null, null, null, null) {
        IsEmpty = true
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static InjectableData FromInjectableAttribute(GeneratorAttributeSyntaxContext context) {
        AttributeData? attribute = context.Attributes.ElementAtOrDefault(0);
        int lifetime = attribute?.ConstructorArguments[0].Value as int? ?? -1;
        return FromInjectableAttribute(context, lifetime, firstArgumentIndex: 1);// Start at 1 to skip the lifetime argument if it exists
    }

    // ReSharper disable once DuplicatedSequentialIfBodies
    // ReSharper disable once ConvertIfStatementToReturnStatement
    public static InjectableData FromInjectableAttribute(GeneratorAttributeSyntaxContext context, int lifetime, int firstArgumentIndex = 0) {
        if (context.TargetNode is not ClassDeclarationSyntax classDeclaration) return Empty;
        if (context.SemanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol) return Empty;
        if (context.Attributes.ElementAtOrDefault(0) is not {} attributeData) return Empty;

        return ExtractServiceData(classSymbol, classDeclaration, attributeData, lifetime, firstArgumentIndex);
    }

    // ReSharper disable once DuplicatedSequentialIfBodies
    // ReSharper disable once ConvertIfStatementToReturnStatement
    public static IEnumerable<InjectableData> FromSyntaxAnalyzer(SyntaxNodeAnalysisContext context) {
        if (context.IsGeneratedCode) yield break;
        if (context.Node is not ClassDeclarationSyntax classDeclaration) yield break;
        if (context.SemanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol) yield break;

        ImmutableArray<AttributeData> attributes = classSymbol.GetAttributes();
        foreach (AttributeData attributeData in attributes) {
            var fullMetadataName = attributeData.AttributeClass?.OriginalDefinition.ToDisplayString().Replace("<T>", "`1");
            switch (fullMetadataName) {
                case SourceCodes.InjectableAttributeMetadataName: {
                    int lifetime = attributeData.ConstructorArguments[0].Value as int? ?? -1;
                    yield return ExtractServiceData(classSymbol, classDeclaration, attributeData, lifetime, 1);
                    break;
                }

                case SourceCodes.InjectableSingletonAttributeMetadataName: {
                    yield return ExtractServiceData(classSymbol, classDeclaration, attributeData, Singleton, 1);
                    break;

                }

                case SourceCodes.InjectableScopedAttributeMetadataName: {
                    yield return ExtractServiceData(classSymbol, classDeclaration, attributeData, Scoped, 1);
                    break;
                }

                case SourceCodes.InjectableTransientAttributeMetadataName: {
                    yield return ExtractServiceData(classSymbol, classDeclaration, attributeData, Transient, 1);
                    break;
                }
            }
        }
    }

    private static InjectableData ExtractServiceData(INamedTypeSymbol classSymbol, ClassDeclarationSyntax classDeclaration, AttributeData attributeData, int lifetime, int firstArgumentIndex) {
        object? keyObject = attributeData.ConstructorArguments.ElementAtOrDefault(firstArgumentIndex).Value;

        string? serviceTypeName = attributeData.AttributeClass?.TypeArguments.First().ToString();
        string className = classSymbol.ToDisplayString();
        LocationInfo? locationInfo = LocationInfo.From(classDeclaration.GetLocation());

        return new InjectableData(
            className,
            lifetime switch {
                Singleton => "Singleton",
                Scoped => "Scoped",
                Transient => "Transient",
                _ => null
            },
            serviceTypeName,
            keyObject,
            locationInfo
        );
    }
}
