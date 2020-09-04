using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class CreateBench
{
    public static object[][] InitStrs() => new object[][]
    {
        new object[]{ "a" },
        new object[]{ Util.Strs[0] },
        new object[]{ "abc" },
    };

    [Benchmark]
    [ArgumentsSource(nameof(InitStrs))]
    public void InitBM(string pattern)
    {
        new BoyerMoore(pattern);
    }

    [Benchmark]
    [ArgumentsSource(nameof(InitStrs))]
    public void InitKMP(string pattern)
    {
        new KMP(pattern);
    }
}