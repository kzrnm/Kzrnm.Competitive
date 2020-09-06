using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Reflection;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ModInvCacheBench
{
    readonly Func<long[]> MakeInverseCacheFunc;
    public ModInvCacheBench()
    {
        var method = typeof(Mod).GetMethod("MakeInverseCache", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static);
        MakeInverseCacheFunc = (Func<long[]>)Delegate.CreateDelegate(typeof(Func<long[]>), method);
    }


    [Benchmark]
    public void MakeInverseCache()
    {
        _ = MakeInverseCacheFunc();
    }
}
