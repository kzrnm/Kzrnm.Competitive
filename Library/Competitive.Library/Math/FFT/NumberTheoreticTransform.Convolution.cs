using AtCoder;
using AtCoder.Internal;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class NumberTheoreticTransform
    {
        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static uint[] Convolution(ReadOnlySpan<int> a, ReadOnlySpan<int> b, int mod)
             => Convolution(MemoryMarshal.Cast<int, uint>(a), MemoryMarshal.Cast<int, uint>(b), mod);

        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static uint[] Convolution(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b, int mod)
            => ConvolutionImpl(a, b, new AnyMod((uint)mod));

        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static uint[] Convolution<TMod>(ReadOnlySpan<int> a, ReadOnlySpan<int> b)
              where TMod : struct, IStaticMod
              => Convolution<TMod>(MemoryMarshal.Cast<int, uint>(a), MemoryMarshal.Cast<int, uint>(b));

        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static StaticModInt<TMod>[] Convolution<TMod>(StaticModInt<TMod>[] a, StaticModInt<TMod>[] b)
             where TMod : struct, IStaticMod
            => Convolution((ReadOnlySpan<StaticModInt<TMod>>)a, b);
        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static MontgomeryModInt<TMod>[] Convolution<TMod>(MontgomeryModInt<TMod>[] a, MontgomeryModInt<TMod>[] b)
             where TMod : struct, IStaticMod
            => Convolution((ReadOnlySpan<MontgomeryModInt<TMod>>)a, b);
        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static StaticModInt<TMod>[] Convolution<TMod>(Span<StaticModInt<TMod>> a, Span<StaticModInt<TMod>> b)
             where TMod : struct, IStaticMod
            => Convolution((ReadOnlySpan<StaticModInt<TMod>>)a, b);
        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static MontgomeryModInt<TMod>[] Convolution<TMod>(Span<MontgomeryModInt<TMod>> a, Span<MontgomeryModInt<TMod>> b)
             where TMod : struct, IStaticMod
            => Convolution((ReadOnlySpan<MontgomeryModInt<TMod>>)a, b);
        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static MontgomeryModInt<TMod>[] Convolution<TMod>(ReadOnlySpan<MontgomeryModInt<TMod>> a, ReadOnlySpan<MontgomeryModInt<TMod>> b)
             where TMod : struct, IStaticMod
        {
            if (a.Length + b.Length - 1 <= NumberTheoreticTransform<TMod>.NttLength())
                return NumberTheoreticTransform<TMod>.Multiply(a, b).ToArray();
            return ConvolutionImpl<TMod>(a.Select(m => (uint)m.Value), b.Select(m => (uint)m.Value))
                .Select(n => (MontgomeryModInt<TMod>)n)
                .ToArray();
        }
        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static StaticModInt<TMod>[] Convolution<TMod>(ReadOnlySpan<StaticModInt<TMod>> a, ReadOnlySpan<StaticModInt<TMod>> b)
             where TMod : struct, IStaticMod
        {
            if (a.Length + b.Length - 1 <= NumberTheoreticTransform<TMod>.NttLength())
                return NumberTheoreticTransform<TMod>.Multiply(a, b).ToArray();
            return
                MemoryMarshal.Cast<uint, StaticModInt<TMod>>(
                    ConvolutionImpl<TMod>(
                        MemoryMarshal.Cast<StaticModInt<TMod>, uint>(a),
                        MemoryMarshal.Cast<StaticModInt<TMod>, uint>(b))).ToArray();
        }
        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static uint[] Convolution<TMod>(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b)
             where TMod : struct, IStaticMod
        {
            if (a.Length + b.Length - 1 <= NumberTheoreticTransform<TMod>.NttLength())
                return NumberTheoreticTransform<TMod>.Multiply(a, b);
            return ConvolutionImpl<TMod>(a, b);
        }
        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        internal static uint[] ConvolutionImpl<TMod>(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b, TMod op = default)
             where TMod : struct, IStaticMod
        {
            if (a.Length == 0 || b.Length == 0)
                return Array.Empty<uint>();
            var r = new uint[a.Length + b.Length - 1];
            ConvolutionImpl(a, b, r, op);
            return r;
        }

        /// <summary>
        /// 任意 Mod で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        internal static void ConvolutionImpl<TMod>(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b, Span<uint> rt, TMod op)
             where TMod : struct, IStaticMod
        {
            unchecked
            {
                Debug.Assert(a.Length + b.Length - 1 == rt.Length);

                if (Math.Min(a.Length, b.Length) <= 60)
                {
                    ref var rPtr = ref MemoryMarshal.GetReference(rt);
                    for (int i = 0; i < a.Length; i++)
                        for (int j = 0; j < b.Length; j++)
                        {
                            var v = (uint)(Unsafe.Add(ref rPtr, i + j) + ((ulong)a[i] * b[j] % op.Mod));
                            if (v < op.Mod)
                                Unsafe.Add(ref rPtr, i + j) = v;
                            else
                                Unsafe.Add(ref rPtr, i + j) = v - op.Mod;
                        }
                    return;
                }

                const long Mod1 = 167772161;
                const long Mod2 = 469762049;
                const long Mod3 = 754974721;

                const long M1i2 = 104391568;
                const long M12i3 = 190329765;
                long M12i = (long)(ulong)(Mod1 * Mod2) % op.Mod;

                Debug.Assert(new FFTMod1().Mod == Mod1);
                Debug.Assert(new FFTMod2().Mod == Mod2);
                Debug.Assert(new FFTMod3().Mod == Mod3);
                Debug.Assert(M1i2 == new StaticModInt<FFTMod2>(Mod1).Inv().Value);
                Debug.Assert(M12i3 == new StaticModInt<FFTMod3>(Mod1 * Mod2).Inv().Value);

                var c1 = NumberTheoreticTransform<FFTMod1>.Multiply(a, b);
                var c2 = NumberTheoreticTransform<FFTMod2>.Multiply(a, b);
                var c3 = NumberTheoreticTransform<FFTMod3>.Multiply(a, b);

                Debug.Assert(c1.Length == rt.Length);
                Debug.Assert(c2.Length == rt.Length);
                Debug.Assert(c3.Length == rt.Length);

                ref var d1 = ref MemoryMarshal.GetReference<uint>(c1);
                ref var d2 = ref MemoryMarshal.GetReference<uint>(c2);
                ref var d3 = ref MemoryMarshal.GetReference<uint>(c3);

                for (int i = 0; i < rt.Length; i++)
                {
                    long dd = Unsafe.Add(ref d1, i);
                    var v1 = ((Unsafe.Add(ref d2, i) - dd) * M1i2) % Mod2;
                    if (v1 < 0) v1 += Mod2;
                    var v2 = (Unsafe.Add(ref d3, i) - (dd + Mod1 * v1) % Mod3) * M12i3 % Mod3;
                    if (v2 < 0) v2 += Mod3;
                    var x = (dd + Mod1 * v1 + M12i * v2) % op.Mod;
                    if (x < 0) x += op.Mod;

                    rt[i] = (uint)x;
                }
            }
        }

        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static ulong[] ConvolutionULong(uint[] a, uint[] b) => ConvolutionULong(a.AsSpan(), b);
        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static ulong[] ConvolutionULong(ulong[] a, ulong[] b) => ConvolutionULong(a.AsSpan(), b);

        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static ulong[] ConvolutionULong(Span<uint> a, Span<uint> b) => ConvolutionULong((ReadOnlySpan<uint>)a, b);
        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static ulong[] ConvolutionULong(Span<ulong> a, Span<ulong> b) => ConvolutionULong((ReadOnlySpan<ulong>)a, b);

        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static ulong[] ConvolutionULong(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b)
            => ConvolutionULong(a.Select(v => (ulong)v), b.Select(v => (ulong)v));

        /// <summary>
        /// 128 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static ulong[] ConvolutionULong(ReadOnlySpan<ulong> a, ReadOnlySpan<ulong> b)
        {
            unchecked
            {
                var n = a.Length;
                var m = b.Length;

                if (Math.Min(a.Length, b.Length) <= 60)
                {
                    if (n == 0 || m == 0)
                    {
                        return Array.Empty<ulong>();
                    }
                    var r = new ulong[n + m - 1];
                    ref var rPtr = ref MemoryMarshal.GetReference(r.AsSpan());
                    for (int i = 0; i < a.Length; i++)
                        for (int j = 0; j < b.Length; j++)
                            Unsafe.Add(ref rPtr, i + j) += a[i] * b[j];
                    return r;
                }
                return (ulong[])(object)ConvolutionInt64(
                    a.Select(n => (MontgomeryModInt<FFTMod1>)n), b.Select(n => (MontgomeryModInt<FFTMod1>)n),
                    a.Select(n => (MontgomeryModInt<FFTMod2>)n), b.Select(n => (MontgomeryModInt<FFTMod2>)n),
                    a.Select(n => (MontgomeryModInt<FFTMod3>)n), b.Select(n => (MontgomeryModInt<FFTMod3>)n));
            }
        }

        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static long[] ConvolutionLong(int[] a, int[] b) => ConvolutionLong(a.AsSpan(), b);
        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static long[] ConvolutionLong(long[] a, long[] b) => ConvolutionLong(a.AsSpan(), b);

        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static long[] ConvolutionLong(Span<int> a, Span<int> b) => ConvolutionLong((ReadOnlySpan<int>)a, b);
        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static long[] ConvolutionLong(Span<long> a, Span<long> b) => ConvolutionLong((ReadOnlySpan<long>)a, b);

        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static long[] ConvolutionLong(ReadOnlySpan<int> a, ReadOnlySpan<int> b)
            => ConvolutionLong(a.Select(v => (long)v), b.Select(v => (long)v));

        /// <summary>
        /// 64 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static long[] ConvolutionLong(ReadOnlySpan<long> a, ReadOnlySpan<long> b)
        {
            unchecked
            {
                var n = a.Length;
                var m = b.Length;

                if (Math.Min(a.Length, b.Length) <= 60)
                {
                    if (n == 0 || m == 0)
                    {
                        return Array.Empty<long>();
                    }
                    var r = new long[n + m - 1];
                    ref var rPtr = ref MemoryMarshal.GetReference(r.AsSpan());
                    for (int i = 0; i < a.Length; i++)
                        for (int j = 0; j < b.Length; j++)
                            Unsafe.Add(ref rPtr, i + j) += a[i] * b[j];
                    return r;
                }
                return (long[])(object)ConvolutionInt64(
                    a.Select(n => (MontgomeryModInt<FFTMod1>)n), b.Select(n => (MontgomeryModInt<FFTMod1>)n),
                    a.Select(n => (MontgomeryModInt<FFTMod2>)n), b.Select(n => (MontgomeryModInt<FFTMod2>)n),
                    a.Select(n => (MontgomeryModInt<FFTMod3>)n), b.Select(n => (MontgomeryModInt<FFTMod3>)n));
            }
        }

        [凾(256)]
        static ulong[] ConvolutionInt64(
            ReadOnlySpan<MontgomeryModInt<FFTMod1>> a1, ReadOnlySpan<MontgomeryModInt<FFTMod1>> b1,
            ReadOnlySpan<MontgomeryModInt<FFTMod2>> a2, ReadOnlySpan<MontgomeryModInt<FFTMod2>> b2,
            ReadOnlySpan<MontgomeryModInt<FFTMod3>> a3, ReadOnlySpan<MontgomeryModInt<FFTMod3>> b3)
        {
            unchecked
            {
                var n = a1.Length;
                var m = b1.Length;

                Debug.Assert(Math.Min(n, m) > 60);
                Debug.Assert(n == a2.Length);
                Debug.Assert(n == a3.Length);
                Debug.Assert(m == b2.Length);
                Debug.Assert(m == b3.Length);

                const ulong Mod1 = 167772161;
                const ulong Mod2 = 469762049;
                const ulong Mod3 = 754974721;
                const ulong M2M3 = Mod2 * Mod3;
                const ulong M1M3 = Mod1 * Mod3;
                const ulong M1M2 = Mod1 * Mod2;
                const ulong M1M2M3 = Mod1 * Mod2 * Mod3;

                const ulong i1 = 58587104;
                const ulong i2 = 187290749;
                const ulong i3 = 190329765;

                Debug.Assert(new FFTMod1().Mod == Mod1);
                Debug.Assert(new FFTMod2().Mod == Mod2);
                Debug.Assert(new FFTMod3().Mod == Mod3);
                Debug.Assert(i1 == (ulong)new StaticModInt<FFTMod1>(M2M3).Inv().Value);
                Debug.Assert(i2 == (ulong)new StaticModInt<FFTMod2>(M1M3).Inv().Value);
                Debug.Assert(i3 == (ulong)new StaticModInt<FFTMod3>(M1M2).Inv().Value);

                var c1 = NumberTheoreticTransform<FFTMod1>.Multiply(a1, b1);
                var c2 = NumberTheoreticTransform<FFTMod2>.Multiply(a2, b2);
                var c3 = NumberTheoreticTransform<FFTMod3>.Multiply(a3, b3);
                var c = new ulong[n + m - 1];

                Debug.Assert(c1.Length == c.Length);
                Debug.Assert(c2.Length == c.Length);
                Debug.Assert(c3.Length == c.Length);

                ref var d1 = ref MemoryMarshal.GetReference<MontgomeryModInt<FFTMod1>>(c1);
                ref var d2 = ref MemoryMarshal.GetReference<MontgomeryModInt<FFTMod2>>(c2);
                ref var d3 = ref MemoryMarshal.GetReference<MontgomeryModInt<FFTMod3>>(c3);

                for (int i = 0; i < c.Length; i++)
                {
                    ref ulong x = ref c[i];
                    x += ((ulong)Unsafe.Add(ref d1, i).Value * i1) % Mod1 * M2M3;
                    x += ((ulong)Unsafe.Add(ref d2, i).Value * i2) % Mod2 * M1M3;
                    x += ((ulong)Unsafe.Add(ref d3, i).Value * i3) % Mod3 * M1M2;

                    long diff = Unsafe.Add(ref d3, i).Value - InternalMath.SafeMod((long)x, (long)Mod3);
                    if (diff < 0)
                    {
                        diff += (long)Mod1;
                    }

                    // 真値を r, 得られた値を x, M1M2M3 % 2^64 = M', B = 2^63 として、
                    // r = x,
                    //     x -  M' + (0 or 2B),
                    //     x - 2M' + (0 or 2B or 4B),
                    //     x - 3M' + (0 or 2B or 4B or 6B)
                    // のいずれかが成り立つ、らしい
                    // -> see atcoder/convolution.hpp
                    switch (diff % 5)
                    {
                        case 2:
                            x -= M1M2M3;
                            break;
                        case 3:
                            x -= 2 * M1M2M3;
                            break;
                        case 4:
                            x -= 3 * M1M2M3;
                            break;
                    }
                }

                return c;
            }
        }

#if NET7_0_OR_GREATER

        /// <summary>
        /// 128 bit で畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [凾(256)]
        public static UInt128[] ConvolutionUInt128(ReadOnlySpan<UInt128> a, ReadOnlySpan<UInt128> b)
        {
            unchecked
            {
                var n = a.Length;
                var m = b.Length;

                if (Math.Min(a.Length, b.Length) <= 60)
                {
                    if (n == 0 || m == 0)
                    {
                        return Array.Empty<UInt128>();
                    }
                    var r = new UInt128[n + m - 1];
                    ref var rPtr = ref MemoryMarshal.GetReference(r.AsSpan());
                    for (int i = 0; i < a.Length; i++)
                        for (int j = 0; j < b.Length; j++)
                            Unsafe.Add(ref rPtr, i + j) += a[i] * b[j];
                    return r;
                }
                return ConvolutionInt128(
                    a.Select(n => (MontgomeryModInt<FFTMod1>)(ulong)(n % new FFTMod1().Mod)), b.Select(n => (MontgomeryModInt<FFTMod1>)(ulong)(n % new FFTMod1().Mod)),
                    a.Select(n => (MontgomeryModInt<FFTMod2>)(ulong)(n % new FFTMod2().Mod)), b.Select(n => (MontgomeryModInt<FFTMod2>)(ulong)(n % new FFTMod2().Mod)),
                    a.Select(n => (MontgomeryModInt<FFTMod3>)(ulong)(n % new FFTMod3().Mod)), b.Select(n => (MontgomeryModInt<FFTMod3>)(ulong)(n % new FFTMod3().Mod)));
            }
        }

        [凾(256)]
        static UInt128[] ConvolutionInt128(
            ReadOnlySpan<MontgomeryModInt<FFTMod1>> a1, ReadOnlySpan<MontgomeryModInt<FFTMod1>> b1,
            ReadOnlySpan<MontgomeryModInt<FFTMod2>> a2, ReadOnlySpan<MontgomeryModInt<FFTMod2>> b2,
            ReadOnlySpan<MontgomeryModInt<FFTMod3>> a3, ReadOnlySpan<MontgomeryModInt<FFTMod3>> b3)
        {
            unchecked
            {
                var n = a1.Length;
                var m = b1.Length;

                Debug.Assert(Math.Min(n, m) > 60);
                Debug.Assert(n == a2.Length);
                Debug.Assert(n == a3.Length);
                Debug.Assert(m == b2.Length);
                Debug.Assert(m == b3.Length);

                const ulong Mod1 = 167772161;
                const ulong Mod2 = 469762049;
                const ulong Mod3 = 754974721;

                const ulong M1i2 = 104391568;
                const ulong M2i3 = 399692502;
                const ulong M12i3 = 190329765;

                Debug.Assert(new FFTMod1().Mod == Mod1);
                Debug.Assert(new FFTMod2().Mod == Mod2);
                Debug.Assert(new FFTMod3().Mod == Mod3);
                Debug.Assert(M1i2 == (ulong)new StaticModInt<FFTMod2>(Mod1).Inv().Value);
                Debug.Assert(M2i3 == (ulong)new StaticModInt<FFTMod3>(Mod2).Inv().Value);
                Debug.Assert(M12i3 == (ulong)new StaticModInt<FFTMod3>(Mod1 * Mod2).Inv().Value);

                var c1 = NumberTheoreticTransform<FFTMod1>.Multiply(a1, b1);
                var c2 = NumberTheoreticTransform<FFTMod2>.Multiply(a2, b2);
                var c3 = NumberTheoreticTransform<FFTMod3>.Multiply(a3, b3);
                var c = new UInt128[n + m - 1];

                Debug.Assert(c1.Length == c.Length);
                Debug.Assert(c2.Length == c.Length);
                Debug.Assert(c3.Length == c.Length);

                ref var d1 = ref MemoryMarshal.GetReference<MontgomeryModInt<FFTMod1>>(c1);
                ref var d2 = ref MemoryMarshal.GetReference<MontgomeryModInt<FFTMod2>>(c2);
                ref var d3 = ref MemoryMarshal.GetReference<MontgomeryModInt<FFTMod3>>(c3);

                for (int i = 0; i < c.Length; i++)
                {
                    ulong a = (ulong)Unsafe.Add(ref d1, i).Value;
                    ulong v1 = ((ulong)Unsafe.Add(ref d2, i).Value + Mod2 - a) * M1i2 % Mod2;
                    ulong v2 = (((ulong)Unsafe.Add(ref d3, i).Value + Mod3 - a) * M12i3 + (Mod3 - v1) * M2i3) % Mod3;
                    c[i] = a + v1 * Mod1 + (UInt128)v2 * Mod1 * Mod2;
                }

                return c;
            }
        }
#endif


#if NET7_0_OR_GREATER
        private readonly record struct AnyMod(uint Mod) : IStaticMod
        {
            public bool IsPrime => false;
        }
#else
        private readonly struct AnyMod : IStaticMod
        {
            public AnyMod(uint m) { Mod = m; }
            public uint Mod { get; }
            public bool IsPrime => false;
        }
#endif
        private readonly struct FFTMod1 : IStaticMod
        {
            public uint Mod => 167772161;
            public bool IsPrime => true;
        }

        private readonly struct FFTMod2 : IStaticMod
        {
            public uint Mod => 469762049;
            public bool IsPrime => true;
        }

        private readonly struct FFTMod3 : IStaticMod
        {
            public uint Mod => 754974721;
            public bool IsPrime => true;
        }
    }
}
