using AtCoder;
using AtCoder.Internal;
using System;
using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// https://nyaannyaan.github.io/library/ntt/ntt-avx2.hpp
namespace Kzrnm.Competitive
{
    using static BitOperations;
    using static SimdMontgomery;
    /// <summary>
    /// 数論変換 number-theoretic transform
    /// </summary>
    public static partial class NumberTheoreticTransform<T> where T : struct, IStaticMod
    {
        [凾(256)]
        static ref Vector128<uint> ToVector128(ref MontgomeryModInt<T> m)
            => ref Unsafe.As<MontgomeryModInt<T>, Vector128<uint>>(ref m);
        [凾(256)]
        static ref Vector256<uint> ToVector256(ref MontgomeryModInt<T> m)
            => ref Unsafe.As<MontgomeryModInt<T>, Vector256<uint>>(ref m);
        [凾(256)]
        static void NttSimd(Span<MontgomeryModInt<T>> a)
        {
            int k = TrailingZeroCount(a.Length) & 31;
            if (k == 0) return;
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
                if (vv < 8)
                {
                    for (int j = 0; j < vv; ++j)
                    {
                        var ajv = a[j + vv];
                        a[j + vv] = a[j] - ajv;
                        a[j] += ajv;
                    }
                }
                else
                {
                    var m0 = Vector256<uint>.Zero;
                    var m2 = Vector256.Create(new T().Mod * 2);
                    for (int j0 = 0, j1 = vv; j0 < vv; j0 += 8, j1 += 8)
                    {
                        ref var T0 = ref ToVector256(ref a[j0]);
                        ref var T1 = ref ToVector256(ref a[j1]);

                        var naj = MontgomeryAdd(T0, T1, m2, m0);
                        var najv = MontgomerySubtract(T0, T1, m2, m0);
                        T0 = naj;
                        T1 = najv;
                    }
                }
            }
            int u = 1 << (2 + (k & 1));
            int v = 1 << (k - 2 - (k & 1));
            MontgomeryModInt<T> one = 1;
            var imag = dw[1];
            while (v != 0)
            {
                if (v == 1)
                {
                    var xx = one;
                    for (int jh = 0; jh < u;)
                    {
                        var ww = xx * xx;
                        var wx = ww * xx;
                        var t0 = a[jh + 0];
                        var t1 = a[jh + 1] * xx;
                        var t2 = a[jh + 2] * ww;
                        var t3 = a[jh + 3] * wx;
                        var t0p2 = t0 + t2;
                        var t1p3 = t1 + t3;
                        var t0m2 = t0 - t2;
                        var t1m3 = (t1 - t3) * imag;
                        a[jh + 0] = t0p2 + t1p3;
                        a[jh + 1] = t0p2 - t1p3;
                        a[jh + 2] = t0m2 + t1m3;
                        a[jh + 3] = t0m2 - t1m3;
                        xx *= dw[TrailingZeroCount(jh += 4)];
                    }
                }
                else if (v == 4)
                {
                    var m0 = Vector128<uint>.Zero;
                    var m1 = Vector128.Create(new T().Mod);
                    var m2 = Vector128.Create(new T().Mod * 2);
                    var r = Vector128.Create(MontgomeryModInt<T>.r);
                    var Imag = Vector128.Create(imag._v);
                    var xx = one;
                    for (int jh = 0; jh < u;)
                    {
                        if (jh == 0)
                        {
                            int j0 = 0;
                            int j1 = v;
                            int j2 = j1 + v;
                            int j3 = j2 + v;
                            int je = v;
                            for (; j0 < je; j0 += 4, j1 += 4, j2 += 4, j3 += 4)
                            {
                                ref var T0 = ref ToVector128(ref a[j0]);
                                ref var T1 = ref ToVector128(ref a[j1]);
                                ref var T2 = ref ToVector128(ref a[j2]);
                                ref var T3 = ref ToVector128(ref a[j3]);

                                var T0P2 = MontgomeryAdd(T0, T2, m2, m0);
                                var T1P3 = MontgomeryAdd(T1, T3, m2, m0);
                                var T0M2 = MontgomerySubtract(T0, T2, m2, m0);
                                var T1M3 = MontgomeryMultiply(MontgomerySubtract(T1, T3, m2, m0), Imag, r, m1);
                                T0 = MontgomeryAdd(T0P2, T1P3, m2, m0);
                                T1 = MontgomerySubtract(T0P2, T1P3, m2, m0);
                                T2 = MontgomeryAdd(T0M2, T1M3, m2, m0);
                                T3 = MontgomerySubtract(T0M2, T1M3, m2, m0);
                            }
                        }
                        else
                        {
                            var ww = xx * xx;
                            var wx = ww * xx;
                            var WW = Vector128.Create(ww._v);
                            var WX = Vector128.Create(wx._v);
                            var XX = Vector128.Create(xx._v);
                            int j0 = jh * v;
                            int j1 = j0 + v;
                            int j2 = j1 + v;
                            int j3 = j2 + v;
                            int je = j1;
                            for (; j0 < je; j0 += 4, j1 += 4, j2 += 4, j3 += 4)
                            {
                                ref var T0 = ref ToVector128(ref a[j0]);
                                ref var T1 = ref ToVector128(ref a[j1]);
                                ref var T2 = ref ToVector128(ref a[j2]);
                                ref var T3 = ref ToVector128(ref a[j3]);

                                var MT1 = MontgomeryMultiply(T1, XX, r, m1);
                                var MT2 = MontgomeryMultiply(T2, WW, r, m1);
                                var MT3 = MontgomeryMultiply(T3, WX, r, m1);
                                var T0P2 = MontgomeryAdd(T0, MT2, m2, m0);
                                var T1P3 = MontgomeryAdd(MT1, MT3, m2, m0);
                                var T0M2 = MontgomerySubtract(T0, MT2, m2, m0);
                                var T1M3 = MontgomeryMultiply(MontgomerySubtract(MT1, MT3, m2, m0), Imag, r, m1);
                                T0 = MontgomeryAdd(T0P2, T1P3, m2, m0);
                                T1 = MontgomerySubtract(T0P2, T1P3, m2, m0);
                                T2 = MontgomeryAdd(T0M2, T1M3, m2, m0);
                                T3 = MontgomerySubtract(T0M2, T1M3, m2, m0);
                            }
                        }
                        xx *= dw[TrailingZeroCount(jh += 4)];
                    }
                }
                else
                {
                    var m0 = Vector256<uint>.Zero;
                    var m1 = Vector256.Create(new T().Mod);
                    var m2 = Vector256.Create(new T().Mod * 2);
                    var r = Vector256.Create(MontgomeryModInt<T>.r);
                    var Imag = Vector256.Create(imag._v);
                    var xx = one;
                    for (int jh = 0; jh < u;)
                    {
                        if (jh == 0)
                        {
                            int j0 = 0;
                            int j1 = v;
                            int j2 = j1 + v;
                            int j3 = j2 + v;
                            int je = v;
                            for (; j0 < je; j0 += 8, j1 += 8, j2 += 8, j3 += 8)
                            {
                                ref var T0 = ref ToVector256(ref a[j0]);
                                ref var T1 = ref ToVector256(ref a[j1]);
                                ref var T2 = ref ToVector256(ref a[j2]);
                                ref var T3 = ref ToVector256(ref a[j3]);

                                var T0P2 = MontgomeryAdd(T0, T2, m2, m0);
                                var T1P3 = MontgomeryAdd(T1, T3, m2, m0);
                                var T0M2 = MontgomerySubtract(T0, T2, m2, m0);
                                var T1M3 = MontgomeryMultiply(MontgomerySubtract(T1, T3, m2, m0), Imag, r, m1);
                                T0 = MontgomeryAdd(T0P2, T1P3, m2, m0);
                                T1 = MontgomerySubtract(T0P2, T1P3, m2, m0);
                                T2 = MontgomeryAdd(T0M2, T1M3, m2, m0);
                                T3 = MontgomerySubtract(T0M2, T1M3, m2, m0);
                            }
                        }
                        else
                        {
                            var ww = xx * xx;
                            var wx = ww * xx;
                            var WW = Vector256.Create(ww._v);
                            var WX = Vector256.Create(wx._v);
                            var XX = Vector256.Create(xx._v);
                            int j0 = jh * v;
                            int j1 = j0 + v;
                            int j2 = j1 + v;
                            int j3 = j2 + v;
                            int je = j1;
                            for (; j0 < je; j0 += 8, j1 += 8, j2 += 8, j3 += 8)
                            {
                                ref var T0 = ref ToVector256(ref a[j0]);
                                ref var T1 = ref ToVector256(ref a[j1]);
                                ref var T2 = ref ToVector256(ref a[j2]);
                                ref var T3 = ref ToVector256(ref a[j3]);

                                var MT1 = MontgomeryMultiply(T1, XX, r, m1);
                                var MT2 = MontgomeryMultiply(T2, WW, r, m1);
                                var MT3 = MontgomeryMultiply(T3, WX, r, m1);
                                var T0P2 = MontgomeryAdd(T0, MT2, m2, m0);
                                var T1P3 = MontgomeryAdd(MT1, MT3, m2, m0);
                                var T0M2 = MontgomerySubtract(T0, MT2, m2, m0);
                                var T1M3 = MontgomeryMultiply(MontgomerySubtract(MT1, MT3, m2, m0), Imag, r, m1);
                                T0 = MontgomeryAdd(T0P2, T1P3, m2, m0);
                                T1 = MontgomerySubtract(T0P2, T1P3, m2, m0);
                                T2 = MontgomeryAdd(T0M2, T1M3, m2, m0);
                                T3 = MontgomerySubtract(T0M2, T1M3, m2, m0);
                            }
                        }
                        xx *= dw[TrailingZeroCount(jh += 4)];
                    }
                }
                u <<= 2;
                v >>= 2;
            }
        }
        [凾(256)]
        static void INttSimd(Span<MontgomeryModInt<T>> a, bool normalize = true)
        {
            int k = TrailingZeroCount(a.Length) & 31;
            if (k == 0) return;
            if (k == 1)
            {
                var a1 = a[1];
                a[1] = a[0] - a[1];
                a[0] = a[0] + a1;
                if (normalize)
                {
                    var iv2 = new MontgomeryModInt<T>(2).Inv();
                    a[0] *= iv2;
                    a[1] *= iv2;
                }
                return;
            }
            int u = 1 << (k - 2);
            int v = 1;
            MontgomeryModInt<T> one = 1, imag = dy[1];
            while (u != 0)
            {
                if (v == 1)
                {
                    var xx = one;
                    u <<= 2;
                    for (int jh = 0; jh < u;)
                    {
                        var ww = xx * xx;
                        var yy = xx * imag;
                        var t0 = a[jh + 0];
                        var t1 = a[jh + 1];
                        var t2 = a[jh + 2];
                        var t3 = a[jh + 3];
                        var t0p1 = t0 + t1;
                        var t2p3 = t2 + t3;
                        var t0m1 = (t0 - t1) * xx;
                        var t2m3 = (t2 - t3) * yy;
                        a[jh + 0] = t0p1 + t2p3;
                        a[jh + 2] = (t0p1 - t2p3) * ww;
                        a[jh + 1] = t0m1 + t2m3;
                        a[jh + 3] = (t0m1 - t2m3) * ww;
                        xx *= dy[TrailingZeroCount(jh += 4)];
                    }
                }
                else if (v == 4)
                {
                    var m0 = Vector128<uint>.Zero;
                    var m1 = Vector128.Create(new T().Mod);
                    var m2 = Vector128.Create(new T().Mod * 2);
                    var r = Vector128.Create(MontgomeryModInt<T>.r);
                    var Imag = Vector128.Create(imag._v);
                    var xx = one;
                    u <<= 2;
                    for (int jh = 0; jh < u;)
                    {
                        if (jh == 0)
                        {
                            int j0 = 0;
                            int j1 = v;
                            int j2 = v + v;
                            int j3 = j2 + v;
                            for (; j0 < v; j0 += 4, j1 += 4, j2 += 4, j3 += 4)
                            {
                                ref var T0 = ref ToVector128(ref a[j0]);
                                ref var T1 = ref ToVector128(ref a[j1]);
                                ref var T2 = ref ToVector128(ref a[j2]);
                                ref var T3 = ref ToVector128(ref a[j3]);

                                var T0P1 = MontgomeryAdd(T0, T1, m2, m0);
                                var T2P3 = MontgomeryAdd(T2, T3, m2, m0);
                                var T0M1 = MontgomerySubtract(T0, T1, m2, m0);
                                var T2M3 = MontgomeryMultiply(MontgomerySubtract(T2, T3, m2, m0), Imag, r, m1);
                                T0 = MontgomeryAdd(T0P1, T2P3, m2, m0);
                                T2 = MontgomerySubtract(T0P1, T2P3, m2, m0);
                                T1 = MontgomeryAdd(T0M1, T2M3, m2, m0);
                                T3 = MontgomerySubtract(T0M1, T2M3, m2, m0);
                            }
                        }
                        else
                        {
                            var ww = xx * xx;
                            var yy = xx * imag;
                            var WW = Vector128.Create(ww._v);
                            var XX = Vector128.Create(xx._v);
                            var YY = Vector128.Create(yy._v);
                            int j0 = jh * v;
                            int j1 = j0 + v;
                            int j2 = j1 + v;
                            int j3 = j2 + v;
                            int je = j1;
                            for (; j0 < je; j0 += 4, j1 += 4, j2 += 4, j3 += 4)
                            {
                                ref var T0 = ref ToVector128(ref a[j0]);
                                ref var T1 = ref ToVector128(ref a[j1]);
                                ref var T2 = ref ToVector128(ref a[j2]);
                                ref var T3 = ref ToVector128(ref a[j3]);

                                var T0P1 = MontgomeryAdd(T0, T1, m2, m0);
                                var T2P3 = MontgomeryAdd(T2, T3, m2, m0);
                                var T0M1 = MontgomeryMultiply(MontgomerySubtract(T0, T1, m2, m0), XX, r, m1);
                                var T2M3 = MontgomeryMultiply(MontgomerySubtract(T2, T3, m2, m0), YY, r, m1);
                                T0 = MontgomeryAdd(T0P1, T2P3, m2, m0);
                                T2 = MontgomeryMultiply(MontgomerySubtract(T0P1, T2P3, m2, m0), WW, r, m1);
                                T1 = MontgomeryAdd(T0M1, T2M3, m2, m0);
                                T3 = MontgomeryMultiply(MontgomerySubtract(T0M1, T2M3, m2, m0), WW, r, m1);
                            }
                        }
                        xx *= dy[TrailingZeroCount(jh += 4)];
                    }
                }
                else
                {
                    var m0 = Vector256<uint>.Zero;
                    var m1 = Vector256.Create(new T().Mod);
                    var m2 = Vector256.Create(new T().Mod * 2);
                    var r = Vector256.Create(MontgomeryModInt<T>.r);
                    var Imag = Vector256.Create(imag._v);

                    var xx = one;
                    u <<= 2;
                    for (int jh = 0; jh < u;)
                    {
                        if (jh == 0)
                        {
                            int j0 = 0;
                            int j1 = v;
                            int j2 = v + v;
                            int j3 = j2 + v;
                            for (; j0 < v; j0 += 8, j1 += 8, j2 += 8, j3 += 8)
                            {
                                ref var T0 = ref ToVector256(ref a[j0]);
                                ref var T1 = ref ToVector256(ref a[j1]);
                                ref var T2 = ref ToVector256(ref a[j2]);
                                ref var T3 = ref ToVector256(ref a[j3]);

                                var T0P1 = MontgomeryAdd(T0, T1, m2, m0);
                                var T2P3 = MontgomeryAdd(T2, T3, m2, m0);
                                var T0M1 = MontgomerySubtract(T0, T1, m2, m0);
                                var T2M3 = MontgomeryMultiply(MontgomerySubtract(T2, T3, m2, m0), Imag, r, m1);
                                T0 = MontgomeryAdd(T0P1, T2P3, m2, m0);
                                T2 = MontgomerySubtract(T0P1, T2P3, m2, m0);
                                T1 = MontgomeryAdd(T0M1, T2M3, m2, m0);
                                T3 = MontgomerySubtract(T0M1, T2M3, m2, m0);
                            }
                        }
                        else
                        {
                            var ww = xx * xx;
                            var yy = xx * imag;
                            var WW = Vector256.Create(ww._v);
                            var XX = Vector256.Create(xx._v);
                            var YY = Vector256.Create(yy._v);
                            int j0 = jh * v;
                            int j1 = j0 + v;
                            int j2 = j1 + v;
                            int j3 = j2 + v;
                            int je = j1;
                            for (; j0 < je; j0 += 8, j1 += 8, j2 += 8, j3 += 8)
                            {
                                ref var T0 = ref ToVector256(ref a[j0]);
                                ref var T1 = ref ToVector256(ref a[j1]);
                                ref var T2 = ref ToVector256(ref a[j2]);
                                ref var T3 = ref ToVector256(ref a[j3]);

                                var T0P1 = MontgomeryAdd(T0, T1, m2, m0);
                                var T2P3 = MontgomeryAdd(T2, T3, m2, m0);
                                var T0M1 = MontgomeryMultiply(MontgomerySubtract(T0, T1, m2, m0), XX, r, m1);
                                var T2M3 = MontgomeryMultiply(MontgomerySubtract(T2, T3, m2, m0), YY, r, m1);
                                T0 = MontgomeryAdd(T0P1, T2P3, m2, m0);
                                T2 = MontgomeryMultiply(MontgomerySubtract(T0P1, T2P3, m2, m0), WW, r, m1);
                                T1 = MontgomeryAdd(T0M1, T2M3, m2, m0);
                                T3 = MontgomeryMultiply(MontgomerySubtract(T0M1, T2M3, m2, m0), WW, r, m1);
                            }
                        }
                        xx *= dy[TrailingZeroCount(jh += 4)];
                    }
                }
                u >>= 4;
                v <<= 2;
            }
            if ((k & 1) != 0)
            {
                v = 1 << (k - 1);
                if (v < 8)
                {
                    for (int j = 0; j < v; ++j)
                    {
                        var ajv = a[j] - a[j + v];
                        a[j] += a[j + v];
                        a[j + v] = ajv;
                    }
                }
                else
                {
                    var m0 = Vector256<uint>.Zero;
                    var m2 = Vector256.Create(new T().Mod * 2);
                    int j0 = 0;
                    int j1 = v;
                    for (; j0 < v; j0 += 8, j1 += 8)
                    {
                        ref var T0 = ref ToVector256(ref a[j0]);
                        ref var T1 = ref ToVector256(ref a[j1]);
                        var naj = MontgomeryAdd(T0, T1, m2, m0);
                        var najv = MontgomerySubtract(T0, T1, m2, m0);
                        T0 = naj;
                        T1 = najv;
                    }
                }
            }
            if (normalize)
            {
                var invn = new MontgomeryModInt<T>(a.Length).Inv();
                for (int i = 0; i < a.Length; i++)
                    a[i] *= invn;
            }
        }

        [凾(512)]
        static MontgomeryModInt<T>[] MultiplySimd(ReadOnlySpan<MontgomeryModInt<T>> a, ReadOnlySpan<MontgomeryModInt<T>> b)
        {
            if (Math.Min(a.Length, b.Length) <= 60)
                return MultiplyNative(a, b);

            int len = a.Length + b.Length - 1;

            var k = InternalBit.CeilPow2(len);
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

                NttSimd(buf1);
                NttSimd(buf2);
                for (int i = 0; i < buf1.Length; i++)
                    buf1[i]._v = MontgomeryModInt<T>.Reduce((ulong)buf1[i]._v * buf2[i]._v);

                INttSimd(buf1, false);

                var invm = new MontgomeryModInt<T>(M).Inv();
                buf1 = buf1[..len];

                for (int i = 0; i < buf1.Length; ++i)
                    buf1[i] *= invm;
                return buf1.ToArray();
            }
            finally
            {
                ArrayPool<MontgomeryModInt<T>>.Shared.Return(buf1Pool);
                ArrayPool<MontgomeryModInt<T>>.Shared.Return(buf2Pool);
            }
        }
    }
}
