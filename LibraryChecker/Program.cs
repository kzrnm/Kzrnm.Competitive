﻿using Cysharp.Diagnostics;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtCoder
{
    class Program
    {
        static async Task Main()
        {
            var solvers = new List<ISolver>();
            var tasks = new List<Task>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                if (type.GetInterfaces().Contains(typeof(ISolver)))
                {
                    var obj = (ISolver)Activator.CreateInstance(type);
                    solvers.Add(obj);
                }

            foreach (var solver in solvers)
                tasks.Add(Run(solver));

            await Task.WhenAll(tasks);
        }

        static readonly UTF8Encoding UTF8NoBom = new UTF8Encoding(false);
        static readonly string CheckerRoot
            = Path.Combine(Environment.CurrentDirectory, "library-checker-problems");
        static async Task Run(ISolver solver)
        {
            var testDir = Directory.EnumerateDirectories(CheckerRoot, solver.Name, SearchOption.AllDirectories).Single();

            try
            {
                await foreach (var line in ProcessX.StartAsync(
                    $"python3 generate.py -p {solver.Name}", workingDirectory: CheckerRoot).ConfigureAwait(false))
                {
                    Console.WriteLine(line);
                }
            }
            catch (ProcessErrorException e)
            {
                Console.WriteLine($"ProcessErrorException on {solver.Name}: {e.Message}");
                if (e.ExitCode != 0)
                    throw;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception on {solver.Name}: {e.Message}");
                throw;
            }

            var inDir = Path.Combine(testDir, "in");
            var outDir = Path.Combine(testDir, "out");
            var gotDir = Path.Combine(testDir, "got");
            Directory.CreateDirectory(gotDir);

            foreach (var inputFile in Directory.EnumerateFiles(inDir))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFile);
                using var rfs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
                using var wfs = new FileStream(Path.Combine(gotDir, fileNameWithoutExtension + ".got"),
                    FileMode.Create, FileAccess.ReadWrite);
                var cr = new ConsoleReader(rfs, UTF8NoBom);
                var cw = new ConsoleWriter(wfs, UTF8NoBom);
                solver.Solve(cr, cw);
                cw.Flush();

                try
                {
                    var checkerPath = Path.Combine(testDir, "checker");
                    var command = string.Join(" ", checkerPath,
                        Path.Combine(inDir, fileNameWithoutExtension + ".in"),
                        Path.Combine(outDir, fileNameWithoutExtension + ".out"),
                        Path.Combine(gotDir, fileNameWithoutExtension + ".got"));
                    Console.WriteLine($"Run: {solver.Name} - {fileNameWithoutExtension}");
                    await foreach (var line in ProcessX.StartAsync(command, workingDirectory: CheckerRoot).ConfigureAwait(false))
                    {
                        Console.WriteLine(line);
                    }
                }
                catch (ProcessErrorException e)
                {
                    Console.WriteLine($"ProcessErrorException on {solver.Name}-{fileNameWithoutExtension}: {e.Message}");
                    if (e.ExitCode != 0)
                        throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception on {solver.Name}-{fileNameWithoutExtension}: {e.Message}");
                    throw;
                }
            }
        }
    }
}
