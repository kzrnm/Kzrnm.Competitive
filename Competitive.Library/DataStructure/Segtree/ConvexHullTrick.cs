//https://tjkendev.github.io/procon-library/cpp/convex_hull_trick/li_chao_tree.html
using AtCoder;
using AtCoder.Internal;
using AtCoder.Operators;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [IsOperator]
    public interface IConvexHullTrickOperator<T> { bool UseLeft(T left, T right); }

    /// <summary>
    /// <para>Convex Hull Trick(Li-Chao (Segment) Tree)</para>
    /// <para>直線 Ax+B 追加をいくつか追加して、区間[l,r]の最小値・最大値を求める</para>
    /// </summary>
    public class ConvexHullTrick<T, TOp, TCmp>
        where TOp : struct, IArithmeticOperator<T>
        where TCmp : struct, IConvexHullTrickOperator<T>
    {
        private static TOp op = default;
        private static TCmp cmp = default;
        public readonly T XINF;
        public readonly T YINF;
        private bool[] u;
        private T[] xs;
        private T[] p;
        private T[] q;
        private readonly int size;
        public int Length { get; }

        /// <summary>
        /// 使用する <paramref name="xs"/> の値を初期化します。<paramref name="xs"/> はソート済みであること。
        /// </summary>
        public ConvexHullTrick(T[] xs, T xinf, T yinf)
        {
            Length = xs.Length;
            XINF = xinf;
            YINF = yinf;

            size = 1 << InternalBit.CeilPow2(Length);
            int n2 = size << 1;
            u = new bool[n2];
            this.xs = new T[n2];
            xs.AsSpan().CopyTo(this.xs);
            this.xs.AsSpan(Length).Fill(XINF);
            p = new T[n2];
            q = new T[n2];
        }
        /// <summary>
        /// 直線 <paramref name="a"/>x + <paramref name="b"/> を追加します
        /// </summary>
        [凾(256)]
        public void AddLine(T a, T b) => AddLine(a, b, 1, 0, size);
        /// <summary>
        /// 線分 <paramref name="a"/>x + <paramref name="b"/> (<see cref="xs"/>[l]≦x&lt;<see cref="xs"/>[r]) を追加します
        /// </summary>
        public void AddSegmentLine(T a, T b, int l, int r)
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

        private void AddLine(T a, T b, int k, int l, int r)
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

                T lx = xs[l], mx = xs[m], rx = xs[r - 1];
                T pk = p[k], qk = q[k];
                bool left = cmp.UseLeft(op.Add(op.Multiply(a, lx), b), op.Add(op.Multiply(pk, lx), qk));
                bool mid = cmp.UseLeft(op.Add(op.Multiply(a, mx), b), op.Add(op.Multiply(pk, mx), qk));
                bool right = cmp.UseLeft(op.Add(op.Multiply(a, rx), b), op.Add(op.Multiply(pk, rx), qk));
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

        /// <summary>
        /// <see cref="xs"/>[<paramref name="i"/>]での最小値・最大値を取得します。
        /// </summary>
        [凾(256)]
        public T Query(int i) => Query(i, xs[i]);
        private T Query(int k, T x)
        {
            k += size;
            T s = u[k] ? op.Add(op.Multiply(p[k], x), q[k]) : YINF;
            while (k > 1)
            {
                k >>= 1;
                if (u[k])
                {
                    T r = op.Add(op.Multiply(p[k], x), q[k]);
                    if (cmp.UseLeft(r, s))
                        s = r;
                }
            }
            return s;
        }
    }
    /// <summary>
    /// <para>Convex Hull Trick(Li-Chao (Segment) Tree)</para>
    /// <para>直線 Ax+B 追加をいくつか追加して、区間[l,r]の最小値を求める</para>
    /// </summary>
    public class LongMinConvexHullTrick : ConvexHullTrick<long, LongOperator, MinConvexHullTrickOperator<long, LongOperator>>
    {
        /// <summary>
        /// 使用する <paramref name="xs"/> の値を初期化します。<paramref name="xs"/> はソート済みであること。
        /// </summary>
        public LongMinConvexHullTrick(long[] xs) : this(xs, 1000000000, 1000000000000000000) { }
        /// <summary>
        /// 使用する <paramref name="xs"/> の値を初期化します。<paramref name="xs"/> はソート済みであること。
        /// </summary>
        public LongMinConvexHullTrick(long[] xs, long xinf, long yinf) : base(xs, xinf, yinf) { }
    }
    /// <summary>
    /// <para>Convex Hull Trick(Li-Chao (Segment) Tree)</para>
    /// <para>直線 Ax+B 追加をいくつか追加して、区間[l,r]の最大値を求める</para>
    /// </summary>
    public class LongMaxConvexHullTrick : ConvexHullTrick<long, LongOperator, MaxConvexHullTrickOperator<long, LongOperator>>
    {
        /// <summary>
        /// 使用する <paramref name="xs"/> の値を初期化します。<paramref name="xs"/> はソート済みであること。
        /// </summary>
        public LongMaxConvexHullTrick(long[] xs) : this(xs, 1000000000, -1000000000000000000) { }
        /// <summary>
        /// 使用する <paramref name="xs"/> の値を初期化します。<paramref name="xs"/> はソート済みであること。
        /// </summary>
        public LongMaxConvexHullTrick(long[] xs, long xinf, long yinf) : base(xs, xinf, yinf) { }
    }
    public struct MinConvexHullTrickOperator<T, TOp> : IConvexHullTrickOperator<T>
        where TOp : struct, ICompareOperator<T>
    {
        private static TOp op = default;
        [凾(256)]
        public bool UseLeft(T left, T right) => op.LessThan(left, right);
    }
    public struct MaxConvexHullTrickOperator<T, TOp> : IConvexHullTrickOperator<T>
        where TOp : struct, ICompareOperator<T>
    {
        private static TOp op = default;
        [凾(256)]
        public bool UseLeft(T left, T right) => op.GreaterThan(left, right);
    }
}
