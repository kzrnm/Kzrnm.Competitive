using AtCoder;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    /// <summary>有理数を既約分数で表す</summary>
    public readonly struct BigFraction : IEquatable<BigFraction>, IComparable<BigFraction>
    {
        public static readonly BigFraction NaN = new BigFraction(0, 0);
        public bool IsNaN => _denominator < 0;

        /// <summary>分子</summary>
        private readonly BigInteger _numerator;
        /// <summary>分子</summary>
        public BigInteger Numerator => _numerator;
        /// <summary>分母 - 1 (default を 0/0 ではなく 0/1 にしたい)</summary>
        private readonly BigInteger _denominator;
        /// <summary>分母</summary>
        public BigInteger Denominator => _denominator + 1;

        public BigFraction(BigInteger 分子, BigInteger 分母)
        {
            if (分母 == 0)
            {
                _numerator = long.MinValue;
                _denominator = long.MinValue;
                return;
            }
            var negative = (分子 ^ 分母) < 0;
            分子 = BigInteger.Abs(分子);
            分母 = BigInteger.Abs(分母);
            if (分子 == 0)
            {
                _numerator = 0;
                _denominator = 0;
            }
            else
            {
                var gcd = BigInteger.GreatestCommonDivisor(分母, 分子);
                _numerator = 分子 / gcd;
                if (negative)
                    _numerator = -_numerator;
                _denominator = 分母 / gcd - 1;
            }
        }
        public override string ToString() => $"{Numerator}/{Denominator}";
        public override bool Equals(object obj) => obj is BigFraction f && Equals(f);
        public bool Equals(BigFraction other) => this._numerator == other._numerator && this._denominator == other._denominator;
        public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

        public static implicit operator BigFraction(long x) => new BigFraction(x, 1);
        public static implicit operator BigFraction(BigInteger x) => new BigFraction(x, 1);
        public int CompareTo(BigFraction other) => (this.Numerator * other.Denominator).CompareTo(other.Numerator * this.Denominator);

        public static BigFraction operator -(BigFraction x) => new BigFraction(-x.Numerator, x.Denominator);
        public static BigFraction operator +(BigFraction x, BigFraction y)
        {
            var gcd = BigInteger.GreatestCommonDivisor(x.Denominator, y.Denominator);
            var lcm = x.Denominator / gcd * y.Denominator;
            return new BigFraction((x.Numerator * y.Denominator + y.Numerator * x.Denominator) / gcd, lcm);
        }
        public static BigFraction operator -(BigFraction x, BigFraction y)
        {
            var gcd = BigInteger.GreatestCommonDivisor(x.Denominator, y.Denominator);
            var lcm = x.Denominator / gcd * y.Denominator;
            return new BigFraction((x.Numerator * y.Denominator - y.Numerator * x.Denominator) / gcd, lcm);
        }
        public static BigFraction operator *(BigFraction x, BigFraction y) => new BigFraction(x.Numerator * y.Numerator, x.Denominator * y.Denominator);
        public static BigFraction operator /(BigFraction x, BigFraction y) => new BigFraction(x.Numerator * y.Denominator, x.Denominator * y.Numerator);
        public static bool operator ==(BigFraction x, BigFraction y) => x.Equals(y);
        public static bool operator !=(BigFraction x, BigFraction y) => !x.Equals(y);
        public static bool operator >=(BigFraction x, BigFraction y) => x.CompareTo(y) >= 0;
        public static bool operator <=(BigFraction x, BigFraction y) => x.CompareTo(y) <= 0;
        public static bool operator >(BigFraction x, BigFraction y) => x.CompareTo(y) > 0;
        public static bool operator <(BigFraction x, BigFraction y) => x.CompareTo(y) < 0;

        public BigFraction Inverse() => new BigFraction(Denominator, Numerator);
        public double ToDouble() => (double)Numerator / (double)Denominator;
    }
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
