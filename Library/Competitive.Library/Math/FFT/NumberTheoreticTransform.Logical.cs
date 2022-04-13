using AtCoder;
using AtCoder.Internal;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// https://nyaannyaan.github.io/library/ntt/ntt.hpp
namespace Kzrnm.Competitive
{
    using static BitOperations;
    /// <summary>
    /// 数論変換 number-theoretic transform
    /// </summary>
    public static partial class NumberTheoreticTransform<T> where T : struct, IStaticMod
    {
        [凾(512)]
        static void Fft4(Span<StaticModInt<T>> a, int k)
        {
            if (a.Length <= 1) return;
            if (k == 1)
            {
                var a1 = a[1];
                a[1] = a[0] - a[1];
                a[0] = a[0] + a1;
                return;
            }
            if ((k & 1) != 0)
            {
                int vv = 1 << (k - 1);
                for (int j = 0; j < vv; ++j)
                {
                    var ajv = a[j + vv];
                    a[j + vv] = a[j] - ajv;
                    a[j] += ajv;
                }
            }
            int u = 1 << (2 + (k & 1));
            int v = 1 << (k - 2 - (k & 1));
            var one = StaticModInt<T>.Raw(1);
            var imag = dws[1];
            while (v != 0)
            {
                // jh = 0
                {
                    int j0 = 0;
                    int j1 = v;
                    int j2 = j1 + v;
                    int j3 = j2 + v;
                    for (; j0 < v; ++j0, ++j1, ++j2, ++j3)
                    {
                        StaticModInt<T> t0 = a[j0], t1 = a[j1], t2 = a[j2], t3 = a[j3];
                        StaticModInt<T> t0p2 = t0 + t2, t1p3 = t1 + t3;
                        StaticModInt<T> t0m2 = t0 - t2, t1m3 = (t1 - t3) * imag;
                        a[j0] = t0p2 + t1p3;
                        a[j1] = t0p2 - t1p3;
                        a[j2] = t0m2 + t1m3;
                        a[j3] = t0m2 - t1m3;
                    }
                }
                // jh >= 1
                var xx = one * dws[2];
                for (int jh = 4; jh < u;)
                {
                    var ww = xx * xx;
                    var wx = ww * xx;
                    int j0 = jh * v;
                    int je = j0 + v;
                    int j2 = je + v;
                    for (; j0 < je; ++j0, ++j2)
                    {
                        StaticModInt<T> t0 = a[j0], t1 = a[j0 + v] * xx, t2 = a[j2] * ww,
                                         t3 = a[j2 + v] * wx;
                        StaticModInt<T> t0p2 = t0 + t2, t1p3 = t1 + t3;
                        StaticModInt<T> t0m2 = t0 - t2, t1m3 = (t1 - t3) * imag;
                        a[j0] = t0p2 + t1p3;
                        a[j0 + v] = t0p2 - t1p3;
                        a[j2] = t0m2 + t1m3;
                        a[j2 + v] = t0m2 - t1m3;
                    }
                    xx *= dws[TrailingZeroCount(jh += 4)];
                }
                u <<= 2;
                v >>= 2;
            }
        }

        [凾(512)]
        static void IFft4(Span<StaticModInt<T>> a, int k)
        {
            if (a.Length <= 1) return;
            if (k == 1)
            {
                var a1 = a[1];
                a[1] = a[0] - a[1];
                a[0] = a[0] + a1;
                return;
            }
            int u = 1 << (k - 2);
            int v = 1;
            var one = StaticModInt<T>.Raw(1);
            var imag = dys[1];
            while (u != 0)
            {
                // jh = 0
                {
                    int j0 = 0;
                    int j1 = v;
                    int j2 = v + v;
                    int j3 = j2 + v;
                    for (; j0 < v; ++j0, ++j1, ++j2, ++j3)
                    {
                        StaticModInt<T> t0 = a[j0], t1 = a[j1], t2 = a[j2], t3 = a[j3];
                        StaticModInt<T> t0p1 = t0 + t1, t2p3 = t2 + t3;
                        StaticModInt<T> t0m1 = t0 - t1, t2m3 = (t2 - t3) * imag;
                        a[j0] = t0p1 + t2p3;
                        a[j2] = t0p1 - t2p3;
                        a[j1] = t0m1 + t2m3;
                        a[j3] = t0m1 - t2m3;
                    }
                }
                // jh >= 1
                var xx = one * dys[2];
                u <<= 2;
                for (int jh = 4; jh < u;)
                {
                    var ww = xx * xx;
                    var yy = xx * imag;
                    int j0 = jh * v;
                    int je = j0 + v;
                    int j2 = je + v;
                    for (; j0 < je; ++j0, ++j2)
                    {
                        StaticModInt<T> t0 = a[j0], t1 = a[j0 + v], t2 = a[j2], t3 = a[j2 + v];
                        StaticModInt<T> t0p1 = t0 + t1, t2p3 = t2 + t3;
                        StaticModInt<T> t0m1 = (t0 - t1) * xx, t2m3 = (t2 - t3) * yy;
                        a[j0] = t0p1 + t2p3;
                        a[j2] = (t0p1 - t2p3) * ww;
                        a[j0 + v] = t0m1 + t2m3;
                        a[j2 + v] = (t0m1 - t2m3) * ww;
                    }
                    xx *= dys[TrailingZeroCount(jh += 4)];
                }
                u >>= 4;
                v <<= 2;
            }
            if ((k & 1) != 0)
            {
                u = 1 << (k - 1);
                for (int j = 0; j < u; ++j)
                {
                    StaticModInt<T> ajv = a[j] - a[j + u];
                    a[j] += a[j + u];
                    a[j + u] = ajv;
                }
            }
        }

        [凾(256)]
        internal static void NttLogical(Span<StaticModInt<T>> a)
        {
            if (a.Length <= 1) return;
            Fft4(a, TrailingZeroCount(a.Length));
        }
        [凾(256)]
        internal static void INttLogical(Span<StaticModInt<T>> a)
        {
            if (a.Length <= 1) return;
            IFft4(a, TrailingZeroCount(a.Length));
            var iv = new StaticModInt<T>(a.Length).Inv();
            for (int i = 0; i < a.Length; i++) a[i] *= iv;
        }

        [凾(512)]
        internal static StaticModInt<T>[] MultiplyLogical(ReadOnlySpan<StaticModInt<T>> a, ReadOnlySpan<StaticModInt<T>> b)
        {
            if (Math.Min(a.Length, b.Length) <= 60)
                return MultiplyNative(a, b);

            int l = a.Length + b.Length - 1;

            var k = InternalBit.CeilPow2(l);
            var M = 1 << k;

            SetWy(k);
            var s = new StaticModInt<T>[M];
            var t = new StaticModInt<T>[M];
            a.CopyTo(s);
            b.CopyTo(t);
            Fft4(s, k);
            Fft4(t, k);
            for (int i = 0; i < s.Length; ++i)
                s[i] *= t[i];
            IFft4(s, k);
            var invm = new StaticModInt<T>(M).Inv();
            var res = new StaticModInt<T>[l];
            for (int i = 0; i < res.Length; ++i)
                res[i] = s[i] * invm;
            return res;
        }
    }
}
