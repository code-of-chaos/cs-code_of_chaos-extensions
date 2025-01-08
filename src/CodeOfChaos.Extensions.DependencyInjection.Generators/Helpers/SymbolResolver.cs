// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators.Helpers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISymbolResolver {
    ISymbol? ResolveSymbol(SyntaxNode node);
}

// Simple wrapper to make testing not a living hell.
public class SymbolResolver(SemanticModel model) : ISymbolResolver {
    public ISymbol? ResolveSymbol(SyntaxNode node) {
        return model.GetSymbolInfo(node).Symbol;
    }
}
