using BenchmarkDotNet.Running;
using System;

class ProgramSand
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            args = new[] { "--filter",
                //nameof(二分探索Bench),
                //nameof(ExComparerBench),
                //nameof(ExtBench),
                //nameof(GlobalBench),
                //nameof(ConsoleReaderBenchSingle),
                //nameof(ConsoleReaderBenchSplit),
                //nameof(ConsoleReaderBenchRepeat),
                //nameof(ConsoleWriterBench),
            };

            if (args.Length == 1)
                args = Array.Empty<string>();
        }
        BenchmarkSwitcher.
            FromAssembly(typeof(ProgramSand).Assembly).Run(args
#if DEBUG
            , new BenchmarkDotNet.Configs.DebugInProcessConfig()
#endif
            );
    }
}
