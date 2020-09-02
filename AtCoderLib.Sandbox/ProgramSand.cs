using BenchmarkDotNet.Running;
using System;

class ProgramSand
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            args = new[] { "--filter",
                //"二分探索",
            };

            if (args.Length == 1)
                args = new string[0];
        }
        BenchmarkSwitcher.
            FromAssembly(typeof(ProgramSand).Assembly).Run(args);
    }
}
