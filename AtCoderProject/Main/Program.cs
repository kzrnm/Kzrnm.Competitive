#region いつもの
using AtCoderProject;
using System;
using System.Collections.Generic;
using System.Linq;
using static AtCoderProject.Global;
using BigInteger = System.Numerics.BigInteger;
using BitOperations = System.Numerics.BitOperations;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;
using StringBuilder = System.Text.StringBuilder;
using Unsafe = System.Runtime.CompilerServices.Unsafe;

namespace AtCoderProject
{
    using System.IO;
    using System.Text;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Linq.Expressions;

    public static class Global
    {
        public static T[] NewArray<T>(int len0, T value) => new T[len0].Fill(value);
        public static T[] NewArray<T>(int len0, Func<T> factory)
        {
            var arr = new T[len0];
            for (int i = 0; i < arr.Length; i++) arr[i] = factory();
            return arr;
        }
        public static T[][] NewArray<T>(int len0, int len1, T value) where T : struct
        {
            var arr = new T[len0][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, value);
            return arr;
        }
        public static T[][] NewArray<T>(int len0, int len1, Func<T> factory)
        {
            var arr = new T[len0][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, factory);
            return arr;
        }
        public static T[][][] NewArray<T>(int len0, int len1, int len2, T value) where T : struct
        {
            var arr = new T[len0][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, value);
            return arr;
        }
        public static T[][][] NewArray<T>(int len0, int len1, int len2, Func<T> factory)
        {
            var arr = new T[len0][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, factory);
            return arr;
        }
        public static T[][][][] NewArray<T>(int len0, int len1, int len2, int len3, T value) where T : struct
        {
            var arr = new T[len0][][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, len3, value);
            return arr;
        }
        public static T[][][][] NewArray<T>(int len0, int len1, int len2, int len3, Func<T> factory)
        {
            var arr = new T[len0][][][];
            for (int i = 0; i < arr.Length; i++) arr[i] = NewArray(len1, len2, len3, factory);
            return arr;
        }


        public static long Pow(long x, int y)
        {
            long res = 1;
            for (; y > 0; y >>= 1)
            {
                if ((y & 1) == 1) res *= x;
                x *= x;
            }
            return res;
        }
        public static BigInteger ParseBigInteger(ReadOnlySpan<char> s)
        {
            /* 自前実装の方が速い */
            BigInteger res;
            if (s.Length % 9 == 0)
                res = 0;
            else
            {
                res = new BigInteger(int.Parse(s.Slice(0, s.Length % 9)));
                s = s.Slice(s.Length % 9);
            }

            while (s.Length > 0)
            {
                var sp = s.Slice(0, 9);
                res *= 1000_000_000;
                res += int.Parse(sp);
                s = s.Slice(9);
            }
            return res;
        }
        public static int PopCount(int x) => BitOperations.PopCount((uint)x);
        public static int PopCount(long x) => BitOperations.PopCount((ulong)x);
        public static int PopCount(ulong x) => BitOperations.PopCount(x);
        public static int MSB(int x) => BitOperations.Log2((uint)x);
        public static int MSB(uint x) => BitOperations.Log2(x);
        public static int MSB(long x) => BitOperations.Log2((ulong)x);
        public static int MSB(ulong x) => BitOperations.Log2(x);
        public static int LSB(int x) => BitOperations.TrailingZeroCount((uint)x);
        public static int LSB(uint x) => BitOperations.TrailingZeroCount(x);
        public static int LSB(long x) => BitOperations.TrailingZeroCount((ulong)x);
        public static int LSB(ulong x) => BitOperations.TrailingZeroCount(x);

    }
    public static class Ext
    {

