using Kzrnm.Competitive.Analyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using VerifyCS = Kzrnm.Competitive.Analyzer.Test.CSharpCodeFixVerifier<
    Kzrnm.Competitive.Analyzer.IntToLong.Analyzer,
    Kzrnm.Competitive.Analyzer.IntToLong.CodeFixProvider>;

namespace Kzrnm.Competitive.Analyzer.Test;

public class IntToLongTest
{
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
            1& 1<<3 &5,
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

        await VerifyCS.VerifyAnalyzerAsync(source, [], TestContext.Current.CancellationToken);
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
            v * v / 2,
            v * v % 2,
            v << 5,
            v << 40 >> 5,
            v >> 5,
            1& 1<<3 &5,
        };

        var toULong = new ulong[]
        {
            2u * 4u * u,
            1u << v,
            1u >> v,
            u * 2 * 5,
            u * u,
            u * u / 2,
            u * u % 2,
            u << 5,
            u << 40 >> 5,
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
            1 >> v,
            (long)v * 2 * 5,
            (long)v * v,
            (long)v * v / 2,
            (long)v * v % 2,
            (long)v << 5,
            (long)v << 40 >> 5,
            v >> 5,
            1& 1<<3 &5,
        };

        var toULong = new ulong[]
        {
            2UL * 4u * u,
            1UL << v,
            1u >> v,
            (ulong)u * 2 * 5,
            (ulong)u * u,
            (ulong)u * u / 2,
            (ulong)u * u % 2,
            (ulong)u << 5,
            (ulong)u << 40 >> 5,
            u >> 5,
        };

        return (toLong, toULong);
    }
}
""";
        await VerifyCS.VerifyCodeFixAsync(source,
            [
                VerifyCS.Diagnostic().WithSpan(8, 13, 8, 18).WithArguments("2 * 4"),
                VerifyCS.Diagnostic().WithSpan(8, 13, 8, 22).WithArguments("2 * 4 * v"),
                VerifyCS.Diagnostic().WithSpan(9, 13, 9, 19).WithArguments("1 << v"),
                VerifyCS.Diagnostic().WithSpan(11, 13, 11, 18).WithArguments("v * 2"),
                VerifyCS.Diagnostic().WithSpan(11, 13, 11, 22).WithArguments("v * 2 * 5"),
                VerifyCS.Diagnostic().WithSpan(12, 13, 12, 18).WithArguments("v * v"),
                VerifyCS.Diagnostic().WithSpan(13, 13, 13, 18).WithArguments("v * v"),
                VerifyCS.Diagnostic().WithSpan(14, 13, 14, 18).WithArguments("v * v"),
                VerifyCS.Diagnostic().WithSpan(15, 13, 15, 19).WithArguments("v << 5"),
                VerifyCS.Diagnostic().WithSpan(16, 13, 16, 20).WithArguments("v << 40"),
                VerifyCS.Diagnostic().WithSpan(23, 13, 23, 20).WithArguments("2u * 4u"),
                VerifyCS.Diagnostic().WithSpan(23, 13, 23, 24).WithArguments("2u * 4u * u"),
                VerifyCS.Diagnostic().WithSpan(24, 13, 24, 20).WithArguments("1u << v"),
                VerifyCS.Diagnostic().WithSpan(26, 13, 26, 18).WithArguments("u * 2"),
                VerifyCS.Diagnostic().WithSpan(26, 13, 26, 22).WithArguments("u * 2 * 5"),
                VerifyCS.Diagnostic().WithSpan(27, 13, 27, 18).WithArguments("u * u"),
                VerifyCS.Diagnostic().WithSpan(28, 13, 28, 18).WithArguments("u * u"),
                VerifyCS.Diagnostic().WithSpan(29, 13, 29, 18).WithArguments("u * u"),
                VerifyCS.Diagnostic().WithSpan(30, 13, 30, 19).WithArguments("u << 5"),
                VerifyCS.Diagnostic().WithSpan(31, 13, 31, 20).WithArguments("u << 40"),
            ],
            fixedSource,
            TestContext.Current.CancellationToken);
    }
}