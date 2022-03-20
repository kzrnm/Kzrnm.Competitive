using Kzrnm.Competitive.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Competitive.Runner
{
    static partial class HandMadeMain
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "expand")
            {
                Console.WriteLine("expand mode");
                string expandedCode = null;
#if DEBUG
                expandedCode = GetSourceCode(BasePath.Replace("HandMadeMain.cs", "Program.cs"))
                    ?.Code
                    ?.Replace("\r\n", "\n")
                    ?.Replace("using MI=System.Runtime.CompilerServices.MethodImplAttribute;", "");
                expandedCode = ReplaceMethodImpl(expandedCode);
                static string ReplaceMethodImpl(string code)
                {
                    if (code is null) return null;

                    return Regex.Replace(code, @"\[(MI|MethodImpl)\(((MethodImplOptions\.)?AggressiveInlining|256)", "[凾(256");
                }
#endif
                if (expandedCode != null)
                    Expand(args.AsSpan(1), expandedCode);
                else
                    Console.WriteLine("expandedCode is null.");
                return;
            }
            var utf8 = new UTF8Encoding(false);
            Stopwatch stopwatch = null;
            PropertyConsoleReader reader;
            var writer = new Utf8ConsoleWriter(Console.OpenStandardOutput());

            if (args.Length > 0)
            {
                reader = new PropertyConsoleReader(new FileStream(args[0], FileMode.Open), utf8);
            }
            else
            {
                var sb = Build();
                if (sb is { Length: 0 } &&
                    LoadInput() is { } fileInput &&
                    !string.IsNullOrWhiteSpace(fileInput))
                    sb.Add(fileInput);

                Trace.Listeners.Add(new TextWriterTraceListener(Console.Error));
                if (IsNotWhiteSpace(sb.sb))
                {
                    reader = new PropertyConsoleReader(new MemoryStream(utf8.GetBytes(sb.ToString())), Encoding.UTF8);
                    stopwatch = new Stopwatch();
                }
                else
                    reader = new PropertyConsoleReader();
            }

            using (new StopwatchWrapper(stopwatch))
            {
                new Program(reader, writer).Run();
            }
        }
        struct StopwatchWrapper : IDisposable
        {
            Stopwatch Stopwatch;
            public StopwatchWrapper(Stopwatch stopwatch)
            {
                this.Stopwatch = stopwatch;
                if (stopwatch != null)
                {
                    Trace.WriteLine("---start---");
                    stopwatch.Start();
                }
            }
            void IDisposable.Dispose()
            {
                if (Stopwatch != null)
                {
                    Stopwatch.Stop();
                    Trace.WriteLine($"---end({Stopwatch.ElapsedMilliseconds}ms)---");
                }
            }
        }

        static void Expand(ReadOnlySpan<string> args, string expandedCode)
        {
            bool writeFile = false;
            bool toClipboard = false;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--toclipboard":
                        toClipboard = true;
                        break;
                    case "--writefile":
                        writeFile = true;
                        break;
                }
            }

            if (toClipboard)
            {
                TextCopy.ClipboardService.SetText(expandedCode);
                Console.WriteLine("Copy to Clipboard");
            }


            if (writeFile)
            {
                var writePath = BasePath.Replace("HandMadeMain.cs", "Combined.csx");
                File.WriteAllText(writePath, expandedCode);
                Console.WriteLine($"Write {writePath}");
            }
            else
                Console.WriteLine(expandedCode);
        }
        static SourceExpander.Expanded.SourceCode GetSourceCode(string filePath)
        {
            var expandedContainerType = typeof(Program).Assembly.GetType("SourceExpander.Expanded.ExpandedContainer");
            if (expandedContainerType is null) return null;
            if (expandedContainerType.GetProperty("Files").GetValue(null)
                is IReadOnlyDictionary<string, SourceExpander.Expanded.SourceCode> dic)
                return dic.GetValueOrDefault(filePath);
            return null;
        }
        static bool IsNotWhiteSpace(StringBuilder sb)
        {
            foreach (var chunk in sb.GetChunks())
                if (!chunk.Span.IsEmpty && !chunk.Span.IsWhiteSpace())
                    return true;
            return false;
        }
        static string LoadInput()
        {
            var path = Path.Combine(Path.GetDirectoryName(BasePath), "input.txt");
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(stream);
            return sr.ReadToEnd().Trim();
        }
        static T[] Shuffle<T>(T[] arr)
        {
            for (int n = arr.Length - 1; n >= 0; n--)
            {
                int k = rnd.Next(n + 1);
                (arr[k], arr[n]) = (arr[n], arr[k]);
            }
            return arr;
        }
        static (int, int) Next2(int min, int max, bool sameOk = false, bool sorted = false)
        {
            var a = rnd.Next(min, max);
            var b = rnd.Next(min, max);
            while (sameOk || a == b) b = rnd.Next(min, max);
            if (sorted && a > b)
                (a, b) = (b, a);
            return (a, b);
        }
        static string CurrentPath([CallerFilePath] string path = "") => path;
    }
}