using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Collections.Generic;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ModBench
{
    public ModBench()
    {
        new Mod(1).Inverse();
    }
    public static IEnumerable<object> Int_Data() => new object[] {
        1,
        2901,
        564400443,
        999999947,
    };

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Int_Data))]
    [BenchmarkCategory("Inverse")]
    public void ModParam(int num)
    {
        _ = (Mod)Mod.EuclideanInverse(num, Mod.mod);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Int_Data))]
    [BenchmarkCategory("Inverse")]
    public void ModInverse(int num)
    {
        _ = new Mod(num).Inverse();
    }
}
