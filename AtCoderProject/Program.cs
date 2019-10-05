using System;
using System.Collections.Generic;
using System.Linq;
using BitArray = System.Collections.BitArray;
using BigInteger = System.Numerics.BigInteger;
using TextReader = System.IO.TextReader;
using System.Text;

namespace AtCoderProject
{
    public class Program
    {
        public object Calc()
        {
            var a = consoleReader.Int;
            var b = consoleReader.Int;
            return new[] { a + b, a - b, a * b }.Max();
        }

        #region いつもの
#pragma warning disable
        private ConsoleReader consoleReader;
        public Program(ConsoleReader consoleReader) { this.consoleReader = consoleReader; }
        static void Main() => Console.WriteLine(new Program(new ConsoleReader(Console.In)).Calc()); static string AllLines<T>(IEnumerable<T> source) => string.Join("\n", source);
    }
    public class ConsoleReader
    {
        private string[] ReadLineSplit() => textReader.ReadLine().Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
        private string[] line = Array.Empty<string>();
        private int linePosition;
        private TextReader textReader;
        public ConsoleReader(TextReader tr) { textReader = tr; }
        public int Int => int.Parse(String);
        public long Long => long.Parse(String);
        public double Double => double.Parse(String);
        public string String
        {
            get
            {
                if (linePosition >= line.Length)
                {
                    linePosition = 0;
                    line = ReadLineSplit();
                }
                return line[linePosition++];
            }
        }
        public class SplitLine
        {
            private string[] splited;
            public SplitLine(ConsoleReader cr) { splited = cr.ReadLineSplit(); cr.line = Array.Empty<string>(); }
            public int[] Int => String.Select(x => int.Parse(x)).ToArray();
            public int[] Int0 => String.Select(x => int.Parse(x) - 1).ToArray();
            public long[] Long => String.Select(x => long.Parse(x)).ToArray();
            public double[] Double => String.Select(x => double.Parse(x)).ToArray();
            public string[] String => splited;
        }
        public SplitLine Split => new SplitLine(this);
        public class RepeatReader : IEnumerable<ConsoleReader>
        {
            ConsoleReader cr; int count;
            public RepeatReader(ConsoleReader cr, int count) { this.cr = cr; this.count = count; }
            public IEnumerator<ConsoleReader> GetEnumerator() => Enumerable.Repeat(cr, count).GetEnumerator();
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerable<string> String => this.Select(cr => cr.String);
            public IEnumerable<int> Int => this.Select(cr => cr.Int);
            public IEnumerable<int> Int0 => this.Select(cr => cr.Int - 1);
            public IEnumerable<long> Long => this.Select(cr => cr.Long);
            public IEnumerable<double> Double => this.Select(cr => cr.Double);
        }
        public RepeatReader Repeat(int count) => new RepeatReader(this, count);
    }
    #endregion
}