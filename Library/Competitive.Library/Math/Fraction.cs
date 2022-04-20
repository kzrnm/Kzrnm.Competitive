using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
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
                _numerator = Math.Sign(分子) switch
                {
                    0 => 0,
                    1 => int.MaxValue,
                    _ => int.MinValue,
                };
                _denominator = -1;
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
        private Fraction(long 分子, long 分母, bool _)
        {
            _numerator = 分子;
            _denominator = 分母 - 1;
        }
        [凾(256)]
        public static Fraction Raw(long 分子, long 分母) => new Fraction(分子, 分母, true);
        public override string ToString() => $"{Numerator}/{Denominator}";
        public override bool Equals(object obj) => obj is Fraction f && Equals(f);
        [凾(256)]
        public bool Equals(Fraction other) => _numerator == other._numerator && _denominator == other._denominator;
        public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

        [凾(256)]
        public static implicit operator Fraction(long x) => new Fraction(x, 1, true);
        [凾(256)]
        public int CompareTo(Fraction other) => (Numerator * other.Denominator).CompareTo(other.Numerator * Denominator);

        [凾(256)]
        public static Fraction operator -(Fraction x) => new Fraction(-x.Numerator, x.Denominator);
        [凾(256)]
        public static Fraction operator +(Fraction x, Fraction y)
        {
            var gcd = MathLibEx.Gcd(x.Denominator, y.Denominator);
            var lcm = x.Denominator / gcd * y.Denominator;
            return new Fraction((x.Numerator * y.Denominator + y.Numerator * x.Denominator) / gcd, lcm);
        }
        [凾(256)]
        public static Fraction operator -(Fraction x, Fraction y)
        {
            var gcd = MathLibEx.Gcd(x.Denominator, y.Denominator);
            var lcm = x.Denominator / gcd * y.Denominator;
            return new Fraction((x.Numerator * y.Denominator - y.Numerator * x.Denominator) / gcd, lcm);
        }
        [凾(256)]
        public static Fraction operator *(Fraction x, Fraction y) => new Fraction(x.Numerator * y.Numerator, x.Denominator * y.Denominator);
        [凾(256)]
        public static Fraction operator /(Fraction x, Fraction y) => new Fraction(x.Numerator * y.Denominator, x.Denominator * y.Numerator);
        [凾(256)]
        public static bool operator ==(Fraction x, Fraction y) => x.Equals(y);
        [凾(256)]
        public static bool operator !=(Fraction x, Fraction y) => !x.Equals(y);
        [凾(256)]
        public static bool operator >=(Fraction x, Fraction y) => x.CompareTo(y) >= 0;
        [凾(256)]
        public static bool operator <=(Fraction x, Fraction y) => x.CompareTo(y) <= 0;
        [凾(256)]
        public static bool operator >(Fraction x, Fraction y) => x.CompareTo(y) > 0;
        [凾(256)]
        public static bool operator <(Fraction x, Fraction y) => x.CompareTo(y) < 0;

        [凾(256)]
        public Fraction Inverse() => new Fraction(Denominator, Numerator);
        [凾(256)]
        public double ToDouble() => (double)Numerator / Denominator;
    }
}
