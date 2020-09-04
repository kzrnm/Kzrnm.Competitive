using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ZAlgorithmBench
{
    private static readonly string allA = new string('a', 100000);

    [Benchmark]
    public void All()
    {
        ZAlgorithmEx.ZAlgorithm(allA);
    }
    [Benchmark]
    public void Normal()
    {
        ZAlgorithmEx.ZAlgorithm(Util.Str);
    }
}
