---
title: 高速素因数分解
documentation_of: ./PrimeFactorization.cs
---

## 概要

https://nyaannyaan.github.io/library/prime/fast-factorize.hpp.html

$\mathrm{O}(N^{\frac{1}{4}})$ で素因数分解を行える。

N が小さいならナイーブな実装でやったほうが早い。

$N = 10^{18}$ 程度の数値でも素因数分解できるのが強み。

<details>
<summary>ベンチマーク</summary>

<div class="code"><pre class="hljs" id="code-body-0"><code class="language-cs">{%- raw -%}using AtCoder;
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
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByJob, BenchmarkLogicalGroupRule.ByCategory)]
public class Benchmark
{
    long[] array;
    const long mask = (1L << 40) - 1;
    const int smallMask = (1 << 30) - 1;
    public Benchmark()
    {
        var rnd = new Random(227);
        array = new long[50];
        rnd.NextBytes(MemoryMarshal.AsBytes(array.AsSpan()));
        for (int i = 0; i < array.Length; i++)
        {
            array[i] &= mask;
        }
    }

    [Benchmark]
    [BenchmarkCategory("Factorize", "Int")]
    public long NaiveFactorizeInt()
    {
        long c = 0;
        foreach (var v in array)
            c += MathLibEx.PrimeFactoring(v & smallMask).Count;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("Factorize", "Int")]
    public long FastFactorizeInt()
    {
        long c = 0;
        foreach (var v in array)
            c += PrimeFactorization.PrimeFactoring(v & smallMask).Count;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("Factorize", "Long")]
    public long NaiveFactorizeLong()
    {
        long c = 0;
        foreach (var v in array)
            c += MathLibEx.PrimeFactoring(v).Count;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("Factorize", "Long")]
    public long FastFactorizeLong()
    {
        long c = 0;
        foreach (var v in array)
            c += PrimeFactorization.PrimeFactoring(v).Count;
        return c;
    }


    [Benchmark]
    [BenchmarkCategory("Divisor", "Int")]
    public long NaiveDivisorInt()
    {
        long c = 0;
        foreach (var v in array)
            foreach (var x in MathLibEx.Divisor(v & smallMask)) c += x;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("Divisor", "Int")]
    public long FastDivisorInt()
    {
        long c = 0;
        foreach (var v in array)
            foreach (var x in PrimeFactorization.Divisor(v & smallMask)) c += x;
        return c;
    }
    [Benchmark]
    [BenchmarkCategory("Divisor", "Long")]
    public long NaiveDivisorLong()
    {
        long c = 0;
        foreach (var v in array)
            foreach (var x in MathLibEx.Divisor(v)) c += x;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("Divisor", "Long")]
    public long FastDivisorLong()
    {
        long c = 0;
        foreach (var v in array)
            foreach (var x in PrimeFactorization.Divisor(v)) c += x;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("ConstFactorize")]
    public long NaiveFactorize_99991_735134400()
    {
        long c = 0;
        c += MathLibEx.PrimeFactoring(99991).Count;
        c += MathLibEx.PrimeFactoring(735134400).Count;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("ConstFactorize")]
    public long FastFactorize_99991_735134400()
    {
        long c = 0;
        c += PrimeFactorization.PrimeFactoring(99991).Count;
        c += PrimeFactorization.PrimeFactoring(735134400).Count;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("ConstFactorize")]
    public long NaiveFactorize_132147483703_963761198400()
    {
        long c = 0;
        c += MathLibEx.PrimeFactoring(132147483703).Count;
        c += MathLibEx.PrimeFactoring(963761198400).Count;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("ConstFactorize")]
    public long FastFactorize_132147483703_963761198400()
    {
        long c = 0;
        c += PrimeFactorization.PrimeFactoring(132147483703).Count;
        c += PrimeFactorization.PrimeFactoring(963761198400).Count;
        return c;
    }


    [Benchmark]
    [BenchmarkCategory("ConstDivisor")]
    public long NaiveDivisor_99991_735134400()
    {
        long c = 0;
        foreach (var x in MathLibEx.Divisor(99991)) c += x;
        foreach (var x in MathLibEx.Divisor(735134400)) c += x;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("ConstDivisor")]
    public long FastDivisor_99991_735134400()
    {
        long c = 0;
        foreach (var x in PrimeFactorization.Divisor(99991)) c += x;
        foreach (var x in PrimeFactorization.Divisor(735134400)) c += x;
        return c;
    }
    [Benchmark]
    [BenchmarkCategory("ConstDivisor")]
    public long NaiveDivisor_132147483703_963761198400()
    {
        long c = 0;
        foreach (var x in MathLibEx.Divisor(132147483703)) c += x;
        foreach (var x in MathLibEx.Divisor(963761198400)) c += x;
        return c;
    }

    [Benchmark]
    [BenchmarkCategory("ConstDivisor")]
    public long FastDivisor_132147483703_963761198400()
    {
        long c = 0;
        foreach (var x in PrimeFactorization.Divisor(132147483703)) c += x;
        foreach (var x in PrimeFactorization.Divisor(963761198400)) c += x;
        return c;
    }
}
{% endraw %}
    </code></pre>
    <div class="btn-area">
        <div class="btn-group"><button type="button" class="code-btn code-copy-btn hint--top hint--always hint--disable" aria-label="Copied!">Copy</button></div>
    </div>
