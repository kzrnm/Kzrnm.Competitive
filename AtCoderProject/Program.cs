#region いつもの
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleReader = AtCoderProject.Reader.ConsoleReader;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;
using BigInteger = System.Numerics.BigInteger;
using TextReader = System.IO.TextReader;
using StringBuilder = System.Text.StringBuilder;
using static AtCoderProject.Global;
using static AtCoderProject.NumGlobal;

namespace AtCoderProject
{
    public static class Global
    {
        public static void Swap<T>(ref T a, ref T b) { T tmp = a; a = b; b = tmp; }
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
        private class ComparerReverseImpl<T> : Comparer<T> where T : IComparable<T> { public override int Compare(T y, T x) => x.CompareTo(y); public override bool Equals(object obj) => obj != null && GetType() == obj.GetType(); public override int GetHashCode() => GetType().GetHashCode(); }
        public static IComparer<T> ComparerReverse<T>() where T : IComparable<T> => new ComparerReverseImpl<T>();
        public static string AllLines<T>(IEnumerable<T> source) => string.Join("\n", source);
        public static string AllJoin<T>(IEnumerable<T> source) => string.Join(" ", source);
    }
    public static class NumGlobal
    {
        public static int Pow(int x, int y)
        {
            int res = 1;
            for (; y > 0; y >>= 1)
            {
                if ((y & 1) == 1) res *= x;
                x *= x;
            }
            return res;
        }
        public static BigInteger ParseBigInteger(string s)
        {
            // 自前実装の方が速い
            var res = BigInteger.Zero;
            var splited = new string[(s.Length + 7) / 8];
            for (var i = 0; i < splited.Length - 1; i++)
            {
                splited[i] = s.Substring(8 * i, 8);
            }
            splited[splited.Length - 1] = s.Substring(8 * (splited.Length - 1));
            foreach (var sp in splited)
            {
                res *= (int)Math.Pow(10, sp.Length);
                res += int.Parse(sp);
            }
            return res;
        }
        public static int BitCount(int x) { x -= (x >> 1) & 0x55555555; x = (x & 0x33333333) + ((x >> 2) & 0x33333333); x = (x + (x >> 4)) & 0x0f0f0f0f; x += x >> 8; x += x >> 16; return x & 0x3f; }
        public static int BitCount(long x) { x -= (x >> 1) & 0x5555555555555555; x = (x & 0x3333333333333333) + ((x >> 2) & 0x3333333333333333); x = (x + (x >> 4)) & 0x0f0f0f0f0f0f0f0f; x += x >> 8; x += x >> 16; x += x >> 32; return (int)(x & 0x0000007f); }
        public static int MSB(int x) { x |= x >> 1; x |= x >> 2; x |= x >> 4; x |= x >> 8; x |= x >> 16; return BitCount(x) - 1; }
        public static int MSB(long x) { x |= x >> 1; x |= x >> 2; x |= x >> 4; x |= x >> 8; x |= x >> 16; x |= x >> 32; return BitCount(x) - 1; }
        public static int LSB(int x) { x |= x << 1; x |= x << 2; x |= x << 4; x |= x << 8; x |= x << 16; return 32 - BitCount(x); }
        public static int LSB(long x) { x |= x << 1; x |= x << 2; x |= x << 4; x |= x << 8; x |= x << 16; x |= x << 32; return 64 - BitCount(x); }
    }
    static class Ext
    {
        public static long ToLong(this int i) => i;
        public static T[] Fill<T>(this T[] arr, T value)
        {
            for (var i = 0; i < arr.Length; i++) arr[i] = value;
            return arr;
        }
        public static T[] Sort<T>(this T[] arr) { Array.Sort(arr); return arr; }
        public static string[] Sort(this string[] arr) => Sort(arr, StringComparer.OrdinalIgnoreCase);
        public static T[] Sort<T, U>(this T[] arr, Func<T, U> selector) where U : IComparable<U> => Sort(arr, (a, b) => selector(a).CompareTo(selector(b)));
        public static T[] Sort<T>(this T[] arr, Comparison<T> comparison) { Array.Sort(arr, comparison); return arr; }
        public static T[] Sort<T>(this T[] arr, IComparer<T> comparer) { Array.Sort(arr, comparer); return arr; }
        public static T[] Reverse<T>(this T[] arr) { Array.Reverse(arr); return arr; }
        public static Tuple<int, T> MaxBy<T>(this T[] arr) where T : IComparable<T>
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
            return Tuple.Create(maxIndex, max);
        }
        public static Tuple<TSource, TMax> MaxBy<TSource, TMax>
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
            return Tuple.Create(maxByItem, max);
        }
        public static Tuple<int, T> MinBy<T>(this T[] arr) where T : IComparable<T>
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
            return Tuple.Create(minIndex, min);
        }
        public static Tuple<TSource, TMin> MinBy<TSource, TMin>
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
            return Tuple.Create(minByItem, min);
        }
        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource element)
        {
            foreach (var item in source)
                yield return item;
            yield return element;
        }
        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource element)
        {
            yield return element;
            foreach (var item in source)
                yield return item;
        }
        public static IComparer<T> Reverse<T>(this IComparer<T> comparer) => Comparer<T>.Create((x, y) => comparer.Compare(y, x));
        public static Dictionary<TKey, int> GroupCount<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.Count());
        public static Dictionary<TKey, int> GroupCount<TKey>(this IEnumerable<TKey> source) => source.GroupCount(i => i);
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            TValue v;
            dic.TryGetValue(key, out v);
            return v;
        }
        public static TValue GetOrInit<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            TValue v;
            if (dic.TryGetValue(key, out v))
                return v;
            return dic[key] = value;
        }
    }
    public class ΔDebugView<T> { private IEnumerable<T> collection; public ΔDebugView(IEnumerable<T> collection) { if (collection == null) throw new ArgumentNullException(nameof(collection)); this.collection = collection; }[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)] public T[] Items => collection.ToArray(); }
}
namespace AtCoderProject.Reader
{
    using System.IO;
    using System.Text;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
#pragma warning disable CA1819
    [DebuggerNonUserCode]
    public class ConsoleReader
    {
        const int BufSize = 1 << 12;
        private readonly byte[] buffer = new byte[BufSize];
        private readonly Stream input;
        private readonly Encoding encoding;
        private int pos = 0;
        private int len = 0;
        public ConsoleReader(Stream input, Encoding encoding) { this.input = input; this.encoding = encoding; }
        public ConsoleReader(Stream input) : this(input, Encoding.UTF8) { }
        public ConsoleReader(string text) : this(new MemoryStream(Encoding.UTF8.GetBytes(text))) { }
        private void MoveNext() { if (++pos >= len) { len = input.Read(buffer, 0, buffer.Length); if (len == 0) { buffer[0] = 10; } pos = 0; } }

