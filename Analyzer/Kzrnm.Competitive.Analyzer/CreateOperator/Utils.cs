using Kzrnm.Competitive.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Analyzer.CreateOperator;

record struct GenericParameterSymbol(ITypeParameterSymbol OriginalType, ITypeSymbol WrittenType);
static class Utils
{
    public static IEnumerable<GenericParameterSymbol> EnumerateNotDefinedTypes(
        this OperatorTypesMatcher types, ISymbol symbol)
        => types.EnumerateTypeSymbols(symbol).NotDefinedOperators;
    public static (IEnumerable<GenericParameterSymbol> DefinedTypes, IEnumerable<GenericParameterSymbol> NotDefinedOperators) EnumerateTypeSymbols(
        this OperatorTypesMatcher types, ISymbol symbol)
    {
        var (typeParameters, typeArguments) = symbol switch
        {
            INamedTypeSymbol s => (s.TypeParameters, s.TypeArguments),
            IMethodSymbol s => (s.TypeParameters, s.TypeArguments),
            _ => ([], []),
        };

        if (typeParameters.Length == 0 || typeParameters.Length != typeArguments.Length)
            return ([], []);

        var definedTypes = new List<GenericParameterSymbol>();
        var notDefinedTypes = new List<GenericParameterSymbol>();
        for (int i = 0; i < typeParameters.Length; i++)
        {
            var originalType = typeParameters[i];
            var writtenType = typeArguments[i];

            if (originalType.ConstraintTypes.OfType<INamedTypeSymbol>()
                    .Select(s => s.ConstructedFrom)
                    .Any(types.IsMatch))
            {
                if (writtenType.TypeKind is TypeKind.Error)
                    notDefinedTypes.Add(new(originalType, writtenType));
            }
            else
            {
                definedTypes.Add(new(originalType, writtenType));
            }
        }
        return (definedTypes, notDefinedTypes);
    }
}
