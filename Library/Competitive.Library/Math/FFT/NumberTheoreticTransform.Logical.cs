using AtCoder;
using AtCoder.Internal;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        static void Fft4(Span<MontgomeryModInt<T>> a, int k)
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
            var one = MontgomeryModInt<T>.One;
            var imag = dw[1];
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
                        MontgomeryModInt<T> t0 = a[j0], t1 = a[j1], t2 = a[j2], t3 = a[j3];
                        MontgomeryModInt<T> t0p2 = t0 + t2, t1p3 = t1 + t3;
                        MontgomeryModInt<T> t0m2 = t0 - t2, t1m3 = (t1 - t3) * imag;
                        a[j0] = t0p2 + t1p3;
                        a[j1] = t0p2 - t1p3;
                        a[j2] = t0m2 + t1m3;
                        a[j3] = t0m2 - t1m3;
                    }
                }
                // jh >= 1
                var xx = one * dw[2];
                for (int jh = 4; jh < u;)
                {
                    var ww = xx * xx;
                    var wx = ww * xx;
                    int j0 = jh * v;
                    int je = j0 + v;
                    int j2 = je + v;
                    for (; j0 < je; ++j0, ++j2)
                    {
                        MontgomeryModInt<T> t0 = a[j0], t1 = a[j0 + v] * xx, t2 = a[j2] * ww,
                                         t3 = a[j2 + v] * wx;
                        MontgomeryModInt<T> t0p2 = t0 + t2, t1p3 = t1 + t3;
                        MontgomeryModInt<T> t0m2 = t0 - t2, t1m3 = (t1 - t3) * imag;
                        a[j0] = t0p2 + t1p3;
                        a[j0 + v] = t0p2 - t1p3;
                        a[j2] = t0m2 + t1m3;
                        a[j2 + v] = t0m2 - t1m3;
                    }
                    xx *= dw[TrailingZeroCount(jh += 4)];
                }
                u <<= 2;
                v >>= 2;
            }
        }

        [凾(512)]
        static void IFft4(Span<MontgomeryModInt<T>> a, int k)
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
            var one = MontgomeryModInt<T>.One;
            var imag = dy[1];
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
                        MontgomeryModInt<T> t0 = a[j0], t1 = a[j1], t2 = a[j2], t3 = a[j3];
                        MontgomeryModInt<T> t0p1 = t0 + t1, t2p3 = t2 + t3;
                        MontgomeryModInt<T> t0m1 = t0 - t1, t2m3 = (t2 - t3) * imag;
                        a[j0] = t0p1 + t2p3;
                        a[j2] = t0p1 - t2p3;
                        a[j1] = t0m1 + t2m3;
                        a[j3] = t0m1 - t2m3;
                    }
                }
                // jh >= 1
                var xx = one * dy[2];
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
                        MontgomeryModInt<T> t0 = a[j0], t1 = a[j0 + v], t2 = a[j2], t3 = a[j2 + v];
                        MontgomeryModInt<T> t0p1 = t0 + t1, t2p3 = t2 + t3;
                        MontgomeryModInt<T> t0m1 = (t0 - t1) * xx, t2m3 = (t2 - t3) * yy;
                        a[j0] = t0p1 + t2p3;
                        a[j2] = (t0p1 - t2p3) * ww;
                        a[j0 + v] = t0m1 + t2m3;
                        a[j2 + v] = (t0m1 - t2m3) * ww;
                    }
                    xx *= dy[TrailingZeroCount(jh += 4)];
                }
                u >>= 4;
                v <<= 2;
            }
            if ((k & 1) != 0)
            {
                u = 1 << (k - 1);
                for (int j = 0; j < u; ++j)
                {
                    MontgomeryModInt<T> ajv = a[j] - a[j + u];
                    a[j] += a[j + u];
                    a[j + u] = ajv;
                }
            }
        }

        [凾(256)]
        internal static void NttLogical(Span<MontgomeryModInt<T>> a)
        {
            if (a.Length <= 1) return;
            Fft4(a, TrailingZeroCount(a.Length));
        }
        [凾(256)]
        internal static void INttLogical(Span<MontgomeryModInt<T>> a)
        {
            if (a.Length <= 1) return;
            IFft4(a, TrailingZeroCount(a.Length));
            var iv = new MontgomeryModInt<T>(a.Length).Inv();
            for (int i = 0; i < a.Length; i++) a[i] *= iv;
        }

        [凾(256)]
        internal static MontgomeryModInt<T>[] MultiplyLogical(ReadOnlySpan<MontgomeryModInt<T>> a, ReadOnlySpan<MontgomeryModInt<T>> b)
        {
            if (a.Length == 0 || b.Length == 0)
                return Array.Empty<MontgomeryModInt<T>>();
            var rt = new MontgomeryModInt<T>[a.Length + b.Length - 1];
            MultiplyLogical(a, b, rt);
            return rt;
        }

        [凾(512)]
        internal static void MultiplyLogical(ReadOnlySpan<MontgomeryModInt<T>> a, ReadOnlySpan<MontgomeryModInt<T>> b, Span<MontgomeryModInt<T>> rt)
        {
            if (Math.Min(a.Length, b.Length) <= 60)
            {
                MultiplyNative(a, b, rt);
                return;
            }

            Debug.Assert(rt.Length == a.Length + b.Length - 1);

            var k = InternalBit.CeilPow2(rt.Length);
            var M = 1 << k;

            MontgomeryModInt<T>[] buf1Pool, buf2Pool;
            var buf1 = (buf1Pool = ArrayPool<MontgomeryModInt<T>>.Shared.Rent(M)).AsSpan(0, M);
            var buf2 = (buf2Pool = ArrayPool<MontgomeryModInt<T>>.Shared.Rent(M)).AsSpan(0, M);
            try
            {
                a.CopyTo(buf1);
                b.CopyTo(buf2);
                buf1[a.Length..].Clear();
                buf2[b.Length..].Clear();

                Fft4(buf1, k);
                Fft4(buf2, k);
                for (int i = 0; i < buf1.Length; ++i)
                    buf1[i] *= buf2[i];
                IFft4(buf1, k);

                var invm = new MontgomeryModInt<T>(M).Inv();
                ref var buf1Ptr = ref MemoryMarshal.GetReference(buf1);

                for (int i = 0; i < rt.Length; ++i)
                    rt[i] = Unsafe.Add(ref buf1Ptr, i) * invm;
            }
            finally
            {
                ArrayPool<MontgomeryModInt<T>>.Shared.Return(buf1Pool);
                ArrayPool<MontgomeryModInt<T>>.Shared.Return(buf2Pool);
            }
        }
    }
}
