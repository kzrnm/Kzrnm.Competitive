using System;
using System.Numerics;
using BigInteger = Kzrnm.Numerics.BigInteger;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>有理数を既約分数で表す</summary>
    public readonly struct BigFraction : IEquatable<BigFraction>, IComparable<BigFraction>
        , INumKz<BigFraction>
    {
        public static readonly BigFraction NaN = new BigFraction(0, -1, true);
        public static bool IsNaN(BigFraction v) => v._denominator0 < 0;

        /// <summary>分子</summary>
        readonly BigInteger _numerator;
        /// <summary>分子</summary>
        public BigInteger Numerator => _numerator;
        /// <summary>分母 - 1 (default を 0/0 ではなく 0/1 にしたい)</summary>
        readonly BigInteger _denominator0;
        /// <summary>分母</summary>
        public BigInteger Denominator => _denominator0 + 1;

        public BigFraction(BigInteger 分子, BigInteger 分母)
        {
            if (分母 == 0)
            {
                _numerator = 分子.Sign switch
                {
                    0 => 0,
                    1 => long.MaxValue,
                    _ => long.MinValue,
                };
                _denominator0 = -1;
                return;
            }
            var negative = (分子 ^ 分母) < 0;
            分子 = BigInteger.Abs(分子);
            分母 = BigInteger.Abs(分母);
            if (分子 == 0)
            {
                _numerator = 0;
                _denominator0 = 0;
            }
            else
            {
                var gcd = BigInteger.GreatestCommonDivisor(分母, 分子);
                _numerator = 分子 / gcd;
                if (negative)
                    _numerator = -_numerator;
                _denominator0 = 分母 / gcd - 1;
            }
        }
        BigFraction(BigInteger 分子, BigInteger 分母, bool _)
        {
            _numerator = 分子;
            _denominator0 = 分母 - 1;
        }
        public override string ToString() => $"{Numerator}/{Denominator}";
        public override bool Equals(object obj) => obj is BigFraction f && Equals(f);
        [凾(256)]
        public bool Equals(BigFraction other) => _numerator == other._numerator && _denominator0 == other._denominator0;
        public override int GetHashCode() => HashCode.Combine(_numerator, _denominator0);

        [凾(256)]
        public static implicit operator BigFraction(long x) => new BigFraction(x, 1);
        [凾(256)]
        public static implicit operator BigFraction(BigInteger x) => new BigFraction(x, 1);
        [凾(256)]
        public int CompareTo(BigFraction other) => (Numerator * other.Denominator).CompareTo(other.Numerator * Denominator);
        [凾(256)]
        public BigFraction Inverse() => new BigFraction(Denominator, Numerator);
        [凾(256)]
        public double ToDouble() => (double)Numerator / (double)Denominator;
        public static BigFraction operator +(BigFraction x) => x;
        [凾(256)]
        public static BigFraction operator -(BigFraction x) => new BigFraction(-x.Numerator, x.Denominator);
        [凾(256)]
        public static BigFraction operator +(BigFraction x, BigFraction y)
        {
            var gcd = BigInteger.GreatestCommonDivisor(x.Denominator, y.Denominator);
            var lcm = x.Denominator / gcd * y.Denominator;
            return new BigFraction((x.Numerator * y.Denominator + y.Numerator * x.Denominator) / gcd, lcm);
        }
        [凾(256)]
        public static BigFraction operator -(BigFraction x, BigFraction y)
        {
            var gcd = BigInteger.GreatestCommonDivisor(x.Denominator, y.Denominator);
            var lcm = x.Denominator / gcd * y.Denominator;
            return new BigFraction((x.Numerator * y.Denominator - y.Numerator * x.Denominator) / gcd, lcm);
        }
        [凾(256)]
        public static BigFraction operator *(BigFraction x, BigFraction y) => new BigFraction(x.Numerator * y.Numerator, x.Denominator * y.Denominator);
        [凾(256)]
        public static BigFraction operator /(BigFraction x, BigFraction y) => new BigFraction(x.Numerator * y.Denominator, x.Denominator * y.Numerator);
        [凾(256)]
        public static bool operator ==(BigFraction x, BigFraction y) => x.Equals(y);
        [凾(256)]
        public static bool operator !=(BigFraction x, BigFraction y) => !x.Equals(y);
        [凾(256)]
        public static bool operator >=(BigFraction x, BigFraction y) => x.CompareTo(y) >= 0;
        [凾(256)]
        public static bool operator <=(BigFraction x, BigFraction y) => x.CompareTo(y) <= 0;
        [凾(256)]
        public static bool operator >(BigFraction x, BigFraction y) => x.CompareTo(y) > 0;
        [凾(256)]
        public static bool operator <(BigFraction x, BigFraction y) => x.CompareTo(y) < 0;
        [凾(256)] public static BigFraction operator --(BigFraction v) => new BigFraction(v.Numerator - v.Denominator, v.Denominator, true);
        [凾(256)] public static BigFraction operator ++(BigFraction v) => new BigFraction(v.Numerator + v.Denominator, v.Denominator, true);

        [凾(256)] public static BigFraction Abs(BigFraction v) => new BigFraction(BigInteger.Abs(v.Numerator), v.Denominator, true);

        public static BigFraction One => new BigFraction(1, 1, true);


        static bool INumberBase<BigFraction>.IsInteger(BigFraction v) => false;
        static bool INumberBase<BigFraction>.IsRealNumber(BigFraction v) => !IsNaN(v);
        static bool INumberBase<BigFraction>.IsNegative(BigFraction v) => BigInteger.IsNegative(v.Numerator);
        static bool INumberBase<BigFraction>.IsPositive(BigFraction v) => BigInteger.IsPositive(v.Numerator);
        static bool INumberBase<BigFraction>.IsNormal(BigFraction v) => !IsNaN(v);
        static bool INumberBase<BigFraction>.IsEvenInteger(BigFraction v) => v._denominator0 == 0 && BigInteger.IsEvenInteger(v.Numerator);
        static bool INumberBase<BigFraction>.IsOddInteger(BigFraction v) => v._denominator0 == 0 && BigInteger.IsOddInteger(v.Numerator);
        static BigFraction INumberBase<BigFraction>.MaxMagnitude(BigFraction x, BigFraction y)
        {
            if (IsNaN(x)) return NaN;
            if (IsNaN(y)) return NaN;
            if (Abs(x) > Abs(y)) return x;
            return y;
        }

        static BigFraction INumberBase<BigFraction>.MaxMagnitudeNumber(BigFraction x, BigFraction y)
        {
            if (IsNaN(x)) return y;
            if (IsNaN(y)) return x;
            if (Abs(x) > Abs(y)) return x;
            return y;
        }

        static BigFraction INumberBase<BigFraction>.MinMagnitude(BigFraction x, BigFraction y)
        {
            if (IsNaN(x)) return NaN;
            if (IsNaN(y)) return NaN;
            if (Abs(x) < Abs(y)) return x;
            return y;
        }

        static BigFraction INumberBase<BigFraction>.MinMagnitudeNumber(BigFraction x, BigFraction y)
        {
            if (IsNaN(x)) return y;
            if (IsNaN(y)) return x;
            if (Abs(x) < Abs(y)) return x;
            return y;
        }

        [凾(256)]
        static bool TryConvertFrom<TOther>(TOther v, out BigFraction res)
        {
            if (typeof(int) == typeof(TOther)) { res = (int)(object)v; return true; }
            else if (typeof(long) == typeof(TOther)) { res = (long)(object)v; return true; }
            else if (typeof(uint) == typeof(TOther)) { res = (uint)(object)v; return true; }
            res = default;
            return false;
        }

        [凾(256)]
        static bool INumberBase<BigFraction>.TryConvertFromChecked<TOther>(TOther v, out BigFraction res) => TryConvertFrom(v, out res);

        [凾(256)]
        static bool INumberBase<BigFraction>.TryConvertFromSaturating<TOther>(TOther v, out BigFraction res) => TryConvertFrom(v, out res);

        [凾(256)]
        static bool INumberBase<BigFraction>.TryConvertFromTruncating<TOther>(TOther v, out BigFraction res) => TryConvertFrom(v, out res);


        [凾(256)]
        static bool TryConvertTo<TOther>(BigFraction v, out TOther res)
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
        static bool INumberBase<BigFraction>.TryConvertToChecked<TOther>(BigFraction v, out TOther res) => TryConvertTo(v, out res);

        [凾(256)]
        static bool INumberBase<BigFraction>.TryConvertToSaturating<TOther>(BigFraction v, out TOther res) => TryConvertTo(v, out res);

        [凾(256)]
        static bool INumberBase<BigFraction>.TryConvertToTruncating<TOther>(BigFraction v, out TOther res) => TryConvertTo(v, out res);
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


        public static BigFraction Parse(ReadOnlySpan<char> s)
            => TryParse(s, out var r) ? r : throw new FormatException();

        [SourceExpander.NotEmbeddingSource] // for xUnit
        public static BigFraction Parse(string s, IFormatProvider provider) => Parse(s);
        public static bool TryParse(ReadOnlySpan<char> s, out BigFraction res)
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
                    res = new BigFraction(n, d);
                    ok = true;
                }
            }
            return ok;
        }
    }
}
