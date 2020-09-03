using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

public class ShortBenchmarkConfig : ManualConfig
{
    public ShortBenchmarkConfig()
    {
        AddDiagnoser(MemoryDiagnoser.Default);
        AddJob(Job.ShortRun.WithLaunchCount(1).WithIterationCount(1).WithWarmupCount(1));
    }
}