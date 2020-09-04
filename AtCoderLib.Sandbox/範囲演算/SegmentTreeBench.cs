using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class SegmentTreeBench
{
    const int N = 100000;
    readonly SegmentTree seg = new SegmentTree(Util.MakeLongArray(N));

    [Params(0, 50000)]
    public int from;
    [Params(50001, 100000)]
    public int toExclusive;

    [Benchmark]
    public void Query()
    {
        seg.Query(from, toExclusive);
    }
}
