using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AtCoderProject.Runner
{
    class MyStringBuilder : IEnumerable
    {
        private readonly StringBuilder sb = new StringBuilder();
        public int Length => sb.Length;
        public override string ToString() => sb.ToString();
        public void Add(object o) => sb.AppendLine(o.ToString());
        public void Add(params object[] objs) => sb.AppendLine(string.Join(" ", objs));
        public void Add<T>(IEnumerable<T> objs) => sb.AppendLine(string.Join(" ", objs));
        IEnumerator IEnumerable.GetEnumerator() { throw new NotSupportedException(); }
    }
    static class HandMadeMain
    {
        static void Main(string[] args)
        {
            var sb = new MyStringBuilder
            {

            };

            TextReader tr;
            if (args.Length > 0)
                tr = new StreamReader(args[0]);
            else if (sb.Length > 0)
                tr = new StringReader(sb.ToString());
            else
                tr = Console.In;
            Run(tr);
        }

        private static void Run(TextReader tr)
        {
            var result = new Program(new ConsoleReader(tr)).Calc();
            Console.WriteLine(result);
        }
    }
}
