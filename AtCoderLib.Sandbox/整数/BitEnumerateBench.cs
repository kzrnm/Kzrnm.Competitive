using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Collections.Generic;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class BitEnumerateBench
{
    public static IEnumerable<object> Int_Data() => new object[] {
        0,
        -1,
        1,
        int.MinValue
    };

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Int")]
    [ArgumentsSource(nameof(Int_Data))]
    public void IntSelf(int num)
    {
        int sum = 0;
        for (var i = 0; i < (sizeof(int) * 8); i++, num >>= 1)
            if ((num & 1) != 0)
                sum += i;
    }

    [Benchmark]
    [BenchmarkCategory("Int")]
    [ArgumentsSource(nameof(Int_Data))]
    public void IntEnumerate(int num)
    {
        int sum = 0;
        foreach (var b in num.Bits())
            sum += b;
    }

    public static IEnumerable<object> Long_Data() => new object[] {
        0L,
        -1L,
        1L,
        unchecked((long)0b_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000),
        long.MinValue
    };

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Long")]
    [ArgumentsSource(nameof(Long_Data))]
    public void LongSelf(long num)
    {
        int sum = 0;
        for (var i = 0; i < (sizeof(long) * 8); i++, num >>= 1)
            if ((num & 1) != 0)
                sum += i;
    }

    [Benchmark]
    [BenchmarkCategory("Long")]
    [ArgumentsSource(nameof(Long_Data))]
    public void LongEnumerate(long num)
    {
        int sum = 0;
        foreach (var b in num.Bits())
            sum += b;
    }
}
