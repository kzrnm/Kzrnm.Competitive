using Kzrnm.Competitive.Analyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using VerifyCS = Kzrnm.Competitive.Analyzer.Test.CSharpCodeFixVerifier<
    Kzrnm.Competitive.Analyzer.IntToLong.Analyzer,
    Kzrnm.Competitive.Analyzer.IntToLong.CodeFixProvider>;

namespace Kzrnm.Competitive.Analyzer.Test;

public class IntToLongTest
{
    static DiagnosticDescriptor KZCOMPETITIVE0001 => DiagnosticDescriptors.KZCOMPETITIVE0001_MultiplyOverflowInt32_Descriptor;

    [Fact]
    public async Task Empty()
    {
        var source = """
public static class Example
{
    public static (long[], ulong[]) KZCOMPETITIVE0001(int v)
    {
        var u = (uint)v;
        var toLong = new long[]
        {
            2L * 4 * v,
            1L << v,
            1L >> v,
            (long)v * 2 * 5,
            (long)v * v,
            (long)v << 5,
            (long)v >> 5,
        };

        var toULong = new ulong[]
        {
            2ul * 4u * u,
            1ul << v,
            1ul >> v,
            (ulong)u * 2 * 5,
            (ulong)u * u,
            (ulong)u << 5,
            (ulong)u >> 5,
        };

        return (toLong, toULong);
    }
}
""";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }
    [Fact]
    public async Task Hit()
    {
        var source = """
public static class Example
{
    public static (long[], ulong[]) KZCOMPETITIVE0001(int v)
    {
        var u = (uint)v;
        var toLong = new long[]
        {
            2 * 4 * v,
            1 << v,
            1 >> v,
            v * 2 * 5,
            v * v,
            v << 5,
            v >> 5,
        };

        var toULong = new ulong[]
        {
            2u * 4u * u,
            1u << v,
            1u >> v,
            u * 2 * 5,
            u * u,
            u << 5,
            u >> 5,
        };

        return (toLong, toULong);
    }
}
""";

        var fixedSource = """
public static class Example
{
    public static (long[], ulong[]) KZCOMPETITIVE0001(int v)
    {
        var u = (uint)v;
        var toLong = new long[]
        {
            2L * 4 * v,
            1L << v,
            1L >> v,
            (long)v * 2 * 5,
            (long)v * v,
            (long)v << 5,
            (long)v >> 5,
        };

        var toULong = new ulong[]
        {
            2UL * 4u * u,
            1UL << v,
            1UL >> v,
            (ulong)u * 2 * 5,
            (ulong)u * u,
            (ulong)u << 5,
            (ulong)u >> 5,
        };

        return (toLong, toULong);
    }
}
""";
        await VerifyCS.VerifyCodeFixAsync(source,
            new DiagnosticResult[] {
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(8, 13, 8, 18).WithArguments("2 * 4"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(8, 13, 8, 22).WithArguments("2 * 4 * v"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(9, 13, 9, 19).WithArguments("1 << v"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(10, 13, 10, 19).WithArguments("1 >> v"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(11, 13, 11, 18).WithArguments("v * 2"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(11, 13, 11, 22).WithArguments("v * 2 * 5"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(12, 13, 12, 18).WithArguments("v * v"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(13, 13, 13, 19).WithArguments("v << 5"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(14, 13, 14, 19).WithArguments("v >> 5"),

                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(19, 13, 19, 20).WithArguments("2u * 4u"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(19, 13, 19, 24).WithArguments("2u * 4u * u"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(20, 13, 20, 20).WithArguments("1u << v"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(21, 13, 21, 20).WithArguments("1u >> v"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(22, 13, 22, 18).WithArguments("u * 2"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(22, 13, 22, 22).WithArguments("u * 2 * 5"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(23, 13, 23, 18).WithArguments("u * u"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(24, 13, 24, 19).WithArguments("u << 5"),
                VerifyCS.Diagnostic(KZCOMPETITIVE0001).WithSpan(25, 13, 25, 19).WithArguments("u >> 5"),
            },
            fixedSource);
    }
}