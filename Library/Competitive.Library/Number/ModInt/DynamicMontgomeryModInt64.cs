// https://nyaannyaan.github.io/library/modint/montgomery-modint.hpp
using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Globalization;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 奇数オンリーの ModInt
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "気にしない")]
    public struct DynamicMontgomeryModInt64<T> : IUtf8ConsoleWriterFormatter, IEquatable<DynamicMontgomeryModInt64<T>>, IFormattable
        , INumberBase<DynamicMontgomeryModInt64<T>>
        where T : struct
    {
        internal static ulong n2;
        internal static ulong r;
        /// <summary>
        /// 1
        /// </summary>
        private static DynamicMontgomeryModInt64<T> _One;
        internal ulong _v;


        public static ulong _mod;
        /// <summary>
        /// mod を返します。
        /// </summary>
        public static long Mod
        {
            get => (long)_mod;
            set
            {
                _mod = (ulong)value;
                n2 = GetN2(_mod);
                r = GetR(_mod);
                _One = 1;
            }
        }

        public static DynamicMontgomeryModInt64<T> Zero => default;
        public static DynamicMontgomeryModInt64<T> One => _One;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        public DynamicMontgomeryModInt64(long v) : this(Reduce(((UInt128)ModCalc.SafeMod(v, Mod) + _mod) * n2)) { }
        DynamicMontgomeryModInt64(ulong a)
        {
            _v = a;
        }

        [凾(256)] public static implicit operator DynamicMontgomeryModInt64<T>(long value) => new DynamicMontgomeryModInt64<T>(value);

        /// <summary>
        /// 格納されている値を返します。
        /// </summary>
        public long Value
        {
            [凾(256)]
            get
            {
                var r = Reduce(_v);
                return (long)(r >= _mod ? r - _mod : r);
            }
        }
        public override string ToString() => Value.ToString();
        [凾(256)] void IUtf8ConsoleWriterFormatter.Write(Utf8ConsoleWriter cw) => cw.Write(Value);
        [凾(256)] public static implicit operator ConsoleOutput(DynamicMontgomeryModInt64<T> m) => m.ToConsoleOutput();
        [凾(256)] public static implicit operator DynamicMontgomeryModInt64<T>(ConsoleReader r) => new DynamicMontgomeryModInt64<T>(r.Long());


        [凾(256)]
        internal static ulong Reduce(UInt128 b)
        {
            var a = (UInt128)((ulong)b * (~r + 1)) * _mod + b;
            return (ulong)(a >> 64);
        }

        [凾(256)]
        public static DynamicMontgomeryModInt64<T> operator +(DynamicMontgomeryModInt64<T> a, DynamicMontgomeryModInt64<T> b)
        {
            ulong r = a._v + b._v - 2 * _mod;
            if ((long)r < 0) r += 2 * _mod;
            return new DynamicMontgomeryModInt64<T>(r);
        }
        [凾(256)]
        public static DynamicMontgomeryModInt64<T> operator -(DynamicMontgomeryModInt64<T> a, DynamicMontgomeryModInt64<T> b)
        {
            ulong r = a._v - b._v;
            if ((long)r < 0) r += 2 * _mod;
            return new DynamicMontgomeryModInt64<T>(r);
        }
        [凾(256)]
        public static DynamicMontgomeryModInt64<T> operator *(DynamicMontgomeryModInt64<T> a, DynamicMontgomeryModInt64<T> b)
            => new DynamicMontgomeryModInt64<T>(Reduce((UInt128)a._v * b._v));
        [凾(256)]
        public static DynamicMontgomeryModInt64<T> operator /(DynamicMontgomeryModInt64<T> a, DynamicMontgomeryModInt64<T> b)
            => a * b.Inv();

        [凾(256)] public static DynamicMontgomeryModInt64<T> operator +(DynamicMontgomeryModInt64<T> a) => a;
        [凾(256)]
        public static DynamicMontgomeryModInt64<T> operator -(DynamicMontgomeryModInt64<T> a)
        {
            ulong r = ~a._v + 1;
            if ((long)r < 0) r += 2 * _mod;
            return new DynamicMontgomeryModInt64<T>(r);
        }
        [凾(256)]
        public static DynamicMontgomeryModInt64<T> operator ++(DynamicMontgomeryModInt64<T> a) => a + 1;
        [凾(256)]
        public static DynamicMontgomeryModInt64<T> operator --(DynamicMontgomeryModInt64<T> a) => a - 1;



        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [凾(256)]
        public DynamicMontgomeryModInt64<T> Pow(long n)
        {
            Contract.Assert(0 <= n, $"{nameof(n)} must be positive.");
            DynamicMontgomeryModInt64<T> x = this, r = 1;

            while (n > 0)
            {
                if ((n & 1) != 0)
                {
                    r *= x;
                }
                x *= x;
                n >>= 1;
            }

            return r;
        }

        /// <summary>
        /// 自身を x として、 xy≡1 なる y を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: gcd(x, mod) = 1</para>
        /// </remarks>
        [凾(256)]
        public DynamicMontgomeryModInt64<T> Inv() => Pow(Mod - 2);

        [凾(256)] public override bool Equals(object obj) => obj is DynamicMontgomeryModInt64<T> m && Equals(m);
        [凾(256)]
        public bool Equals(DynamicMontgomeryModInt64<T> other)
            => GetHashCodeUInt64() == other.GetHashCodeUInt64();
        [凾(256)] public static bool operator ==(DynamicMontgomeryModInt64<T> left, DynamicMontgomeryModInt64<T> right) => left.Equals(right);
        [凾(256)] public static bool operator !=(DynamicMontgomeryModInt64<T> left, DynamicMontgomeryModInt64<T> right) => !left.Equals(right);
        [凾(256)]
        public ulong GetHashCodeUInt64()
        {
            var v = _v;
            if (v >= _mod) v -= _mod;
            return v;
        }
        [凾(256)]
        public override int GetHashCode() => GetHashCodeUInt64().GetHashCode();

        public static bool TryParse(ReadOnlySpan<char> s, out DynamicMontgomeryModInt64<T> result)
        {
            result = Zero;
            DynamicMontgomeryModInt64<T> ten = 10u;
            s = s.Trim();
            bool minus = false;
            if (s.Length > 0 && s[0] == '-')
            {
                minus = true;
                s = s[1..];
            }
            for (int i = 0; i < s.Length; i++)
            {
                var d = (uint)(s[i] - '0');
                if (d >= 10) return false;
                result = result * ten + d;
            }
            if (minus)
                result = -result;
            return true;
        }
        public static DynamicMontgomeryModInt64<T> Parse(ReadOnlySpan<char> s)
        {
            if (!TryParse(s, out var r))
                Throw();
            return r;
            void Throw() => throw new FormatException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) => Value.TryFormat(destination, out charsWritten, format, provider);
        public string ToString(string format, IFormatProvider formatProvider) => Value.ToString(format, formatProvider);
        static int INumberBase<DynamicMontgomeryModInt64<T>>.Radix => 2;
        static DynamicMontgomeryModInt64<T> IAdditiveIdentity<DynamicMontgomeryModInt64<T>, DynamicMontgomeryModInt64<T>>.AdditiveIdentity => default;
        static DynamicMontgomeryModInt64<T> IMultiplicativeIdentity<DynamicMontgomeryModInt64<T>, DynamicMontgomeryModInt64<T>>.MultiplicativeIdentity => _One;
        static DynamicMontgomeryModInt64<T> INumberBase<DynamicMontgomeryModInt64<T>>.Abs(DynamicMontgomeryModInt64<T> v) => v;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsCanonical(DynamicMontgomeryModInt64<T> v) => true;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsComplexNumber(DynamicMontgomeryModInt64<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsRealNumber(DynamicMontgomeryModInt64<T> v) => true;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsImaginaryNumber(DynamicMontgomeryModInt64<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsEvenInteger(DynamicMontgomeryModInt64<T> v) => long.IsEvenInteger(v.Value);
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsOddInteger(DynamicMontgomeryModInt64<T> v) => long.IsOddInteger(v.Value);
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsFinite(DynamicMontgomeryModInt64<T> v) => true;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsInfinity(DynamicMontgomeryModInt64<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsInteger(DynamicMontgomeryModInt64<T> v) => true;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsPositive(DynamicMontgomeryModInt64<T> v) => true;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsNegative(DynamicMontgomeryModInt64<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsPositiveInfinity(DynamicMontgomeryModInt64<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsNegativeInfinity(DynamicMontgomeryModInt64<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsNormal(DynamicMontgomeryModInt64<T> v) => v.Value != 0;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsSubnormal(DynamicMontgomeryModInt64<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsZero(DynamicMontgomeryModInt64<T> v) => v.Value == 0;
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.IsNaN(DynamicMontgomeryModInt64<T> v) => false;
        static DynamicMontgomeryModInt64<T> INumberBase<DynamicMontgomeryModInt64<T>>.MaxMagnitude(DynamicMontgomeryModInt64<T> x, DynamicMontgomeryModInt64<T> y) => new DynamicMontgomeryModInt64<T>(long.Max(x.Value, y.Value));
        static DynamicMontgomeryModInt64<T> INumberBase<DynamicMontgomeryModInt64<T>>.MaxMagnitudeNumber(DynamicMontgomeryModInt64<T> x, DynamicMontgomeryModInt64<T> y) => new DynamicMontgomeryModInt64<T>(long.Max(x.Value, y.Value));
        static DynamicMontgomeryModInt64<T> INumberBase<DynamicMontgomeryModInt64<T>>.MinMagnitude(DynamicMontgomeryModInt64<T> x, DynamicMontgomeryModInt64<T> y) => new DynamicMontgomeryModInt64<T>(long.Min(x.Value, y.Value));
        static DynamicMontgomeryModInt64<T> INumberBase<DynamicMontgomeryModInt64<T>>.MinMagnitudeNumber(DynamicMontgomeryModInt64<T> x, DynamicMontgomeryModInt64<T> y) => new DynamicMontgomeryModInt64<T>(long.Min(x.Value, y.Value));

        static DynamicMontgomeryModInt64<T> INumberBase<DynamicMontgomeryModInt64<T>>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => Parse(s);
        static DynamicMontgomeryModInt64<T> INumberBase<DynamicMontgomeryModInt64<T>>.Parse(string s, NumberStyles style, IFormatProvider provider) => Parse(s);
        static DynamicMontgomeryModInt64<T> ISpanParsable<DynamicMontgomeryModInt64<T>>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => Parse(s);
        static DynamicMontgomeryModInt64<T> IParsable<DynamicMontgomeryModInt64<T>>.Parse(string s, IFormatProvider provider) => Parse(s);
        static bool ISpanParsable<DynamicMontgomeryModInt64<T>>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out DynamicMontgomeryModInt64<T> result) => TryParse(s, out result);
        static bool IParsable<DynamicMontgomeryModInt64<T>>.TryParse(string s, IFormatProvider provider, out DynamicMontgomeryModInt64<T> result) => TryParse(s, out result);
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out DynamicMontgomeryModInt64<T> result) => TryParse(s, out result);
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.TryParse(string s, NumberStyles style, IFormatProvider provider, out DynamicMontgomeryModInt64<T> result) => TryParse(s, out result);

        static bool INumberBase<DynamicMontgomeryModInt64<T>>.TryConvertFromChecked<TOther>(TOther v, out DynamicMontgomeryModInt64<T> r)
        {
            if (WrapChecked(v, out long l))
            {
                r = new(l);
                return true;
            }
            r = default;
            return false;
        }
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.TryConvertFromSaturating<TOther>(TOther v, out DynamicMontgomeryModInt64<T> r)
        {
            if (WrapSaturating(v, out long l))
            {
                r = new(l);
                return true;
            }
            r = default;
            return false;
        }
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.TryConvertFromTruncating<TOther>(TOther v, out DynamicMontgomeryModInt64<T> r)
        {
            if (WrapTruncating(v, out long l))
            {
                r = new(l);
                return true;
            }
            r = default;
            return false;
        }
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.TryConvertToChecked<TOther>(DynamicMontgomeryModInt64<T> v, out TOther r) where TOther : default => WrapChecked(v.Value, out r);
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.TryConvertToSaturating<TOther>(DynamicMontgomeryModInt64<T> v, out TOther r) where TOther : default => WrapSaturating(v.Value, out r);
        static bool INumberBase<DynamicMontgomeryModInt64<T>>.TryConvertToTruncating<TOther>(DynamicMontgomeryModInt64<T> v, out TOther r) where TOther : default => WrapTruncating(v.Value, out r);

        [凾(256)]
        static bool WrapChecked<TFrom, TTo>(TFrom v, out TTo r) where TFrom : INumberBase<TFrom> where TTo : INumberBase<TTo>
            => typeof(TFrom) == typeof(TTo)
            ? (r = (TTo)(object)v) is { }
            : TTo.TryConvertFromChecked(v, out r) || TFrom.TryConvertToChecked(v, out r);
        [凾(256)]
        static bool WrapSaturating<TFrom, TTo>(TFrom v, out TTo r) where TFrom : INumberBase<TFrom> where TTo : INumberBase<TTo>
            => typeof(TFrom) == typeof(TTo)
            ? (r = (TTo)(object)v) is { }
            : TTo.TryConvertFromSaturating(v, out r) || TFrom.TryConvertToSaturating(v, out r);
        [凾(256)]
        static bool WrapTruncating<TFrom, TTo>(TFrom v, out TTo r) where TFrom : INumberBase<TFrom> where TTo : INumberBase<TTo>
            => typeof(TFrom) == typeof(TTo)
            ? (r = (TTo)(object)v) is { }
            : TTo.TryConvertFromTruncating(v, out r) || TFrom.TryConvertToTruncating(v, out r);

        /// <summary>
        /// Montgomery の r ?
        /// </summary>
        /// <param name="m">Mod</param>
        /// <returns></returns>
        [凾(256)]
        public static ulong GetR(ulong m)
        {
            var r = m;
            r *= 2 - m * r;
            r *= 2 - m * r;
            r *= 2 - m * r;
            r *= 2 - m * r;
            r *= 2 - m * r;
            return r;
        }
        /// <summary>
        /// Montgomery の n2 ?
        /// </summary>
        /// <param name="m">Mod</param>
        /// <returns></returns>
        [凾(256)]
        public static ulong GetN2(ulong m)
            // -m == ~m + 1
            => Modulus(ulong.MaxValue, ~m + 1, m);

        [凾(256)]
        static ulong Modulus(ulong hi, ulong lo, ulong m)
            => (ulong)(new UInt128(hi, lo) % m);

    }
}
