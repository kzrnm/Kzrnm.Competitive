; Shipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/master/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 1.0.0

### New Rules
Rule ID | Category | Severity | Notes
--------|----------|----------|-------
KZCOMPETITIVE0001 | Overflow | Warning | 32 bit number multiply expression is assigned to 64 bit number
KZCOMPETITIVE0002 | Overflow | Warning | 32 bit number shift expression is assigned to 64 bit number
KZCOMPETITIVE0003 | Type Define | Info | Some operator methods don't have `MethodImpl` attribute
KZCOMPETITIVE0004 | Type Define | Error | Not defined operator type