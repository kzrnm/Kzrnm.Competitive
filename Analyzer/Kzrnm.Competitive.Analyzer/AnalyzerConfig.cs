using System;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Kzrnm.Competitive.Analyzer;

public record AnalyzerConfig(bool UseMethodImplNumeric)
{
    public static AnalyzerConfig Parse(AnalyzerConfigOptions analyzerConfigOptions)
    {
        var useMethodImplNumeric = analyzerConfigOptions.TryGetValue("build_property.AtCoderAnalyzer_UseMethodImplNumeric", out var v) &&
            StringComparer.OrdinalIgnoreCase.Equals(v, "true");

        return new(
            UseMethodImplNumeric: useMethodImplNumeric
        );
    }
}
