using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class StarrySkyAndSegmentTreeLazyBench
{
    const int N = 100000;

    public static object[][] AddAndQueryArgs() => new object[][]
    {
        new object[]{ 0 },
        new object[]{ N/2 },
    };

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AddAndQuery")]
    [ArgumentsSource(nameof(AddAndQueryArgs))]
    public void SegAddAndQuery(int from)
    {
        var seg = new SegmentTreeLazy(N);
        for (int i = from; i < N; i++)
            seg.Apply(from, i, i);
        for (int i = from; i < N; i++)
            seg.Query(from, i);
    }
    [Benchmark]
    [BenchmarkCategory("AddAndQuery")]
    [ArgumentsSource(nameof(AddAndQueryArgs))]
    public void StarrySkyAddAndQuery(int from)
    {
        var sst = new StarrySkyTree(N);
        for (int i = from; i < N; i++)
            sst.Add(from, i, i);
        for (int i = from; i < N; i++)
            sst.Query(from, i);
    }


    public static object[][] AddArgs() => new object[][]
    {
        new object[]{ 0 },
        new object[]{ N/2 },
    };
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Add")]
    [ArgumentsSource(nameof(AddArgs))]
    public void SegAdd(int from)
    {
        var seg = new SegmentTreeLazy(N);
        for (int i = from; i < N; i++)
            seg.Apply(from, i, i);
    }
    [Benchmark]
    [BenchmarkCategory("Add")]
    [ArgumentsSource(nameof(AddArgs))]
    public void StarrySkyAdd(int from)
    {
        var sst = new StarrySkyTree(N);
        for (int i = from; i < N; i++)
            sst.Add(from, i, i);
    }

    class SegmentTreeLazy : SegmentTreeLazyAbstract<long, long>
    {
        protected override long DefaultValue => 0;
        protected override long DefaultLazy => 0;
        protected override long Operate(long v1, long v2) => v1 + v2;
        protected override long ApplyLazy(long v, long l) => v + l;
        protected override long Merge(long l1, long l2) => l1 + l2;
        public SegmentTreeLazy(long[] initArray) : base(initArray) { }
        public SegmentTreeLazy(int size) : base(size) { }
    }
}
