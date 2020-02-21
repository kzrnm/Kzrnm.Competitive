using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitArray = System.Collections.BitArray;
using BigInteger = System.Numerics.BigInteger;
using TextReader = System.IO.TextReader;
using static Global;


namespace AtCoderProject.Hide
{
    readonly struct Point : IEquatable<Point>, IComparable<Point>
    {
        public readonly int x;
        public readonly int y;
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
            if (obj is Point p) return Equals(p);
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
            if (obj is Status s) return Equals(s);
            return false;
        }
        public override int GetHashCode() => i ^ ((int)t << 30);
        public override string ToString() => $"({i}, {t})";
    }
    class Sums
    {
        private long[] impl;
        public int Length { get; }
        public Sums(int[] arr)
        {
            this.Length = arr.Length;
            impl = new long[arr.Length + 1];
            for (var i = 0; i < arr.Length; i++)
                impl[i + 1] = impl[i] + arr[i];
        }
        public long this[int toExclusive] => impl[toExclusive];
        public long this[int from, int toExclusive] => impl[toExclusive] - impl[from];
    }
    class Sums2D
    {
        private long[][] impl;
        public int Length1 { get; }
        public int Length2 { get; }
        public Sums2D(int[][] arr)
        {
            this.Length1 = arr.Length;
            this.Length2 = arr[0].Length;
            impl = NewArray(Length1 + 1, Length2 + 1, 0L);
            for (var i = 0; i < Length1; i++)
                for (var j = 0; j < Length2; j++)
                    impl[i + 1][j + 1] = impl[i + 1][j] + impl[i][j + 1] - impl[i][j] + arr[i][j];
        }
        public long this[int left, int rightExclusive, int top, int bottomExclusive]
            => impl[rightExclusive][bottomExclusive]
                  - impl[left][bottomExclusive]
                  - impl[rightExclusive][top]
                  + impl[left][top];
    }

    class Matrix
    {
        long[][] Pow(long[][] mat, int y)
        {
            var K = mat.Length;
            long[][] res = NewArray(K, K, 0L);
            for (var i = 0; i < res.Length; i++)
                res[i][i] = 1;
            for (; y > 0; y >>= 1)
            {
                if ((y & 1) == 1) res = Mul(res, mat);
                mat = Mul(mat, mat);
            }
            return res;
        }
        long[][] Mul(long[][] l, long[][] r)
        {
            var K = l[0].Length;
            long[][] res = NewArray(K, K, 0L);
            for (var i = 0; i < res.Length; i++)
                for (var j = 0; j < res.Length; j++)
                    for (var k = 0; k < res.Length; k++)
                        res[i][j] += l[i][k] * r[k][j];
            return res;
        }
    }
}
