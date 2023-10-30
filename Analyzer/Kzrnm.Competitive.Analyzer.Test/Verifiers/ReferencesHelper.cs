using Microsoft.CodeAnalysis.Testing;
using System.Collections.Immutable;

namespace Kzrnm.Competitive.Analyzer.Test;

internal static class ReferencesHelper
{
    internal static ImmutableArray<PackageIdentity> Packages
        = ImmutableArray.Create(new PackageIdentity("ac-library-csharp", "3.5.0"));
}
