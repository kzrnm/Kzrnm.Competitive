#pragma warning disable SYSLIB1045, IDE0251
using Kzrnm.Competitive.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
#nullable enable
[assembly: DebuggerDisplay("{Kzrnm.Competitive.BitArrayDebug.ToDebugString(this)}", Target = typeof(BitArray))]
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
#if DEBUG
                Expand(args.AsSpan(1));
#else
                ThrowEmptyExpanded();
#endif
                return;
            }
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var utf8 = new UTF8Encoding(false);
            Console.InputEncoding = utf8;
            Console.OutputEncoding = utf8;
            Stopwatch? stopwatch = null;
            PropertyConsoleReader reader;
            var writer = new Utf8ConsoleWriter(Console.OpenStandardOutput());

            if (args.Length > 0)
            {
                reader = new PropertyConsoleReader(new FileStream(args[0], FileMode.Open), utf8);
            }
            else
            {
                Trace.Listeners.Add(new TraceListener(Console.Error));
                var sb = Build();
                if (sb is { Length: 0 } &&
                    LoadInput() is { } fileInput &&
                    !string.IsNullOrWhiteSpace(fileInput))
                    sb.Add(fileInput);

                if (IsNotWhiteSpace(sb.sb))
                {
                    stopwatch = new Stopwatch();
                    var input = sb.ToString();
                    reader = new PropertyConsoleReader(new MemoryStream(utf8.GetBytes(input)), Encoding.UTF8);
                    if (input.Length < 1000)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Error.WriteLine($"File input:\n{input}");
                        Console.ResetColor();
                    }
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
            Stopwatch? Stopwatch;
            public StopwatchWrapper(Stopwatch? stopwatch)
            {
                Stopwatch = stopwatch;
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

        static void Expand(ReadOnlySpan<string> args)
        {
            var expandedCode = GetSourceCode(BasePath.Replace("HandMadeMain.cs", "Program.cs"))?.Code;
            if (expandedCode == null)
                ThrowEmptyExpanded();

            Expand(args, FormatCode(expandedCode));
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


        [GeneratedRegex(
            @"(Property)?(Console|Repeat)(Reader|Writer)|ConsoleOutput|bufferSize|EnsureBuf|FillEntireNumber|Write(Many|LineJoin)|Kzrnm\.Competitive(?!/b)|(?<!\.)ReadOnlySpan<(char|byte)>")]
        private static partial Regex SimpleReplaceTarget();
        private static string SimpleReplace(Match m) => m.ValueSpan switch
        {
            "PropertyConsoleReader" => "PrpConR",
            "ConsoleWriter" => "ConW",
            "ConsoleReader" => "ConR",
            "PropertyRepeatReader" => "PrpRepR",
            "RepeatReader" => "RepR",
            "bufferSize" => "bfsz",
            "EnsureBuf" => "EnsBf",
            "FillEntireNumber" => "FlNum",
            "ConsoleOutput" => "ConO",
            "WriteMany" => "WrMany",
            "WriteLineJoin" => "WrJoin",
            "Kzrnm.Competitive" => "Kzrnm",
            _ => m.Value,
        };

        [GeneratedRegex(@"#if !LOCAL_RUNNING\n(.*)#endif\n", RegexOptions.Singleline)]
        private static partial Regex NotLocalRunning();
        [GeneratedRegex(@"\[(MI|MethodImpl|凾)\(((MethodImplOptions\.)?AggressiveInlining|256)")]
        private static partial Regex AggressiveInliningRegex();

        [GeneratedRegex(@"(?<!\.)ReadOnlySpan<((c)har|(b)yte)>")]
        private static partial Regex ReadOnlySpanText();
        static string FormatCode(string expandedCode)
        {
            expandedCode = expandedCode
                .Replace("\r\n", "\n")
                .Replace("using 凾", "using MAttribute")
                .Replace("using M=MethodImplAttribute;", "")
                .Replace("using MI=System.Runtime.CompilerServices.MethodImplAttribute;", "");
            expandedCode = NotLocalRunning().Replace(expandedCode, "$1");
            expandedCode = AggressiveInliningRegex().Replace(expandedCode, "[M(256");
            expandedCode = ReadOnlySpanText().Replace(expandedCode, "ROS$2$3 ");
            expandedCode = SimpleReplaceTarget().Replace(expandedCode, SimpleReplace);

            if (expandedCode.Replace("namespace AtCoder.Extension", "namespace MyAtCoder.Extension") is var rep && rep.Length != expandedCode.Length)
            {
                expandedCode = rep.Replace("using AtCoder.Extension", "using MyAtCoder.Extension");
            }
            return expandedCode;
        }
        static SourceExpander.Expanded.SourceCode? GetSourceCode(string filePath)
        {
            var expandedContainerType = typeof(Program).Assembly.GetType("SourceExpander.Expanded.ExpandedContainer");
            if (expandedContainerType is null) return null;
            if (expandedContainerType.GetProperty("Files")?.GetValue(null)
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
            var path = Path.Combine(Path.GetDirectoryName(BasePath)!, "input.txt");
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(stream);
            return sr.ReadToEnd().Trim();
        }
        static string CurrentPath([CallerFilePath] string path = "") => path;
        [DoesNotReturn]
        static void ThrowEmptyExpanded()
        {
            new Exception("expandedCode is null.");
            Debug.Assert(false);
        }
    }
}