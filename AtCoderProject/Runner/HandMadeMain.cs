using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtCoderProject.Runner
{
    class MyStringBuilder : IEnumerable
    {
        private readonly StringBuilder sb = new StringBuilder();
        public override string ToString() => sb.ToString();
        public void Add(object o) => sb.AppendLine(o.ToString());
        public void Add(params object[] objs) => sb.AppendLine(string.Join(" ", objs));
        IEnumerator IEnumerable.GetEnumerator() { throw new NotSupportedException(); }
    }
    static class HandMadeMain
    {
        static void Main()
        {
            var sb = new MyStringBuilder
            {

            };


            Run(sb.ToString());
        }

        private static void Run(string input)
        {
            TextReader tr;
            if (string.IsNullOrEmpty(input))
                tr = Console.In;
            else
                tr = new StringReader(input);
            var result = new Program(new ConsoleReader(tr)).Calc();
            Console.WriteLine(result);
        }
    }
}
