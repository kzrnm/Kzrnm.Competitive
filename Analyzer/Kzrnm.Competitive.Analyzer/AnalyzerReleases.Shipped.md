; Shipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/master/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 1.0.0

### New Rules
Rule ID | Category | Severity | Notes
--------|----------|----------|-------
KZCOMPETITIVE0001 | Overflow | Warning | 32 bit number multiply expression is assigned to 64 bit number
KZCOMPETITIVE0002 | Overflow | Warning | 32 bit number shift expression is assigned to 64 bit number
KZCOMPETITIVE0003 | Type Define | Error | Not defined IStaticMod
KZCOMPETITIVE0004 | Type Define | Error | Not defined IDynamicModID
KZCOMPETITIVE0005 | Type Define | Error | Not defined ISegtreeOperator<T>
KZCOMPETITIVE0006 | Type Define | Error | Not defined ILazySegtreeOperator<T, F>
KZCOMPETITIVE0007 | Type Define | Info | Some operator methods don't have `MethodImpl` attribute

## Release 1.0.4

### New Rules
Rule ID | Category | Severity | Notes
--------|----------|----------|-------
KZCOMPETITIVE0008 | Type Define | Error | Not defined operator type

### Removed Rules
Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
KZCOMPETITIVE0003 | Type Define | Error | Not defined IStaticMod
KZCOMPETITIVE0004 | Type Define | Error | Not defined IDynamicModID
KZCOMPETITIVE0005 | Type Define | Error | Not defined ISegtreeOperator<T>
KZCOMPETITIVE0006 | Type Define | Error | Not defined ILazySegtreeOperator<T, F>
