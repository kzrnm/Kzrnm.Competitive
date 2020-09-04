using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class BMBench
{
    readonly BoyerMoore small = new BoyerMoore("a");
    readonly BoyerMoore smallTail = new BoyerMoore("z");
    readonly BoyerMoore head = new BoyerMoore(Util.Strs[0]);
    readonly BoyerMoore tail = new BoyerMoore(Util.Strs[^1]);
    readonly BoyerMoore notMatch = new BoyerMoore("abc");

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Small")]
    public void SmallString()
    {
        Util.Str.IndexOf("a");
    }
    [Benchmark]
    [BenchmarkCategory("Small")]
    public void SmallBM()
    {
        small.Match(Util.Str);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SmallTail")]
    public void SmallTailString()
    {
        Util.Str.IndexOf("z");
    }
    [Benchmark]
    [BenchmarkCategory("SmallTail")]
    public void SmallTailBM()
    {
        smallTail.Match(Util.Str);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Head")]
    public void HeadString()
    {
        Util.Str.IndexOf(Util.Strs[0]);
    }
    [Benchmark]
    [BenchmarkCategory("Head")]
    public void HeadBM()
    {
        head.Match(Util.Str);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Tail")]
    public void TailString()
    {
        Util.Str.IndexOf(Util.Strs[^1]);
    }
    [Benchmark]
    [BenchmarkCategory("Tail")]
    public void TailBM()
    {
        tail.Match(Util.Str);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("NotMatch")]
    public void NotMatchString()
    {
        Util.Str.IndexOf("abc");
    }
    [Benchmark]
    [BenchmarkCategory("NotMatch")]
    public void NotMatchBM()
    {
        notMatch.Match(Util.Str);
    }
}