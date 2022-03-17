using AtCoder.Operators;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public struct BigFractionOperator : INumOperator<BigFraction>, ICompareOperator<BigFraction>
    {
        public BigFraction MinValue => BigInteger.MinusOne << 10000;
        public BigFraction MaxValue => BigInteger.One << 10000;
        public BigFraction MultiplyIdentity => new BigFraction(1, 1);

        [凾(256)]
        public BigFraction Add(BigFraction x, BigFraction y) => x + y;
        [凾(256)]
        public BigFraction Subtract(BigFraction x, BigFraction y) => x - y;
        [凾(256)]
        public BigFraction Multiply(BigFraction x, BigFraction y) => x * y;
        [凾(256)]
        public BigFraction Divide(BigFraction x, BigFraction y) => x / y;
        [凾(256)]
        public BigFraction Modulo(BigFraction x, BigFraction y) => throw new NotSupportedException();

        [凾(256)]
        public int Compare(BigFraction x, BigFraction y) => x.CompareTo(y);
        [凾(256)]
        public bool GreaterThan(BigFraction x, BigFraction y) => x > y;
        [凾(256)]
        public bool GreaterThanOrEqual(BigFraction x, BigFraction y) => x >= y;
        [凾(256)]
        public bool LessThan(BigFraction x, BigFraction y) => x < y;
        [凾(256)]
        public bool LessThanOrEqual(BigFraction x, BigFraction y) => x <= y;

        [凾(256)]
        public BigFraction Minus(BigFraction x) => -x;
        [凾(256)]
        public BigFraction Increment(BigFraction x) => throw new NotSupportedException();
        [凾(256)]
        public BigFraction Decrement(BigFraction x) => throw new NotSupportedException();
    }
}
