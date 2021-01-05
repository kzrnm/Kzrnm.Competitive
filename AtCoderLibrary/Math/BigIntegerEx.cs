using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
#pragma warning disable IDE0057
    public static class BigIntegerEx
    {
        public static BigInteger ParseBigInteger(ReadOnlySpan<char> s)
        {
            /* 自前実装の方が速い */
            if (s[0] == '-') return -ParseBigInteger(s[1..]);
            BigInteger res;
            if (s.Length % 9 == 0)
                res = 0;
            else
            {
                res = new BigInteger(int.Parse(s.Slice(0, s.Length % 9)));
                s = s.Slice(s.Length % 9);
            }

            while (s.Length > 0)
            {
                var sp = s.Slice(0, 9);
                res *= 1000_000_000;
                res += int.Parse(sp);
                s = s.Slice(9);
            }
            return res;
        }
        /// <summary>
        /// <paramref name="y"/> 乗した値を返します。
        /// </summary>
        public static BigInteger Pow(this BigInteger x, long y) => MathLibGeneric.Pow<BigInteger, BigIntegerOperator>(x, y);
    }

    public readonly struct BigIntegerOperator : INumOperator<BigInteger>, IShiftOperator<BigInteger>
    {
        public BigInteger MinValue => BigInteger.One << 10000;
        public BigInteger MaxValue => -BigInteger.One << 10000;
        public BigInteger MultiplyIdentity => BigInteger.One;

        [MethodImpl(AggressiveInlining)]
        public BigInteger Add(BigInteger x, BigInteger y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public BigInteger Subtract(BigInteger x, BigInteger y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public BigInteger Multiply(BigInteger x, BigInteger y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public BigInteger Divide(BigInteger x, BigInteger y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public BigInteger Modulo(BigInteger x, BigInteger y) => x % y;
        [MethodImpl(AggressiveInlining)]
        public BigInteger Minus(BigInteger x) => -x;
        [MethodImpl(AggressiveInlining)]
        public BigInteger Increment(BigInteger x) => ++x;
        [MethodImpl(AggressiveInlining)]
        public BigInteger Decrement(BigInteger x) => --x;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(BigInteger x, BigInteger y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(BigInteger x, BigInteger y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(BigInteger x, BigInteger y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(BigInteger x, BigInteger y) => x <= y;
        [MethodImpl(AggressiveInlining)]
        public int Compare(BigInteger x, BigInteger y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public BigInteger LeftShift(BigInteger x, int y) => x << y;
        [MethodImpl(AggressiveInlining)]
        public BigInteger RightShift(BigInteger x, int y) => x >> y;
    }
}
