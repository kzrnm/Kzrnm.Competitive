using AtCoder;
using AtCoder.Internal;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class ConvolutionAnyMod
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
        {
            uint umod = (uint)mod;
            unchecked
            {
                var n = a.Length;
                var m = b.Length;

                var la = new long[n];
                for (int i = 0; i < la.Length; i++) la[i] = a[i] % umod;
                var lb = new long[m];
                for (int i = 0; i < lb.Length; i++) lb[i] = b[i] % umod;

                if (n == 0 || m == 0)
                {
                    return Array.Empty<uint>();
                }

                const long Mod1 = 167772161;
                const long Mod2 = 469762049;
                const long Mod3 = 754974721;

                const long M1i2 = 104391568;
                const long M12i3 = 190329765;
                long M12i = (long)(ulong)(Mod1 * Mod2) % umod;

                Debug.Assert(new FFTMod1().Mod == Mod1);
                Debug.Assert(new FFTMod2().Mod == Mod2);
                Debug.Assert(new FFTMod3().Mod == Mod3);
                Debug.Assert(M1i2 == new StaticModInt<FFTMod2>(Mod1).Inv().Value);
                Debug.Assert(M12i3 == new StaticModInt<FFTMod3>(Mod1 * Mod2).Inv().Value);

                var c1 = MathLib.Convolution<FFTMod1>(la, lb);
                var c2 = MathLib.Convolution<FFTMod2>(la, lb);
                var c3 = MathLib.Convolution<FFTMod3>(la, lb);

                var c = new uint[n + m - 1];
                for (int i = 0; i < c.Length; i++)
                {
                    var v1 = ((c2[i] - c1[i]) * M1i2) % Mod2;
                    if (v1 < 0) v1 += Mod2;
                    var v2 = (c3[i] - (c1[i] + Mod1 * v1) % Mod3) * M12i3 % Mod3;
                    if (v2 < 0) v2 += Mod3;
                    var x = (c1[i] + Mod1 * v1 + M12i * v2) % mod;
                    if (x < 0) x += mod;

                    c[i] = (uint)x;
                }

                return c;
            }
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
        public static StaticModInt<TMod>[] Convolution<TMod>(ReadOnlySpan<StaticModInt<TMod>> a, ReadOnlySpan<StaticModInt<TMod>> b)
             where TMod : struct, IStaticMod
        {
            var mod = new TMod().Mod;
            if (new TMod().IsPrime && a.Length + b.Length - 1 <= (1 << InternalBit.Bsf(mod - 1)))
            {
                // ACL で解けるならOK
                return MathLib.Convolution(a, b);
            }
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
            if (new TMod().IsPrime && a.Length + b.Length - 1 <= (1 << InternalBit.Bsf(new TMod().Mod - 1)))
            {
                // ACL で解けるならOK
                return MathLib.Convolution<TMod>(a, b);
            }
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
        private static uint[] ConvolutionImpl<TMod>(ReadOnlySpan<uint> a, ReadOnlySpan<uint> b)
             where TMod : struct, IStaticMod
        {
            var op = new TMod();
            unchecked
            {
                var n = a.Length;
                var m = b.Length;

                var la = new long[n];
                for (int i = 0; i < la.Length; i++) la[i] = a[i] % op.Mod;
                var lb = new long[m];
                for (int i = 0; i < lb.Length; i++) lb[i] = b[i] % op.Mod;

                if (n == 0 || m == 0)
                {
                    return Array.Empty<uint>();
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

                var c1 = MathLib.Convolution<FFTMod1>(la, lb);
                var c2 = MathLib.Convolution<FFTMod2>(la, lb);
                var c3 = MathLib.Convolution<FFTMod3>(la, lb);

                var c = new uint[n + m - 1];
                for (int i = 0; i < c.Length; i++)
                {
                    var v1 = ((c2[i] - c1[i]) * M1i2) % Mod2;
                    if (v1 < 0) v1 += Mod2;
                    var v2 = (c3[i] - (c1[i] + Mod1 * v1) % Mod3) * M12i3 % Mod3;
                    if (v2 < 0) v2 += Mod3;
                    var x = (c1[i] + Mod1 * v1 + M12i * v2) % op.Mod;
                    if (x < 0) x += op.Mod;

                    c[i] = (uint)x;
                }

                return c;
            }
        }

        /// <summary>
        /// NTT(数論変換)で使えるかどうかを返します。
        /// </summary>
        [凾(256)]
        internal static bool CanNtt<TMod>() where TMod : struct, IStaticMod
        {
            var m = new TMod();
            return m.IsPrime && BitOperations.TrailingZeroCount(m.Mod - 1) >= 23;
        }

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
