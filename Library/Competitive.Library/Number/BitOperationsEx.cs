using System.Numerics;
using System.Runtime.Intrinsics.X86;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using BO = BitOperations;
    public static class BitOperationsEx
    {
        /// <summary>
        /// <paramref name="x"/> の立っているビットの数
        /// </summary>
        [凾(256)]
        public static int PopCount(uint x) => BO.PopCount(x);
        /// <summary>
        /// <paramref name="x"/> の立っているビットの数
        /// </summary>
        [凾(256)]
        public static int PopCount(ulong x) => BO.PopCount(x);
        /// <summary>
        /// <paramref name="x"/> の立っているビットの数
        /// </summary>
        [凾(256)]
        public static int PopCount(int x) => BO.PopCount((uint)x);
        /// <summary>
        /// <paramref name="x"/> の立っているビットの数
        /// </summary>
        [凾(256)]
        public static int PopCount(long x) => BO.PopCount((ulong)x);
        /// <summary>
        /// <paramref name="x"/> の最上位ビット
        /// </summary>
        [凾(256)]
        public static int Msb(uint x) => BO.Log2(x);
        /// <summary>
        /// <paramref name="x"/> の最上位ビット
        /// </summary>
        [凾(256)]
        public static int Msb(ulong x) => BO.Log2(x);
        /// <summary>
        /// <paramref name="x"/> の最上位ビット
        /// </summary>
        [凾(256)]
        public static int Msb(int x) => BO.Log2((uint)x);
        /// <summary>
        /// <paramref name="x"/> の最上位ビット
        /// </summary>
        [凾(256)]
        public static int Msb(long x) => BO.Log2((ulong)x);
        /// <summary>
        /// <paramref name="x"/> の最下位ビット
        /// </summary>
        [凾(256)]
        public static int Lsb(uint x) => BO.TrailingZeroCount(x);
        /// <summary>
        /// <paramref name="x"/> の最下位ビット
        /// </summary>
        [凾(256)]
        public static int Lsb(ulong x) => BO.TrailingZeroCount(x);
        /// <summary>
        /// <paramref name="x"/> の最下位ビット
        /// </summary>
        [凾(256)]
        public static int Lsb(int x) => BO.TrailingZeroCount((uint)x);
        /// <summary>
        /// <paramref name="x"/> の最下位ビット
        /// </summary>
        [凾(256)]
        public static int Lsb(long x) => BO.TrailingZeroCount((ulong)x);

        /// <summary>
        /// <para><paramref name="x"/> を <paramref name="mask"/> に移す</para>
        /// </summary>
        /// <example>
        /// <para>ex. x=0b1101 mask=0b11110000 → 0b11010000</para>
        /// <para>https://atcoder.jp/contests/abc187/submissions/19171980</para>
        /// </example>
        [凾(256)]
        public static int ParallelBitDeposit(int x, int mask) => (int)ParallelBitDeposit((uint)x, (uint)mask);
        /// <summary>
        /// <para><paramref name="x"/> を <paramref name="mask"/> に移す</para>
        /// </summary>
        /// <example>
        /// <para>ex. x=0b1101 mask=0b11110000 → 0b11010000</para>
        /// <para>https://atcoder.jp/contests/abc187/submissions/19171980</para>
        /// </example>
        [凾(256)]
        public static uint ParallelBitDeposit(uint x, uint mask)
        {
            if (Bmi2.IsSupported)
                return Bmi2.ParallelBitDeposit(x, mask);
            return ParallelBitDepositLogic(x, mask);
        }
        /// <summary>
        /// <para><paramref name="x"/> を <paramref name="mask"/> に移す</para>
        /// </summary>
        /// <example>
        /// <para>ex. x=0b1101 mask=0b11110000 → 0b11010000</para>
        /// <para>https://atcoder.jp/contests/abc187/submissions/19171980</para>
        /// </example>
        [凾(256)]
        public static long ParallelBitDeposit(long x, long mask) => (long)ParallelBitDeposit((ulong)x, (ulong)mask);
        /// <summary>
        /// <para><paramref name="x"/> を <paramref name="mask"/> に移す</para>
        /// </summary>
        /// <example>
        /// <para>ex. x=0b1101 mask=0b11110000 → 0b11010000</para>
        /// <para>https://atcoder.jp/contests/abc187/submissions/19171980</para>
        /// </example>
        [凾(256)]
        public static ulong ParallelBitDeposit(ulong x, ulong mask)
        {
            if (Bmi2.X64.IsSupported)
                return Bmi2.X64.ParallelBitDeposit(x, mask);
            return ParallelBitDepositLogic(x, mask);
        }
        internal static T ParallelBitDepositLogic<T>(T x, T mask) where T : IBinaryInteger<T>
        {
            T res = T.Zero;
            for (int i = 0; !T.IsZero(mask); i++, mask >>= 1)
            {
                if (!T.IsZero(mask & T.One))
                {
                    res |= (x & T.One) << i;
                    x >>= 1;
                }
            }
            return res;
        }

        /// <summary>
        /// <para><paramref name="x"/> の <paramref name="mask"/> に合致する箇所を取り出す</para>
        /// </summary>
        /// <example>ex. x=0b01101 mask=0b11110 → 0b110</example>
        [凾(256)]
        public static int ParallelBitExtract(int x, int mask) => (int)ParallelBitExtract((uint)x, (uint)mask);
        /// <summary>
        /// <para><paramref name="x"/> の <paramref name="mask"/> に合致する箇所を取り出す</para>
        /// </summary>
        /// <example>ex. x=0b01101 mask=0b11110 → 0b110</example>
        [凾(256)]
        public static uint ParallelBitExtract(uint x, uint mask)
        {
            if (Bmi2.IsSupported)
                return Bmi2.ParallelBitExtract(x, mask);
            return ParallelBitExtractLogic(x, mask);
        }
        /// <summary>
        /// <para><paramref name="x"/> の <paramref name="mask"/> に合致する箇所を取り出す</para>
        /// </summary>
        /// <example>ex. x=0b01101 mask=0b11110 → 0b110</example>
        [凾(256)]
        public static long ParallelBitExtract(long x, long mask) => (long)ParallelBitExtract((ulong)x, (ulong)mask);
        /// <summary>
        /// <para><paramref name="x"/> の <paramref name="mask"/> に合致する箇所を取り出す</para>
        /// </summary>
        /// <example>ex. x=0b01101 mask=0b11110 → 0b110</example>
        [凾(256)]
        public static ulong ParallelBitExtract(ulong x, ulong mask)
        {
            if (Bmi2.X64.IsSupported)
                return Bmi2.X64.ParallelBitExtract(x, mask);
            return ParallelBitExtractLogic(x, mask);
        }
        internal static T ParallelBitExtractLogic<T>(T x, T mask) where T : IBinaryInteger<T>
        {
            T res = T.Zero;
            int k = 0;
            do
            {
                if (!T.IsZero(mask & T.One))
                    res |= (x & T.One) << k++;
                x >>= 1;
                mask >>= 1;
            } while (!T.IsZero(mask));
            return res;
        }

        /// <summary>
        /// ビットの並びを逆順にします。
        /// </summary>
        [凾(256)]
        public static uint BitReverse(uint x)
        {
            x = x << 16 | x >> 16;
            x = (x & 0x00ff00ff) << 8 | (x >> 8 & 0x00ff00ff);
            x = (x & 0x0f0f0f0f) << 4 | (x >> 4 & 0x0f0f0f0f);
            x = (x & 0x33333333) << 2 | (x >> 2 & 0x33333333);
            x = (x & 0x55555555) << 1 | (x >> 1 & 0x55555555);
            return x;
        }

        /// <summary>
        /// ビットの並びを逆順にします。
        /// </summary>
        [凾(256)]
        public static ulong BitReverse(ulong x)
            => (ulong)BitReverse((uint)x) << 32 | BitReverse((uint)(x >> 32));

        /// <summary>
        /// <para><paramref name="x"/> 以上で最も小さい2のべき乗を返します。</para>
        /// </summary>
        [凾(256)]
        public static int RoundUpToPowerOf2(int x)
            => (int)BO.RoundUpToPowerOf2((uint)x);
    }
}
