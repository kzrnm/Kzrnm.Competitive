using AtCoder;
using System;
using System.Numerics;
using BigInteger = Kzrnm.Numerics.BigInteger;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>有理数を既約分数で表す</summary>
    public readonly struct BigFraction : IEquatable<BigFraction>, IComparable<BigFraction>, IIntBase<BigFraction>, INumber<BigFraction>
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
        [凾(256)] public double ToDouble() => (double)Numerator / (double)Denominator;
        [凾(256)] public static explicit operator double(BigFraction x) => x.ToDouble();
        [凾(256)]
        public static explicit operator BigFraction(double x)
        {
            var b = BitConverter.DoubleToInt64Bits(x);
            var e = (int)((b >>> 52) & 0x7FF) - 1023 - 52;
            var v = b & 0xFFFFFFFFFFFFF;

            if (e == -1023 - 52) e++; // 非正規化数
            else v |= 1L << 52; // 正規化のケチ表現

            if (b < 0) v = -v;

            if (e == 0) return v;
            if (e > 0) return new BigInteger(v) << e;
            return new(v, BigInteger.One << -e);
        }
        public static BigFraction operator +(BigFraction x) => x;
        [凾(256)]
        public static BigFraction operator -(BigFraction x) => new BigFraction(-x.Numerator, x.Denominator);
        [凾(256)]
        public static BigFraction operator +(BigFraction x, BigFraction y)
        {
            var gcd = BigInteger.GreatestCommonDivisor(x.Denominator, y.Denominator);
            var xd = x.Denominator / gcd;
            var yd = y.Denominator / gcd;
            var lcm = xd * y.Denominator;
            var numerator = x.Numerator * yd + y.Numerator * xd;
            return new BigFraction(numerator, lcm);
        }
        [凾(256)]
        public static BigFraction operator -(BigFraction x, BigFraction y)
        {
            var gcd = BigInteger.GreatestCommonDivisor(x.Denominator, y.Denominator);
            var xd = x.Denominator / gcd;
            var yd = y.Denominator / gcd;
            var lcm = xd * y.Denominator;
            var numerator = x.Numerator * yd - y.Numerator * xd;
            return new BigFraction(numerator, lcm);
        }
        [凾(256)] public static BigFraction operator *(BigFraction x, BigFraction y) => MulImpl(x.Numerator, x.Denominator, y.Numerator, y.Denominator);
        [凾(256)] public static BigFraction operator /(BigFraction x, BigFraction y) => MulImpl(x.Numerator, x.Denominator, y.Denominator, y.Numerator);
        [凾(256)]
        static BigFraction MulImpl(BigInteger xn, BigInteger xd, BigInteger yn, BigInteger yd)
        {
            var g1 = BigInteger.GreatestCommonDivisor(xn, yd);
            xn /= g1;
            yd /= g1;

            var g2 = BigInteger.GreatestCommonDivisor(yn, xd);
            yn /= g2;
            xd /= g2;

            return new(xn * yn, xd * yd);
        }
        [凾(256)] public static bool operator ==(BigFraction x, BigFraction y) => x.Equals(y);
        [凾(256)] public static bool operator !=(BigFraction x, BigFraction y) => !x.Equals(y);
        [凾(256)] public static bool operator >=(BigFraction x, BigFraction y) => x.CompareTo(y) >= 0;
        [凾(256)] public static bool operator <=(BigFraction x, BigFraction y) => x.CompareTo(y) <= 0;
        [凾(256)] public static bool operator >(BigFraction x, BigFraction y) => x.CompareTo(y) > 0;
        [凾(256)] public static bool operator <(BigFraction x, BigFraction y) => x.CompareTo(y) < 0;
        [凾(256)] public static BigFraction operator --(BigFraction v) => new BigFraction(v.Numerator - v.Denominator, v.Denominator, true);
        [凾(256)] public static BigFraction operator ++(BigFraction v) => new BigFraction(v.Numerator + v.Denominator, v.Denominator, true);

        static BigFraction IModulusOperators<BigFraction, BigFraction, BigFraction>.operator %(BigFraction left, BigFraction right) => default;

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

        int IComparable.CompareTo(object obj) => obj is BigFraction f ? CompareTo(f) : ToDouble().CompareTo(Convert.ToDouble(obj));
    }
}
