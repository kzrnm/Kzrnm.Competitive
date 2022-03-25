using AtCoder;
using AtCoder.Extension;
using AtCoder.Internal;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using Kzrnm.Competitive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;



public class BenchmarkConfig : ManualConfig
{
    static void Main(string[] args)
    {
#if DEBUG
        BenchmarkSwitcher.FromAssembly(typeof(BenchmarkConfig).Assembly).Run(args, new DebugInProcessConfig());
#else
_ = BenchmarkRunner.Run(typeof(Benchmark).Assembly);
#endif
    }
    public BenchmarkConfig()
    {
        //AddDiagnoser(MemoryDiagnoser.Default);
        AddExporter(BenchmarkDotNet.Exporters.MarkdownExporter.GitHub);
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp31));
    }
}

[Config(typeof(BenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
public class Benchmark
{
    private readonly Random rnd = new Random(42);
    const int MAX_N = 1 << 20;
    [Params(
        //1 << 10,
        //1 << 15,
        1 << 20)]
    public int N;
    Set<int> set;
    Kzrnm.Competitive2.Set<int> set2;

    [GlobalSetup]
    public void Setup()
    {
        var array = new int[N];
        rnd.NextBytes(MemoryMarshal.Cast<int, byte>(array));
        set = new Set<int>(array);
        set2 = new Kzrnm.Competitive2.Set<int>(array);
    }

    [Benchmark]
    [BenchmarkCategory("LowerBound")]
    public int CuurentLowerBound()
    {
        int ix = 0;
        for (int i = 0; i < 1_000_000; i++)
            ix ^= set2.LowerBoundIndex(ix);
        return ix;
    }

    [Benchmark]
    [BenchmarkCategory("LowerBound")]
    public int NewLowerBound()
    {
        int ix = 0;
        for (int i = 0; i < 1_000_000; i++)
            ix ^= set.LowerBoundIndex(ix);
        return ix;
    }
}
