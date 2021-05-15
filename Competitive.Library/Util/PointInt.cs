using AtCoder.Internal;
using System;

namespace Kzrnm.Competitive
{

    public readonly struct PointInt : IEquatable<PointInt>, IComparable<PointInt>
    {
        public readonly int x;
        public readonly int y;
        public PointInt(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void Deconstruct(out int v1, out int v2) { v1 = x; v2 = y; }
        public static implicit operator PointInt((int x, int y) tuple) => new PointInt(tuple.x, tuple.y);
        public double Distance(PointInt other) => Math.Sqrt(Distance2(other));
        public long Distance2(PointInt other)
        {
            var p = other - this;
            return (long)p.x * p.x + (long)p.y * p.y;
        }
        /// <summary>
        /// 内積
        /// </summary>
        public long Inner(PointInt other) => (long)x * other.x + (long)y * other.y;
        /// <summary>
        /// 外積
        /// </summary>
        public long Cross(PointInt other) => (long)x * other.y - (long)y * other.x;
        public static PointInt operator +(PointInt a, PointInt b) => new PointInt(a.x + b.x, a.y + b.y);
        public static PointInt operator -(PointInt a, PointInt b) => new PointInt(a.x - b.x, a.y - b.y);
        public int CompareTo(PointInt other)
        {
            var sy = Math.Sign(y);
            var syo = Math.Sign(other.y);

            if (sy == 0)
            {
                var sx = Math.Sign(x);
                var sxo = Math.Sign(other.x);
                if (syo == 0)
                {
                    if (sx == sxo)
                        return Math.Abs(x).CompareTo(Math.Abs(other.x));
                    if (sx == 0) return -1;
                    if (sxo == 0) return 1;
                    return sxo.CompareTo(sx);
                }
                if (sx == 0) return -1;
                if (sx < 0) return 1;
                return 0.CompareTo(syo);
            }
            if (syo == 0)
            {
                var sxo = Math.Sign(other.x);
                if (sxo == 0) return 1;
                if (sxo < 0) return -1;
                return sy.CompareTo(0);
            }

            var ycmp = sy.CompareTo(syo);
            if (ycmp == 0)
            {
                var cr = CrossSign(other.x, other.y, x, y);
                if (cr == 0)
                    return this.Distance2(default).CompareTo(other.Distance2(default));
                return cr;
            }
            return ycmp;
        }

        public bool Equals(PointInt other) => this.x == other.x && this.y == other.y;
        public override bool Equals(object obj) => obj is PointInt p && this.Equals(p);
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"{x} {y}";

        public static bool operator ==(PointInt left, PointInt right) => left.Equals(right);
        public static bool operator !=(PointInt left, PointInt right) => !left.Equals(right);

        static int CrossSign(long x1, long y1, long x2, long y2) => Math.Sign(x1 * y2 - y1 * x2);

        /// <summary>
        /// 凸包(一番外側の多角形)を求める
        /// </summary>
        public static int[] ConvexHull(PointInt[] points)
        {
            Contract.Assert(points.Length >= 3);
            var pts = new (int x, int y, int ix)[points.Length];
            for (int i = 0; i < points.Length; i++)
                pts[i] = (points[i].x, points[i].y, i);
            Array.Sort(pts);

            var upper = new SimpleList<(int x, int y, int ix)> { pts[0], pts[1] };
            var lower = new SimpleList<(int x, int y, int ix)> { pts[0], pts[1] };
            for (int i = 2; i < pts.Length; i++)
            {
                while (upper.Count > 1
                    && CrossSign(upper[^1].x - upper[^2].x, upper[^1].y - upper[^2].y, pts[i].x - upper[^2].x, pts[i].y - upper[^2].y) > 0)
                {
                    upper.RemoveLast();
                }
                upper.Add(pts[i]);
                while (lower.Count > 1
                    && CrossSign(lower[^1].x - lower[^2].x, lower[^1].y - lower[^2].y, pts[i].x - lower[^2].x, pts[i].y - lower[^2].y) < 0)
                {
                    lower.RemoveLast();
                }
                lower.Add(pts[i]);
            }

            var res = new int[upper.Count + lower.Count - 2];
            var span = upper.AsSpan();
            for (int i = 0; i < span.Length; i++)
                res[res.Length - i - 1] = span[i].ix;
            span = lower.AsSpan()[1..^1];
            for (int i = 0; i < span.Length; i++)
                res[i] = span[i].ix;
            return res;
        }
        /// <summary>
        /// 多角形の面積を求める
        /// </summary>
        public static double Area(PointInt[] points) => Area2(points) / 2.0;

        /// <summary>
        /// 多角形の面積×2を求める
        /// </summary>
        public static long Area2(PointInt[] points)
        {
            Contract.Assert(points.Length >= 3);
            long res = ((long)points[^1].x - points[0].x) * ((long)points[^1].y + points[0].y);
            for (int i = 1; i < points.Length; i++)
            {
                var p1 = points[i - 1];
                var p2 = points[i];
                res += ((long)p1.x - p2.x) * ((long)p1.y + p2.y);
            }
            return Math.Abs(res);
        }
    }
}
