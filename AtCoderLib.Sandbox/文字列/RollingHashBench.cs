using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Linq;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class RollingHashBench
{
    const int N = 10000;
    public static readonly string Str = string.Join("", Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", N));
    private readonly RollingHash rh = new RollingHash(Str);

    [Benchmark]
    public void Create()
    {
        new RollingHash(Str);
    }

    [Benchmark]
    public void Slice()
    {
        _ = rh[0..(26 * N)];
    }
}