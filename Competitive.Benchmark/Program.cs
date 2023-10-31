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
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp70));
    }
}

[Config(typeof(BenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
public class Benchmark
{
    private readonly Random rnd = new Random(42);
    const int MAX_N = 1 << 20;
#if true
    [Params(new object[]
    {
        1 << 5,
        1 << 10,
        1 << 15,
        1 << 20,
    })]
    public int N;
#else
    public const int N = 1 << 15;
#endif

    public int[] array;
    [GlobalSetup]
    public void Setup()
    {
        array = new int[N];
        rnd.NextBytes(MemoryMarshal.Cast<int, byte>(array));
    }

    [Benchmark]
    [BenchmarkCategory("ListAdd")]
    public long OldListAdd()
    {
        var list = new List<long>(N);
        for (int i = 0; i < N; i++)
            list.Add(rnd.Next());
        return list[^1];
    }

    [Benchmark]
    [BenchmarkCategory("ListAdd")]
    public long NewListAdd()
    {
        var list = new PoolList<long>(N);
        for (int i = 0; i < N; i++)
            list.Add(rnd.Next());
        return list[^1];
    }
}