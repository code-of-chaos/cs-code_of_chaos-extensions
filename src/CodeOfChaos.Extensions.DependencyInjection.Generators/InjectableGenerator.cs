// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.GeneratorTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Generator(LanguageNames.CSharp)]
public class InjectableGenerator : IIncrementalGenerator {
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        IncrementalValueProvider<ImmutableArray<InjectableData>> injectablePipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            SourceCodes.InjectableAttributeMetadataName,
            predicate: static (node, _) => node is ClassDeclarationSyntax,
            transform: static (context, _) => InjectableData.FromInjectableAttribute(context)
        ).Collect();
        
        IncrementalValueProvider<ImmutableArray<InjectableData>> injectableSingletonPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            SourceCodes.InjectableSingletonAttributeMetadataName,
            predicate: static (node, _) => node is ClassDeclarationSyntax,
            transform: static (context, _) => InjectableData.FromInjectableAttribute(context, InjectableData.Singleton)
        ).Collect();
        
        IncrementalValueProvider<ImmutableArray<InjectableData>> injectableScopedPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            SourceCodes.InjectableScopedAttributeMetadataName,
            predicate: static (node, _) => node is ClassDeclarationSyntax,
            transform: static (context, _) => InjectableData.FromInjectableAttribute(context, InjectableData.Scoped)
        ).Collect();
        
        IncrementalValueProvider<ImmutableArray<InjectableData>> injectableTransientPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            SourceCodes.InjectableTransientAttributeMetadataName,
            predicate: static (node, _) => node is ClassDeclarationSyntax,
            transform: static (context, _) => InjectableData.FromInjectableAttribute(context, InjectableData.Transient)
        ).Collect();
        
        IncrementalValueProvider<string> assemblyNamePipeline = IncrementalGeneratorUtilities.GetAssemblyNamePipeline(context);
        
        // Register Outputs
        context.RegisterSourceOutput(assemblyNamePipeline, (productionContext, assemblyName) => {
            productionContext.AddSource("InjectableRegistration.g.cs", SourceCodes.GetInjectableGenerator(assemblyName));
        });
        
        context.RegisterSourceOutput(injectablePipeline.Combine(assemblyNamePipeline), (productionContext, box) => {
            (ImmutableArray<InjectableData> data,string assemblyName) = box;
            productionContext.AddSource("InjectableUtility_Services.g.cs", InjectableWriter.GetSource(data, assemblyName));
        });
        
        context.RegisterSourceOutput(injectableSingletonPipeline.Combine(assemblyNamePipeline), (productionContext, box) => {
            (ImmutableArray<InjectableData> data,string assemblyName) = box;
            productionContext.AddSource("InjectableUtility_Singleton.g.cs", InjectableWriter.GetSource(data, assemblyName));
        });
        
        context.RegisterSourceOutput(injectableScopedPipeline.Combine(assemblyNamePipeline), (productionContext, box) => {
            (ImmutableArray<InjectableData> data,string assemblyName) = box;
            productionContext.AddSource("InjectableUtility_Scoped.g.cs", InjectableWriter.GetSource(data, assemblyName));
        });
        
        context.RegisterSourceOutput(injectableTransientPipeline.Combine(assemblyNamePipeline), (productionContext, box) => {
            (ImmutableArray<InjectableData> data,string assemblyName) = box;
            productionContext.AddSource("InjectableUtility_Transient.g.cs", InjectableWriter.GetSource(data, assemblyName));
        });
    }
}
