using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Collections.Generic;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class BitOnBench
{
    public static IEnumerable<object> Int_Data() => new object[] { 0, -1, 1, int.MinValue };

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Int")]
    [ArgumentsSource(nameof(Int_Data))]
    public void IntAnd(int num)
    {
        for (var i = 0; i < (sizeof(int) * 8); i++)
            _ = ((num >> i) & 1) != 0;
    }

    [Benchmark]
    [BenchmarkCategory("Int")]
    [ArgumentsSource(nameof(Int_Data))]
    public void IntOn(int num)
    {
        for (var i = 0; i < (sizeof(int) * 8); i++)
            _ = num.On(i);
    }


    public static IEnumerable<object> Long_Data() => new object[] { 0L, -1L, 1L, long.MinValue };

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Long")]
    [ArgumentsSource(nameof(Long_Data))]
    public void LongAnd(long num)
    {
        for (var i = 0; i < (sizeof(long) * 8); i++)
            _ = ((num >> i) & 1) != 0;
    }

    [Benchmark]
    [BenchmarkCategory("Long")]
    [ArgumentsSource(nameof(Long_Data))]
    public void LongOn(long num)
    {
        for (var i = 0; i < (sizeof(long) * 8); i++)
            _ = num.On(i);
    }
}
