using Kzrnm.Competitive.Analyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using System.Text.RegularExpressions;
using VerifyCS = Kzrnm.Competitive.Analyzer.Test.CSharpCodeFixVerifier<
    Kzrnm.Competitive.Analyzer.CreateOperator.Analyzer,
    Kzrnm.Competitive.Analyzer.CreateOperator.CodeFixProvider>;

namespace Kzrnm.Competitive.Analyzer.Test;

public partial class CreateOperatorCodeFixProviderTest
{
    public class Test(LanguageVersion languageVersion) : VerifyCS.Test
    {
        protected override ParseOptions CreateParseOptions()
            => ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(languageVersion);
    }

    static DiagnosticDescriptor KZCOMPETITIVE0004 => DiagnosticDescriptors.KZCOMPETITIVE0004_DefineOperatorType_Descriptor;

    static async Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource, Action<Test>? updateTest = null)
        => await VerifyCodeFixAsync(source, [expected], fixedSource, updateTest: updateTest);

    static async Task VerifyCodeFixAsync(string source, DiagnosticResult[] expected, string fixedSource, Action<Test>? updateTest = null)
    {
        foreach (var languageVersion in new LanguageVersion[]
        {
            LanguageVersion.CSharp7_1,
            LanguageVersion.CSharp7_2,
            LanguageVersion.CSharp10,
        })
        {
            var test = new Test(languageVersion)
            {
                TestCode = source,
                FixedCode = ConvertFixedSource(fixedSource, languageVersion),
            };
            test.ExpectedDiagnostics.AddRange(expected);

            updateTest?.Invoke(test);

            await test.RunAsync(CancellationToken.None);
        }
    }


    [GeneratedRegex(@"\$\$([^\$]*)\$\$")]
    private static partial Regex ConvertFixedSourceRegex();
    static string ConvertFixedSource(string fixedSource, LanguageVersion languageVersion)
    {
        return languageVersion switch
        {
            LanguageVersion.CSharp7_1 => ConvertFixedSourceRegex().Replace(fixedSource, m => m.Value switch
            {
                "$$TypeDefinition$$" => "struct",
                _ => throw new InvalidOperationException(),
            }),
            LanguageVersion.CSharp7_2 => ConvertFixedSourceRegex().Replace(fixedSource, m => m.Value switch
            {
                "$$TypeDefinition$$" => "readonly struct",
                _ => throw new InvalidOperationException(),
            }),
            LanguageVersion.CSharp10 => ConvertFixedSourceRegex().Replace(fixedSource, m => m.Value switch
            {
                "$$TypeDefinition$$" => "readonly record struct",
                _ => throw new InvalidOperationException(),
            }),
            _ => throw new InvalidOperationException(),
        };
    }

    #region StaticModInt
    [Fact]
    public async Task StaticModInt_Using()
    {
        var source = """
using AtCoder;
class Program
{
    StaticModInt<Op> notDefined;
    StaticModInt<Mod1000000007> defined;
}
""";
        var fixedSource = """
using AtCoder;
class Program
{
    StaticModInt<Op> notDefined;
    StaticModInt<Mod1000000007> defined;
}

$$TypeDefinition$$ Op : IStaticMod
{
    public uint Mod => default;

    public bool IsPrime => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(4, 5, 4, 21).WithArguments("Op"),
            fixedSource);
    }

    [Fact]
    public async Task StaticModInt_Qualified_Using()
    {
        var source = """
using AtCoder;
class Program
{
    AtCoder.StaticModInt<Op> notDefined;
    AtCoder.StaticModInt<Mod1000000007> defined;
}
""";
        var fixedSource = """
using AtCoder;
class Program
{
    AtCoder.StaticModInt<Op> notDefined;
    AtCoder.StaticModInt<Mod1000000007> defined;
}

$$TypeDefinition$$ Op : IStaticMod
{
    public uint Mod => default;

    public bool IsPrime => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(4, 13, 4, 29).WithArguments("Op"),
            fixedSource);
    }

    [Fact]
    public async Task StaticModInt_Qualified()
    {
        var source = """
class Program
{
    AtCoder.StaticModInt<Op> notDefined;
    AtCoder.StaticModInt<AtCoder.Mod1000000007> defined;
}
""";
        var fixedSource = """
class Program
{
    AtCoder.StaticModInt<Op> notDefined;
    AtCoder.StaticModInt<AtCoder.Mod1000000007> defined;
}

$$TypeDefinition$$ Op : AtCoder.IStaticMod
{
    public uint Mod => default;

    public bool IsPrime => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(3, 13, 3, 29).WithArguments("Op"),
            fixedSource);
    }
    #endregion StaticModInt

    #region Segtree
    [Fact]
    public async Task Segtree_Using()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

$$TypeDefinition$$ OpSeg : ISegtreeOperator<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => default;

    public int Identity => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(6, 5, 6, 24).WithArguments("OpSeg"),
            fixedSource);
    }

    [Fact]
    public async Task Segtree_Qualified_Using()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    AtCoder.Segtree<int, MinOp> defined;
    AtCoder.Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    AtCoder.Segtree<int, MinOp> defined;
    AtCoder.Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

$$TypeDefinition$$ OpSeg : ISegtreeOperator<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => default;

    public int Identity => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(6, 13, 6, 32).WithArguments("OpSeg"),
            fixedSource);
    }

    [Fact]
    public async Task Segtree_Qualified()
    {
        var source = """
class Program
{
    AtCoder.Segtree<int, MinOp> defined;
    AtCoder.Segtree<int, OpSeg> notDefined;
}
struct MinOp : AtCoder.ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
""";
        var fixedSource = """
class Program
{
    AtCoder.Segtree<int, MinOp> defined;
    AtCoder.Segtree<int, OpSeg> notDefined;
}
struct MinOp : AtCoder.ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

$$TypeDefinition$$ OpSeg : AtCoder.ISegtreeOperator<int>
{
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => default;

    public int Identity => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(4, 13, 4, 32).WithArguments("OpSeg"),
            fixedSource);
    }

    [Fact]
    public async Task Segtree_Using_With_System_Runtime_CompilerServices()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

