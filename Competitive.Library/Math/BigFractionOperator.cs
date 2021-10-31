using AtCoder.Operators;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    public struct BigFractionOperator : INumOperator<BigFraction>, ICompareOperator<BigFraction>
    {
        public BigFraction MinValue => BigInteger.MinusOne << 10000;
        public BigFraction MaxValue => BigInteger.One << 10000;
        public BigFraction MultiplyIdentity => new BigFraction(1, 1);

        [MethodImpl(AggressiveInlining)]
        public BigFraction Add(BigFraction x, BigFraction y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public BigFraction Subtract(BigFraction x, BigFraction y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public BigFraction Multiply(BigFraction x, BigFraction y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public BigFraction Divide(BigFraction x, BigFraction y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public BigFraction Modulo(BigFraction x, BigFraction y) => throw new NotSupportedException();

        [MethodImpl(AggressiveInlining)]
        public int Compare(BigFraction x, BigFraction y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(BigFraction x, BigFraction y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(BigFraction x, BigFraction y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(BigFraction x, BigFraction y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(BigFraction x, BigFraction y) => x <= y;

        [MethodImpl(AggressiveInlining)]
        public BigFraction Minus(BigFraction x) => -x;
        [MethodImpl(AggressiveInlining)]
        public BigFraction Increment(BigFraction x) => throw new NotSupportedException();
        [MethodImpl(AggressiveInlining)]
        public BigFraction Decrement(BigFraction x) => throw new NotSupportedException();
    }
}
