using AtCoder;
using AtCoder.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AtCoderProject.Runner
{
    static class HandMadeMain
    {
        internal static Random rnd = new Random();
        static MyStringBuilder Build()
        {
            var sb = new MyStringBuilder
            {

            };

            return sb;
        }
        static IReadOnlyDictionary<string, string> files;
        static bool toClipboard = true;

        [STAThread]
        static void Main(string[] args)
        {
            files = (IReadOnlyDictionary<string, string>)Type.GetType("Expanded.Expanded").GetProperty("Files", BindingFlags.Public | BindingFlags.Static).GetValue(null);
            var expandedCode = ExpandCode();
            if (args.Length == 1 && args[0] == "expand")
            {
                Console.WriteLine(ExpandCode());
                return;
            }
            File.WriteAllText(CurrentPath().Replace("HandMadeMain.cs", "Combined.csx"), expandedCode);

            var sb = Build();

            var fileInput = LoadInput();
            if (!string.IsNullOrWhiteSpace(fileInput))
            {
                sb.Clear();
                sb.Add(fileInput);
            }

            ConsoleReader reader;
            var writer = new ConsoleWriter();
            if (args.Length > 0)
                reader = new ConsoleReader(new FileStream(args[0], FileMode.Open), new UTF8Encoding(false));
            else if (sb.Length > 0)
            {
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Error));
                reader = new ConsoleReader(new MemoryStream(new UTF8Encoding(false).GetBytes(sb.ToString())), Encoding.UTF8);
            }
            else
                reader = new ConsoleReader();


            Trace.WriteLine("---start---");
            var stopwatch = Stopwatch.StartNew();
            new Program(reader, writer).Run();
            Trace.WriteLine($"---end({stopwatch.ElapsedMilliseconds}ms)---");

            if (toClipboard)
                TextCopy.ClipboardService.SetText(expandedCode);
        }
        static string LoadInput()
        {
            const string path = @"AtCoderProject.input.txt";
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            using var sr = new StreamReader(stream);
            return sr.ReadToEnd().Trim();
        }

        static string CurrentPath([CallerFilePath] string path = "") => path;
        static string ExpandCode()
            => files[CurrentPath().Replace("HandMadeMain.cs", "Program.cs")];

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