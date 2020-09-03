using AtCoderProject;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;




[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ExComparerBench
{
    readonly IComparer<long> genericComparer;
    readonly IComparer<long> expComparer;
    public ExComparerBench()
    {
        genericComparer = Comparer<long>.Create((l1, l2) => l2.CompareTo(l1));
        expComparer = ExComparer<long>.CreateExp(l => -l);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Compare")]
    public void Default()
    {
        Comparer<long>.Default.Compare(long.MinValue, long.MaxValue);
    }
    [Benchmark]
    [BenchmarkCategory("Compare")]
    public void Generic()
    {
        genericComparer.Compare(long.MinValue, long.MaxValue);
    }
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Create")]
    public void CreateGeneric()
    {
        Comparer<string>.Create((s1, s2) => s1.Length.CompareTo(s2.Length));
    }

    [Benchmark]
    [BenchmarkCategory("Compare")]
    public void DefaultReverse()
    {
        ExComparer<long>.DefaultReverse.Compare(long.MinValue, long.MaxValue);
    }

    [Benchmark]
    [BenchmarkCategory("Create")]
    public void CreateExp()
    {
        ExComparer<string>.CreateExp(s => s.Length);
    }

    [Benchmark]
    [BenchmarkCategory("Compare")]
    public void Exp()
    {
        expComparer.Compare(long.MinValue, long.MaxValue);
    }
}