$$TypeDefinition$$ OpSeg : ISegtreeOperator<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => default;

    public int Identity => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(6, 5, 6, 24).WithArguments("OpSeg"),
            fixedSource);
    }
    #endregion Segtree

    #region LazySegtree
    [Fact]
    public async Task LazySegtree_Using()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    LazySegtree<long, int, Op> defined;
    LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    LazySegtree<long, int, Op> defined;
    LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}

$$TypeDefinition$$ OpSeg : ILazySegtreeOperator<(int v, int size), (int b, int c)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) nf, (int b, int c) cf) => default;

    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(6, 5, 6, 58).WithArguments("OpSeg"),
            fixedSource);
    }

    [Fact]
    public async Task LazySegtree_Qualified_Using()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    AtCoder.LazySegtree<long, int, Op> defined;
    AtCoder.LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : AtCoder.ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    AtCoder.LazySegtree<long, int, Op> defined;
    AtCoder.LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : AtCoder.ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}

$$TypeDefinition$$ OpSeg : ILazySegtreeOperator<(int v, int size), (int b, int c)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) nf, (int b, int c) cf) => default;

    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(6, 13, 6, 66).WithArguments("OpSeg"),
            fixedSource);
    }

    [Fact]
    public async Task LazySegtree_Qualified()
    {
        var source = """
using System.Runtime.CompilerServices;

class Program
{
    AtCoder.LazySegtree<long, int, Op> defined;
    AtCoder.LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : AtCoder.ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}
""";
        var fixedSource = """
using System.Runtime.CompilerServices;

class Program
{
    AtCoder.LazySegtree<long, int, Op> defined;
    AtCoder.LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : AtCoder.ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}

$$TypeDefinition$$ OpSeg : AtCoder.ILazySegtreeOperator<(int v, int size), (int b, int c)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) nf, (int b, int c) cf) => default;

    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(6, 13, 6, 66).WithArguments("OpSeg"),
            fixedSource);
    }

    [Fact]
    public async Task LazySegtree_Using_With_System_Runtime_CompilerServices()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    LazySegtree<long, int, Op> defined;
    LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    LazySegtree<long, int, Op> defined;
    LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}

$$TypeDefinition$$ OpSeg : ILazySegtreeOperator<(int v, int size), (int b, int c)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) nf, (int b, int c) cf) => default;

    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(6, 5, 6, 58).WithArguments("OpSeg"),
            fixedSource);
    }
    #endregion LazySegtree

    #region Others
    [Fact]
    public async Task ICompare()
    {
        var source = """
using System;
using AtCoder;
public class Generic<T, TOp> where TOp : System.Collections.Generic.IComparer<T> { }
class Program
{
    Generic<short, Op> notDefined;
    Type Type = typeof(Generic<,>);
}
""";
        var fixedSource = """
using System;
using AtCoder;
public class Generic<T, TOp> where TOp : System.Collections.Generic.IComparer<T> { }
class Program
{
    Generic<short, Op> notDefined;
    Type Type = typeof(Generic<,>);
}

$$TypeDefinition$$ Op : System.Collections.Generic.IComparer<short>
{
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Compare(short x, short y) => x.CompareTo(y);
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(6, 5, 6, 23).WithArguments("Op"),
            fixedSource);
    }

    [Fact]
    public async Task AnyDefinedType()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    void Init(ref T val, out bool success, params int[] nums);
    T Prop1 { set; get; }
    T Prop2 { get; set; }
}
public class Generic<T, TOp> where TOp : IAny<T> { }
class Program
{
    Generic<(int, long), Op> notDefined;
    Generic<(int, long), Def<(int, long)>> defined;
    System.Type Type = typeof(Generic<,>);
}
struct Def<T> : IAny<T> {
    public void Init(ref T val, out bool success, params int[] nums) { success = true; }
    public T Prop1 { set; get; }
    public T Prop2 { set; get; }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    void Init(ref T val, out bool success, params int[] nums);
    T Prop1 { set; get; }
    T Prop2 { get; set; }
}
public class Generic<T, TOp> where TOp : IAny<T> { }
class Program
{
    Generic<(int, long), Op> notDefined;
    Generic<(int, long), Def<(int, long)>> defined;
    System.Type Type = typeof(Generic<,>);
}
struct Def<T> : IAny<T> {
    public void Init(ref T val, out bool success, params int[] nums) { success = true; }
    public T Prop1 { set; get; }
    public T Prop2 { set; get; }
}

