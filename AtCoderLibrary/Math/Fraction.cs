using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
    /** <summary>有理数を既約分数で表す</summary> */
    public readonly struct Fraction : IEquatable<Fraction>, IComparable<Fraction>
    {
        /** <summary>分子</summary> */
        public readonly long numerator;
        /** <summary>分母</summary> */
        public readonly long denominator;
        public Fraction(long 分子, long 分母)
        {
            var negative = (分子 ^ 分母) < 0;
            分子 = Math.Abs(分子);
            分母 = Math.Abs(分母);
            var gcd = MathLibEx.Gcd(分母, 分子);
            numerator = 分子 / gcd;
            if (negative)
                numerator = -numerator;
            denominator = 分母 / gcd;
        }
        public override string ToString() => $"{numerator}/{denominator}";
        public override bool Equals(object obj) => obj is Fraction f && Equals(f);
        public bool Equals(Fraction other) => this.numerator == other.numerator && this.denominator == other.denominator;
        public override int GetHashCode() => HashCode.Combine(numerator, denominator);

        public static implicit operator Fraction(long x) => new Fraction(x, 1);
        public int CompareTo(Fraction other) => (this.numerator * other.denominator).CompareTo(other.numerator * this.denominator);

        public static Fraction operator -(Fraction x) => new Fraction(-x.numerator, x.denominator);
        public static Fraction operator +(Fraction x, Fraction y)
        {
            var gcd = MathLibEx.Gcd(x.denominator, y.denominator);
            var lcm = x.denominator / gcd * y.denominator;
            return new Fraction((x.numerator * y.denominator + y.numerator * x.denominator) / gcd, lcm);
        }
        public static Fraction operator -(Fraction x, Fraction y)
        {
            var gcd = MathLibEx.Gcd(x.denominator, y.denominator);
            var lcm = x.denominator / gcd * y.denominator;
            return new Fraction((x.numerator * y.denominator - y.numerator * x.denominator) / gcd, lcm);
        }
        public static Fraction operator *(Fraction x, Fraction y) => new Fraction(x.numerator * y.numerator, x.denominator * y.denominator);
        public static Fraction operator /(Fraction x, Fraction y) => new Fraction(x.numerator * y.denominator, x.denominator * y.numerator);
        public static bool operator ==(Fraction x, Fraction y) => x.Equals(y);
        public static bool operator !=(Fraction x, Fraction y) => !x.Equals(y);
        public static bool operator >=(Fraction x, Fraction y) => x.CompareTo(y) >= 0;
        public static bool operator <=(Fraction x, Fraction y) => x.CompareTo(y) <= 0;
        public static bool operator >(Fraction x, Fraction y) => x.CompareTo(y) > 0;
        public static bool operator <(Fraction x, Fraction y) => x.CompareTo(y) < 0;

        public Fraction Inverse() => new Fraction(denominator, numerator);
        public double ToDouble() => (double)numerator / denominator;
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
