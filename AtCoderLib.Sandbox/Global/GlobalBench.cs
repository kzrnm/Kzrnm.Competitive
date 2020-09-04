using AtCoderProject;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class GlobalBench
{
    readonly string longNumStr;

    public GlobalBench()
    {
        var rnd = new Xorshift(42);
        var sb = new StringBuilder(10000);
        sb.Append('-');
        for (int i = 0; i < 1000; i++)
            sb.Append(rnd.NextUInt32());
        longNumStr = sb.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Pow")]
    public long Standard()
    {
        long r = 1;
        for (int i = 0; i < 63; i++)
            r *= 2;
        return r;
    }
    [Benchmark]
    [BenchmarkCategory("Pow")]
    public long Pow()
    {
        return Global.Pow(2, 63);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("BigInteger")]
    public BigInteger StandardParse()
    {
        return BigInteger.Parse(longNumStr);
    }
    [Benchmark]
    [BenchmarkCategory("BigInteger")]
    public BigInteger Parse()
    {
        return Global.ParseBigInteger(longNumStr);
    }


    [Benchmark]
    [BenchmarkCategory("BitOp")]
    public int PopCount()
    {
        return Global.PopCount(-1L);
    }

    [Benchmark]
    [BenchmarkCategory("BitOp")]
    public int MSB()
    {
        return Global.MSB(-1L);
    }

    [Benchmark]
    [BenchmarkCategory("BitOp")]
    public int LSB()
    {
        return Global.LSB(-1L);
    }
}