// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection.Generators.Registrations;
using CodeOfChaos.GeneratorTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SymbolResolver=CodeOfChaos.Extensions.DependencyInjection.Generators.Helpers.SymbolResolver;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Generator(LanguageNames.CSharp)]
public class ServiceRegistrationGenerator : IIncrementalGenerator {
    private const string ServiceRegistrationFileName = "ServiceRegistration.g.cs";
    private const string PooledServicesFileName = "AutoPooledServices.g.cs";

    private const string InjectableServiceAttributeMetadataName = "CodeOfChaos.Extensions.DependencyInjection.InjectableServiceAttribute`1";
    private const string InjectableSingletonAttributeMetadataName = "CodeOfChaos.Extensions.DependencyInjection.InjectableSingletonAttribute`1";
    private const string InjectableScopedAttributeMetadataName = "CodeOfChaos.Extensions.DependencyInjection.InjectableScopedAttribute`1";
    private const string InjectableTransientAttributeMetadataName = "CodeOfChaos.Extensions.DependencyInjection.InjectableTransientAttribute`1";
    private const string FactoryCreatedServiceAttributeMetadataName = "CodeOfChaos.Extensions.DependencyInjection.FactoryCreatedServiceAttribute`2";
    private const string PooledInjectableServiceAttributeMetadataName = "CodeOfChaos.Extensions.DependencyInjection.PooledInjectableServiceAttribute`2";

    private static readonly string[] MetaDataNames = [
        InjectableServiceAttributeMetadataName,
        InjectableSingletonAttributeMetadataName,
        InjectableScopedAttributeMetadataName,
        InjectableTransientAttributeMetadataName,

        FactoryCreatedServiceAttributeMetadataName,
        PooledInjectableServiceAttributeMetadataName
    ];

    private static Regex RegexSanitizeAssemblyName { get; } = new(@"((?im)[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?)|(\.dll)", RegexOptions.Compiled);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        IncrementalValueProvider<ImmutableArray<ClassDeclarationSyntax>> syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (node, _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
                transform: (ctx, _) => (ClassDeclarationSyntax)ctx.Node
            ).Collect();

