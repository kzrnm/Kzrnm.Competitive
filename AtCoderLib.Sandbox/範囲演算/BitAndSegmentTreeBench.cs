using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class BitAndSegmentTreeBench
{
    const int N = 100000;
    readonly SegmentTree seg = new SegmentTree(Util.MakeLongArray(N));
    readonly BinaryIndexedTree bit = new BinaryIndexedTree(Util.MakeLongArray(N));

    public static object[][] QueryArgs() => new object[][]
    {
        new object[]{ 0 },
        new object[]{ N/2 },
    };

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Query")]
    [ArgumentsSource(nameof(QueryArgs))]
    public void SegQuery(int from)
    {
        for (int i = from; i < N; i++)
            seg.Query(from, i);
    }
    [Benchmark]
    [BenchmarkCategory("Query")]
    [ArgumentsSource(nameof(QueryArgs))]
    public void BitQuery(int from)
    {
        for (int i = from; i < N; i++)
            bit.Query(from, i);
    }


    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Add")]
    public void SegAdd()
    {
        for (int i = 0; i < N; i++)
            seg[i] += i;
    }
    [Benchmark]
    [BenchmarkCategory("Add")]
    public void BitAdd()
    {
        for (int i = 0; i < N; i++)
            bit.Add(i, i);
    }

    class SegmentTree : SegmentTreeAbstract<long>
    {
        protected override long DefaultValue => 0;
        protected override long Operate(long v1, long v2) => v1 + v2;
        public SegmentTree(long[] initArray) : base(initArray) { }
        public SegmentTree(int size) : base(size) { }
    }
}
