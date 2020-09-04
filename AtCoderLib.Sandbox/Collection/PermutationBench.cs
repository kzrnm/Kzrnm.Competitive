using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Linq;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]

public class PermutationBench
{
    [Params(9, 10, 11)]
    public int N;
    private static int[] arr = Util.MakeIntArray(20);

    [Benchmark]
    public void Permutation()
    {
        foreach (var p in 順列を求める.Permutation(arr.Take(N))) ;
    }
}
