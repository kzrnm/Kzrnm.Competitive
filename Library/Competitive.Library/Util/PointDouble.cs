using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.ComponentModel;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct PointDouble : IEquatable<PointDouble>, IComparable<PointDouble>, IUtf8ConsoleWriterFormatter
    {
        public readonly double x;
        public readonly double y;
        public PointDouble(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [凾(256)]
        public void Deconstruct(out double v1, out double v2) { v1 = x; v2 = y; }
        [凾(256)]
        public static implicit operator PointDouble((double x, double y) tuple) => new PointDouble(tuple.x, tuple.y);
        [凾(256)]
        public double Distance(PointDouble other) => Math.Sqrt(Distance2(other));
        [凾(256)]
        public double Distance2(PointDouble other)
        {
            var u = other.x - x;
            var v = other.y - y;
            return u * u + v * v;
        }
        /// <summary>
        /// 内積
        /// </summary>
        [凾(256)]
        public double Inner(PointDouble other) => x * other.x + y * other.y;
        /// <summary>
        /// 外積
        /// </summary>
        [凾(256)]
        public double Cross(PointDouble other) => x * other.y - y * other.x;
        [凾(256)]
        public static PointDouble operator +(PointDouble a, PointDouble b) => new PointDouble(a.x + b.x, a.y + b.y);
        [凾(256)]
        public static PointDouble operator -(PointDouble a, PointDouble b) => new PointDouble(a.x - b.x, a.y - b.y);

        /// <summary>
        /// <para>Atan2 の値で比較する(偏角ソート)。</para>
        /// <para>偏角は(0≦θ&lt;2π)とする。</para>
        /// <para>(0, 0) の偏角は 0 とする。</para>
        /// <para>偏角が等しければベクトルの絶対値で比較する。</para>
        /// </summary>
        [凾(256)]
        public int CompareTo(PointDouble other)
        {
            // 90°回転させると x の符号が同じなら tan(θ) の大小関係が θ の大小関係と等しくなる。
            // x=0
            var x1 = -y;
            var y1 = x;
            var x2 = -other.y;
            var y2 = other.x;

            int xo1 = x1 > 0 ? 1 : 0;
            int xo2 = x2 > 0 ? 1 : 0;

            int cmp = xo1 - xo2;
            if (cmp != 0) return cmp;

            if (xo1 == 0) (x1, y1) = (-x1, -y1);
            if (xo2 == 0) (x2, y2) = (-x2, -y2);
            // tan(θ) = y/x の大小関係は θ の大小関係と等しい
            cmp = (x2 * y1).CompareTo(x1 * y2);
            if (cmp != 0) return cmp;

            // θ が等しいのでベクトルの絶対値の大小関係で比較する
            // x=0 でなければ x の大小関係が絶対値の大小関係と等しい
            cmp = x1.CompareTo(x2);
            if (cmp != 0) return cmp;

            // x=0 のときは y の符号ごとに場合分け
            // y の符号が同じなら y の絶対値の大小関係がベクトルの大小関係と等しい
            if (y1 == 0) return y2 == 0 ? 0 : -1;
            if (y2 == 0) return 1;

            if ((Math.Sign(y1) ^ Math.Sign(y2)) < 0) return y1.CompareTo(y2);
            return Math.Abs(y1).CompareTo(Math.Abs(y2));
        }
        public bool IsNaN => double.IsNaN(x) || double.IsNaN(y);

        [凾(256)]
        public bool Equals(PointDouble other) => this.x == other.x && this.y == other.y;
        public override bool Equals(object obj) => obj is PointDouble p && this.Equals(p);
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"{x} {y}";
        [凾(256)] void IUtf8ConsoleWriterFormatter.Write(Utf8ConsoleWriter cw) => cw.Write(x).Write(' ').Write(y);
        public static implicit operator ConsoleOutput(PointDouble p) => p.ToConsoleOutput();
        public static bool operator ==(PointDouble left, PointDouble right) => left.Equals(right);
        public static bool operator !=(PointDouble left, PointDouble right) => !left.Equals(right);

        [凾(256)]
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
        [凾(256)]
        public double 直線との距離(double A, double B, double C)
            => Math.Abs(A * x + B * y + C) / Math.Sqrt(A * A + B * B);

        /// <summary>
        /// 2点を通る直線 A*x+B*y+C=0
        /// </summary>
        [凾(256)]
        public (double A, double B, double C) 直線(PointDouble other)
            => (other.y - this.y, this.x - other.x, this.y * (other.x - this.x) - this.x * (other.y - this.y));


        /// <summary>
        /// 2点の垂直二等分線 A*x+B*y+C=0
        /// </summary>
        [凾(256)]
        public (double A, double B, double C) 垂直二等分線(PointDouble other)
            => (this.x - other.x, this.y - other.y, (this.x * this.x - other.x * other.x + this.y * this.y - other.y * other.y) * -.5);


        /// <summary>
        /// P をとおる A*x+B*y=Any の垂線
        /// </summary>
        [凾(256)]
        public static (double A, double B, double C) 直線の垂線(double a, double b, PointDouble p) => (b, -a, b * p.x - a * p.y);

        /// <summary>
        /// <paramref name="x"/> での A*x+B*y+C=0 の垂線
        /// </summary>
        [凾(256)]
        public static (double A, double B, double C) 直線の垂線をXから求める(double a, double b, double c, double x) => (b, -a, b * x - a * (c - a * x));

        /// <summary>
        /// <paramref name="y"/> での A*x+B*y+C=0 の垂線
        /// </summary>
        [凾(256)]
        public static (double A, double B, double C) 直線の垂線をYから求める(double a, double b, double c, double y) => (b, -a, b * (c - a * y) - a * y);

        /// <summary>
        /// A*x+B*y+C=0, U*x+V*y+W=0の交点
        /// </summary>
        [凾(256)]
        public static PointDouble 直線と直線の交点(double a, double b, double c, double u, double v, double w)
        {
            var dd = a * v - b * u;
            return new PointDouble((b * w - c * v) / dd, (c * u - a * w) / dd);
        }


        /// <summary>
        /// A*x+B*y+C=0 と Pを中心とする半径rの円の交点
        /// </summary>
        [凾(256)]
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
        [凾(256)]
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
        [凾(256)]
        public static bool 線分が交差しているか(PointDouble a1, PointDouble b1, PointDouble a2, PointDouble b2)
        {
            var ta = (a2.x - b2.x) * (a1.y - a2.y) + (a2.y - b2.y) * (a2.x - a1.x);
            var tb = (a2.x - b2.x) * (b1.y - a2.y) + (a2.y - b2.y) * (a2.x - b1.x);
            var tc = (a1.x - b1.x) * (a2.y - a1.y) + (a1.y - b1.y) * (a1.x - a2.x);
            var td = (a1.x - b1.x) * (b2.y - a1.y) + (a1.y - b1.y) * (a1.x - b2.x);

            return tc * td < 0 && ta * tb < 0;
        }

        /// <summary>
        /// 多角形の面積を求める
        /// </summary>
        [凾(256)]
        public static double Area(PointDouble[] points) => Area2(points) / 2.0;

        /// <summary>
        /// 多角形の面積×2を求める
        /// </summary>
        [凾(256)]
        public static double Area2(PointDouble[] points)
        {
            Contract.Assert(points.Length >= 3);
            double res = (points[^1].x - points[0].x) * (points[^1].y + points[0].y);
            for (int i = 1; i < points.Length; i++)
            {
                var p1 = points[i - 1];
                var p2 = points[i];
                res += (p1.x - p2.x) * (p1.y + p2.y);
            }
            return Math.Abs(res);
        }
    }
}