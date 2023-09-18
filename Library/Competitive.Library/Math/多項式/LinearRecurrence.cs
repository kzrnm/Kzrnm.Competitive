using AtCoder;
using AtCoder.Internal;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// https://nyaannyaan.github.io/library/fps/kitamasa.hpp
namespace Kzrnm.Competitive
{
    /// <summary>
    /// 線形漸化式
    /// </summary>
    public static class LinearRecurrence
    {
        /// <summary>
        /// <para> Bostan-Mori Algorithm で [x^k] P(x) / Q(x) を求めます</para>
        /// </summary>
        public static MontgomeryModInt<T> BostanMori<T>(FormalPowerSeries<T> Q, FormalPowerSeries<T> P, long k) where T : struct, IStaticMod
        {
            MontgomeryModInt<T> ret = default;
            if (P.Count >= Q.Count)
            {
                var (R, Rm) = P.DivRem(Q);
                P = Rm;
                if (k < R.Count) ret += R.Coefficients[k];
            }
            if (P.Count == 0) return ret;

            if (NumberTheoreticTransform<T>.CanNtt())
            {
                int N = 1 << InternalBit.CeilPow2(Q.Count);

                var prr = new MontgomeryModInt<T>[2 * N];
                var qrr = new MontgomeryModInt<T>[2 * N];
                var srr = new MontgomeryModInt<T>[2 * N];
                var trr = new MontgomeryModInt<T>[2 * N];

                P.Coefficients.AsSpan().CopyTo(prr);
                Q.Coefficients.AsSpan().CopyTo(qrr);

                NumberTheoreticTransform<T>.Ntt(prr);
                NumberTheoreticTransform<T>.Ntt(qrr);


                var btr = new int[N];
                for (int i = 0, logn = BitOperations.TrailingZeroCount(N); i < btr.Length; i++)
                    btr[i] = (btr[i >> 1] >> 1) + ((i & 1) << (logn - 1));

                var dw = NumberTheoreticTransform<T>.pr.Inv()
                    .Pow((MontgomeryModInt<T>.Mod - 1) / (2L * N));

                while (k != 0)
                {
                    var pp = prr.AsSpan();
                    var qq = qrr.AsSpan();
                    var ss = srr.AsSpan(0, N);
                    var tt = trr.AsSpan(0, N);

                    var inv2 = (MontgomeryModInt<T>.One + MontgomeryModInt<T>.One).Inv();

                    // even degree of Q(x)Q(-x)
                    for (int i = 0; i < tt.Length; i++)
                        tt[i] = qq[(i << 1) | 0] * qq[(i << 1) | 1];

                    if ((k & 1) != 0)
                    {
                        // odd degree of P(x)Q(-x)
                        foreach (int i in btr)
                        {
                            ss[i] = (pp[(i << 1) | 0] * qq[(i << 1) | 1] -
                                     pp[(i << 1) | 1] * qq[(i << 1) | 0]) * inv2;
                            inv2 *= dw;
                        }
                    }
                    else
                    {
                        // even degree of P(x)Q(-x)
                        for (int i = 0; i < ss.Length; i++)
                        {
                            ss[i] = (pp[(i << 1) | 0] * qq[(i << 1) | 1] +
                                     pp[(i << 1) | 1] * qq[(i << 1) | 0]) * inv2;
                        }
                    }
                    (prr, srr) = (srr, prr);
                    (qrr, trr) = (trr, qrr);
                    if ((k >>= 1) < N) break;
                    prr = NumberTheoreticTransform<T>.NttDoubling(prr.AsSpan(0, N));
                    qrr = NumberTheoreticTransform<T>.NttDoubling(qrr.AsSpan(0, N));
                }
                var p2 = prr.AsSpan(0, N);
                var q2 = qrr.AsSpan(0, N).ToArray();
                NumberTheoreticTransform<T>.INtt(p2);
                NumberTheoreticTransform<T>.INtt(q2);
                return ret + new FormalPowerSeries<T>.Impl(q2).Inv().Multiply(p2).a[k];
            }
            else
            {
                var pp = new MontgomeryModInt<T>[Q.Count - 1];
                P.Coefficients.AsSpan().CopyTo(pp);
                var qq = (MontgomeryModInt<T>[])Q.Coefficients.Clone();
                while (k != 0)
                {
                    var Q2 = (MontgomeryModInt<T>[])qq.Clone();
                    for (int i = 1; i < Q2.Length; i += 2) Q2[i] = -Q2[i];

                    var ss = new FormalPowerSeries<T>.Impl((MontgomeryModInt<T>[])pp.Clone()).Multiply(Q2).AsSpan();
                    var tt = new FormalPowerSeries<T>.Impl((MontgomeryModInt<T>[])qq.Clone()).Multiply(Q2).AsSpan();
                    if ((k & 1) != 0)
                    {
                        for (int i = 1; i < ss.Length; i += 2) pp[i >> 1] = ss[i];
                        for (int i = 0; i < tt.Length; i += 2) qq[i >> 1] = tt[i];
                    }
                    else
                    {
                        for (int i = 0; i < ss.Length; i += 2) pp[i >> 1] = ss[i];
                        for (int i = 0; i < tt.Length; i += 2) qq[i >> 1] = tt[i];
                    }
                    k >>= 1;
                }
                return ret + pp[0];
            }
        }

        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        [凾(256)]
        public static MontgomeryModInt<T> Kitamasa<T>(ReadOnlySpan<int> a, ReadOnlySpan<int> c, long n) where T : struct, IStaticMod
            => Kitamasa(a.Select(v => new MontgomeryModInt<T>(v)), c.Select(v => new MontgomeryModInt<T>(v)), n);
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        [凾(256)]
        public static MontgomeryModInt<T> Kitamasa<T>(ReadOnlySpan<uint> a, ReadOnlySpan<uint> c, long n) where T : struct, IStaticMod
            => Kitamasa(a.Select(v => new MontgomeryModInt<T>((ulong)v)), c.Select(v => new MontgomeryModInt<T>((ulong)v)), n);

        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        [凾(256)]
        public static MontgomeryModInt<T> Kitamasa<T>(MontgomeryModInt<T>[] a, MontgomeryModInt<T>[] c, long n) where T : struct, IStaticMod
            => Kitamasa((ReadOnlySpan<MontgomeryModInt<T>>)a, c, n);

        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        [凾(256)]
        public static MontgomeryModInt<T> Kitamasa<T>(Span<MontgomeryModInt<T>> a, Span<MontgomeryModInt<T>> c, long n) where T : struct, IStaticMod
            => Kitamasa((ReadOnlySpan<MontgomeryModInt<T>>)a, c, n);

        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        public static MontgomeryModInt<T> Kitamasa<T>(ReadOnlySpan<MontgomeryModInt<T>> a, ReadOnlySpan<MontgomeryModInt<T>> c, long n) where T : struct, IStaticMod
        {
            Contract.Assert(a.Length == c.Length, reason: "漸化式の係数 c と数列 a の数が違います");
            if (n < a.Length) return a[(int)n];
            var Q = new MontgomeryModInt<T>[c.Length + 1];
            Q[0] = 1;
            for (int i = 0; i < c.Length; i++) Q[i + 1] = -c[i];
            var P = new FormalPowerSeries<T>.Impl(a.ToArray()).Multiply(Q).Pre(c.Length);
            return BostanMori(new FormalPowerSeries<T>(Q), P.ToFps(), n);
        }

