using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

public class MyBenchmarkConfig : ManualConfig
{
    public MyBenchmarkConfig()
    {
        AddDiagnoser(MemoryDiagnoser.Default);
        AddJob(Job.ShortRun.WithLaunchCount(1).WithIterationCount(1).WithWarmupCount(1));
    }
}