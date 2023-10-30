using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Kzrnm.Competitive.Analyzer.Diagnostics;
public static class DiagnosticDescriptors
{
    public static Diagnostic KZCOMPETITIVE0001_MultiplyOverflowInt32(SyntaxNode node, bool isUnsigned)
        => Diagnostic.Create(
            descriptor: KZCOMPETITIVE0001_MultiplyOverflowInt32_Descriptor,
            location: node.GetLocation(),
            properties: ImmutableDictionary.CreateRange(new[]
            {
                new KeyValuePair<string, string>("IsUnsigned", isUnsigned?"true":"false"),
            }),
            messageArgs: node.ToString());
    public static readonly DiagnosticDescriptor KZCOMPETITIVE0001_MultiplyOverflowInt32_Descriptor = new(
        "KZCOMPETITIVE0001",
        new LocalizableResourceString(
            nameof(DiagnosticsResources.KZCOMPETITIVE0001_Title),
            DiagnosticsResources.ResourceManager,
            typeof(DiagnosticsResources)),
        new LocalizableResourceString(
            nameof(DiagnosticsResources.KZCOMPETITIVE0001_KZCOMPETITIVE0002_MessageFormat),
            DiagnosticsResources.ResourceManager,
            typeof(DiagnosticsResources)),
        "Overflow",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );
    public static Diagnostic KZCOMPETITIVE0002_ShiftOverflowInt32(SyntaxNode node, bool isUnsigned)
        => Diagnostic.Create(
            descriptor: KZCOMPETITIVE0002_ShiftOverflowInt32_Descriptor,
            location: node.GetLocation(),
            properties: ImmutableDictionary.CreateRange(new[]
            {
                new KeyValuePair<string, string>("IsUnsigned", isUnsigned?"true":"false"),
            }),
            messageArgs: node.ToString());
    public static readonly DiagnosticDescriptor KZCOMPETITIVE0002_ShiftOverflowInt32_Descriptor = new(
        "KZCOMPETITIVE0002",
        new LocalizableResourceString(
            nameof(DiagnosticsResources.KZCOMPETITIVE0002_Title),
            DiagnosticsResources.ResourceManager,
            typeof(DiagnosticsResources)),
        new LocalizableResourceString(
            nameof(DiagnosticsResources.KZCOMPETITIVE0001_KZCOMPETITIVE0002_MessageFormat),
            DiagnosticsResources.ResourceManager,
            typeof(DiagnosticsResources)),
        "Overflow",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    public static Diagnostic KZCOMPETITIVE0003_AgressiveInlining(Location location, IEnumerable<string> methods)
        => Diagnostic.Create(KZCOMPETITIVE0003_AgressiveInlining_Descriptor, location, string.Join(", ", methods));

    public static readonly DiagnosticDescriptor KZCOMPETITIVE0003_AgressiveInlining_Descriptor = new(
        "KZCOMPETITIVE0003",
        new LocalizableResourceString(
            nameof(DiagnosticsResources.KZCOMPETITIVE0003_Title),
            DiagnosticsResources.ResourceManager,
            typeof(DiagnosticsResources)),
        new LocalizableResourceString(
            nameof(DiagnosticsResources.KZCOMPETITIVE0003_MessageFormat),
            DiagnosticsResources.ResourceManager,
            typeof(DiagnosticsResources)),
        "Type Define",
        DiagnosticSeverity.Info,
        isEnabledByDefault: true
        );

    public static Diagnostic KZCOMPETITIVE0004_DefineOperatorType(Location location, IEnumerable<string> types)
        => Diagnostic.Create(KZCOMPETITIVE0004_DefineOperatorType_Descriptor, location, string.Join(", ", types));
    public static readonly DiagnosticDescriptor KZCOMPETITIVE0004_DefineOperatorType_Descriptor = new(
        "KZCOMPETITIVE0004",
        new LocalizableResourceString(
            nameof(DiagnosticsResources.KZCOMPETITIVE0004_Title),
            DiagnosticsResources.ResourceManager,
            typeof(DiagnosticsResources)),
        new LocalizableResourceString(
            nameof(DiagnosticsResources.KZCOMPETITIVE0004_MessageFormat),
            DiagnosticsResources.ResourceManager,
            typeof(DiagnosticsResources)),
        "Type Define",
        DiagnosticSeverity.Error,

        isEnabledByDefault: true
        );
}

