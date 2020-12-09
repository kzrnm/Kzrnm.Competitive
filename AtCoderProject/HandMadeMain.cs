using Kzrnm.Competitive.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AtCoderProject.Runner
{
    static class HandMadeMain
    {
        internal static Random rnd = new Random();
        private static int[] RandomArray(int length, int min, int maxExclusive) { var arr = new int[length]; for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next(min, maxExclusive); return arr; }
        static MyStringBuilder Build()
        {
            var sb = new MyStringBuilder
            {

            };

            return sb;
        }

        [STAThread]
        static void Main(string[] args)
        {
            string expandedCode = null;
#if DEBUG
            var files = SourceExpander.Expanded.ExpandedContainer.Files;
            expandedCode = files[CurrentPath().Replace("HandMadeMain.cs", "Program.cs")].Code;
#endif

            PropertyConsoleReader reader;
            var writer = new ConsoleWriter();

            if (args.Length > 0 && args[0] == "expand")
            {
                if (expandedCode != null)
                    Expand(args.AsSpan(1), expandedCode);
                return;
            }
            else if (args.Length > 0)
            {
                reader = new PropertyConsoleReader(new FileStream(args[0], FileMode.Open), new UTF8Encoding(false));
            }
            else
            {
                if (expandedCode != null)
                    File.WriteAllText(CurrentPath().Replace("HandMadeMain.cs", "Combined.csx"), expandedCode);

                var sb = Build();

                var fileInput = LoadInput();
                if (!string.IsNullOrWhiteSpace(fileInput))
                {
                    sb.Clear();
                    sb.Add(fileInput);
                }
                if (IsNotWhiteSpace(sb.sb))
                {
                    Trace.Listeners.Add(new TextWriterTraceListener(Console.Error));
                    reader = new PropertyConsoleReader(new MemoryStream(new UTF8Encoding(false).GetBytes(sb.ToString())), Encoding.UTF8);
                }
                else
                    reader = new PropertyConsoleReader();
            }

            Trace.WriteLine("---start---");
            var stopwatch = Stopwatch.StartNew();
            new Program(reader, writer).Run();
            stopwatch.Stop();
            Trace.WriteLine($"---end({stopwatch.ElapsedMilliseconds}ms)---");
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
                var writePath = CurrentPath().Replace("HandMadeMain.cs", "Combined.csx");
                File.WriteAllText(writePath, expandedCode);
                Console.WriteLine($"Write {writePath}");
            }
            else
                Console.WriteLine(expandedCode);
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
            var path = Path.Combine(Path.GetDirectoryName(CurrentPath()), "input.txt");
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(stream);
            return sr.ReadToEnd().Trim();
        }

        static string CurrentPath([CallerFilePath] string path = "") => path;
    }
    class MyStringBuilder : IEnumerable
    {
        public readonly StringBuilder sb = new StringBuilder();
        public int Length => sb.Length;
        public override string ToString() => sb.ToString();
        public MyStringBuilder Add(object o) { sb.AppendLine(o.ToString()); return this; }
        public MyStringBuilder Add(string s) { sb.AppendLine(s); return this; }
        public MyStringBuilder Add(params object[] objs) { sb.AppendLine(string.Join(" ", objs)); return this; }
        public MyStringBuilder Add<T>(IEnumerable<T> objs) { sb.AppendLine(string.Join(" ", objs)); return this; }
        public MyStringBuilder Clear() { sb.Clear(); return this; }
        IEnumerator IEnumerable.GetEnumerator() { throw new NotSupportedException(); }
    }
}