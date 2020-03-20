using System;
using System.Collections.Generic;
using System.Linq;
using static Global;
using static NumGlobal;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;
using BigInteger = System.Numerics.BigInteger;


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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void Deconstruct(out int v1, out int v2) { v1 = x; v2 = y; }
        public static implicit operator Point(Tuple<int, int> tuple) => new Point(tuple.Item1, tuple.Item2);
        public double Distance(Point other) => Math.Sqrt(Distance2(other));
        private long Distance2(Point other)
        {
            var p = other - this;
            return (long)p.x * p.x + (long)p.y * p.y;
        }
        public long Inner(Point other) => (long)x * other.x + (long)y * other.y;
        public long Cross(Point other) => (long)x * other.y - (long)y * other.x;
        public static Point operator +(Point a, Point b) => new Point(a.x + b.x, a.y + b.y);
        public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y - b.y);
        public int CompareTo(Point other)
        {
            var xd = this.x.CompareTo(other.x);
            if (xd != 0) return xd;
            return this.y.CompareTo(other.y);
        }

        public bool Equals(Point other) => this.x == other.x && this.y == other.y;
        public override bool Equals(object obj) => obj is Point && Equals((Point)obj);
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
        public override bool Equals(object obj) => obj is Status && Equals((Status)obj);
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
        public long this[int toExclusive]
        {
            get { return impl[toExclusive]; }
        }
        public long this[int from, int toExclusive]
        {
            get { return impl[toExclusive] - impl[from]; }
        }
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
        {
            get
            {
                return impl[rightExclusive][bottomExclusive]
                  - impl[left][bottomExclusive]
                  - impl[rightExclusive][top]
                  + impl[left][top];
            }
        }
    }

    struct BitArray : IEquatable<BitArray>, IEnumerable<bool>
    {
        private readonly long num;
        public BitArray(long num) { this.num = num; }
        public bool this[int index] => ((num >> index) & 1) != 0;

        public static BitArray operator &(BitArray bits, long r) => new BitArray(bits.num & r);
        public static BitArray operator |(BitArray bits, long r) => new BitArray(bits.num | r);
        public static BitArray operator ^(BitArray bits, long r) => new BitArray(bits.num ^ r);
        public static BitArray operator +(BitArray bits, long r) => new BitArray(bits.num + r);
        public static BitArray operator -(BitArray bits, long r) => new BitArray(bits.num - r);
        public static implicit operator BitArray(long num) => new BitArray(num);
        public static implicit operator long(BitArray bits) => bits.num;

        public override string ToString() => Convert.ToString(num, 2).PadLeft(sizeof(long) * 8, '0');
        public bool Equals(BitArray other) => this.num == other.num;
        public override bool Equals(object obj) => obj is BitArray && Equals((BitArray)obj);
        public override int GetHashCode() => this.num.GetHashCode();
        public IEnumerable<int> Bits()
        {
            var msb = MSB(this) + 1;
            for (var i = 0; i < msb; i++)
                if (((num >> i) & 1) == 1)
                    yield return i;
        }
        public IEnumerator<bool> GetEnumerator()
        {
            const int len = sizeof(long) * 8;
            for (var i = 0; i < len; i++)
                yield return ((num >> i) & 1) == 1;
        }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    class Matrix
    {
        public long[][] Pow(long[][] mat, int y)
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
        public long[][] Mul(long[][] l, long[][] r)
        {
            var h = l.Length;
            var w = r[0].Length;
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
