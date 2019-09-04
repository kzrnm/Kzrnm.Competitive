using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitArray = System.Collections.BitArray;
using BigInteger = System.Numerics.BigInteger;
using Unsafe = System.Runtime.CompilerServices.Unsafe;
using TextReader = System.IO.TextReader;

#pragma warning disable

namespace AtCoderProject.Hide
{
    public class ConsoleReader
    {
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
                    line = textReader.ReadLine().Split();
                }
                return line[linePosition++];
            }
        }
        public class SplitLine
        {
            private string[] splited;
            public SplitLine(ConsoleReader cr) { splited = cr.textReader.ReadLine().Split(); }
            public int[] Int => String.Select(x => int.Parse(x)).ToArray();
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

    struct Index : IEquatable<Index>
    {
        public int h;
        public int w;
        public Index(int i, int j)
        {
            this.h = i;
            this.w = j;
        }
        public bool Equals(Index other) => this.h == other.h && this.w == other.w;
    }


    static class DicExt
    {
        public static V GetOrDefault<K, V>(IDictionary<K, V> dic, K key, V defaultValue = default(V))
        {
            V val;
            return dic.TryGetValue(key, out val) ? val : defaultValue;
        }
        public static V GetOrInit<K, V>(IDictionary<K, V> dic, K key) where V : new()
        {
            V val;
            if (dic.TryGetValue(key, out val))
                return val;

            val = new V();
            dic.Add(key, val);
            return val;
        }
    }

    struct Work
    {
        public static Work Create(int[] l) => new Work { size = l[0], limit = l[1] };
        public int size;
        public int limit;
    }

    public class Utils
    {
        void 多次元配列の初期化(object[,] array, int n, int m, object defaultValue)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    array[i, j] = defaultValue;
                }
        }
    }
}