</div>
</details>


``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 10 (10.0.19045.2486)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.101
  [Host]   : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```


|                                   Method |     Toolchain |            Mean |           Error |        StdDev |
|----------------------------------------- |-------------- |----------------:|----------------:|--------------:|
|             NaiveDivisor_99991_735134400 |      .NET 7.0 |    101,631.8 ns |     7,582.54 ns |     415.62 ns |
|              FastDivisor_99991_735134400 |      .NET 7.0 |    103,815.0 ns |     8,832.00 ns |     484.11 ns |
|   NaiveDivisor_132147483703_963761198400 |      .NET 7.0 |  2,098,131.6 ns | 1,552,311.87 ns |  85,087.45 ns |
|    FastDivisor_132147483703_963761198400 |      .NET 7.0 |    566,515.7 ns |   172,651.19 ns |   9,463.59 ns |
|                                          |               |                 |                 |               |
|           NaiveFactorize_99991_735134400 |      .NET 7.0 |        828.2 ns |       545.11 ns |      29.88 ns |
|            FastFactorize_99991_735134400 |      .NET 7.0 |      5,894.0 ns |     3,576.29 ns |     196.03 ns |
| NaiveFactorize_132147483703_963761198400 |      .NET 7.0 |  1,608,975.8 ns |   713,249.95 ns |  39,095.64 ns |
|  FastFactorize_132147483703_963761198400 |      .NET 7.0 |     10,393.3 ns |       957.10 ns |      52.46 ns |
|                                          |               |                 |                 |               |
|                          NaiveDivisorInt |      .NET 7.0 |    765,625.1 ns |   165,349.96 ns |   9,063.39 ns |
|                           FastDivisorInt |      .NET 7.0 |    231,200.5 ns |    43,307.48 ns |   2,373.83 ns |
|                                          |               |                 |                 |               |
|                         NaiveDivisorLong |      .NET 7.0 | 14,038,412.5 ns | 2,027,511.73 ns | 111,134.76 ns |
|                          FastDivisorLong |      .NET 7.0 |    407,988.2 ns |   229,181.50 ns |  12,562.21 ns |
|                                          |               |                 |                 |               |
|                        NaiveFactorizeInt |      .NET 7.0 |    692,646.1 ns |   112,523.93 ns |   6,167.82 ns |
|                         FastFactorizeInt |      .NET 7.0 |    153,680.0 ns |    50,282.19 ns |   2,756.14 ns |
|                                          |               |                 |                 |               |
|                       NaiveFactorizeLong |      .NET 7.0 | 14,087,616.7 ns | 1,230,937.56 ns |  67,471.84 ns |
|                        FastFactorizeLong |      .NET 7.0 |    340,504.9 ns |    93,071.99 ns |   5,101.59 ns |
|                                          |               |                 |                 |               |
|             NaiveDivisor_99991_735134400 | .NET Core 3.1 |    104,938.9 ns |     7,776.23 ns |     426.24 ns |
|              FastDivisor_99991_735134400 | .NET Core 3.1 |    111,000.3 ns |    56,966.91 ns |   3,122.55 ns |
|   NaiveDivisor_132147483703_963761198400 | .NET Core 3.1 |  2,351,247.8 ns |   694,534.79 ns |  38,069.80 ns |
|    FastDivisor_132147483703_963761198400 | .NET Core 3.1 |    610,463.1 ns |   220,834.53 ns |  12,104.69 ns |
|                                          |               |                 |                 |               |
|           NaiveFactorize_99991_735134400 | .NET Core 3.1 |        770.6 ns |        28.46 ns |       1.56 ns |
|            FastFactorize_99991_735134400 | .NET Core 3.1 |      7,031.7 ns |     3,625.28 ns |     198.71 ns |
| NaiveFactorize_132147483703_963761198400 | .NET Core 3.1 |  1,581,689.2 ns |   212,024.77 ns |  11,621.79 ns |
|  FastFactorize_132147483703_963761198400 | .NET Core 3.1 |     11,761.1 ns |     5,577.17 ns |     305.70 ns |
|                                          |               |                 |                 |               |
|                          NaiveDivisorInt | .NET Core 3.1 |    798,812.8 ns |   168,145.03 ns |   9,216.60 ns |
|                           FastDivisorInt | .NET Core 3.1 |    284,086.8 ns |    46,552.50 ns |   2,551.70 ns |
|                                          |               |                 |                 |               |
|                         NaiveDivisorLong | .NET Core 3.1 | 14,104,268.8 ns | 1,147,443.90 ns |  62,895.27 ns |
|                          FastDivisorLong | .NET Core 3.1 |    490,276.8 ns |    20,682.14 ns |   1,133.66 ns |
|                                          |               |                 |                 |               |
|                        NaiveFactorizeInt | .NET Core 3.1 |    716,554.2 ns |   145,712.86 ns |   7,987.01 ns |
|                         FastFactorizeInt | .NET Core 3.1 |    189,289.9 ns |    71,629.86 ns |   3,926.27 ns |
|                                          |               |                 |                 |               |
|                       NaiveFactorizeLong | .NET Core 3.1 | 14,032,726.0 ns | 1,336,312.29 ns |  73,247.79 ns |
|                        FastFactorizeLong | .NET Core 3.1 |    414,656.6 ns |   103,225.29 ns |   5,658.13 ns |
