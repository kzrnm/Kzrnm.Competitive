using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitArray = System.Collections.BitArray;
using BigInteger = System.Numerics.BigInteger;
using TextReader = System.IO.TextReader;

#pragma warning disable

namespace AtCoderProject.Hide
{
    struct Point : IEquatable<Point>, IComparable<Point>
    {
        public int x;
        public int y;
        public Point(int[] arr) : this(arr[0], arr[1]) { }
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int Inner(Point other) => x * other.x + y * other.y;
        public int Cross(Point other) => x * other.y - y * other.x;
        public static Point operator +(Point a, Point b) => new Point(a.x + b.x, a.y + b.y);
        public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y - b.y);

        public int CompareTo(Point other) => this.x.CompareTo(other.x);

        public bool Equals(Point other) => this.x == other.x && this.y == other.y;
        public override bool Equals(object obj)
        {
            if (obj is Point) return Equals((Point)obj);
            return false;
        }
        public override int GetHashCode() => ((x << 5) + x) ^ y;
        public override string ToString() => $"({x}, {y})";
    }
    struct Status : IEquatable<Status>, IComparable<Status>
    {
        public enum Type
        {
            End,
            Start,
        }
        public int i;
        public Type t;
        public Status(int i, Type t)
        {
            this.i = i;
            this.t = t;
        }

        public int CompareTo(Status other) => this.i.CompareTo(other.i);

        public bool Equals(Status other) => this.i == other.i && this.t == other.t;
        public override bool Equals(object obj)
        {
            if (obj is Status) return Equals((Status)obj);
            return false;
        }
        public override int GetHashCode() => i ^ ((int)t << 30);
        public override string ToString() => $"({i}, {t})";
    }
    class Sums
    {
        private int[] impl;
        public int Length { get; }
        public Sums(int[] arr)
        {
            this.Length = arr.Length;
            impl = new int[arr.Length + 1];
            for (int i = 0; i < arr.Length; i++)
                impl[i + 1] = impl[i] + arr[i];
        }
        public int this[int toExclusive]
        {
            get { return impl[toExclusive]; }
        }
        public int this[int from, int toExclusive]
        {
            get { return impl[toExclusive] - impl[from]; }
        }
    }
    class Sums2D
    {
        private int[,] impl;
        public int Length1 { get; }
        public int Length2 { get; }
        public Sums2D(int[][] arr)
        {
            this.Length1 = arr.Length;
            this.Length2 = arr[0].Length;
            impl = new int[Length1 + 1, Length2 + 1];
            for (int i = 0; i < Length1; i++)
                for (int j = 0; j < Length2; j++)
                    impl[i + 1, j + 1] = impl[i + 1, j] + impl[i, j + 1] - impl[i, j] + arr[i][j];
        }
        public Sums2D(int[,] arr)
        {
            this.Length1 = arr.GetLength(0);
            this.Length2 = arr.GetLength(1);
            impl = new int[Length1 + 1, Length2 + 1];
            for (int i = 0; i < Length1; i++)
                for (int j = 0; j < Length2; j++)
                    impl[i + 1, j + 1] = impl[i + 1, j] + impl[i, j + 1] - impl[i, j] + arr[i, j];
        }
        public int this[int left, int rightExclusive, int top, int bottomExclusive]
        {
            get
            {
                return impl[rightExclusive, bottomExclusive]
                  - impl[left, bottomExclusive]
                  - impl[rightExclusive, top]
                  + impl[left, top];
            }
        }
    }

    class Matrix
    {
        long[,] Pow(long[,] mat, int y)
        {
            var K = mat.GetLength(0);
            long[,] res = new long[K, K];
            for (var i = 0; i < K; i++)
                res[i, i] = 1;
            for (; y > 0; y >>= 1)
            {
                if ((y & 1) == 1) res = Mul(res, mat);
                mat = Mul(mat, mat);
            }
            return res;
        }
        long[,] Mul(long[,] l, long[,] r)
        {
            var h = l.GetLength(0);
            var w = r.GetLength(1);
            var K = l.GetLength(1);

            long[,] res = new long[K, K];
            for (var i = 0; i < K; i++)
                for (var j = 0; j < K; j++)
                    for (var k = 0; k < K; k++)
                        res[i, j] += l[i, k] * r[k, j];
            return res;
        }
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
    public static class Utility
    {
        public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
            => Comparer<T>.Create((x, y) => comparer.Compare(y, x));
    }
 }
