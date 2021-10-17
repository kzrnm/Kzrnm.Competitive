using AtCoder;
using AtCoder.Operators;
using System;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    /// <summary>有理数を既約分数で表す</summary>
    public readonly struct Fraction : IEquatable<Fraction>, IComparable<Fraction>
    {
        public static readonly Fraction NaN = new Fraction(0, 0);
        public bool IsNaN => _denominator < 0;

        /// <summary>分子</summary>
        private readonly long _numerator;
        /// <summary>分子</summary>
        public long Numerator => _numerator;
        /// <summary>分母 - 1 (default を 0/0 ではなく 0/1 にしたい)</summary>
        private readonly long _denominator;
        /// <summary>分母</summary>
        public long Denominator => _denominator + 1;

        public Fraction(long 分子, long 分母)
        {
            if (分母 == 0)
            {
                _numerator = long.MinValue;
                _denominator = long.MinValue;
                return;
            }
            var negative = (分子 ^ 分母) < 0;
            分子 = Math.Abs(分子);
            分母 = Math.Abs(分母);
            if (分子 == 0)
            {
                _numerator = 0;
                _denominator = 0;
            }
            else
            {
                var gcd = MathLibEx.Gcd(分母, 分子);
                _numerator = 分子 / gcd;
                if (negative)
                    _numerator = -_numerator;
                _denominator = 分母 / gcd - 1;
            }
        }
        public override string ToString() => $"{Numerator}/{Denominator}";
        public override bool Equals(object obj) => obj is Fraction f && Equals(f);
        public bool Equals(Fraction other) => this._numerator == other._numerator && this._denominator == other._denominator;
        public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

        public static implicit operator Fraction(long x) => new Fraction(x, 1);
        public int CompareTo(Fraction other) => (this.Numerator * other.Denominator).CompareTo(other.Numerator * this.Denominator);

        public static Fraction operator -(Fraction x) => new Fraction(-x.Numerator, x.Denominator);
        public static Fraction operator +(Fraction x, Fraction y)
        {
            var gcd = MathLibEx.Gcd(x.Denominator, y.Denominator);
            var lcm = x.Denominator / gcd * y.Denominator;
            return new Fraction((x.Numerator * y.Denominator + y.Numerator * x.Denominator) / gcd, lcm);
        }
        public static Fraction operator -(Fraction x, Fraction y)
        {
            var gcd = MathLibEx.Gcd(x.Denominator, y.Denominator);
            var lcm = x.Denominator / gcd * y.Denominator;
            return new Fraction((x.Numerator * y.Denominator - y.Numerator * x.Denominator) / gcd, lcm);
        }
        public static Fraction operator *(Fraction x, Fraction y) => new Fraction(x.Numerator * y.Numerator, x.Denominator * y.Denominator);
        public static Fraction operator /(Fraction x, Fraction y) => new Fraction(x.Numerator * y.Denominator, x.Denominator * y.Numerator);
        public static bool operator ==(Fraction x, Fraction y) => x.Equals(y);
        public static bool operator !=(Fraction x, Fraction y) => !x.Equals(y);
        public static bool operator >=(Fraction x, Fraction y) => x.CompareTo(y) >= 0;
        public static bool operator <=(Fraction x, Fraction y) => x.CompareTo(y) <= 0;
        public static bool operator >(Fraction x, Fraction y) => x.CompareTo(y) > 0;
        public static bool operator <(Fraction x, Fraction y) => x.CompareTo(y) < 0;

        public Fraction Inverse() => new Fraction(Denominator, Numerator);
        public double ToDouble() => (double)Numerator / Denominator;
    }
    public struct FractionOperator : INumOperator<Fraction>, ICompareOperator<Fraction>
    {
        public Fraction MinValue => new Fraction(long.MinValue, 1);
        public Fraction MaxValue => new Fraction(long.MaxValue, 1);
        public Fraction MultiplyIdentity => new Fraction(1, 1);

        [MethodImpl(AggressiveInlining)]
        public Fraction Add(Fraction x, Fraction y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public Fraction Subtract(Fraction x, Fraction y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public Fraction Multiply(Fraction x, Fraction y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public Fraction Divide(Fraction x, Fraction y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public Fraction Modulo(Fraction x, Fraction y) => throw new NotSupportedException();

        [MethodImpl(AggressiveInlining)]
        public int Compare(Fraction x, Fraction y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(Fraction x, Fraction y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(Fraction x, Fraction y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(Fraction x, Fraction y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(Fraction x, Fraction y) => x <= y;

        [MethodImpl(AggressiveInlining)]
        public Fraction Minus(Fraction x) => -x;
        [MethodImpl(AggressiveInlining)]
        public Fraction Increment(Fraction x) => throw new NotSupportedException();
        [MethodImpl(AggressiveInlining)]
        public Fraction Decrement(Fraction x) => throw new NotSupportedException();
    }
}
