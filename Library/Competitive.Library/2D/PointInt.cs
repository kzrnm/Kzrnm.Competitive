using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using P = PointInt;
    public readonly record struct PointInt(int X, int Y) : IComparable<P>, IUtf8ConsoleWriterFormatter
        , IAdditionOperators<P, P, P>, ISubtractionOperators<P, P, P>, IUnaryPlusOperators<P, P>, IUnaryNegationOperators<P, P>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]

        [凾(256)]
        public void Deconstruct(out int v1, out int v2) { v1 = X; v2 = Y; }

        [凾(256)]
        public static implicit operator P((int x, int y) tuple) => new P(tuple.x, tuple.y);

        /// <summary>
        /// <paramref name="other"/> との距離
        /// </summary>
        [凾(256)]
        public double Distance(P other) => Math.Sqrt(Distance2(other));

        /// <summary>
        /// <paramref name="other"/> との距離の 2 乗
        /// </summary>
        [凾(256)]
        public long Distance2(P other)
        {
            var u = other.X - X;
            var v = other.Y - Y;
            return (long)u * u + (long)v * v;
        }
        [凾(256)]
        public long Distance2Origin() => (long)X * X + (long)Y * Y;
        /// <summary>
        /// 内積
        /// </summary>

        [凾(256)]
        public long Inner(P other) => (long)X * other.X + (long)Y * other.Y;
        /// <summary>
        /// 外積
        /// </summary>

        [凾(256)]
        public long Cross(P other) => (long)X * other.Y - (long)Y * other.X;

        [凾(256)]
        public static P operator +(P a, P b) => new P(a.X + b.X, a.Y + b.Y);

        [凾(256)]
        public static P operator -(P a, P b) => new P(a.X - b.X, a.Y - b.Y);

        [凾(256)]
        public static P operator +(P a) => a;
        [凾(256)]
        public static P operator -(P a) => new P(-a.X, -a.Y);

        /// <summary>
        /// <para>Atan2 の値で比較する(偏角ソート)。</para>
        /// <para>偏角は(0≦θ&lt;2π)とする。</para>
        /// <para>(0, 0) の偏角は 0 とする。</para>
        /// <para>偏角が等しければベクトルの絶対値で比較する。</para>
        /// </summary>
        [凾(256)]
        public int CompareTo(P other)
        {
            // 90°回転させると x の符号が同じなら tan(θ) の大小関係が θ の大小関係と等しくなる。
            var x1 = -Y;
            var y1 = X;
            var x2 = -other.Y;
            var y2 = other.X;

            int xo1 = x1 > 0 ? 1 : 0;
            int xo2 = x2 > 0 ? 1 : 0;

            int cmp = xo1 - xo2;
            if (cmp != 0) return cmp;

            if (xo1 == 0) (x1, y1) = (-x1, -y1);
            if (xo2 == 0) (x2, y2) = (-x2, -y2);
            // tan(θ) = y/x の大小関係は θ の大小関係と等しい
            cmp = ((long)x2 * y1).CompareTo((long)x1 * y2);
            if (cmp != 0) return cmp;

            // θ が等しいのでベクトルの絶対値の大小関係で比較する
            // x=0 でなければ x の大小関係が絶対値の大小関係と等しい
            cmp = x1.CompareTo(x2);
            if (cmp != 0) return cmp;

            // x=0 のときは y の符号ごとに場合分け
            // y の符号が同じなら y の絶対値の大小関係がベクトルの大小関係と等しい
            if (y1 == 0) return y2 == 0 ? 0 : -1;
            if (y2 == 0) return 1;

            if ((y1 ^ y2) < 0) return y1.CompareTo(y2);
            return Math.Abs(y1).CompareTo(Math.Abs(y2));
        }

        [凾(256)]
        public bool Equals(P other) => X == other.X && Y == other.Y;
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"{X} {Y}";
        [凾(256)] void IUtf8ConsoleWriterFormatter.Write(Utf8ConsoleWriter cw) => cw.Write(X).Write(' ').Write(Y);
        public static implicit operator ConsoleOutput(P p) => p.ToConsoleOutput();


        [凾(256)]
        static int CrossSign(in P origin, in P p1, in P p2)
        {
            long x1 = p1.X - origin.X;
            long y1 = p1.Y - origin.Y;
            long x2 = p2.X - origin.X;
            long y2 = p2.Y - origin.Y;
            return Math.Sign(x1 * y2 - y1 * x2);
        }

        /// <summary>
        /// 凸包(一番外側の多角形)を求める
        /// </summary>
        [凾(256)]
        public static int[] ConvexHull(ReadOnlySpan<P> points)
        {
            // https://en.wikipedia.org/wiki/Graham_scan
            Contract.Assert(points.Length >= 2);
            var idx = new int[points.Length];
            P[] pts = new P[points.Length];
            {
                // 最もy座標が小さいものを起点とする
                // 最もy座標が小さいものが複数あるならばx座標が小さいものを起点とする
                P origin = points[0];
                for (int i = 1; i < points.Length; i++)
                {
                    idx[i] = i;
                    int cmp = points[i].Y.CompareTo(origin.Y);
                    if (cmp < 0 || (cmp == 0 && points[i].X < origin.X))
                        origin = points[i];
                }


                for (int i = 0; i < pts.Length; i++)
                    pts[i] = points[i] - origin;

                // 偏角ソート
                // 偏角が同じなら原点から近い順
                Array.Sort(pts, idx);
            }

            var st = new List<int>(pts.Length) { 0 };
            for (int i = 1; i < pts.Length; i++)
            {
                while (st.Count > 1 && CrossSign(pts[st[^2]], pts[st[^1]], pts[i]) < 0)
                    st.RemoveAt(st.Count - 1);
                st.Add(i);
            }

            // 最後の並びが起点から一直線に並んでいる場合は保存されていないので追加で確認する
            for (int i = st[^1] - 1; i > 0; i--)
            {
                if (CrossSign(pts[st[^1]], pts[i], pts[0]) == 0)
                    st.Add(i);
                else break;
            }

            var res = new int[st.Count];
            for (int i = st.Count - 1; i >= 0; i--)
                res[i] = idx[st[i]];
            return res;
        }

        /// <summary>
        /// 多角形の面積を求める
        /// </summary>

        [凾(256)]
        public static double Area(ReadOnlySpan<P> points) => Area2(points) / 2.0;

        /// <summary>
        /// 多角形の面積×2を求める
        /// </summary>

        [凾(256)]
        public static long Area2(ReadOnlySpan<P> points)
        {
            Contract.Assert(points.Length >= 3);
            long res = ((long)points[^1].X - points[0].X) * ((long)points[^1].Y + points[0].Y);
            for (int i = 1; i < points.Length; i++)
            {
                var p1 = points[i - 1];
                var p2 = points[i];
                res += ((long)p1.X - p2.X) * ((long)p1.Y + p2.Y);
            }
            return Math.Abs(res);
        }
        /// <summary>
        /// 2点を通る直線 A*x+B*y+C=0
        /// </summary>
        [凾(256)]
        public (long A, long B, long C) 直線(P other)
            => (other.Y - Y, X - other.X, (long)Y * (other.X - X) - (long)X * (other.Y - Y));

        /// <summary>
        /// <para><paramref name="a1"/> から <paramref name="b1"/>までの線分と<paramref name="a2"/> から <paramref name="b2"/>までの線分が交差しているかを返します。</para>
        /// <para>端点で交差していれば 0, 端点以外で交差していれば 1, 交差していなければ -1 を返します。</para>
        /// </summary>
        [凾(256)]
        public static int 線分が交差しているか(P a1, P b1, P a2, P b2)
        {
            var (p, q, r) = a1.直線(b1);
            if (p * a2.X + q * a2.Y + r == 0 && p * b2.X + q * b2.Y + r == 0)
            {
                // 直線状に並んでいる場合
                var (xa1, xb1, xa2, xb2) = (a1.X, b1.X, a2.X, b2.X);
                if (xa1 == xb1) (xa1, xb1, xa2, xb2) = (a1.Y, b1.Y, a2.Y, b2.Y); // y軸に平行なとき

                if (xa1 > xb1) (xa1, xb1) = (xb1, xa1);
                if (xa2 > xb2) (xa2, xb2) = (xb2, xa2);

                if (xa1 > xb2 || xa2 > xb1) return -1;
                if (xa1 == xb2 || xa2 == xb1) return 0;
                return 1;
            }

            var ta = (a2.X - b2.X) * (a1.Y - a2.Y) + (a2.Y - b2.Y) * (a2.X - a1.X);
            var tb = (a2.X - b2.X) * (b1.Y - a2.Y) + (a2.Y - b2.Y) * (a2.X - b1.X);
            var tc = (a1.X - b1.X) * (a2.Y - a1.Y) + (a1.Y - b1.Y) * (a1.X - a2.X);
            var td = (a1.X - b1.X) * (b2.Y - a1.Y) + (a1.Y - b1.Y) * (a1.X - b2.X);

            return -Math.Max(Math.Sign(tc) * Math.Sign(td), Math.Sign(ta) * Math.Sign(tb));
        }

        /// <summary>
        /// 多角形 <paramref name="s"/> を三角形に分割します。
        /// </summary>
        public static (P, P, P)[] 三角形に分割(ReadOnlySpan<P> s)
        {
            if (s.Length < 3) return Array.Empty<(P, P, P)>();

            var ret = new (P, P, P)[s.Length - 2];
            int a = 0;
            int b = s.Length - 1;

            for (int i = 0; i < ret.Length; i++)
            {
                if ((i & 1) != 0)
                {
                    var p = s[b];
                    ret[i] = (s[a], p, s[--b]);
                }
                else
                {
                    var p = s[a];
                    ret[i] = (s[++a], p, s[b]);
                }
            }

            return ret;
        }
    }
}
