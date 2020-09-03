using AtCoderProject;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ExtBench
{
    const int N = 1000000;
    int[] arr = Util.MakeIntArray(N);

    [Benchmark]
    [BenchmarkCategory("GroupCount")]
    public void GroupCount()
    {
        arr.GroupCount();
    }

    [Benchmark]
    [BenchmarkCategory("GroupCount")]
    public void GroupCountMod()
    {
        arr.GroupCount(i => i % 12345);
    }
}