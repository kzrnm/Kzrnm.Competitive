using AtCoder;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>有理数を <see cref="Int128"/> の既約分数で表す</summary>
    public readonly struct Fraction128 : IEquatable<Fraction128>, IComparable<Fraction128>, IIntBase<Fraction128>, INumber<Fraction128>
    {
        public static readonly Fraction128 NaN = new Fraction128(0, -1, true);
        public static bool IsNaN(Fraction128 v) => v._denominator0 < 0;

        /// <summary>分子</summary>
        readonly Int128 _numerator;
        /// <summary>分子</summary>
        public Int128 Numerator => _numerator;
        /// <summary>分母 - 1 (default を 0/0 ではなく 0/1 にしたい)</summary>
        readonly Int128 _denominator0;
        /// <summary>分母</summary>
        public Int128 Denominator => _denominator0 + 1;

        public Fraction128(Int128 分子, Int128 分母)
        {
            if (分母 == 0)
            {
                _numerator = Int128.Sign(分子) switch
                {
                    0 => 0,
                    1 => int.MaxValue,
                    _ => int.MinValue,
                };
                _denominator0 = -1;
                return;
            }
            var negative = (分子 ^ 分母) < 0;
            分子 = Int128.Abs(分子);
            分母 = Int128.Abs(分母);
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
        Fraction128(Int128 分子, Int128 分母, bool _)
        {
            _numerator = 分子;
            _denominator0 = 分母 - 1;
        }
        [凾(256)]
        public static Fraction128 Raw(Int128 分子, Int128 分母) => new Fraction128(分子, 分母, true);
        public override string ToString() => $"{Numerator}/{Denominator}";
        public override bool Equals(object obj) => obj is Fraction128 f && Equals(f);
        [凾(256)]
        public bool Equals(Fraction128 other) => _numerator == other._numerator && _denominator0 == other._denominator0;
        public override int GetHashCode() => HashCode.Combine(_numerator, _denominator0);

        [凾(256)] public static implicit operator Fraction128(Int128 x) => new Fraction128(x, 1, true);
        [凾(256)] public static implicit operator Fraction128(long x) => new Fraction128(x, 1, true);
        [凾(256)] public static implicit operator Fraction128(Fraction x) => new Fraction128(x.Numerator, x.Denominator, true);
        [凾(256)]
        public int CompareTo(Fraction128 other)
        {
            int neg = 1;
            switch (IsNegative(this), IsNegative(other))
            {
                case (true, true):
                    neg = -1;
                    break;
                case (true, _):
                    return -1;
                case (_, true):
                    return 1;
            }

#if NET10_0_OR_GREATER
            var liHi = UInt128.BigMul((UInt128)Int128.Abs(Numerator), (UInt128)other.Denominator, out var liLo);
            var riHi = UInt128.BigMul((UInt128)Denominator, (UInt128)Int128.Abs(other.Numerator), out var riLo);
#else
            static UInt128 BigMul(UInt128 left, UInt128 right, out UInt128 lo)
            {
                ulong al = (ulong)left;
                ulong ah = (ulong)(left >> 64);

                ulong bl = (ulong)right;
                ulong bh = (ulong)(right >> 64);

                UInt128 mull = Math.BigMul(al, bl);
                UInt128 t = Math.BigMul(ah, bl) + (ulong)(mull >> 64);
                UInt128 tl = Math.BigMul(al, bh) + (ulong)t;

                lo = new UInt128((ulong)tl, (ulong)mull);
                return Math.BigMul(ah, bh) + (ulong)(t >> 64) + (ulong)(tl >> 64);
            }
            var liHi = BigMul((UInt128)Int128.Abs(Numerator), (UInt128)other.Denominator, out var liLo);
            var riHi = BigMul((UInt128)Denominator, (UInt128)Int128.Abs(other.Numerator), out var riLo);
#endif
            return neg * (liHi.CompareTo(riHi) switch { 0 => liLo.CompareTo(riLo), var c => c });
        }


        /// <summary>
        /// 分母と分子を最大 <paramref name="bit"/> まで精度を落とします。
        /// </summary>
        [凾(256)]
        public Fraction128 RoundOff(int bit = 64)
        {
            var hi = Int128.Abs(Numerator);
            var lo = Denominator;
            if (hi < lo) (hi, lo) = (lo, hi);
            int shift = (int)Int128.Log2(hi) - bit;
            if (shift <= 0 || shift >= Int128.Log2(lo)) return this;
            return new(Numerator >> shift, Denominator >> shift);
        }

        [凾(256)]
        public Fraction128 Inverse() => new Fraction128(Denominator, Numerator);
        [凾(256)]
        public double ToDouble() => (double)Numerator / (double)Denominator;
        [凾(256)] public static explicit operator double(Fraction128 x) => x.ToDouble();
        [凾(256)]
        public static explicit operator Fraction128(double x)
        {
            var b = BitConverter.DoubleToInt64Bits(x);
            var e = (int)((b >>> 52) & 0x7FF) - 1023 - 52;
            var v = b & 0xFFFFFFFFFFFFF;

            if (e == -1023 - 52) e++; // 非正規化数
            else v |= 1L << 52; // 正規化のケチ表現

            if (b < 0) v = -v;

            if (e == 0) return v;
            if (e > 0) return (Int128)v << e;

            if ((e = -e) < 127)
                return new(v, Int128.One << e);

            return new(v >> (e - 126), Int128.One << 126);
        }
        public static Fraction128 operator +(Fraction128 x) => x;
        [凾(256)]
        public static Fraction128 operator -(Fraction128 x) => new Fraction128(-x.Numerator, x.Denominator);
        [凾(256)]
        public static Fraction128 operator +(Fraction128 x, Fraction128 y)
        {
            var gcd = MathLibEx.Gcd(x.Denominator, y.Denominator);
            var xd = x.Denominator / gcd;
            var yd = y.Denominator / gcd;
            var lcm = xd * y.Denominator;
            var numerator = (Int128)x.Numerator * yd + y.Numerator * xd;
            return new Fraction128((long)numerator, lcm);
        }
        [凾(256)]
        public static Fraction128 operator -(Fraction128 x, Fraction128 y)
        {
            var gcd = MathLibEx.Gcd(x.Denominator, y.Denominator);
            var xd = x.Denominator / gcd;
            var yd = y.Denominator / gcd;
            var lcm = xd * y.Denominator;
            var numerator = (Int128)x.Numerator * yd - y.Numerator * xd;
            return new Fraction128((long)numerator, lcm);
        }

        [凾(256)] public static Fraction128 operator *(Fraction128 x, Fraction128 y) => MulImpl(x.Numerator, x.Denominator, y.Numerator, y.Denominator);
        [凾(256)] public static Fraction128 operator /(Fraction128 x, Fraction128 y) => MulImpl(x.Numerator, x.Denominator, y.Denominator, y.Numerator);
        [凾(256)]
        static Fraction128 MulImpl(Int128 xn, Int128 xd, Int128 yn, Int128 yd)
        {
            var g1 = MathLibEx.Gcd(xn, yd);
            xn /= g1;
            yd /= g1;

            var g2 = MathLibEx.Gcd(yn, xd);
            yn /= g2;
            xd /= g2;

            return new(xn * yn, xd * yd);
        }
        [凾(256)] public static bool operator ==(Fraction128 x, Fraction128 y) => x.Equals(y);
        [凾(256)] public static bool operator !=(Fraction128 x, Fraction128 y) => !x.Equals(y);
        [凾(256)] public static bool operator >=(Fraction128 x, Fraction128 y) => x.CompareTo(y) >= 0;
        [凾(256)] public static bool operator <=(Fraction128 x, Fraction128 y) => x.CompareTo(y) <= 0;
        [凾(256)] public static bool operator >(Fraction128 x, Fraction128 y) => x.CompareTo(y) > 0;
        [凾(256)] public static bool operator <(Fraction128 x, Fraction128 y) => x.CompareTo(y) < 0;
        [凾(256)] public static Fraction128 operator --(Fraction128 v) => new Fraction128(v.Numerator - v.Denominator, v.Denominator, true);
        [凾(256)] public static Fraction128 operator ++(Fraction128 v) => new Fraction128(v.Numerator + v.Denominator, v.Denominator, true);

        static Fraction128 IModulusOperators<Fraction128, Fraction128, Fraction128>.operator %(Fraction128 left, Fraction128 right) => default;

        [凾(256)] public static Fraction128 Abs(Fraction128 v) => new Fraction128(Int128.Abs(v.Numerator), v.Denominator, true);
        public static Fraction128 One => new Fraction128(1, 1, true);

        static bool INumberBase<Fraction128>.IsRealNumber(Fraction128 v) => !IsNaN(v);
        public static bool IsNegative(Fraction128 v) => Int128.IsNegative(v.Numerator);
        public static bool IsPositive(Fraction128 v) => Int128.IsPositive(v.Numerator);
        static bool INumberBase<Fraction128>.IsNormal(Fraction128 v) => !IsNaN(v);
        static bool INumberBase<Fraction128>.IsInteger(Fraction128 v) => v._denominator0 == 0;
        static bool INumberBase<Fraction128>.IsEvenInteger(Fraction128 v) => v._denominator0 == 0 && Int128.IsEvenInteger(v.Numerator);
        static bool INumberBase<Fraction128>.IsOddInteger(Fraction128 v) => v._denominator0 == 0 && Int128.IsOddInteger(v.Numerator);
        static Fraction128 INumberBase<Fraction128>.MaxMagnitude(Fraction128 x, Fraction128 y)
        {
            if (IsNaN(x)) return NaN;
            if (IsNaN(y)) return NaN;
            if (Abs(x) > Abs(y)) return x;
            return y;
        }

        static Fraction128 INumberBase<Fraction128>.MaxMagnitudeNumber(Fraction128 x, Fraction128 y)
        {
            if (IsNaN(x)) return y;
            if (IsNaN(y)) return x;
            if (Abs(x) > Abs(y)) return x;
            return y;
        }

        static Fraction128 INumberBase<Fraction128>.MinMagnitude(Fraction128 x, Fraction128 y)
        {
            if (IsNaN(x)) return NaN;
            if (IsNaN(y)) return NaN;
            if (Abs(x) < Abs(y)) return x;
            return y;
        }

        static Fraction128 INumberBase<Fraction128>.MinMagnitudeNumber(Fraction128 x, Fraction128 y)
        {
            if (IsNaN(x)) return y;
            if (IsNaN(y)) return x;
            if (Abs(x) < Abs(y)) return x;
            return y;
        }

        [凾(256)]
        static bool TryConvertFrom<TOther>(TOther v, out Fraction128 res)
        {
            if (typeof(int) == typeof(TOther)) { res = (int)(object)v; return true; }
            else if (typeof(long) == typeof(TOther)) { res = (long)(object)v; return true; }
            else if (typeof(Int128) == typeof(TOther)) { res = (long)(object)v; return true; }
            else if (typeof(uint) == typeof(TOther)) { res = (uint)(object)v; return true; }
            res = default;
            return false;
        }

        [凾(256)]
        static bool INumberBase<Fraction128>.TryConvertFromChecked<TOther>(TOther v, out Fraction128 res) => TryConvertFrom(v, out res);

        [凾(256)]
        static bool INumberBase<Fraction128>.TryConvertFromSaturating<TOther>(TOther v, out Fraction128 res) => TryConvertFrom(v, out res);

        [凾(256)]
        static bool INumberBase<Fraction128>.TryConvertFromTruncating<TOther>(TOther v, out Fraction128 res) => TryConvertFrom(v, out res);


        [凾(256)]
        static bool TryConvertTo<TOther>(Fraction128 v, out TOther res)
        {
            res = default;
            if (v.Denominator <= 0) return false;
            if (typeof(int) == typeof(TOther)) { res = (TOther)(object)(v.Numerator / v.Denominator); return true; }
            else if (typeof(long) == typeof(TOther)) { res = (TOther)(object)(v.Numerator / v.Denominator); return true; }
            else if (typeof(Int128) == typeof(TOther)) { res = (TOther)(object)(v.Numerator / v.Denominator); return true; }
            else if (typeof(uint) == typeof(TOther)) { res = (TOther)(object)(v.Numerator / v.Denominator); return true; }
            else if (typeof(double) == typeof(TOther)) { res = (TOther)(object)v.ToDouble(); return true; }
            else if (typeof(float) == typeof(TOther)) { res = (TOther)(object)(float)v.ToDouble(); return true; }
            return false;
        }
        [凾(256)]
        static bool INumberBase<Fraction128>.TryConvertToChecked<TOther>(Fraction128 v, out TOther res) => TryConvertTo(v, out res);

        [凾(256)]
        static bool INumberBase<Fraction128>.TryConvertToSaturating<TOther>(Fraction128 v, out TOther res) => TryConvertTo(v, out res);

        [凾(256)]
        static bool INumberBase<Fraction128>.TryConvertToTruncating<TOther>(Fraction128 v, out TOther res) => TryConvertTo(v, out res);
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

        public static Fraction128 Parse(ReadOnlySpan<char> s)
            => TryParse(s, out var r) ? r : throw new FormatException();

        [SourceExpander.NotEmbeddingSource] // for xUnit
        public static Fraction128 Parse(string s, IFormatProvider provider) => Parse(s);
        public static bool TryParse(ReadOnlySpan<char> s, out Fraction128 res)
        {
            var ok = false;
            res = default;
            var ix = s.IndexOf('/');
            if (ix < 0)
            {
                ok = Int128.TryParse(s, out var l);
                res = l;
            }
            else if (ix < s.Length - 1)
            {
                if (Int128.TryParse(s[..ix], out var n) && Int128.TryParse(s[(ix + 1)..], out var d))
                {
                    res = new Fraction128(n, d);
                    ok = true;
                }
            }
            return ok;
        }

        int IComparable.CompareTo(object obj) => obj is Fraction128 f ? CompareTo(f) : ToDouble().CompareTo(Convert.ToDouble(obj));
    }
}
