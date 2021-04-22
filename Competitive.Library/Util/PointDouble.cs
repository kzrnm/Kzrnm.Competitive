using AtCoder.Internal;
using System;
using System.Net.Http.Headers;

namespace Kzrnm.Competitive
{

    public readonly struct PointDouble : IEquatable<PointDouble>, IComparable<PointDouble>
    {
        public readonly double x;
        public readonly double y;
        public PointDouble(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void Deconstruct(out double v1, out double v2) { v1 = x; v2 = y; }
        public static implicit operator PointDouble((double x, double y) tuple) => new PointDouble(tuple.x, tuple.y);
        public double Distance(PointDouble other) => Math.Sqrt(Distance2(other));
        public double Distance2(PointDouble other)
        {
            var p = other - this;
            return p.x * p.x + p.y * p.y;
        }
        /// <summary>
        /// 内積
        /// </summary>
        public double Inner(PointDouble other) => x * other.x + y * other.y;
        /// <summary>
        /// 外積
        /// </summary>
        public double Cross(PointDouble other) => x * other.y - y * other.x;
        public static PointDouble operator +(PointDouble a, PointDouble b) => new PointDouble(a.x + b.x, a.y + b.y);
        public static PointDouble operator -(PointDouble a, PointDouble b) => new PointDouble(a.x - b.x, a.y - b.y);
        public int CompareTo(PointDouble other)
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
        public bool IsNaN => double.IsNaN(x) || double.IsNaN(y);

        public bool Equals(PointDouble other) => this.x == other.x && this.y == other.y;
        public override bool Equals(object obj) => obj is PointDouble p && this.Equals(p);
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"{x} {y}";

        public static bool operator ==(PointDouble left, PointDouble right) => left.Equals(right);
        public static bool operator !=(PointDouble left, PointDouble right) => !left.Equals(right);

        static int CrossSign(double x1, double y1, double x2, double y2) => Math.Sign(x1 * y2 - y1 * x2);

        /// <summary>
        /// 凸包(一番外側の多角形)を求める
        /// </summary>
        public static int[] ConvexHull(PointDouble[] points)
        {
            Contract.Assert(points.Length >= 3);
            var pts = new (double x, double y, int ix)[points.Length];
            for (int i = 0; i < points.Length; i++)
                pts[i] = (points[i].x, points[i].y, i);
            Array.Sort(pts);

            var upper = new SimpleList<(double x, double y, int ix)> { pts[0], pts[1] };
            var lower = new SimpleList<(double x, double y, int ix)> { pts[0], pts[1] };
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
        /// 3点の外心(外接円の中心=3点と等距離な点)を求める。3点が直線上にあるときはNaNとなる
        /// </summary>
        public static PointDouble 外心(PointDouble a, PointDouble b, PointDouble c)
        {
            var ab = a.Distance(b);
            var cb = c.Distance(b);
            var ac = a.Distance(c);

            var cosA = (ab * ab + ac * ac - cb * cb) / 2 / ab / ac;
            var cosB = (ab * ab + cb * cb - ac * ac) / 2 / ab / cb;
            var cosC = (cb * cb + ac * ac - ab * ab) / 2 / cb / ac;

            var d = cb * cosA + ac * cosB + ab * cosC;
            return new PointDouble(cb * cosA * a.x / d, cb * cosA * a.y / d)
                + new PointDouble(ac * cosB * b.x / d, ac * cosB * b.y / d)
                + new PointDouble(ab * cosC * c.x / d, ab * cosC * c.y / d);
        }
        /// <summary>
        /// A*x+B*y+C=0 との距離
        /// </summary>
        public double 直線との距離(double A, double B, double C)
            => Math.Abs(A * x + B * y + C) / Math.Sqrt(A * A + B * B);

        /// <summary>
        /// 2点を通る直線 A*x+B*y+C=0
        /// </summary>
        public (double A, double B, double C) 直線(PointDouble other)
            => (other.y - this.y, this.x - other.x, this.y * (other.x - this.x) - this.x * (other.y - this.y));


        /// <summary>
        /// 2点の垂直二等分線 A*x+B*y+C=0
        /// </summary>
        public (double A, double B, double C) 垂直二等分線(PointDouble other)
            => (this.x - other.x, this.y - other.y, (this.x * this.x - other.x * other.x + this.y * this.y - other.y * other.y) * -.5);


        /// <summary>
        /// P をとおる A*x+B*y=Any の垂線
        /// </summary>
        public static (double A, double B, double C) 直線の垂線(double a, double b, PointDouble p) => (b, -a, b * p.x - a * p.y);

        /// <summary>
        /// <paramref name="x"/> での A*x+B*y+C=0 の垂線
        /// </summary>
        public static (double A, double B, double C) 直線の垂線をXから求める(double a, double b, double c, double x) => (b, -a, b * x - a * (c - a * x));

        /// <summary>
        /// <paramref name="y"/> での A*x+B*y+C=0 の垂線
        /// </summary>
        public static (double A, double B, double C) 直線の垂線をYから求める(double a, double b, double c, double y) => (b, -a, b * (c - a * y) - a * y);

        /// <summary>
        /// A*x+B*y+C=0, U*x+V*y+W=0の交点
        /// </summary>
        public static PointDouble 直線と直線の交点(double a, double b, double c, double u, double v, double w)
        {
            var dd = a * v - b * u;
            return new PointDouble((b * w - c * v) / dd, (c * u - a * w) / dd);
        }


        /// <summary>
        /// A*x+B*y+C=0 と Pを中心とする半径rの円の交点
        /// </summary>
        public static PointDouble[] 直線と円の交点(double a, double b, double c, PointDouble p, double r)
        {
            var l = a * a + b * b;
            var k = a * p.x + b * p.y + c;
            var d = l * r * r - k * k;

            if (d < 0)
                return Array.Empty<PointDouble>();

            var apl = a / l;
            var bpl = b / l;
            var xc = p.x - apl * k;
            var yc = p.y - bpl * k;
            if (d == 0)
                return new[] { new PointDouble(xc, yc), };

            var ds = Math.Sqrt(d);
            var xd = bpl * ds;
            var yd = apl * ds;
            return new[]
            {
                new PointDouble(xc - xd, yc + yd),
                new PointDouble(xc + xd, yc - yd),
            };
        }

        /// <summary>
        /// P1 を中心とする半径 r1 の円とP2 を中心とする半径 r2 の円の交点
        /// </summary>
        public static PointDouble[] 円の交点(PointDouble p1, double r1, PointDouble p2, double r2)
        {
            var xx = p1.x - p2.x;
            var yy = p1.y - p2.y;
            return 直線と円の交点(
                xx,
                yy,
                0.5 * ((r1 - r2) * (r1 + r2) - xx * (p1.x + p2.x) - yy * (p1.y + p2.y)), p1, r1);
        }

        /// <summary>
        /// <paramref name="a1"/> から <paramref name="b1"/>までの線分と<paramref name="a2"/> から <paramref name="b2"/>までの線分が交差しているか
        /// </summary>
        public static bool 線分が交差しているか(PointDouble a1, PointDouble b1, PointDouble a2, PointDouble b2)
        {
            var ta = (a2.x - b2.x) * (a1.y - a2.y) + (a2.y - b2.y) * (a2.x - a1.x);
            var tb = (a2.x - b2.x) * (b1.y - a2.y) + (a2.y - b2.y) * (a2.x - b1.x);
            var tc = (a1.x - b1.x) * (a2.y - a1.y) + (a1.y - b1.y) * (a1.x - a2.x);
            var td = (a1.x - b1.x) * (b2.y - a1.y) + (a1.y - b1.y) * (a1.x - b2.x);

            return tc * td < 0 && ta * tb < 0;
        }
    }
}