// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record InjectableData(
    string ClassName,
    int ServiceLifetimeValue, 
    string? ServiceTypeName
) {
    public string ServiceLifetime => ServiceLifetimeValue switch {
        Singleton => "Singleton",
        Scoped => "Scoped",
        Transient => "Transient",
        _ => "Unknown"
    };

    public const int Singleton = 0;
    public const int Scoped = 1;
    public const int Transient = 2;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static InjectableData FromInjectableAttribute( GeneratorAttributeSyntaxContext context) {
        AttributeData? attribute = context.Attributes.FirstOrDefault(a => a.AttributeClass?.MetadataName == SourceCodes.InjectableAttributeTypeName);
        int lifetime = attribute?.ConstructorArguments[0].Value as int? ?? -1;
        return FromInjectableAttribute(context, lifetime, SourceCodes.InjectableAttributeTypeName);
    }
    
    public static InjectableData FromInjectableAttribute( GeneratorAttributeSyntaxContext context, int lifetime, string attributeMetaDataName) {
        var classDeclaration = (ClassDeclarationSyntax)context.TargetNode;
        AttributeData? attribute = context.Attributes.FirstOrDefault(a => a.AttributeClass?.MetadataName == attributeMetaDataName);
        string? serviceTypeName = attribute?.AttributeClass?.TypeArguments.First().ToString();
        return new InjectableData(classDeclaration.Identifier.Text, lifetime, serviceTypeName);
    }
}