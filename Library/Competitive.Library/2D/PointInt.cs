using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
#if NET7_0_OR_GREATER
using System.Numerics;
#endif

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE Point(int)
    using P = PointInt;
    public readonly struct PointInt : IEquatable<P>, IComparable<P>, IUtf8ConsoleWriterFormatter
#if NET7_0_OR_GREATER
        , IAdditionOperators<P, P, P>, ISubtractionOperators<P, P, P>, IUnaryPlusOperators<P, P>, IUnaryNegationOperators<P, P>
#endif
    {
        public readonly int x;
        public readonly int y;
        public PointInt(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        [凾(256)]
        public void Deconstruct(out int v1, out int v2) { v1 = x; v2 = y; }

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
            var u = other.x - x;
            var v = other.y - y;
            return (long)u * u + (long)v * v;
        }
        [凾(256)]
        public long Distance2Origin() => (long)x * x + (long)y * y;
        /// <summary>
        /// 内積
        /// </summary>

        [凾(256)]
        public long Inner(P other) => (long)x * other.x + (long)y * other.y;
        /// <summary>
        /// 外積
        /// </summary>

        [凾(256)]
        public long Cross(P other) => (long)x * other.y - (long)y * other.x;

        [凾(256)]
        public static P operator +(P a, P b) => new P(a.x + b.x, a.y + b.y);

        [凾(256)]
        public static P operator -(P a, P b) => new P(a.x - b.x, a.y - b.y);

        [凾(256)]
        public static P operator +(P a) => a;
        [凾(256)]
        public static P operator -(P a) => new P(-a.x, -a.y);

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
        public bool Equals(P other) => x == other.x && y == other.y;
        public override bool Equals(object obj) => obj is P p && Equals(p);
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"{x} {y}";
        [凾(256)] void IUtf8ConsoleWriterFormatter.Write(Utf8ConsoleWriter cw) => cw.Write(x).Write(' ').Write(y);
        public static implicit operator ConsoleOutput(P p) => p.ToConsoleOutput();

        [凾(256)]
        public static bool operator ==(P left, P right) => left.Equals(right);

        [凾(256)]
        public static bool operator !=(P left, P right) => !left.Equals(right);


        [凾(256)]
        static int CrossSign(in P origin, in P p1, in P p2)
        {
            long x1 = p1.x - origin.x;
            long y1 = p1.y - origin.y;
            long x2 = p2.x - origin.x;
            long y2 = p2.y - origin.y;
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
                    int cmp = points[i].y.CompareTo(origin.y);
                    if (cmp < 0 || (cmp == 0 && points[i].x < origin.x))
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
            long res = ((long)points[^1].x - points[0].x) * ((long)points[^1].y + points[0].y);
            for (int i = 1; i < points.Length; i++)
            {
                var p1 = points[i - 1];
                var p2 = points[i];
                res += ((long)p1.x - p2.x) * ((long)p1.y + p2.y);
            }
            return Math.Abs(res);
        }
        /// <summary>
        /// 2点を通る直線 A*x+B*y+C=0
        /// </summary>
        [凾(256)]
        public (long A, long B, long C) 直線(P other)
            => (other.y - y, x - other.x, (long)y * (other.x - x) - (long)x * (other.y - y));

        /// <summary>
        /// <para><paramref name="a1"/> から <paramref name="b1"/>までの線分と<paramref name="a2"/> から <paramref name="b2"/>までの線分が交差しているかを返します。</para>
        /// <para>端点で交差していれば 0, 端点以外で交差していれば 1, 交差していなければ -1 を返します。</para>
        /// </summary>
        [凾(256)]
        public static int 線分が交差しているか(P a1, P b1, P a2, P b2)
        {
            var (p, q, r) = a1.直線(b1);
            if (p * a2.x + q * a2.y + r == 0 && p * b2.x + q * b2.y + r == 0)
            {
                // 直線状に並んでいる場合
                var (xa1, xb1, xa2, xb2) = (a1.x, b1.x, a2.x, b2.x);
                if (xa1 == xb1) (xa1, xb1, xa2, xb2) = (a1.y, b1.y, a2.y, b2.y); // y軸に平行なとき

                if (xa1 > xb1) (xa1, xb1) = (xb1, xa1);
                if (xa2 > xb2) (xa2, xb2) = (xb2, xa2);

                if (xa1 > xb2 || xa2 > xb1) return -1;
                if (xa1 == xb2 || xa2 == xb1) return 0;
                return 1;
            }

            var ta = (a2.x - b2.x) * (a1.y - a2.y) + (a2.y - b2.y) * (a2.x - a1.x);
            var tb = (a2.x - b2.x) * (b1.y - a2.y) + (a2.y - b2.y) * (a2.x - b1.x);
            var tc = (a1.x - b1.x) * (a2.y - a1.y) + (a1.y - b1.y) * (a1.x - a2.x);
            var td = (a1.x - b1.x) * (b2.y - a1.y) + (a1.y - b1.y) * (a1.x - b2.x);

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
