using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ModConstructBench
{
    const int N = 10000000;

    [Benchmark(Baseline = true)]
    public void ConstructorPositive()
    {
        var rnd = new Xorshift((int)Mod.mod);
        var arr = new Mod[N];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = new Mod(rnd.Next(0, (int)Mod.mod));
    }
    [Benchmark]
    public void ConstructorPositiveOver()
    {
        var rnd = new Xorshift((int)Mod.mod);
        var arr = new Mod[N];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = new Mod(rnd.Next((int)Mod.mod, int.MaxValue));
    }
    [Benchmark]
    public void ConstructorNegative()
    {
        var rnd = new Xorshift((int)Mod.mod);
        var arr = new Mod[N];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = new Mod(rnd.Next(int.MinValue, 0));
    }
    [Benchmark]
    public void UnsafeInit()
    {
        var rnd = new Xorshift((int)Mod.mod);
        var arr = new Mod[N];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = Mod.Unsafe(rnd.Next(int.MinValue, 0));
    }
}