        // https://twitter.com/e869120/status/1414166592565383169/photo/1
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="m"/> 項目までを求めます。</para>
        /// <para>計算量: O(m log m)</para>
        /// </summary>
        [凾(256)]
        public static MontgomeryModInt<T>[] Recurrence<T>(ReadOnlySpan<int> a, ReadOnlySpan<int> c, int m) where T : struct, IStaticMod
            => Recurrence(a.Select(v => new MontgomeryModInt<T>(v)), c.Select(v => new MontgomeryModInt<T>(v)), m);
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="m"/> 項目までを求めます。</para>
        /// <para>計算量: O(m log m)</para>
        /// </summary>
        [凾(256)]
        public static MontgomeryModInt<T>[] Recurrence<T>(ReadOnlySpan<uint> a, ReadOnlySpan<uint> c, int m) where T : struct, IStaticMod
            => Recurrence(a.Select(v => new MontgomeryModInt<T>((ulong)v)), c.Select(v => new MontgomeryModInt<T>((ulong)v)), m);

        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="m"/> 項目までを求めます。</para>
        /// <para>計算量: O(m log m)</para>
        /// </summary>
        [凾(256)]
        public static MontgomeryModInt<T>[] Recurrence<T>(MontgomeryModInt<T>[] a, MontgomeryModInt<T>[] c, int m) where T : struct, IStaticMod
            => Recurrence((ReadOnlySpan<MontgomeryModInt<T>>)a, c, m);

        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="m"/> 項目までを求めます。</para>
        /// <para>計算量: O(m log m)</para>
        /// </summary>
        [凾(256)]
        public static MontgomeryModInt<T>[] Recurrence<T>(Span<MontgomeryModInt<T>> a, Span<MontgomeryModInt<T>> c, int m) where T : struct, IStaticMod
            => Recurrence((ReadOnlySpan<MontgomeryModInt<T>>)a, c, m);

        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="m"/> 項目までを求めます。</para>
        /// <para>計算量: O(m log m)</para>
        /// </summary>
        public static MontgomeryModInt<T>[] Recurrence<T>(ReadOnlySpan<MontgomeryModInt<T>> a, ReadOnlySpan<MontgomeryModInt<T>> c, int m) where T : struct, IStaticMod
        {
            Contract.Assert(a.Length <= c.Length, reason: "漸化式の係数 c より数列 a の数が多いです");
            if (m < a.Length) return a[..m].ToArray();

            var Q = new MontgomeryModInt<T>[c.Length + 1];
            Q[0] = 1;
            for (int i = 0; i < c.Length; i++) Q[i + 1] = -c[i];

            var f0 = new FormalPowerSeries<T>.Impl(a.ToArray()).Multiply(Q);
            var f1 = f0.Pre(a.Length).Reverse().LeftShift(m + c.Length - a.Length);

            Q.AsSpan().Reverse();
            return f1.Divide(Q).Reverse().Pre(m).AsSpan().ToArray();
        }
    }
}
