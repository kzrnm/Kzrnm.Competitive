using System;
using System.Globalization;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>有理数を既約分数で表す</summary>
    public readonly struct Fraction : IEquatable<Fraction>, IComparable<Fraction>
        , INumberBase<Fraction>
    {
        public static readonly Fraction NaN = new Fraction(0, -1, true);
        public static bool IsNaN(Fraction v) => v._denominator0 < 0;

        /// <summary>分子</summary>
        private readonly long _numerator;
        /// <summary>分子</summary>
        public long Numerator => _numerator;
        /// <summary>分母 - 1 (default を 0/0 ではなく 0/1 にしたい)</summary>
        private readonly long _denominator0;
        /// <summary>分母</summary>
        public long Denominator => _denominator0 + 1;

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
                _denominator0 = -1;
                return;
            }
            var negative = (分子 ^ 分母) < 0;
            分子 = Math.Abs(分子);
            分母 = Math.Abs(分母);
            if (分子 == 0)
            {
                _numerator = 0;
                _denominator0 = 0;
            }
            else
            {
                var gcd = MathLibEx.Gcd(分母, 分子);
                _numerator = 分子 / gcd;
                if (negative)
                    _numerator = -_numerator;
                _denominator0 = 分母 / gcd - 1;
            }
        }
        private Fraction(long 分子, long 分母, bool _)
        {
            _numerator = 分子;
            _denominator0 = 分母 - 1;
        }
        [凾(256)]
        public static Fraction Raw(long 分子, long 分母) => new Fraction(分子, 分母, true);
        public override string ToString() => $"{Numerator}/{Denominator}";
        public override bool Equals(object obj) => obj is Fraction f && Equals(f);
        [凾(256)]
        public bool Equals(Fraction other) => _numerator == other._numerator && _denominator0 == other._denominator0;
        public override int GetHashCode() => HashCode.Combine(_numerator, _denominator0);

        [凾(256)]
        public static implicit operator Fraction(long x) => new Fraction(x, 1, true);
        [凾(256)]
        public int CompareTo(Fraction other) => (Numerator * other.Denominator).CompareTo(other.Numerator * Denominator);

        [凾(256)]
        public Fraction Inverse() => new Fraction(Denominator, Numerator);
        [凾(256)]
        public double ToDouble() => (double)Numerator / Denominator;
        public static Fraction operator +(Fraction x) => x;
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
        [凾(256)] public static Fraction operator --(Fraction v) => new Fraction(v.Numerator - v.Denominator, v.Denominator, true);
        [凾(256)] public static Fraction operator ++(Fraction v) => new Fraction(v.Numerator + v.Denominator, v.Denominator, true);


        [凾(256)] public static Fraction Abs(Fraction v) => new Fraction(Math.Abs(v.Numerator), v.Denominator, true);
        public static Fraction One => new Fraction(1, 1, true);
        static int INumberBase<Fraction>.Radix => 2;
        static Fraction INumberBase<Fraction>.Zero => default;
        static Fraction IAdditiveIdentity<Fraction, Fraction>.AdditiveIdentity => default;
        static Fraction IMultiplicativeIdentity<Fraction, Fraction>.MultiplicativeIdentity => One;

        static bool INumberBase<Fraction>.IsCanonical(Fraction v) => true;
        static bool INumberBase<Fraction>.IsComplexNumber(Fraction v) => true;
        static bool INumberBase<Fraction>.IsImaginaryNumber(Fraction v) => true;
        static bool INumberBase<Fraction>.IsRealNumber(Fraction v) => !IsNaN(v);
        static bool INumberBase<Fraction>.IsFinite(Fraction v) => true;
        static bool INumberBase<Fraction>.IsInfinity(Fraction v) => false;
        static bool INumberBase<Fraction>.IsNegativeInfinity(Fraction v) => false;
        static bool INumberBase<Fraction>.IsPositiveInfinity(Fraction v) => false;
        static bool INumberBase<Fraction>.IsNegative(Fraction v) => long.IsNegative(v.Numerator);
        static bool INumberBase<Fraction>.IsPositive(Fraction v) => long.IsPositive(v.Numerator);
        static bool INumberBase<Fraction>.IsNormal(Fraction v) => !IsNaN(v);
        static bool INumberBase<Fraction>.IsSubnormal(Fraction v) => false;
        static bool INumberBase<Fraction>.IsInteger(Fraction v) => v._denominator0 == 0;
        static bool INumberBase<Fraction>.IsEvenInteger(Fraction v) => v._denominator0 == 0 && long.IsEvenInteger(v.Numerator);
        static bool INumberBase<Fraction>.IsOddInteger(Fraction v) => v._denominator0 == 0 && long.IsOddInteger(v.Numerator);
        static bool INumberBase<Fraction>.IsZero(Fraction v) => v != default;
        static Fraction INumberBase<Fraction>.MaxMagnitude(Fraction x, Fraction y)
        {
            if (IsNaN(x)) return NaN;
            if (IsNaN(y)) return NaN;
            if (Abs(x) > Abs(y)) return x;
            return y;
        }

        static Fraction INumberBase<Fraction>.MaxMagnitudeNumber(Fraction x, Fraction y)
        {
            if (IsNaN(x)) return y;
            if (IsNaN(y)) return x;
            if (Abs(x) > Abs(y)) return x;
            return y;
        }

        static Fraction INumberBase<Fraction>.MinMagnitude(Fraction x, Fraction y)
        {
            if (IsNaN(x)) return NaN;
            if (IsNaN(y)) return NaN;
            if (Abs(x) < Abs(y)) return x;
            return y;
        }

        static Fraction INumberBase<Fraction>.MinMagnitudeNumber(Fraction x, Fraction y)
        {
            if (IsNaN(x)) return y;
            if (IsNaN(y)) return x;
            if (Abs(x) < Abs(y)) return x;
            return y;
        }

        [凾(256)]
        static bool TryConvertFrom<TOther>(TOther v, out Fraction res)
        {
            if (typeof(int) == typeof(TOther)) { res = (int)(object)v; return true; }
            else if (typeof(long) == typeof(TOther)) { res = (long)(object)v; return true; }
            else if (typeof(uint) == typeof(TOther)) { res = (uint)(object)v; return true; }
            res = default;
            return false;
        }

        [凾(256)]
        static bool INumberBase<Fraction>.TryConvertFromChecked<TOther>(TOther v, out Fraction res) => TryConvertFrom(v, out res);

        [凾(256)]
        static bool INumberBase<Fraction>.TryConvertFromSaturating<TOther>(TOther v, out Fraction res) => TryConvertFrom(v, out res);

        [凾(256)]
        static bool INumberBase<Fraction>.TryConvertFromTruncating<TOther>(TOther v, out Fraction res) => TryConvertFrom(v, out res);


        [凾(256)]
        static bool TryConvertTo<TOther>(Fraction v, out TOther res)
        {
            res = default;
            if (v.Denominator <= 0) return false;
            if (typeof(int) == typeof(TOther)) { res = (TOther)(object)(v.Numerator / v.Denominator); return true; }
            else if (typeof(long) == typeof(TOther)) { res = (TOther)(object)(v.Numerator / v.Denominator); return true; }
            else if (typeof(uint) == typeof(TOther)) { res = (TOther)(object)(v.Numerator / v.Denominator); return true; }
            else if (typeof(double) == typeof(TOther)) { res = (TOther)(object)v.ToDouble(); return true; }
            else if (typeof(float) == typeof(TOther)) { res = (TOther)(object)(float)v.ToDouble(); return true; }
            return false;
        }
        [凾(256)]
        static bool INumberBase<Fraction>.TryConvertToChecked<TOther>(Fraction v, out TOther res) => TryConvertTo(v, out res);

        [凾(256)]
        static bool INumberBase<Fraction>.TryConvertToSaturating<TOther>(Fraction v, out TOther res) => TryConvertTo(v, out res);

        [凾(256)]
        static bool INumberBase<Fraction>.TryConvertToTruncating<TOther>(Fraction v, out TOther res) => TryConvertTo(v, out res);
        bool ISpanFormattable.TryFormat(Span<char> dst, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        {
            var n = Numerator.ToString();
            var d = Denominator.ToString();
            if (dst.Length < n.Length + d.Length + 1)
            {
                charsWritten = 0;
                return false;
            }
            n.TryCopyTo(dst);
            dst[n.Length] = '/';
            charsWritten = n.Length + 1;
            d.TryCopyTo(dst[charsWritten..]);
            charsWritten += d.Length;
            return true;
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString();


        static bool INumberBase<Fraction>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out Fraction res) => TryParse(s, out res);
        static bool INumberBase<Fraction>.TryParse(string s, NumberStyles style, IFormatProvider provider, out Fraction res) => TryParse(s, out res);
        static bool ISpanParsable<Fraction>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out Fraction res) => TryParse(s, out res);
        static bool IParsable<Fraction>.TryParse(string s, IFormatProvider provider, out Fraction res) => TryParse(s, out res);
        public static bool TryParse(ReadOnlySpan<char> s, out Fraction res)
        {
            var ok = false;
            res = default;
            var ix = s.IndexOf('/');
            if (ix < 0)
            {
                ok = long.TryParse(s, out var l);
                res = l;
            }
            else if (ix < s.Length - 1)
            {
                if (long.TryParse(s[..ix], out var n) && long.TryParse(s[(ix + 1)..], out var d))
                {
                    res = new Fraction(n, d);
                    ok = true;
                }
            }
            return ok;
        }

        static Fraction INumberBase<Fraction>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => TryParse(s, out var res) ? res : throw new FormatException();
        static Fraction INumberBase<Fraction>.Parse(string s, NumberStyles style, IFormatProvider provider) => TryParse(s, out var res) ? res : throw new FormatException();
        static Fraction ISpanParsable<Fraction>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => TryParse(s, out var res) ? res : throw new FormatException();
        static Fraction IParsable<Fraction>.Parse(string s, IFormatProvider provider) => TryParse(s, out var res) ? res : throw new FormatException();
    }
}
