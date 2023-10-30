using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Kzrnm.Competitive.Analyzer.Diagnostics;
using static DiagnosticsResources;

public static class DiagnosticDescriptors
{
    public static Diagnostic KZCOMPETITIVE0001_OverflowInt32(SyntaxNode node, bool isUnsigned)
        => Diagnostic.Create(
            descriptor: KZCOMPETITIVE0001_OverflowInt32_Descriptor,
            location: node.GetLocation(),
            properties: ImmutableDictionary.CreateRange(new[]
            {
                new KeyValuePair<string, string>("IsUnsigned", isUnsigned?"true":"false"),
            }),
            messageArgs: node.ToString());
    public static readonly DiagnosticDescriptor KZCOMPETITIVE0001_OverflowInt32_Descriptor = new(
        nameof(KZCOMPETITIVE0001),
        new LocalizableResourceString(
            nameof(KZCOMPETITIVE0001),
            ResourceManager,
            typeof(DiagnosticsResources)),
        new LocalizableResourceString(
            nameof(KZCOMPETITIVE0001_MessageFormat),
            ResourceManager,
            typeof(DiagnosticsResources)),
        "Overflow",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    public static Diagnostic KZCOMPETITIVE0003_AgressiveInlining(Location location, IEnumerable<string> methods)
        => Diagnostic.Create(KZCOMPETITIVE0003_AgressiveInlining_Descriptor, location, string.Join(", ", methods));
    public static readonly DiagnosticDescriptor KZCOMPETITIVE0003_AgressiveInlining_Descriptor = new(
        nameof(KZCOMPETITIVE0003),
        new LocalizableResourceString(
            nameof(KZCOMPETITIVE0003),
            ResourceManager,
            typeof(DiagnosticsResources)),
        new LocalizableResourceString(
            nameof(KZCOMPETITIVE0003_MessageFormat),
            ResourceManager,
            typeof(DiagnosticsResources)),
        "Type Define",
        DiagnosticSeverity.Info,
        isEnabledByDefault: true
        );

    public static Diagnostic KZCOMPETITIVE0004_DefineOperatorType(Location location, IEnumerable<string> types)
        => Diagnostic.Create(KZCOMPETITIVE0004_DefineOperatorType_Descriptor, location, string.Join(", ", types));
    public static readonly DiagnosticDescriptor KZCOMPETITIVE0004_DefineOperatorType_Descriptor = new(
        nameof(KZCOMPETITIVE0004),
        new LocalizableResourceString(
            nameof(KZCOMPETITIVE0004),
            ResourceManager,
            typeof(DiagnosticsResources)),
        new LocalizableResourceString(
            nameof(KZCOMPETITIVE0004_MessageFormat),
            ResourceManager,
            typeof(DiagnosticsResources)),
        "Type Define",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
        );
}

