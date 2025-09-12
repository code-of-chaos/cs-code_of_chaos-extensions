// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.GeneratorTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Generator(LanguageNames.CSharp)]
public class InjectableGenerator : IIncrementalGenerator {
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        IncrementalValueProvider<ImmutableArray<InjectableData>> injectablePipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            SourceCodes.InjectableAttributeTypeName,
            predicate: static (node, token) => node is ClassDeclarationSyntax,
            transform: static (context, token) => InjectableData.FromInjectableAttribute(context)
        ).Collect();
        
        IncrementalValueProvider<ImmutableArray<InjectableData>> injectableSingletonPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            SourceCodes.InjectableSingletonAttributeTypeName,
            predicate: static (node, token) => node is ClassDeclarationSyntax,
            transform: static (context, token) => InjectableData.FromInjectableAttribute(context, InjectableData.Singleton, SourceCodes.InjectableSingletonAttributeTypeName)
        ).Collect();
        
        IncrementalValueProvider<ImmutableArray<InjectableData>> injectableScopedPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            SourceCodes.InjectableScopedAttributeTypeName,
            predicate: static (node, token) => node is ClassDeclarationSyntax,
            transform: static (context, token) => InjectableData.FromInjectableAttribute(context, InjectableData.Scoped, SourceCodes.InjectableScopedAttributeTypeName)
        ).Collect();
        
        IncrementalValueProvider<ImmutableArray<InjectableData>> injectableTransientPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            SourceCodes.InjectableTransientAttributeTypeName,
            predicate: static (node, token) => node is ClassDeclarationSyntax,
            transform: static (context, token) => InjectableData.FromInjectableAttribute(context, InjectableData.Transient, SourceCodes.InjectableTransientAttributeTypeName)
        ).Collect();
        
        IncrementalValueProvider<string> assemblyNamePipeline = IncrementalGeneratorUtilities.GetAssemblyNamePipeline(context);
        
        // Register Outputs
        context.RegisterSourceOutput(assemblyNamePipeline, (productionContext, assemblyName) => {
            productionContext.AddSource("InjectableRegistration.g.cs", SourceCodes.GetInjectableGenerator(assemblyName));
        });
        
        context.RegisterSourceOutput(injectablePipeline, (productionContext, syntaxContext) => {
            productionContext.AddSource($"InjectableGenerator_{Guid.NewGuid():N}.g.cs", string.Join("\n", syntaxContext.Select(s => s.ToString())));
        });
    }
}
