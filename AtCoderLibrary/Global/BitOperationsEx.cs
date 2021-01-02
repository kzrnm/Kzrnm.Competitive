using System.Numerics;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
    public static class BitOperationsEx
    {
        [MethodImpl(AggressiveInlining)]
        public static int PopCount(int x) => BitOperations.PopCount((uint)x);
        [MethodImpl(AggressiveInlining)]
        public static int PopCount(long x) => BitOperations.PopCount((ulong)x);
        [MethodImpl(AggressiveInlining)]
        public static int MSB(int x) => BitOperations.Log2((uint)x);
        [MethodImpl(AggressiveInlining)]
        public static int MSB(long x) => BitOperations.Log2((ulong)x);
        [MethodImpl(AggressiveInlining)]
        public static int LSB(int x) => BitOperations.TrailingZeroCount((uint)x);
        [MethodImpl(AggressiveInlining)]
        public static int LSB(long x) => BitOperations.TrailingZeroCount((ulong)x);
    }
}