$$TypeDefinition$$ Op : IAny<(int, long)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Init(ref (int, long) val, out bool success, params int[] nums)
    {
    }

    public (int, long) Prop1 { set; get; }
    public (int, long) Prop2 { set; get; }
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(12, 5, 12, 29).WithArguments("Op"),
           fixedSource);
    }

    [Fact]
    public async Task AnyDefinedMethod()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    void Init();
    T Prop1 { set; get; }
    T Prop2 { get; set; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<(int n, long m), Op>();
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    void Init();
    T Prop1 { set; get; }
    T Prop2 { get; set; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<(int n, long m), Op>();
    }
}

$$TypeDefinition$$ Op : IAny<(int n, long m)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Init()
    {
    }

    public (int n, long m) Prop1 { set; get; }
    public (int n, long m) Prop2 { set; get; }
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(14, 9, 14, 31).WithArguments("Op"),
           fixedSource);
    }

    [Fact]
    public async Task Array()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;

[IsOperator]
public interface IAny<T> {
    T Prop { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<System.Numerics.BigInteger[], BigOp>();
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;

[IsOperator]
public interface IAny<T> {
    T Prop { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<System.Numerics.BigInteger[], BigOp>();
    }
}

$$TypeDefinition$$ BigOp : IAny<System.Numerics.BigInteger[]>
{
    public System.Numerics.BigInteger[] Prop => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(13, 9, 13, 47).WithArguments("BigOp"),
            fixedSource);
    }

    [Fact]
    public async Task Generic()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;

[IsOperator]
public interface IAny<T> {
    T Prop { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<StaticModInt<Mod1000000007>, ModOp>();
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;

[IsOperator]
public interface IAny<T> {
    T Prop { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<StaticModInt<Mod1000000007>, ModOp>();
    }
}

$$TypeDefinition$$ ModOp : IAny<StaticModInt<Mod1000000007>>
{
    public StaticModInt<Mod1000000007> Prop => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(13, 9, 13, 46).WithArguments("ModOp"),
            fixedSource);
    }

