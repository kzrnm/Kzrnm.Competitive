//https://tjkendev.github.io/procon-library/cpp/convex_hull_trick/li_chao_tree.html
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// <para>Convex Hull Trick(Li-Chao (Segment) Tree)</para>
    /// <para>直線 Ax+B 追加をいくつか追加して、区間[l,r]の最小値・最大値を求める</para>
    /// </summary>
    public class ConvexHullTrick
    {
        private readonly long XINF;
        private readonly long YINF;
        private bool[] u;
        private long[] xs;
        private long[] p;
        private long[] q;
        private readonly int size;
        public int Length { get; }

        public ConvexHullTrick(long[] xs) : this(xs, 1000000000, 1000000000000000000) { }
        public ConvexHullTrick(long[] xs, long xinf, long yinf)
        {
            Length = xs.Length;
            XINF = xinf;
            YINF = yinf;

            size = InternalBit.CeilPow2(Length);
            int n2 = size << 1;
            u = new bool[n2];
            this.xs = new long[n2];
            xs.AsSpan().CopyTo(this.xs);
            this.xs.AsSpan(Length).Fill(XINF);
            p = new long[n2];
            q = new long[n2];
        }
        /// <summary>
        /// 直線 <paramref name="a"/>x + <paramref name="b"/> を追加します
        /// </summary>
        public void AddLine(long a, long b) => AddLine(a, b, 1, 0, size);
        /// <summary>
        /// 線分 <paramref name="a"/>x + <paramref name="b"/> (<see cref="xs"/>[l]≦x&lt;<see cref="xs"/>[r]) を追加します
        /// </summary>
        public void AddSegmentLine(long a, long b, int l, int r)
        {
            int l0 = l + size, r0 = r + size;
            int s0 = l, t0 = r, sz = 1;
            while (l0 < r0)
            {
                if ((r0 & 1) != 0)
                {
                    --r0; t0 -= sz;
                    AddLine(a, b, r0, t0, t0 + sz);
                }
                if ((l0 & 1) != 0)
                {
                    AddLine(a, b, l0, s0, s0 + sz);
                    ++l0; s0 += sz;
                }
                l0 >>= 1; r0 >>= 1;
                sz <<= 1;
            }
        }

        private void AddLine(long a, long b, int k, int l, int r)
        {
            while (r - l > 0)
            {
                int m = (l + r) >> 1;
                if (!u[k])
                {
                    p[k] = a; q[k] = b;
                    u[k] = true;
                    return;
                }

                long lx = xs[l], mx = xs[m], rx = xs[r - 1];
                long pk = p[k], qk = q[k];
                bool left = (a * lx + b < pk * lx + qk);
                bool mid = (a * mx + b < pk * mx + qk);
                bool right = (a * rx + b < pk * rx + qk);
                if (left && right)
                {
                    p[k] = a; q[k] = b;
                    return;
                }
                if (!left && !right)
                {
                    return;
                }
                if (mid)
                {
                    (p[k], a) = (a, p[k]);
                    (q[k], b) = (b, q[k]);
                }
                if (left != mid)
                {
                    k = 2 * k;
                    r = m;
                }
                else
                {
                    k = 2 * k + 1;
                    l = m;
                }
            }
        }
    }
}
