#region いつもの
using System;
using System.Collections.Generic;
using System.Linq;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;
using BigInteger = System.Numerics.BigInteger;
using TextReader = System.IO.TextReader;
using static Global;
using static NumGlobal;

static class Global
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
    private class ComparerReverseImpl<T> : Comparer<T> where T : IComparable<T> { public override int Compare(T y, T x) => x.CompareTo(y); public override bool Equals(object obj) => obj != null && GetType() == obj.GetType(); public override int GetHashCode() => GetType().GetHashCode(); }
    public static IComparer<T> ComparerReverse<T>() where T : IComparable<T> => new ComparerReverseImpl<T>();
    public static string AllLines<T>(IEnumerable<T> source) => string.Join("\n", source);
    public static string AllJoin<T>(IEnumerable<T> source) => string.Join(" ", source);
}
static class NumGlobal
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
        // MonoのBigInteger.Parseが遅いので自前実装
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
    public static T[] Fill<T>(this T[] arr, T value)
    {
        for (var i = 0; i < arr.Length; i++) arr[i] = value;
        return arr;
    }
    public static T[] Sort<T>(this T[] arr)
    {
        Array.Sort(arr);
        return arr;
    }
    public static string[] Sort(this string[] arr)
    {
        Array.Sort(arr, StringComparer.OrdinalIgnoreCase);
        return arr;
    }
    public static T[] Reverse<T>(this T[] arr)
    {
        Array.Reverse(arr);
        return arr;
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
    public static IComparer<T> Reverse<T>(this IComparer<T> comparer) => Comparer<T>.Create((x, y) => comparer.Compare(y, x));
    public static Dictionary<TKey, int> GroupCount<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.Count());
    public static Dictionary<TKey, int> GroupCount<TKey>(this IEnumerable<TKey> source) => source.GroupCount(i => i);
}
#pragma warning disable CA1819
[System.Diagnostics.DebuggerNonUserCode]
public class ConsoleReader { private string[] ReadLineSplit() => textReader.ReadLine().Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries); private string[] line = Array.Empty<string>(); private int linePosition; private TextReader textReader; public ConsoleReader(TextReader tr) { textReader = tr; } public int Int => int.Parse(String); public int Int0 => Int - 1; public long Long => long.Parse(String); public double Double => double.Parse(String); public string String { get { if (linePosition >= line.Length) { linePosition = 0; line = ReadLineSplit(); } return line[linePosition++]; } } public class SplitLine { public SplitLine(ConsoleReader cr) { String = cr.ReadLineSplit(); cr.line = Array.Empty<string>(); } public int[] Int => String.Select(x => int.Parse(x)).ToArray(); public int[] Int0 => String.Select(x => int.Parse(x) - 1).ToArray(); public long[] Long => String.Select(x => long.Parse(x)).ToArray(); public double[] Double => String.Select(x => double.Parse(x)).ToArray(); public string[] String { get; } } public SplitLine Split => new SplitLine(this); public class RepeatReader { ConsoleReader cr; int count; public RepeatReader(ConsoleReader cr, int count) { this.cr = cr; this.count = count; } public T[] Select<T>(Func<ConsoleReader, T> factory) { var arr = new T[count]; for (int i = 0; i < count; i++) arr[i] = factory(cr); return arr; } public string[] String => this.Select(cr => cr.String); public int[] Int => this.Select(cr => cr.Int); public int[] Int0 => this.Select(cr => cr.Int - 1); public long[] Long => this.Select(cr => cr.Long); public double[] Double => this.Select(cr => cr.Double); } public RepeatReader Repeat(int count) => new RepeatReader(this, count); }
public class Program
{
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] private ConsoleReader cr;

    public Program(ConsoleReader consoleReader) { this.cr = consoleReader; }
    static void Main(string[] args) => Console.WriteLine(new Program(new ConsoleReader(args.Length > 0 ? new System.IO.StreamReader(args[0]) : Console.In)).Calc());

    #endregion
    public object Calc()
    {
        var N = cr.Int;

        var arrs = cr.Repeat(N).Select(cr => cr.Split.Int);

        return N;
    }
}
