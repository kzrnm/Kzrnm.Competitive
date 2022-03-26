﻿using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly struct PointInt : IEquatable<PointInt>, IComparable<PointInt>, IUtf8ConsoleWriterFormatter
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
        public static implicit operator PointInt((int x, int y) tuple) => new PointInt(tuple.x, tuple.y);

        [凾(256)]
        public double Distance(PointInt other) => Math.Sqrt(Distance2(other));

        [凾(256)]
        public long Distance2(PointInt other)
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
        public long Inner(PointInt other) => (long)x * other.x + (long)y * other.y;
        /// <summary>
        /// 外積
        /// </summary>

        [凾(256)]
        public long Cross(PointInt other) => (long)x * other.y - (long)y * other.x;

        [凾(256)]
        public static PointInt operator +(PointInt a, PointInt b) => new PointInt(a.x + b.x, a.y + b.y);

        [凾(256)]
        public static PointInt operator -(PointInt a, PointInt b) => new PointInt(a.x - b.x, a.y - b.y);

        /// <summary>
        /// <para>Atan2 の値で比較する(偏角ソート)。</para>
        /// <para>偏角は(0≦θ&lt;2π)とする。</para>
        /// <para>(0, 0) の偏角は 0 とする。</para>
        /// <para>偏角が等しければベクトルの絶対値で比較する。</para>
        /// </summary>
        [凾(256)]
        public int CompareTo(PointInt other)
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
        public bool Equals(PointInt other) => x == other.x && y == other.y;
        public override bool Equals(object obj) => obj is PointInt p && Equals(p);
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"{x} {y}";
        [凾(256)] void IUtf8ConsoleWriterFormatter.Write(Utf8ConsoleWriter cw) => cw.Write(x).Write(' ').Write(y);
        public static implicit operator ConsoleOutput(PointInt p) => p.ToConsoleOutput();

        [凾(256)]
        public static bool operator ==(PointInt left, PointInt right) => left.Equals(right);

        [凾(256)]
        public static bool operator !=(PointInt left, PointInt right) => !left.Equals(right);

        static int CrossSign(long x1, long y1, long x2, long y2) => Math.Sign(x1 * y2 - y1 * x2);

        /// <summary>
        /// 凸包(一番外側の多角形)を求める
        /// </summary>

        [凾(256)]
        public static int[] ConvexHull(PointInt[] points)
        {
            Contract.Assert(points.Length >= 3);
            var pts = new (int x, int y, int ix)[points.Length];
            for (int i = 0; i < points.Length; i++)
                pts[i] = (points[i].x, points[i].y, i);
            Array.Sort(pts);

            var upper = new List<(int x, int y, int ix)> { pts[0], pts[1] };
            var lower = new List<(int x, int y, int ix)> { pts[0], pts[1] };
            for (int i = 2; i < pts.Length; i++)
            {
                while (upper.Count > 1
                    && CrossSign(upper[^1].x - upper[^2].x, upper[^1].y - upper[^2].y, pts[i].x - upper[^2].x, pts[i].y - upper[^2].y) > 0)
                {
                    upper.RemoveAt(upper.Count - 1);
                }
                upper.Add(pts[i]);
                while (lower.Count > 1
                    && CrossSign(lower[^1].x - lower[^2].x, lower[^1].y - lower[^2].y, pts[i].x - lower[^2].x, pts[i].y - lower[^2].y) < 0)
                {
                    lower.RemoveAt(lower.Count - 1);
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

        [凾(256)]
        public static double Area(PointInt[] points) => Area2(points) / 2.0;

        /// <summary>
        /// 多角形の面積×2を求める
        /// </summary>

        [凾(256)]
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