        public int Int
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int res = 0;
                bool neg = false;
                while (buffer[pos] < 48) { neg = buffer[pos] == 45; MoveNext(); }
                do { res = res * 10 + (buffer[pos] ^ 48); MoveNext(); } while (48 <= buffer[pos]);
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
                while (buffer[pos] < 32) MoveNext();
                do { sb.Add(buffer[pos]); MoveNext(); } while (buffer[pos] != 10 && buffer[pos] != 13);
                return this.encoding.GetString(sb.ToArray());
            }
        }
        public double Double => double.Parse(this.Ascii);

        [DebuggerNonUserCode]
        public struct RepeatReader
        {
            ConsoleReader cr;
            int count;
            public RepeatReader(ConsoleReader cr, int count) { this.cr = cr; this.count = count; }
            public T[] Select<T>(Func<ConsoleReader, T> factory) { var arr = new T[count]; for (var i = 0; i < count; i++) arr[i] = factory(cr); return arr; }
            public string[] Line { get { var arr = new string[count]; for (var i = 0; i < count; i++) arr[i] = cr.Line; return arr; } }
            public string[] String { get { var arr = new string[count]; for (var i = 0; i < count; i++) arr[i] = cr.String; return arr; } }
            public string[] Ascii { get { var arr = new string[count]; for (var i = 0; i < count; i++) arr[i] = cr.Ascii; return arr; } }
            public int[] Int { get { var arr = new int[count]; for (var i = 0; i < count; i++) arr[i] = cr.Int; return arr; } }
            public int[] Int0 { get { var arr = new int[count]; for (var i = 0; i < count; i++) arr[i] = cr.Int0; return arr; } }
            public long[] Long { get { var arr = new long[count]; for (var i = 0; i < count; i++) arr[i] = cr.Long; return arr; } }
            public long[] Long0 { get { var arr = new long[count]; for (var i = 0; i < count; i++) arr[i] = cr.Long0; return arr; } }
            public double[] Double { get { var arr = new double[count]; for (var i = 0; i < count; i++) arr[i] = cr.Double; return arr; } }
        }
        public RepeatReader Repeat(int count) => new RepeatReader(this, count);

        [DebuggerNonUserCode]
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
        }
        public SplitReader Split => new SplitReader(this);
    }
}
public class Program
{
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] private ConsoleReader cr;

    public Program(ConsoleReader consoleReader) { this.cr = consoleReader; }
    static void Main() => Console.WriteLine(new Program(new ConsoleReader(Console.OpenStandardInput())).Result());
    #endregion
    public string Result() { var obj = Calc(); if (obj is bool) return (bool)obj ? "Yes" : "No"; if (obj is double) return ((double)obj).ToString("0.####################"); return obj.ToString(); }
    private object Calc()
    {
        var N = cr.Int;
        var arr = cr.Split.Int;
        return N;
    }
}