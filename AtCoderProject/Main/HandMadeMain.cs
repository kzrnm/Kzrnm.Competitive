using AtCoderProject.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AtCoderProject.Runner
{
    static class HandMadeMain
    {
        internal static Random rnd = new Random();
        static void Main(string[] args)
        {
            var sb = new MyStringBuilder
            {

            };


            var fileInput = LoadInput();
            if (!string.IsNullOrWhiteSpace(fileInput))
                sb.Add(fileInput);

            ConsoleReader reader;
            var writer = new ConsoleWriter(Console.OpenStandardOutput());
            if (args.Length > 0)
                reader = new ConsoleReader(new FileStream(args[0], FileMode.Open), Encoding.UTF8);
            else if (sb.Length > 0)
                reader = new ConsoleReader(new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())), Encoding.UTF8);
            else
                reader = new ConsoleReader(Console.OpenStandardInput());

            new Program(reader, writer).Run();
        }
        static string LoadInput()
        {
            const string path = @"AtCoderProject.Main.input.txt";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            using (var sr = new StreamReader(stream))
                return sr.ReadToEnd();
        }
    }
    class MyStringBuilder : IEnumerable
    {
        public readonly StringBuilder sb = new StringBuilder();
        public int Length => sb.Length;
        public override string ToString() => sb.ToString();
        public void Add(object o) => sb.AppendLine(o.ToString());
        public void Add(string s) => sb.AppendLine(s);
        public void Add(params object[] objs) => sb.AppendLine(string.Join(" ", objs));
        public void Add<T>(IEnumerable<T> objs) => sb.AppendLine(string.Join(" ", objs));
        public void Clear() => sb.Clear();
        IEnumerator IEnumerable.GetEnumerator() { throw new NotSupportedException(); }
    }
}
