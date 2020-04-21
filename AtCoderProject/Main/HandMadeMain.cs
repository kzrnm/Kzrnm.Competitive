using AtCoderProject.Reader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            Program p;
            if (args.Length > 0)
                p = new Program(new ConsoleReader(new FileStream(args[0], FileMode.Open)));
            else if (sb.Length > 0)
                p = new Program(new ConsoleReader(sb.ToString()));
            else
                p = new Program(new ConsoleReader(Console.OpenStandardInput(), Console.InputEncoding));

            var result = p.Result();
            Console.WriteLine(result);
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
        IEnumerator IEnumerable.GetEnumerator() { throw new NotSupportedException(); }
    }
}
