using AtCoder;
using AtCoder.Expand;
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
        static bool toClipboard = false;
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "expand")
            {
                Console.WriteLine(ExpandCode());
                return;
            }

            var task = ExpandTask();

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
            task.Wait();
        }
        static string LoadInput()
        {
            const string path = @"AtCoderProject.input.txt";
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            using var sr = new StreamReader(stream);
            return sr.ReadToEnd().Trim();
        }

        static Task ExpandTask([CallerFilePath] string path = null)
            => Task.Run(() =>
            {
                //var inpath = path.Replace("HandMadeMain.cs", "Program.cs");
                //var outpath = path.Replace("HandMadeMain.cs", "Combined.csx");
                //Expander.Expand(inpath, outpath, checkLastWriteTime: true, expandMethod: ExpandMethod.Strict);
                //if (toClipboard)
                //{
                //    var psi = new ProcessStartInfo("cmd.exe", $"/c \"type {outpath} | clip\"")
                //    {
                //        CreateNoWindow = false,
                //    };
                //    Process.Start(psi);
                //}
            });

        static string ExpandCode([CallerFilePath] string path = null)
            => CodeExpander.Expand(File.ReadAllText(path.Replace("HandMadeMain.cs", "Program.cs")), expandMethod: ExpandMethod.Strict);

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