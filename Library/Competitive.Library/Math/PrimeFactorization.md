---
title: 高速素因数分解
documentation_of: ./PrimeFactorization.cs
---

## 概要

https://nyaannyaan.github.io/library/prime/fast-factorize.hpp.html

$\mathrm{O}(N^{\frac{1}{4}})$ で素因数分解を行える。

32 bit に収まる程度ならナイーブな実装でやったほうが早い。

$N = 10^{18}$ 程度の数値でも素因数分解できるのが強み。

``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 10 (10.0.19045.2486)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.101
  [Host]   : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|             Method |     Toolchain |           Mean |          Error |        StdDev |
|------------------- |-------------- |---------------:|---------------:|--------------:|
|  NaiveFactorizeInt |      .NET 7.0 |       806.1 ns |       393.3 ns |      21.56 ns |
|  NaiveFactorizeInt | .NET Core 3.1 |       768.4 ns |       103.2 ns |       5.66 ns |
|                    |               |                |                |               |
|   FastFactorizeInt |      .NET 7.0 |     5,872.5 ns |     1,056.7 ns |      57.92 ns |
|   FastFactorizeInt | .NET Core 3.1 |    22,376.9 ns |    11,421.7 ns |     626.06 ns |
|                    |               |                |                |               |
| NaiveFactorizeLong |      .NET 7.0 | 1,598,861.1 ns |   533,586.6 ns |  29,247.68 ns |
| NaiveFactorizeLong | .NET Core 3.1 | 1,685,799.4 ns |   930,786.0 ns |  51,019.52 ns |
|                    |               |                |                |               |
|  FastFactorizeLong |      .NET 7.0 |    10,970.5 ns |     4,661.3 ns |     255.50 ns |
|  FastFactorizeLong | .NET Core 3.1 |    32,498.9 ns |    16,347.4 ns |     896.06 ns |
|                    |               |                |                |               |
|    NaiveDivisorInt |      .NET 7.0 |    97,206.1 ns |    47,630.8 ns |   2,610.81 ns |
|    NaiveDivisorInt | .NET Core 3.1 |   102,888.0 ns |    34,547.9 ns |   1,893.69 ns |
|                    |               |                |                |               |
|     FastDivisorInt |      .NET 7.0 |   110,266.3 ns |    28,517.7 ns |   1,563.15 ns |
|     FastDivisorInt | .NET Core 3.1 |   133,520.2 ns |   132,961.5 ns |   7,288.07 ns |
|                    |               |                |                |               |
|   NaiveDivisorLong |      .NET 7.0 | 2,316,846.7 ns | 2,394,909.3 ns | 131,273.06 ns |
|   NaiveDivisorLong | .NET Core 3.1 | 2,180,188.7 ns | 1,593,489.7 ns |  87,344.55 ns |
|                    |               |                |                |               |
|    FastDivisorLong |      .NET 7.0 |   504,378.5 ns |   187,616.6 ns |  10,283.90 ns |
|    FastDivisorLong | .NET Core 3.1 |   638,197.4 ns |   108,286.1 ns |   5,935.53 ns |


<details>
<summary>ベンチマーク</summary>

```csharp
using AtCoder;
using AtCoder.Extension;
using AtCoder.Internal;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using Kzrnm.Competitive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;



public class BenchmarkConfig : ManualConfig
{
    static void Main(string[] args)
    {
#if DEBUG
        BenchmarkSwitcher.FromAssembly(typeof(BenchmarkConfig).Assembly).Run(args, new DebugInProcessConfig());
#else
        _ = BenchmarkRunner.Run(typeof(Benchmark).Assembly);
#endif
    }
    public BenchmarkConfig()
    {
        //AddDiagnoser(MemoryDiagnoser.Default);
        AddExporter(BenchmarkDotNet.Exporters.MarkdownExporter.GitHub);
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp31));
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp70));
    }
}

[Config(typeof(BenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
public class Benchmark
{

    [Benchmark]
    [BenchmarkCategory("Factorize")]
    public long NaiveFactorizeInt()
    {
        long c = 0;
        c += MathLibEx.PrimeFactoring(99991).Count;
        c += MathLibEx.PrimeFactoring(735134400).Count;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("Factorize")]
    public long FastFactorizeInt()
    {
        long c = 0;
        c += PrimeFactorization.PrimeFactoring(99991).Count;
        c += PrimeFactorization.PrimeFactoring(735134400).Count;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("Factorize")]
    public long NaiveFactorizeLong()
    {
        long c = 0;
        c += MathLibEx.PrimeFactoring(132147483703).Count;
        c += MathLibEx.PrimeFactoring(963761198400).Count;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("Factorize")]
    public long FastFactorizeLong()
    {
        long c = 0;
        c += PrimeFactorization.PrimeFactoring(132147483703).Count;
        c += PrimeFactorization.PrimeFactoring(963761198400).Count;
        return c;
    }


    [Benchmark]
    [BenchmarkCategory("Divisor")]
    public long NaiveDivisorInt()
    {
        long c = 0;
        foreach (var x in MathLibEx.Divisor(99991)) c += x;
        foreach (var x in MathLibEx.Divisor(735134400)) c += x;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("Divisor")]
    public long FastDivisorInt()
    {
        long c = 0;
        foreach (var x in PrimeFactorization.Divisor(99991)) c += x;
        foreach (var x in PrimeFactorization.Divisor(735134400)) c += x;
        return c;
    }
    [Benchmark]
    [BenchmarkCategory("Divisor")]
    public long NaiveDivisorLong()
    {
        long c = 0;
        foreach (var x in MathLibEx.Divisor(132147483703)) c += x;
        foreach (var x in MathLibEx.Divisor(963761198400)) c += x;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("Divisor")]
    public long FastDivisorLong()
    {
        long c = 0;
        foreach (var x in PrimeFactorization.Divisor(132147483703)) c += x;
        foreach (var x in PrimeFactorization.Divisor(963761198400)) c += x;
        return c;
    }
}
```

</details>