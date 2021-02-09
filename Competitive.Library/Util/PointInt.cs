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
            => Math.Atan2(this.y, this.x).CompareTo(Math.Atan2(other.y, other.x));

        public bool Equals(PointInt other) => this.x == other.x && this.y == other.y;
        public override bool Equals(object obj) => obj is PointInt p && this.Equals(p);
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"{x} {y}";

        public static bool operator ==(PointInt left, PointInt right) => left.Equals(right);
        public static bool operator !=(PointInt left, PointInt right) => !left.Equals(right);

        /// <summary>
        /// 凸包(一番外側の多角形)を求める
        /// </summary>
        public static int[] ConvexHull(PointInt[] points)
        {
            static int Cross(long x1, long y1, long x2, long y2) => Math.Sign(x1 * y2 - y1 * x2);

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
                    && Cross(upper[^1].x - upper[^2].x, upper[^1].y - upper[^2].y, pts[i].x - upper[^2].x, pts[i].y - upper[^2].y) > 0)
                {
                    upper.RemoveLast();
                }
                upper.Add(pts[i]);
                while (lower.Count > 1
                    && Cross(lower[^1].x - lower[^2].x, lower[^1].y - lower[^2].y, pts[i].x - lower[^2].x, pts[i].y - lower[^2].y) < 0)
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
    }
}
