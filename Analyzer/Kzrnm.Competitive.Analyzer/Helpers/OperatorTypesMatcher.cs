using Microsoft.CodeAnalysis;

namespace Kzrnm.Competitive.Analyzer.Helpers;

using static Constants;

public class OperatorTypesMatcher(INamedTypeSymbol isOperator, INamedTypeSymbol iComparer)
{
    public INamedTypeSymbol IsOperatorAttribute { get; } = isOperator;
    public INamedTypeSymbol IComparer { get; } = iComparer;

    public bool IsMatch(ITypeSymbol typeSymbol)
    {
        if (SymbolEqualityComparer.Default.Equals(typeSymbol, IComparer))
            return true;

        foreach (var at in typeSymbol.GetAttributes())
        {
            if (SymbolEqualityComparer.Default.Equals(at.AttributeClass, IsOperatorAttribute))
                return true;
        }
        return false;
    }

    public static bool TryParseTypes(Compilation compilation, out OperatorTypesMatcher types)
    {
        types = null;
        var isOperatorAttr = compilation.GetTypeByMetadataName(AtCoder_IsOperatorAttribute);
        if (isOperatorAttr is null)
            return false;
        var iComparer = compilation.GetTypeByMetadataName(System_Collections_Generic_IComparer);
        if (iComparer is null)
            return false;
        types = new OperatorTypesMatcher(isOperatorAttr, iComparer);
        return true;
    }
}
