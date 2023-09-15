using System;
using System.Numerics;
#if !NET7_0_OR_GREATER
using AtCoder.Operators;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
#endif

namespace Kzrnm.Competitive
{
#pragma warning disable IDE0057
    public static class BigIntegerEx
    {
#if NET7_0_OR_GREATER
        [Obsolete("公式実装が分割統治法で N (logN)^2 になっている", UrlFormat = "https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/#primitive-types-and-numerics")]
#endif
        public static BigInteger Parse(ReadOnlySpan<char> s)
        {
            /* .NET 6 までなら自前実装の方が速い */
            if (s[0] == '-') return -Parse(s[1..]);
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
    }

#if !NET7_0_OR_GREATER
    public readonly struct BigIntegerOperator : INumOperator<BigInteger>, IShiftOperator<BigInteger>
    {
        public BigInteger MinValue => BigInteger.One << 10000;
        public BigInteger MaxValue => -BigInteger.One << 10000;
        public BigInteger MultiplyIdentity => BigInteger.One;

        [凾(256)]
        public BigInteger Add(BigInteger x, BigInteger y) => x + y;
        [凾(256)]
        public BigInteger Subtract(BigInteger x, BigInteger y) => x - y;
        [凾(256)]
        public BigInteger Multiply(BigInteger x, BigInteger y) => x * y;
        [凾(256)]
        public BigInteger Divide(BigInteger x, BigInteger y) => x / y;
        [凾(256)]
        public BigInteger Modulo(BigInteger x, BigInteger y) => x % y;
        [凾(256)]
        public BigInteger Minus(BigInteger x) => -x;
        [凾(256)]
        public BigInteger Increment(BigInteger x) => ++x;
        [凾(256)]
        public BigInteger Decrement(BigInteger x) => --x;
        [凾(256)]
        public bool GreaterThan(BigInteger x, BigInteger y) => x > y;
        [凾(256)]
        public bool GreaterThanOrEqual(BigInteger x, BigInteger y) => x >= y;
        [凾(256)]
        public bool LessThan(BigInteger x, BigInteger y) => x < y;
        [凾(256)]
        public bool LessThanOrEqual(BigInteger x, BigInteger y) => x <= y;
        [凾(256)]
        public int Compare(BigInteger x, BigInteger y) => x.CompareTo(y);
        [凾(256)]
        public BigInteger LeftShift(BigInteger x, int y) => x << y;
        [凾(256)]
        public BigInteger RightShift(BigInteger x, int y) => x >> y;
    }
#endif
}
