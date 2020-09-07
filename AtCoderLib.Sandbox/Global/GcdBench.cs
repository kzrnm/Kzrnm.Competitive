using AtCoderProject;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Collections.Generic;


[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class GcdBench
{
    public static IEnumerable<object> Gcd_Data() => new object[] {
        new object[]{ 1, 2 },
        new object[]{ 2, 1000000007 },
        new object[]{ 564400443, 1000000007 },
    };

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("GCD")]
    [ArgumentsSource(nameof(Gcd_Data))]
    public void GcdLong(int num1, int num2) => _ = Global.Gcd(num1, (long)num2);
    [Benchmark]
    [BenchmarkCategory("GCD")]
    [ArgumentsSource(nameof(Gcd_Data))]
    public void GcdInt(int num1, int num2) => _ = Global.Gcd(num1, num2);


    [Benchmark(Baseline = true)]
    [BenchmarkCategory("LCM")]
    [ArgumentsSource(nameof(Gcd_Data))]
    public void LcmLong(int num1, int num2) => _ = Global.Lcm(num1, (long)num2);
    [Benchmark]
    [BenchmarkCategory("LCM")]
    [ArgumentsSource(nameof(Gcd_Data))]
    public void LcmInt(int num1, int num2) => _ = Global.Lcm(num1, num2);
}
