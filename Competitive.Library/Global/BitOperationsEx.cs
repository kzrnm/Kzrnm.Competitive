using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    public static class BitOperationsEx
    {
        /// <summary>
        /// <paramref name="x"/> の立っているビットの数
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public static int PopCount(int x) => BitOperations.PopCount((uint)x);
        /// <summary>
        /// <paramref name="x"/> の立っているビットの数
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public static int PopCount(long x) => BitOperations.PopCount((ulong)x);
        /// <summary>
        /// <paramref name="x"/> の最上位ビット
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public static int MSB(int x) => BitOperations.Log2((uint)x);
        /// <summary>
        /// <paramref name="x"/> の最上位ビット
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public static int MSB(long x) => BitOperations.Log2((ulong)x);
        /// <summary>
        /// <paramref name="x"/> の最下位ビット
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public static int LSB(int x) => BitOperations.TrailingZeroCount((uint)x);
        /// <summary>
        /// <paramref name="x"/> の最下位ビット
        /// </summary>
        [MethodImpl(AggressiveInlining)]
        public static int LSB(long x) => BitOperations.TrailingZeroCount((ulong)x);

        /// <summary>
        /// <para><paramref name="x"/> を <paramref name="mask"/> に移す</para>
        /// </summary>
        /// <returns>ex. x=0b1101 mask=0b11110000 → 0b11010000</returns>
        [MethodImpl(AggressiveInlining)]
        public static int ParallelBitDeposit(int x, uint mask) => (int)ParallelBitDeposit((uint)x, mask);
        /// <summary>
        /// <para><paramref name="x"/> を <paramref name="mask"/> に移す</para>
        /// </summary>
        /// <returns>ex. x=0b1101 mask=0b11110000 → 0b11010000</returns>
        [MethodImpl(AggressiveInlining)]
        public static uint ParallelBitDeposit(uint x, uint mask)
        {
            if (Bmi2.IsSupported)
                return Bmi2.ParallelBitDeposit(x, mask);
            return ParallelBitDepositLogic(x, mask);
        }
        internal static uint ParallelBitDepositLogic(uint x, uint mask)
        {
            uint res = 0;
            for (int i = 0; mask > 0; i++, mask >>= 1)
            {
                if ((mask & 1U) != 0)
                {
                    res |= (x & 1U) << i;
                    x >>= 1;
                }
            }
            return res;
        }

        /// <summary>
        /// <para><paramref name="x"/> の <paramref name="mask"/> に合致する箇所を取り出す</para>
        /// </summary>
        /// <returns>ex. x=0b01101 mask=0b11110 → 0b110</returns>
        [MethodImpl(AggressiveInlining)]
        public static int ParallelBitExtract(int x, uint mask) => (int)ParallelBitExtract((uint)x, mask);
        /// <summary>
        /// <para><paramref name="x"/> の <paramref name="mask"/> に合致する箇所を取り出す</para>
        /// </summary>
        /// <returns>ex. x=0b101101 mask=0b011110 → 0b110</returns>
        [MethodImpl(AggressiveInlining)]
        public static uint ParallelBitExtract(uint x, uint mask)
        {
            if (Bmi2.IsSupported)
                return Bmi2.ParallelBitExtract(x, mask);
            return ParallelBitExtractLogic(x, mask);
        }
        internal static uint ParallelBitExtractLogic(uint x, uint mask)
        {
            uint res = 0;
            int k = 0;
            do
            {
                if ((mask & 1U) != 0)
                    res |= (x & 1U) << k++;
                x >>= 1;
                mask >>= 1;
            } while (mask != 0);
            return res;
        }
    }
}