    [Fact]
    public async Task NumOperatorAndShiftOperator()
    {
        var source = """
using AtCoder.Operators;
using System.Runtime.CompilerServices;
class Program
{
    void M<T>() where T : INumOperator<int>, IShiftOperator<int>, INumOperator<float>, ICastOperator<short, char> { }
    public void Main()
    {
        M<Op>();
    }
}
""";
        var fixedSource = """
using AtCoder.Operators;
using System.Runtime.CompilerServices;
class Program
{
    void M<T>() where T : INumOperator<int>, IShiftOperator<int>, INumOperator<float>, ICastOperator<short, char> { }
    public void Main()
    {
        M<Op>();
    }
}

$$TypeDefinition$$ Op : ICastOperator<short, char>, INumOperator<float>, INumOperator<int>, IShiftOperator<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public char Cast(short y) => (char)y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Add(float x, float y) => x + y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Subtract(float x, float y) => x - y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Divide(float x, float y) => x / y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Modulo(float x, float y) => x % y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Multiply(float x, float y) => x * y;

    public float MultiplyIdentity => 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Minus(float x) => -x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Increment(float x) => ++x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Decrement(float x) => --x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThan(float x, float y) => x > y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThanOrEqual(float x, float y) => x >= y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(float x, float y) => x < y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThanOrEqual(float x, float y) => x <= y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(float x, float y) => x.CompareTo(y);

    public float MinValue => float.MinValue;

    public float MaxValue => float.MaxValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Add(int x, int y) => x + y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Subtract(int x, int y) => x - y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Divide(int x, int y) => x / y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Modulo(int x, int y) => x % y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Multiply(int x, int y) => x * y;

    public int MultiplyIdentity => 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Minus(int x) => -x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Increment(int x) => ++x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Decrement(int x) => --x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThan(int x, int y) => x > y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThanOrEqual(int x, int y) => x >= y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(int x, int y) => x < y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThanOrEqual(int x, int y) => x <= y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(int x, int y) => x.CompareTo(y);

    public int MinValue => int.MinValue;

    public int MaxValue => int.MaxValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LeftShift(int x, int y) => x << y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int RightShift(int x, int y) => x >> y;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(8, 9, 8, 14).WithArguments("Op"),
            fixedSource);
    }

    [Fact]
    public async Task UsingAlias()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
using ModInt = StaticModInt<Mod1000000007>;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<ModInt, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
using ModInt = StaticModInt<Mod1000000007>;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<ModInt, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

$$TypeDefinition$$ OpSeg : ISegtreeOperator<ModInt>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ModInt Operate(ModInt x, ModInt y) => default;

    public ModInt Identity => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(7, 5, 7, 27).WithArguments("OpSeg"),
            fixedSource);
    }

    [Fact]
    public async Task MethodImplAlias()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<long, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<long, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

$$TypeDefinition$$ OpSeg : ISegtreeOperator<long>
{
    [MI(MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => default;

    public long Identity => default;
}
""";

        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(7, 5, 7, 25).WithArguments("OpSeg"),
            fixedSource);
    }

    [Fact]
    public async Task MethodImpl256()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<long, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<long, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

$$TypeDefinition$$ OpSeg : ISegtreeOperator<long>
{
    [MethodImpl(256)]
    public long Operate(long x, long y) => default;

    public long Identity => default;
}
""";

        static void updateTest(Test t) => t.TestState.AnalyzerConfigFiles.Add(("/.editorconfig", """
is_global = true
build_property.CompetitiveAnalyzer_UseMethodImplNumeric = true
"""));
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(6, 5, 6, 25).WithArguments("OpSeg"),
            fixedSource,
            updateTest: updateTest);
    }


    [Fact]
    public async Task Virtual()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny {
    void Run() { }
}
class Program
{
    static void M<TOp>() where TOp : IAny {}
    static void Run()
    {
        M<Op>();
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny {
    void Run() { }
}
class Program
{
    static void M<TOp>() where TOp : IAny {}
    static void Run()
    {
        M<Op>();
    }
}

$$TypeDefinition$$ Op : IAny
{
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(12, 9, 12, 14).WithArguments("Op"),
           fixedSource);
    }

    [Fact]
    public async Task StaticAbstract()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    static abstract bool Init();
    static abstract T Prop1 { set; get; }
    static abstract (T, T) Prop2 { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : IAny<T> {}
    static void Run()
    {
        M<(int n, long m), Op>();
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    static abstract bool Init();
    static abstract T Prop1 { set; get; }
    static abstract (T, T) Prop2 { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : IAny<T> {}
    static void Run()
    {
        M<(int n, long m), Op>();
    }
}

$$TypeDefinition$$ Op : IAny<(int n, long m)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Init() => default;

    public static (int n, long m) Prop1 { set; get; }

    public static ((int n, long m), (int n, long m)) Prop2 => default;
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(14, 9, 14, 31).WithArguments("Op"),
           fixedSource);
    }

    [Fact]
    public async Task StaticVirtual()
    {
        var source = """
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    static virtual bool Init();
    static virtual T Prop1 { set; get; }
    static virtual (T, T) Prop2 { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : IAny<T> {}
    static void Run()
    {
        M<(int n, long m), Op>();
    }
}
""";
        var fixedSource = """
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    static virtual bool Init();
    static virtual T Prop1 { set; get; }
    static virtual (T, T) Prop2 { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : IAny<T> {}
    static void Run()
    {
        M<(int n, long m), Op>();
    }
}

$$TypeDefinition$$ Op : IAny<(int n, long m)>
{
}
""";
        await VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0004).WithSpan(14, 9, 14, 31).WithArguments("Op"),
           fixedSource);
    }
    #endregion Others
}
