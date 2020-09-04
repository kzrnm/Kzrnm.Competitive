using AtCoderProject;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class KMPBench
{
    readonly KMP small = new KMP("a");
    readonly KMP smallTail = new KMP("z");
    readonly KMP head = new KMP(Util.Strs[0]);
    readonly KMP tail = new KMP(Util.Strs[^1]);
    readonly KMP notMatch = new KMP("abc");

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Small")]
    public void SmallString()
    {
        var str = Util.Str;
        for (var i = 0; i < str.Length; ++i)
            str.AsSpan(i).StartsWith("a");
    }
    [Benchmark]
    [BenchmarkCategory("Small")]
    public void SmallKMP()
    {
        foreach (var ix in small.Matches(Util.Str)) ;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SmallTail")]
    public void SmallTailString()
    {
        var str = Util.Str;
        for (var i = 0; i < str.Length; ++i)
            str.AsSpan(i).StartsWith("z");
    }
    [Benchmark]
    [BenchmarkCategory("SmallTail")]
    public void SmallTailKMP()
    {
        foreach (var ix in smallTail.Matches(Util.Str)) ;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Head")]
    public void HeadString()
    {
        var str = Util.Str;
        for (var i = 0; i < str.Length; ++i)
            str.AsSpan(i).StartsWith(Util.Strs[0]);
    }
    [Benchmark]
    [BenchmarkCategory("Head")]
    public void HeadKMP()
    {
        foreach (var ix in head.Matches(Util.Str)) ;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Tail")]
    public void TailString()
    {
        var str = Util.Str;
        for (var i = 0; i < str.Length; ++i)
            str.AsSpan(i).StartsWith(Util.Strs[^1]);
    }
    [Benchmark]
    [BenchmarkCategory("Tail")]
    public void TailKMP()
    {
        foreach (var ix in tail.Matches(Util.Str)) ;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("NotMatch")]
    public void NotMatchString()
    {
        var str = Util.Str;
        for (var i = 0; i < str.Length; ++i)
            str.AsSpan(i).StartsWith("abc");
    }
    [Benchmark]
    [BenchmarkCategory("NotMatch")]
    public void NotMatchKMP()
    {
        foreach (var ix in notMatch.Matches(Util.Str)) ;
    }
}