        public static bool UpdateMax<T>(this ref T r, T val) where T : struct, IComparable<T>
        {
            if (r.CompareTo(val) < 0) { r = val; return true; }
            return false;
        }
        public static bool UpdateMin<T>(this ref T r, T val) where T : struct, IComparable<T>
        {
            if (r.CompareTo(val) > 0) { r = val; return true; }
            return false;
        }
        public static long ToLong(this int i) => i;
        public static T[] Fill<T>(this T[] arr, T value)
        {
            Array.Fill(arr, value);
            return arr;
        }
        public static T[] Sort<T>(this T[] arr) { Array.Sort(arr); return arr; }
        public static string[] Sort(this string[] arr) => Sort(arr, StringComparer.OrdinalIgnoreCase);
        public static T[] Sort<T, U>(this T[] arr, Expression<Func<T, U>> selector) where U : IComparable<U> => Sort(arr, ExComparer<T>.CreateExp(selector));
        public static T[] Sort<T>(this T[] arr, Comparison<T> comparison) { Array.Sort(arr, comparison); return arr; }
        public static T[] Sort<T>(this T[] arr, IComparer<T> comparer) { Array.Sort(arr, comparer); return arr; }
        public static T[] Reverse<T>(this T[] arr) { Array.Reverse(arr); return arr; }
        public static (int index, T max) MaxBy<T>(this T[] arr) where T : IComparable<T>
        {
            T max = arr[0];
            int maxIndex = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (max.CompareTo(arr[i]) < 0)
                {
                    max = arr[i];
                    maxIndex = i;
                }
            }
            return (maxIndex, max);
        }
        public static (TSource item, TMax max) MaxBy<TSource, TMax>
            (this IEnumerable<TSource> source, Func<TSource, TMax> maxBySelector)
            where TMax : IComparable<TMax>
        {
            TMax max;
            TSource maxByItem;
            var e = source.GetEnumerator();
            e.MoveNext();
            maxByItem = e.Current;
            max = maxBySelector(maxByItem);
            while (e.MoveNext())
            {
                var item = e.Current;
                var next = maxBySelector(item);
                if (max.CompareTo(next) < 0)
                {
                    max = next;
                    maxByItem = item;
                }
            }
            return (maxByItem, max);
        }
        public static (int index, T min) MinBy<T>(this T[] arr) where T : IComparable<T>
        {
            T min = arr[0];
            int minIndex = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (min.CompareTo(arr[i]) > 0)
                {
                    min = arr[i];
                    minIndex = i;
                }
            }
            return (minIndex, min);
        }
        public static (TSource item, TMin min) MinBy<TSource, TMin>
            (this IEnumerable<TSource> source, Func<TSource, TMin> minBySelector)
            where TMin : IComparable<TMin>
        {
            TMin min;
            TSource minByItem;

            var e = source.GetEnumerator();
            e.MoveNext();
            minByItem = e.Current;
            min = minBySelector(minByItem);
            while (e.MoveNext())
            {
                var item = e.Current;
                var next = minBySelector(item);
                if (min.CompareTo(next) > 0)
                {
                    min = next;
                    minByItem = item;
                }
            }
            return (minByItem, min);
        }
        public static IComparer<T> Reverse<T>(this IComparer<T> comparer) => Comparer<T>.Create((x, y) => comparer.Compare(y, x));
        public static Dictionary<TKey, int> GroupCount<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.Count());
        public static Dictionary<TKey, int> GroupCount<TKey>(this IEnumerable<TKey> source) => source.GroupCount(i => i);
        public static Span<T> AsSpan<T>(this List<T> list, int start = 0) => Unsafe.As<Tuple<T[]>>(list).Item1.AsSpan(start, list.Count);
        public static ref T Get<T>(this T[] arr, int index)
        {
            if (index < 0)
                return ref arr[arr.Length + index];
            return ref arr[index];
        }
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            dic.TryGetValue(key, out var v);
            return v;
        }
        public static TValue GetOrInit<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (dic.TryGetValue(key, out var v))
                return v;
            return dic[key] = value;
        }
    }
    public static class ExComparer<T>
    {
        class ExpressionComparer<K> : IComparer<T> where K : IComparable<K>
        {
            private class ParameterReplaceVisitor : ExpressionVisitor
            {
                private readonly ParameterExpression from;
                private readonly ParameterExpression to;
                public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
                {
                    this.from = from;
                    this.to = to;
                }
                protected override Expression VisitParameter(ParameterExpression node) => node == from ? to : base.VisitParameter(node);
            }

            readonly Comparison<T> func;
            public ExpressionComparer(Expression<Func<T, K>> expression)
            {
                var paramA = expression.Parameters[0];
                var paramB = Expression.Parameter(typeof(T));
                var f2 = (Expression<Func<T, K>>)new ParameterReplaceVisitor(expression.Parameters[0], paramB).Visit(expression);
                var compExp = Expression.Lambda<Comparison<T>>(Expression.Call(
                        expression.Body,
                        typeof(K).GetMethod(nameof(IComparable<K>.CompareTo), new[] { typeof(K) }),
                        f2.Body),
                        paramA, paramB);
                this.func = compExp.Compile();
            }
            public int Compare(T x, T y) => func(x, y);
            public override bool Equals(object obj) => obj is ExpressionComparer<K> c && this.func == c.func;
            public override int GetHashCode() => func.GetHashCode();
        }
        class ReverseComparer : IComparer<T>
        {
            private static Comparer<T> orig = Comparer<T>.Default;
            public int Compare(T y, T x) => orig.Compare(x, y);
            public override bool Equals(object obj) => obj is ReverseComparer;
            public override int GetHashCode() => GetType().GetHashCode();
        }
        public static IComparer<T> CreateExp<K>(Expression<Func<T, K>> expression) where K : IComparable<K> => new ExpressionComparer<K>(expression);
        public static IComparer<T> DefaultReverse => new ReverseComparer();
    }
    public class ΔDebugView<T> { private IEnumerable<T> collection; public ΔDebugView(IEnumerable<T> collection) { this.collection = collection ?? throw new ArgumentNullException(nameof(collection)); }[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)] public T[] Items => collection.ToArray(); }


    [DebuggerStepThrough]
    public class ConsoleReader
    {
        const int BufSize = 1 << 12;
        private readonly byte[] buffer = new byte[BufSize];
        private readonly Stream input;
        private readonly Encoding encoding;
        private int pos = 0;
        private int len = 0;
        public ConsoleReader(Stream input, Encoding encoding) { this.input = input; this.encoding = encoding; }
        public ConsoleReader() : this(Console.OpenStandardInput(), Console.InputEncoding) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveNext() { if (++pos >= len) { len = input.Read(buffer, 0, buffer.Length); if (len == 0) { buffer[0] = 10; } pos = 0; } }

        public int Int
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int res = 0;
                bool neg = false;
                while (buffer[pos] < 48) { neg = buffer[pos] == 45; MoveNext(); }
                do { res = checked(res * 10 + (buffer[pos] ^ 48)); MoveNext(); } while (48 <= buffer[pos]);
                return neg ? -res : res;
            }
        }
        public int Int0 => this.Int - 1;
        public long Long
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                long res = 0;
                bool neg = false;
                while (buffer[pos] < 48) { neg = buffer[pos] == 45; MoveNext(); }
                do { res = res * 10 + (buffer[pos] ^ 48); MoveNext(); } while (48 <= buffer[pos]);
                return neg ? -res : res;
            }
        }
        public long Long0 => this.Long - 1;
        public string String
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var sb = new List<byte>();
                while (buffer[pos] <= 32) MoveNext();
                do { sb.Add(buffer[pos]); MoveNext(); } while (32 < buffer[pos]);
                return this.encoding.GetString(sb.ToArray());
            }
        }
        public string Ascii
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var sb = new StringBuilder();
                while (buffer[pos] <= 32) MoveNext();
                do { sb.Append((char)buffer[pos]); MoveNext(); } while (32 < buffer[pos]);
                return sb.ToString();
            }
        }
        public string Line
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var sb = new List<byte>();
                while (buffer[pos] <= 32) MoveNext();
                do { sb.Add(buffer[pos]); MoveNext(); } while (buffer[pos] != 10 && buffer[pos] != 13);
                return this.encoding.GetString(sb.ToArray());
            }
        }
        public char Char
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                while (buffer[pos] <= 32) MoveNext();
                char res = (char)buffer[pos];
                MoveNext();
                return res;
            }
        }
        public double Double => double.Parse(this.Ascii);

        [DebuggerStepThrough]
        public struct RepeatReader
        {
            ConsoleReader cr;
            int count;
            public RepeatReader(ConsoleReader cr, int count) { this.cr = cr; this.count = count; }
            public T[] Select<T>(Func<ConsoleReader, T> factory) { var arr = new T[count]; for (var i = 0; i < count; i++) arr[i] = factory(cr); return arr; }
            public T[] Select<T>(Func<ConsoleReader, int, T> factory) { var arr = new T[count]; for (var i = 0; i < count; i++) arr[i] = factory(cr, i); return arr; }
            public string[] Line { get { var arr = new string[count]; for (var i = 0; i < count; i++) arr[i] = cr.Line; return arr; } }
            public string[] String { get { var arr = new string[count]; for (var i = 0; i < count; i++) arr[i] = cr.String; return arr; } }
            public string[] Ascii { get { var arr = new string[count]; for (var i = 0; i < count; i++) arr[i] = cr.Ascii; return arr; } }
            public int[] Int { get { var arr = new int[count]; for (var i = 0; i < count; i++) arr[i] = cr.Int; return arr; } }
            public int[] Int0 { get { var arr = new int[count]; for (var i = 0; i < count; i++) arr[i] = cr.Int0; return arr; } }
            public long[] Long { get { var arr = new long[count]; for (var i = 0; i < count; i++) arr[i] = cr.Long; return arr; } }
            public long[] Long0 { get { var arr = new long[count]; for (var i = 0; i < count; i++) arr[i] = cr.Long0; return arr; } }
            public double[] Double { get { var arr = new double[count]; for (var i = 0; i < count; i++) arr[i] = cr.Double; return arr; } }

            public static implicit operator int[](RepeatReader rr) => rr.Int;
            public static implicit operator long[](RepeatReader rr) => rr.Long;
            public static implicit operator double[](RepeatReader rr) => rr.Double;
            public static implicit operator string[](RepeatReader rr) => rr.Ascii;
        }
        public RepeatReader Repeat(int count) => new RepeatReader(this, count);

        [DebuggerStepThrough]
        public struct SplitReader
        {
            ConsoleReader cr;
            public SplitReader(ConsoleReader cr) { this.cr = cr; }
            public string[] String { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<string>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.String); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public string[] Ascii { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<string>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Ascii); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public int[] Int { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<int>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Int); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public int[] Int0 { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<int>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Int0); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public long[] Long { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<long>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Long); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public long[] Long0 { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<long>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Long0); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public double[] Double { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<double>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Double); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }

            public static implicit operator int[](SplitReader sr) => sr.Int;
            public static implicit operator long[](SplitReader sr) => sr.Long;
            public static implicit operator double[](SplitReader sr) => sr.Double;
            public static implicit operator string[](SplitReader sr) => sr.Ascii;
        }
        public SplitReader Split => new SplitReader(this);

        public static implicit operator int(ConsoleReader cr) => cr.Int;
        public static implicit operator long(ConsoleReader cr) => cr.Long;
        public static implicit operator double(ConsoleReader cr) => cr.Double;
        public static implicit operator string(ConsoleReader cr) => cr.Ascii;
    }
    [DebuggerStepThrough]
    public class ConsoleWriter
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] public readonly StreamWriter sw;
        public ConsoleWriter() : this(Console.OpenStandardOutput(), Console.OutputEncoding) { }
        public ConsoleWriter(Stream output, Encoding encoding) { sw = new StreamWriter(output, encoding); }
        public void Flush() => sw.Flush();
        public ConsoleWriter WriteLine(ReadOnlySpan<char> obj) { sw.WriteLine(obj); return this; }
        public ConsoleWriter WriteLine<T>(T obj) { sw.WriteLine(obj.ToString()); return this; }
        public ConsoleWriter WriteLineJoin<T>(ReadOnlySpan<T> col) => WriteMany(' ', col);
        public ConsoleWriter WriteLineJoin<T>(IEnumerable<T> col) => WriteMany(' ', col);
        public ConsoleWriter WriteLines<T>(ReadOnlySpan<T> col) => WriteMany('\n', col);
        public ConsoleWriter WriteLines<T>(IEnumerable<T> col) => WriteMany('\n', col);
        public ConsoleWriter WriteLineGrid<T>(IEnumerable<IEnumerable<T>> cols)
        {
            var en = cols.GetEnumerator();
            while (en.MoveNext())
                WriteLineJoin(en.Current);
            return this;
        }
        private ConsoleWriter WriteMany<T>(char sep, ReadOnlySpan<T> col)
        {
            var en = col.GetEnumerator();
            if (!en.MoveNext())
                return this;
            sw.Write(en.Current.ToString());
            while (en.MoveNext())
            {
                sw.Write(sep);
                sw.Write(en.Current.ToString());
            }
            sw.WriteLine();
            return this;
        }
        private ConsoleWriter WriteMany<T>(char sep, IEnumerable<T> col)
        {
            var en = col.GetEnumerator();
            if (!en.MoveNext())
                return this;
            sw.Write(en.Current.ToString());
            while (en.MoveNext())
            {
                sw.Write(sep);
                sw.Write(en.Current.ToString());
            }
            sw.WriteLine();
            return this;
        }
    }
}
public partial class Program
{
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public ConsoleReader cr;
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public ConsoleWriter cw;
    public Program(ConsoleReader reader, ConsoleWriter writer) { this.cr = reader; this.cw = writer; System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture; }
    static void Main() => new Program(new ConsoleReader(), new ConsoleWriter()).Run();
    public void Run()
    {
        var res = Calc();
        if (res is double)
            cw.WriteLine(Result((double)res));
        else if (res is bool)
            cw.WriteLine(Result((bool)res));
        else if (res != null)
            cw.WriteLine(res.ToString());
        cw.Flush();
    }
}
public partial class Program
{
    public static string Result(double d) => d.ToString("0.####################", System.Globalization.CultureInfo.InvariantCulture);
    #endregion
    public static string Result(bool b) => b ? "Yes" : "No";
    private object Calc()
    {
        N = cr;

        return null;
    }
    int N;
}
