// https://nyaannyaan.github.io/library/modint/montgomery-modint.hpp
using AtCoder;
using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Globalization;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using static Internal.Montgomery;
    /// <summary>
    /// 奇数オンリーの ModInt
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "気にしない")]
    public struct DynamicMontgomeryModInt<T> : IUtf8ConsoleWriterFormatter, IEquatable<DynamicMontgomeryModInt<T>>, IFormattable, IModInt<DynamicMontgomeryModInt<T>>
        , INumberBase<DynamicMontgomeryModInt<T>>
        where T : struct
    {
        internal static uint n2;
        internal static uint r;
        /// <summary>
        /// 1
        /// </summary>
        private static DynamicMontgomeryModInt<T> _One;
        internal uint _v;


        public static uint _mod;
        /// <summary>
        /// mod を返します。
        /// </summary>
        public static int Mod
        {
            get => (int)_mod;
            set
            {
                _mod = (uint)value;
                n2 = GetN2(_mod);
                r = GetR(_mod);
                _One = 1;
            }
        }

        public static DynamicMontgomeryModInt<T> Zero => default;
        public static DynamicMontgomeryModInt<T> One => _One;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        public DynamicMontgomeryModInt(long v) : this(Reduce((ulong)(v % _mod + _mod) * n2)) { }
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        public DynamicMontgomeryModInt(ulong v) : this(Reduce(v % _mod * n2)) { }
        DynamicMontgomeryModInt(uint a)
        {
            _v = a;
        }

        [凾(256)] public static implicit operator DynamicMontgomeryModInt<T>(long value) => new DynamicMontgomeryModInt<T>(value);

        /// <summary>
        /// 格納されている値を返します。
        /// </summary>
        public int Value
        {
            [凾(256)]
            get
            {
                var r = Reduce(_v);
                return (int)(r >= _mod ? r - _mod : r);
            }
        }
        public override string ToString() => Value.ToString();
        [凾(256)] void IUtf8ConsoleWriterFormatter.Write(Utf8ConsoleWriter cw) => cw.Write(Value);
        [凾(256)] public static implicit operator ConsoleOutput(DynamicMontgomeryModInt<T> m) => m.ToConsoleOutput();
        [凾(256)] public static implicit operator DynamicMontgomeryModInt<T>(ConsoleReader r) => new DynamicMontgomeryModInt<T>(r.Long());


        [凾(256)] internal static uint Reduce(ulong b) => (uint)((b + (ulong)((uint)b * (uint)-r) * _mod) >> 32);



        [凾(256)]
        public static DynamicMontgomeryModInt<T> operator +(DynamicMontgomeryModInt<T> a, DynamicMontgomeryModInt<T> b)
        {
            uint r = a._v + b._v - 2 * _mod;
            if ((int)r < 0) r += 2 * _mod;
            return new DynamicMontgomeryModInt<T>(r);
        }
        [凾(256)]
        public static DynamicMontgomeryModInt<T> operator -(DynamicMontgomeryModInt<T> a, DynamicMontgomeryModInt<T> b)
        {
            uint r = a._v - b._v;
            if ((int)r < 0) r += 2 * _mod;
            return new DynamicMontgomeryModInt<T>(r);
        }
        [凾(256)]
        public static DynamicMontgomeryModInt<T> operator *(DynamicMontgomeryModInt<T> a, DynamicMontgomeryModInt<T> b)
            => new DynamicMontgomeryModInt<T>(Reduce((ulong)a._v * b._v));
        [凾(256)]
        public static DynamicMontgomeryModInt<T> operator /(DynamicMontgomeryModInt<T> a, DynamicMontgomeryModInt<T> b)
            => a * b.Inv();

        [凾(256)] public static DynamicMontgomeryModInt<T> operator +(DynamicMontgomeryModInt<T> a) => a;
        [凾(256)]
        public static DynamicMontgomeryModInt<T> operator -(DynamicMontgomeryModInt<T> a)
        {
            uint r = (uint)-a._v;
            if ((int)r < 0) r += 2 * _mod;
            return new DynamicMontgomeryModInt<T>(r);
        }
        [凾(256)]
        public static DynamicMontgomeryModInt<T> operator ++(DynamicMontgomeryModInt<T> a) => a + 1;
        [凾(256)]
        public static DynamicMontgomeryModInt<T> operator --(DynamicMontgomeryModInt<T> a) => a - 1;


        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [凾(256)]
        public DynamicMontgomeryModInt<T> Pow(ulong n)
        {
            DynamicMontgomeryModInt<T> x = this, r = 1;

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
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [凾(256)]
        public DynamicMontgomeryModInt<T> Pow(long n)
        {
            Contract.Assert(0 <= n, $"{nameof(n)} must be positive.");
            return Pow((ulong)n);
        }

        /// <summary>
        /// 自身を x として、 xy≡1 なる y を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: gcd(x, mod) = 1</para>
        /// </remarks>
        [凾(256)]
        public DynamicMontgomeryModInt<T> Inv() => Pow(_mod - 2);

        [凾(256)] public override bool Equals(object obj) => obj is DynamicMontgomeryModInt<T> m && Equals(m);
        [凾(256)]
        public bool Equals(DynamicMontgomeryModInt<T> other)
            => GetHashCode() == other.GetHashCode();
        [凾(256)] public static bool operator ==(DynamicMontgomeryModInt<T> left, DynamicMontgomeryModInt<T> right) => left.Equals(right);
        [凾(256)] public static bool operator !=(DynamicMontgomeryModInt<T> left, DynamicMontgomeryModInt<T> right) => !left.Equals(right);
        [凾(256)]
        public override int GetHashCode()
        {
            var v = _v;
            if (v >= _mod) v -= _mod;
            return (int)v;
        }

        public static bool TryParse(ReadOnlySpan<char> s, out DynamicMontgomeryModInt<T> result)
        {
            result = Zero;
            DynamicMontgomeryModInt<T> ten = 10u;
            s = s.Trim();
            bool minus = false;
            if (s.Length > 0 && s[0] == '-')
            {
                minus = true;
                s = s.Slice(1);
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
        public static DynamicMontgomeryModInt<T> Parse(ReadOnlySpan<char> s)
        {
            if (!TryParse(s, out var r))
                Throw();
            return r;
            void Throw() => throw new FormatException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) => Value.TryFormat(destination, out charsWritten, format, provider);
        public string ToString(string format, IFormatProvider formatProvider) => Value.ToString(format, formatProvider);
        static int INumberBase<DynamicMontgomeryModInt<T>>.Radix => 2;
        static DynamicMontgomeryModInt<T> IAdditiveIdentity<DynamicMontgomeryModInt<T>, DynamicMontgomeryModInt<T>>.AdditiveIdentity => default;
        static DynamicMontgomeryModInt<T> IMultiplicativeIdentity<DynamicMontgomeryModInt<T>, DynamicMontgomeryModInt<T>>.MultiplicativeIdentity => _One;
        static DynamicMontgomeryModInt<T> INumberBase<DynamicMontgomeryModInt<T>>.Abs(DynamicMontgomeryModInt<T> v) => v;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsCanonical(DynamicMontgomeryModInt<T> v) => true;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsComplexNumber(DynamicMontgomeryModInt<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsRealNumber(DynamicMontgomeryModInt<T> v) => true;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsImaginaryNumber(DynamicMontgomeryModInt<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsEvenInteger(DynamicMontgomeryModInt<T> v) => int.IsEvenInteger(v.Value);
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsOddInteger(DynamicMontgomeryModInt<T> v) => int.IsOddInteger(v.Value);
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsFinite(DynamicMontgomeryModInt<T> v) => true;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsInfinity(DynamicMontgomeryModInt<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsInteger(DynamicMontgomeryModInt<T> v) => true;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsPositive(DynamicMontgomeryModInt<T> v) => true;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsNegative(DynamicMontgomeryModInt<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsPositiveInfinity(DynamicMontgomeryModInt<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsNegativeInfinity(DynamicMontgomeryModInt<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsNormal(DynamicMontgomeryModInt<T> v) => v.Value != 0;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsSubnormal(DynamicMontgomeryModInt<T> v) => false;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsZero(DynamicMontgomeryModInt<T> v) => v.Value == 0;
        static bool INumberBase<DynamicMontgomeryModInt<T>>.IsNaN(DynamicMontgomeryModInt<T> v) => false;
        static DynamicMontgomeryModInt<T> INumberBase<DynamicMontgomeryModInt<T>>.MaxMagnitude(DynamicMontgomeryModInt<T> x, DynamicMontgomeryModInt<T> y) => new DynamicMontgomeryModInt<T>(int.Max(x.Value, y.Value));
        static DynamicMontgomeryModInt<T> INumberBase<DynamicMontgomeryModInt<T>>.MaxMagnitudeNumber(DynamicMontgomeryModInt<T> x, DynamicMontgomeryModInt<T> y) => new DynamicMontgomeryModInt<T>(int.Max(x.Value, y.Value));
        static DynamicMontgomeryModInt<T> INumberBase<DynamicMontgomeryModInt<T>>.MinMagnitude(DynamicMontgomeryModInt<T> x, DynamicMontgomeryModInt<T> y) => new DynamicMontgomeryModInt<T>(int.Min(x.Value, y.Value));
        static DynamicMontgomeryModInt<T> INumberBase<DynamicMontgomeryModInt<T>>.MinMagnitudeNumber(DynamicMontgomeryModInt<T> x, DynamicMontgomeryModInt<T> y) => new DynamicMontgomeryModInt<T>(int.Min(x.Value, y.Value));

        static DynamicMontgomeryModInt<T> INumberBase<DynamicMontgomeryModInt<T>>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => Parse(s);
        static DynamicMontgomeryModInt<T> INumberBase<DynamicMontgomeryModInt<T>>.Parse(string s, NumberStyles style, IFormatProvider provider) => Parse(s);
        static DynamicMontgomeryModInt<T> ISpanParsable<DynamicMontgomeryModInt<T>>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => Parse(s);
        static DynamicMontgomeryModInt<T> IParsable<DynamicMontgomeryModInt<T>>.Parse(string s, IFormatProvider provider) => Parse(s);
        static bool ISpanParsable<DynamicMontgomeryModInt<T>>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out DynamicMontgomeryModInt<T> result) => TryParse(s, out result);
        static bool IParsable<DynamicMontgomeryModInt<T>>.TryParse(string s, IFormatProvider provider, out DynamicMontgomeryModInt<T> result) => TryParse(s, out result);
        static bool INumberBase<DynamicMontgomeryModInt<T>>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out DynamicMontgomeryModInt<T> result) => TryParse(s, out result);
        static bool INumberBase<DynamicMontgomeryModInt<T>>.TryParse(string s, NumberStyles style, IFormatProvider provider, out DynamicMontgomeryModInt<T> result) => TryParse(s, out result);

        static bool INumberBase<DynamicMontgomeryModInt<T>>.TryConvertFromChecked<TOther>(TOther v, out DynamicMontgomeryModInt<T> r)
        {
            if (WrapChecked(v, out long l))
            {
                r = new(l);
                return true;
            }
            if (WrapChecked(v, out ulong u))
            {
                r = new(u);
                return true;
            }
            r = default;
            return false;
        }
        static bool INumberBase<DynamicMontgomeryModInt<T>>.TryConvertFromSaturating<TOther>(TOther v, out DynamicMontgomeryModInt<T> r)
        {
            if (WrapSaturating(v, out long l))
            {
                r = new(l);
                return true;
            }
            if (WrapSaturating(v, out ulong u))
            {
                r = new(u);
                return true;
            }
            r = default;
            return false;
        }
        static bool INumberBase<DynamicMontgomeryModInt<T>>.TryConvertFromTruncating<TOther>(TOther v, out DynamicMontgomeryModInt<T> r)
        {
            if (WrapTruncating(v, out long l))
            {
                r = new(l);
                return true;
            }
            if (WrapTruncating(v, out ulong u))
            {
                r = new(u);
                return true;
            }
            r = default;
            return false;
        }
        static bool INumberBase<DynamicMontgomeryModInt<T>>.TryConvertToChecked<TOther>(DynamicMontgomeryModInt<T> v, out TOther r) where TOther : default => WrapChecked(v.Value, out r);
        static bool INumberBase<DynamicMontgomeryModInt<T>>.TryConvertToSaturating<TOther>(DynamicMontgomeryModInt<T> v, out TOther r) where TOther : default => WrapSaturating(v.Value, out r);
        static bool INumberBase<DynamicMontgomeryModInt<T>>.TryConvertToTruncating<TOther>(DynamicMontgomeryModInt<T> v, out TOther r) where TOther : default => WrapTruncating(v.Value, out r);

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
    }
}
