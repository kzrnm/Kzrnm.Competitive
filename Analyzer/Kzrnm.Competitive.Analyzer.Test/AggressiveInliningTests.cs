using Kzrnm.Competitive.Analyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using VerifyCS = Kzrnm.Competitive.Analyzer.Test.CSharpCodeFixVerifier<
    Kzrnm.Competitive.Analyzer.AggressiveInlining.Analyzer,
    Kzrnm.Competitive.Analyzer.AggressiveInlining.CodeFixProvider>;

namespace Kzrnm.Competitive.Analyzer.Test;

public class AggressiveInliningTests
{
    static DiagnosticDescriptor KZCOMPETITIVE0003 => DiagnosticDescriptors.KZCOMPETITIVE0003_AgressiveInlining_Descriptor;
    [Fact]
    public async Task Empty()
    {
        var source = @"
using System.Collections.Generic;
struct IntComparer : IComparer<int>
{
    public int Compare(int x,  int y) => x.CompareTo(y);
}
";
        await VerifyCS.VerifyAnalyzerAsync(source, [], TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task NumOperator()
    {
        var source = @"
using AtCoder.Operators;
using System;
using System.Runtime.CompilerServices;
struct BoolOp : INumOperator<bool>
{
    public bool MinValue => true;
    public bool MaxValue => false;
    public bool Add(bool x, bool y) => x || y;
    public int Compare(bool x, bool y) => x.CompareTo(y);
    public bool Decrement(bool x) => false;
    public bool Divide(bool x, bool y) => throw new NotImplementedException();
    public bool Equals(bool x, bool y) => x == y;
    public int GetHashCode(bool obj) => obj.GetHashCode();
    public bool GreaterThan(bool x, bool y) => x && !y;
    public bool GreaterThanOrEqual(bool x, bool y) => x || !y;
    public bool Increment(bool x) => true;
    public bool LessThan(bool x, bool y) => y && !x;
    public bool LessThanOrEqual(bool x, bool y) => y || !x;
    public bool Minus(bool x) => false;
    public bool Modulo(bool x, bool y) => true ? true : throw new NotImplementedException();
    public bool Multiply(bool x, bool y)
    {
        throw new NotImplementedException();
    }
    public bool Subtract(bool x, bool y)
    {
        return default(bool?) ?? throw new NotImplementedException();
    }
}
";

        var fixedSource = @"
using AtCoder.Operators;
using System;
using System.Runtime.CompilerServices;
struct BoolOp : INumOperator<bool>
{
    public bool MinValue => true;
    public bool MaxValue => false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Add(bool x, bool y) => x || y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(bool x, bool y) => x.CompareTo(y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Decrement(bool x) => false;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Divide(bool x, bool y) => throw new NotImplementedException();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(bool x, bool y) => x == y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(bool obj) => obj.GetHashCode();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThan(bool x, bool y) => x && !y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThanOrEqual(bool x, bool y) => x || !y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Increment(bool x) => true;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(bool x, bool y) => y && !x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThanOrEqual(bool x, bool y) => y || !x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Minus(bool x) => false;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Modulo(bool x, bool y) => true ? true : throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Multiply(bool x, bool y)
    {
        throw new NotImplementedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Subtract(bool x, bool y)
    {
        return default(bool?) ?? throw new NotImplementedException();
    }
}
";
        await VerifyCS.VerifyCodeFixAsync(source, [
            VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(5, 1, 30, 2).WithArguments(
                "Add, Compare, Decrement, Divide, Equals, GetHashCode, GreaterThan, GreaterThanOrEqual, Increment, LessThan, LessThanOrEqual, Minus, Modulo, Multiply, Subtract"),
        ], fixedSource, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task SegtreeOperator()
    {
        var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct OpSeg : ISegtreeOperator<int>
{
    public int Identity => default;
    public int Operate(int x, int y) => System.Math.Max(x, y);
}
";
        var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct OpSeg : ISegtreeOperator<int>
{
    public int Identity => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => System.Math.Max(x, y);
}
";
        await VerifyCS.VerifyCodeFixAsync(source,
        [
            VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(4, 1, 8, 2).WithArguments("Operate"),
        ], fixedSource, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task SegtreeOperator_With_AggressiveInlining()
    {
        var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct OpSeg : ISegtreeOperator<int>
{
    public int Identity => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => System.Math.Max(x, y);
}
";
        await VerifyCS.VerifyAnalyzerAsync(source, [], TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task LazySegtreeOperator()
    {
        var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    public int Composition(int f, int g) => 0;
    public long Mapping(int f, long x) => 0L;
    public long Operate(long x, long y) => 0L;
}
";
        var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
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
";
        await VerifyCS.VerifyCodeFixAsync(source,
        [
            VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(4, 1, 11, 2).WithArguments("Composition, Mapping, Operate"),
        ], fixedSource, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task LazySegtreeOperator_Without_Using()
    {
        var source = @"
using AtCoder;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    public int Composition(int f, int g) => 0;
    public long Mapping(int f, long x) => 0L;
    public long Operate(long x, long y) => 0L;
}
";
        var fixedSource = @"
using AtCoder;
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
";
        await VerifyCS.VerifyCodeFixAsync(source,
        [
VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(3, 1, 10, 2).WithArguments("Composition, Mapping, Operate"),
        ], fixedSource, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task LazySegtreeOperator_With_AggressiveInlining()
    {
        var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
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
";
        await VerifyCS.VerifyAnalyzerAsync(source, [], TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task LazySegtreeOperator_With_Qualified_AggressiveInlining()
    {
        var source = @"
using AtCoder;
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
";
        await VerifyCS.VerifyAnalyzerAsync(source, [], TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task LazySegtreeOperator_Without_AggressiveInlining()
    {
        var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public int Composition(int f, int g) => 0;
    [MethodImpl(0b1010000)]
    public long Mapping(int f, long x) => 0L;
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.NoOptimization)]
    public long Operate(long x, long y) => 0L;
}
";
        var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;

    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [MethodImpl(0b1010000 | 256)]
    public long Mapping(int f, long x) => 0L;
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.NoOptimization)]
    public long Operate(long x, long y) => 0L;
}
";
        await VerifyCS.VerifyCodeFixAsync(source,
        [
VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(4, 1, 14, 2).WithArguments("Composition, Mapping"),
        ], fixedSource, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task LazySegtreeOperator_Without_AggressiveInlining_Equal_Colon()
    {
        var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [MethodImpl(methodImplOptions: MethodImplOptions.NoOptimization)]
    public int Composition(int f, int g) => 0;
    [MethodImpl(value: 0b1010000)]
    public long Mapping(int f, long x) => 0L;
    [MethodImpl(MethodImplOptions.NoOptimization, MethodCodeType = MethodCodeType.IL)]
    public long Operate(long x, long y) => 0L;
}
";
        var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;

    [MethodImpl(methodImplOptions: MethodImplOptions.NoOptimization | MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [MethodImpl(value: 0b1010000 | 256)]
    public long Mapping(int f, long x) => 0L;
    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.AggressiveInlining, MethodCodeType = MethodCodeType.IL)]
    public long Operate(long x, long y) => 0L;
}
";
        await VerifyCS.VerifyCodeFixAsync(source,
        [
VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(4, 1, 14, 2).WithArguments("Composition, Mapping, Operate"),
        ], fixedSource, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task LazySegtreeOperator_With_ArgumentList()
    {
        var source = @"
using System;
using AtCoder;
using System.Runtime.CompilerServices;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [MethodImpl]
    public int Composition(int f, int g) => 0;
    [My, MethodImpl, My]
    public long Mapping(int f, long x) => 0L;
    [My]
    [My, MethodImpl(256|4)]
    public long Operate(long x, long y) => 0L;
}
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
sealed class MyAttribute : Attribute{}
";
        var fixedSource = @"
using System;
using AtCoder;
using System.Runtime.CompilerServices;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [My, MethodImpl(MethodImplOptions.AggressiveInlining), My]
    public long Mapping(int f, long x) => 0L;
    [My]
    [My, MethodImpl(256|4)]
    public long Operate(long x, long y) => 0L;
}
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
sealed class MyAttribute : Attribute{}
";
        await VerifyCS.VerifyCodeFixAsync(source,
        [
VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(5, 1, 16, 2).WithArguments("Composition, Mapping"),
        ], fixedSource, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task LazySegtreeOperator_With_Alias_AggressiveInlining()
    {
        var source = @"
using AtCoder;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [MI(256)]
    public int Composition(int f, int g) => 0;
    [MI(256)]
    public long Mapping(int f, long x) => 0L;
    [MI(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}
";
        await VerifyCS.VerifyAnalyzerAsync(source, [], TestContext.Current.CancellationToken);
    }


    [Fact]
    public async Task AnyDefinedType()
    {
        var source = @"
using AtCoder;
[IsOperator]
public interface IAny<T> {
    T Fun1();
    string Fun2(T v);
}
struct Def<T> : IAny<T> {
    public T Fun1() => default;
    public string Fun2(T v)
    {
        return v.ToString();
    }
}
";
        var fixedSource = @"
using AtCoder;
[IsOperator]
public interface IAny<T> {
    T Fun1();
    string Fun2(T v);
}
struct Def<T> : IAny<T> {
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public T Fun1() => default;

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public string Fun2(T v)
    {
        return v.ToString();
    }
}
";
        await VerifyCS.VerifyCodeFixAsync(
            source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(8, 1, 14, 2).WithArguments("Fun1, Fun2"),
            fixedSource,
            TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task UsingAlias()
    {
        var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    public int Composition(int f, int g) => 0;
    public long Mapping(int f, long x) => 0L;
    public long Operate(long x, long y) => 0L;
}
";
        var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;

    [MI(MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [MI(MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [MI(MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}
";
        await VerifyCS.VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(5, 1, 12, 2).WithArguments("Composition, Mapping, Operate"),
            fixedSource,
            TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task MethodImpl256()
    {
        var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    public int Composition(int f, int g) => 0;
    public long Mapping(int f, long x) => 0L;
    public long Operate(long x, long y) => 0L;
}
";
        var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;

    [MI(256)]
    public int Composition(int f, int g) => 0;
    [MI(256)]
    public long Mapping(int f, long x) => 0L;
    [MI(256)]
    public long Operate(long x, long y) => 0L;
}
";
        var test = new VerifyCS.Test
        {
            TestCode = source,
            FixedCode = fixedSource,
            TestState =
            {
                AnalyzerConfigFiles =
                {
                    ("/.editorconfig", @"
is_global = true
build_property.CompetitiveAnalyzer_UseMethodImplNumeric = true
"),
                },
            },
        };
        test.ExpectedDiagnostics.Add(VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(5, 1, 12, 2).WithArguments("Composition, Mapping, Operate"));
        await test.RunAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Class()
    {
        var source = @"
using AtCoder;
[IsOperator]
public interface IAny<T> {
    T Fun1();
    string Fun2(T v);
}
class Def<T> : IAny<T> {
    public T Fun1() => default;
    public string Fun2(T v)
    {
        return v.ToString();
    }
}
";
        var fixedSource = @"
using AtCoder;
[IsOperator]
public interface IAny<T> {
    T Fun1();
    string Fun2(T v);
}
class Def<T> : IAny<T> {
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public T Fun1() => default;

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public string Fun2(T v)
    {
        return v.ToString();
    }
}
";
        await VerifyCS.VerifyCodeFixAsync(
            source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(8, 1, 14, 2).WithArguments("Fun1, Fun2"),
            fixedSource,
            TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task RecordClass()
    {
        var source = @"
using AtCoder;
[IsOperator]
public interface IAny<T> {
    T Fun1();
    string Fun2(T v);
}
record Def<T> : IAny<T> {
    public T Fun1() => default;
    public string Fun2(T v)
    {
        return v.ToString();
    }
}
";
        var fixedSource = @"
using AtCoder;
[IsOperator]
public interface IAny<T> {
    T Fun1();
    string Fun2(T v);
}
record Def<T> : IAny<T> {
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public T Fun1() => default;

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public string Fun2(T v)
    {
        return v.ToString();
    }
}
";
        await VerifyCS.VerifyCodeFixAsync(source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(8, 1, 14, 2).WithArguments("Fun1, Fun2"),
            fixedSource,
            TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task RecordStruct()
    {
        var source = @"
using AtCoder;
[IsOperator]
public interface IAny<T> {
    T Fun1();
    string Fun2(T v);
}
record struct Def<T> : IAny<T> {
    public T Fun1() => default;
    public string Fun2(T v)
    {
        return v.ToString();
    }
}
";
        var fixedSource = @"
using AtCoder;
[IsOperator]
public interface IAny<T> {
    T Fun1();
    string Fun2(T v);
}
record struct Def<T> : IAny<T> {
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public T Fun1() => default;

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public string Fun2(T v)
    {
        return v.ToString();
    }
}
";
        await VerifyCS.VerifyCodeFixAsync(
            source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(8, 1, 14, 2).WithArguments("Fun1, Fun2"),
            fixedSource,
            TestContext.Current.CancellationToken);
    }


    [Fact]
    public async Task StaticAbstract()
    {
        var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IInterface
{
    static abstract int Merge(int a, int b);
}

public class Impl : IInterface
{
    public static int Merge(int a, int b) => a + b;
}
";
        var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IInterface
{
    static abstract int Merge(int a, int b);
}

public class Impl : IInterface
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Merge(int a, int b) => a + b;
}
";
        await VerifyCS.VerifyCodeFixAsync(
            source,
            VerifyCS.Diagnostic(KZCOMPETITIVE0003).WithSpan(10, 1, 13, 2).WithArguments("Merge"),
            fixedSource,
            TestContext.Current.CancellationToken);
    }
}
