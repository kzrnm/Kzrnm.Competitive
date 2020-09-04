using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class SuffixArrayBench
{
    public static object[][] Positions() => new object[][]
   {
        new object[]{ 0, Util.Str.Length/2 },
        new object[]{ 0, Util.Strs[0].Length },
        new object[]{ 0, 1 },
   };

    readonly SuffixArray sa = new SuffixArray(Util.Str);


    [Benchmark]
    [ArgumentsSource(nameof(Positions))]
    public void LCP(int i, int j)
    {
        sa.GetLCP(i, j);
    }
}

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class SuffixArrayCreateBench
{
    [Benchmark]
    public void Init()
    {
        new SuffixArray(Util.Str);
    }
}