        context.RegisterSourceOutput(context.CompilationProvider.Combine(syntaxProvider), GenerateSources);
    }

    #region SourceGenerator
    private static void GenerateSources(SourceProductionContext context, (Compilation, ImmutableArray<ClassDeclarationSyntax>) source) {
        (Compilation? compilation, ImmutableArray<ClassDeclarationSyntax> classDeclarations) = source;

        if (compilation.AssemblyName is not {} assemblyName) {
            ReportDiagnostic(context, Rules.NoAssemblyNameFound);
            return;
        }

        IServiceRegistration[] registrations = GetRegistrations(context, compilation, classDeclarations)
            .OrderBy(registration => registration.LifeTime)
            .ThenBy(registration => registration.ServiceTypeName.ToDisplayString())
            .ThenBy(registration => registration.ImplementationTypeName.ToDisplayString())
            .ToArray();

        // This fixes an issue with the testing environment, where we add a guid to the assembly name, to deter conflicts
        string assemblyNameSanitized = RegexSanitizeAssemblyName.Replace(assemblyName, string.Empty)
            .Replace("-", "_")
            .TrimEnd('-', '_');


        context.AddSource(
            PooledServicesFileName,
            SourceText.From(GeneratePooledServicesFile(
                context,
                assemblyNameSanitized,
                registrations
            ), Encoding.UTF8)
        );

        context.AddSource(
            ServiceRegistrationFileName,
            SourceText.From(GenerateServiceRegistrationFile(
                context,
                assemblyNameSanitized,
                registrations
            ), Encoding.UTF8)
        );
    }

    private static List<IServiceRegistration> GetRegistrations(SourceProductionContext context, Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classDeclarations) {
        Dictionary<string, INamedTypeSymbol?> types = MetaDataNames.ToDictionary(keySelector: name => name, compilation.GetTypeByMetadataName);
        if (types.Values.Any(t => t is null)) {
            ReportDiagnostic(context, Rules.NoAttributesFound);
            return [];
        }

        // See above if check to know why we ! for nullablity
        INamedTypeSymbol injectableServiceAttributeType = types[InjectableServiceAttributeMetadataName]!;
        INamedTypeSymbol injectableSingletonAttributeType = types[InjectableSingletonAttributeMetadataName]!;
        INamedTypeSymbol injectableScopedAttributeType = types[InjectableScopedAttributeMetadataName]!;
        INamedTypeSymbol injectableTransientAttributeType = types[InjectableTransientAttributeMetadataName]!;

        INamedTypeSymbol factoryCreateServiceAttributeType = types[FactoryCreatedServiceAttributeMetadataName]!;
        INamedTypeSymbol injectablePooledServiceAttributeType = types[PooledInjectableServiceAttributeMetadataName]!;

        List<IServiceRegistration> registrations = [];
        foreach (ClassDeclarationSyntax candidate in classDeclarations) {
            SemanticModel model = compilation.GetSemanticModel(candidate.SyntaxTree);
            if (model.GetDeclaredSymbol(candidate) is not {} implementationTypeSymbol) continue;

            foreach (AttributeSyntax attribute in candidate.AttributeLists.SelectMany(attrList => attrList.Attributes)) {
                if (model.GetTypeInfo(attribute).Type is not INamedTypeSymbol attributeTypeInfo) continue;
                INamedTypeSymbol constructedFrom = attributeTypeInfo.ConstructedFrom;

                if (SymbolEqualityComparer.Default.Equals(constructedFrom, factoryCreateServiceAttributeType)
                    && FactoryCreatedServiceRegistration.TryCreateFromModel(implementationTypeSymbol, attribute, new SymbolResolver(model), out FactoryCreatedServiceRegistration factoryCreated)) {
                    registrations.Add(factoryCreated);
                    continue;
                }
                if ((SymbolEqualityComparer.Default.Equals(constructedFrom, injectableSingletonAttributeType)
                        || SymbolEqualityComparer.Default.Equals(constructedFrom, injectableScopedAttributeType)
                        || SymbolEqualityComparer.Default.Equals(constructedFrom, injectableTransientAttributeType))
                    && SpecifcInjectableServiceRegistration.TryCreateFromModel(implementationTypeSymbol, attribute, new SymbolResolver(model), out SpecifcInjectableServiceRegistration specificInjectable)) {
                    registrations.Add(specificInjectable);
                    continue;
                }

                if (SymbolEqualityComparer.Default.Equals(constructedFrom, injectableServiceAttributeType)
                    && InjectableServiceRegistration.TryCreateFromModel(implementationTypeSymbol, attribute, new SymbolResolver(model), out InjectableServiceRegistration injectable)) {
                    registrations.Add(injectable);
                    continue;
                }

                // ReSharper disable once InvertIf
                // ReSharper disable once RedundantJumpStatement
                if (SymbolEqualityComparer.Default.Equals(constructedFrom, injectablePooledServiceAttributeType)
                    && InjectablePoolableServiceRegistration.TryCreateFromModel(attribute, new SymbolResolver(model), out InjectablePoolableServiceRegistration pooledInjectable)) {
                    registrations.Add(pooledInjectable);
                    continue;
                }
            }
        }

        return registrations;
    }

    private static string GenerateServiceRegistrationFile(SourceProductionContext _, string assemblyName, IServiceRegistration[] registrations) {
        GeneratorStringBuilder builder = new GeneratorStringBuilder()
            .AppendAutoGenerated()
            .AppendUsings("Microsoft.Extensions.DependencyInjection")
            .AppendNamespace(assemblyName)
            .AppendLine()
            .AppendLine("public static class ServiceRegistration {")
            .Indent(b => {
                b.AppendLine($"public static IServiceCollection RegisterServicesFrom{Sanitize(assemblyName)}(this IServiceCollection services) {{");
                if (registrations.Any(r => r is InjectablePoolableServiceRegistration)) {
                    b.AppendLineIndented($"services.AddSingleton<{assemblyName}.AutoPooledServices>();");
                }

                b.Indent(b2 => b2.ForEach(
                    registrations,
                    itemFormatter: static (builder, registration, assemblyName) => registration.FormatText(builder, assemblyName),
                    assemblyName
                ));
                b.AppendLineIndented("return services;");
                b.AppendLine("}");
            })
            .AppendLine("}");

        return builder.ToString();
    }

    private static string GeneratePooledServicesFile(SourceProductionContext _, string assemblyName, IServiceRegistration[] registrations) {
        GeneratorStringBuilder builder = new GeneratorStringBuilder()
            .AppendAutoGenerated()
            .AppendUsings("Microsoft.Extensions.ObjectPool")
            .AppendNamespace(assemblyName)
            .AppendLine()
            .AppendLine("public partial class AutoPooledServices {")
            .Indent(b => b
                .AppendLine("private static readonly DefaultObjectPoolProvider _objectPoolProvider = new();")
                .Indent(b2 => b2.ForEach(
                    registrations.OfType<InjectablePoolableServiceRegistration>(),
                    itemFormatter: static (builder, registration) => registration.FormatPoolText(builder)
                ))
                .AppendLine()
            )
            .AppendLine("}");

        return builder.ToString();
    }
    #endregion

    #region Helper Methods
    private static void ReportDiagnostic(SourceProductionContext context, DiagnosticDescriptor rule) => context.ReportDiagnostic(Diagnostic.Create(rule, Location.None));
    private static string Sanitize(string input) => new(input.Where(char.IsLetterOrDigit).ToArray());
    #endregion
}
