// https://nyaannyaan.github.io/library/modint/montgomery-modint.hpp
using AtCoder;
using Kzrnm.Competitive.IO;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
#if NET7_0_OR_GREATER
using System.Numerics;
using System.Globalization;
#else
using AtCoder.Internal;
using AtCoder.Operators;
#endif

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 奇数オンリーの ModInt
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "気にしない")]
    public struct MontgomeryModInt<T> : IUtf8ConsoleWriterFormatter, IEquatable<MontgomeryModInt<T>>, IFormattable
#if NET7_0_OR_GREATER
        , INumberBase<MontgomeryModInt<T>>
#endif
        where T : struct, IStaticMod
    {
        static readonly T op = new T();
        internal static readonly uint n2 = (uint)(((ulong)-op.Mod) % op.Mod);
        internal static readonly uint r = GetR();
        /// <summary>
        /// 1
        /// </summary>
        private static readonly MontgomeryModInt<T> _One = 1;
        internal uint _v;
        static uint GetR()
        {
            var mod = new T().Mod;
            var ret = mod;
            for (int i = 0; i < 4; ++i)
                ret *= 2 - mod * ret;
            return ret;
        }

        /// <summary>
        /// mod を返します。
        /// </summary>
        public static int Mod => (int)op.Mod;

        public static MontgomeryModInt<T> Zero => default;
        public static MontgomeryModInt<T> One => _One;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        public MontgomeryModInt(long v) : this(Reduce((ulong)(v % op.Mod + op.Mod) * n2)) { }
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        public MontgomeryModInt(ulong v) : this(Reduce(v % op.Mod * n2)) { }
        MontgomeryModInt(uint a)
        {
            _v = a;
        }

        [凾(256)] public static implicit operator MontgomeryModInt<T>(ulong value) => new MontgomeryModInt<T>(value);
        [凾(256)] public static implicit operator MontgomeryModInt<T>(uint value) => new MontgomeryModInt<T>((ulong)value);
        [凾(256)] public static implicit operator MontgomeryModInt<T>(long value) => new MontgomeryModInt<T>(value);
        [凾(256)] public static implicit operator MontgomeryModInt<T>(int value) => new MontgomeryModInt<T>(value);

        [凾(256)] public static explicit operator ulong(MontgomeryModInt<T> value) => (ulong)value.Value;
        [凾(256)] public static explicit operator uint(MontgomeryModInt<T> value) => (uint)value.Value;
        [凾(256)] public static explicit operator long(MontgomeryModInt<T> value) => value.Value;
        [凾(256)] public static explicit operator int(MontgomeryModInt<T> value) => value.Value;

        /// <summary>
        /// 格納されている値を返します。
        /// </summary>
        public int Value
        {
            [凾(256)]
            get
            {
                var r = Reduce(_v);
                return (int)(r >= op.Mod ? r - op.Mod : r);
            }
        }
        public override string ToString() => Value.ToString();
        [凾(256)] void IUtf8ConsoleWriterFormatter.Write(Utf8ConsoleWriter cw) => cw.Write(Value);
        [凾(256)] public static implicit operator ConsoleOutput(MontgomeryModInt<T> m) => m.ToConsoleOutput();
        [凾(256)] public static implicit operator MontgomeryModInt<T>(ConsoleReader r) => new MontgomeryModInt<T>(r.Long());



        [凾(256)] internal static uint Reduce(ulong b) => (uint)((b + (ulong)((uint)b * (uint)-(int)r) * op.Mod) >> 32);



        [凾(256)]
        public static MontgomeryModInt<T> operator +(MontgomeryModInt<T> a, MontgomeryModInt<T> b)
        {
            uint r = a._v + b._v - 2 * op.Mod;
            if ((int)r < 0) r += 2 * op.Mod;
            return new MontgomeryModInt<T>(r);
        }
        [凾(256)]
        public static MontgomeryModInt<T> operator -(MontgomeryModInt<T> a, MontgomeryModInt<T> b)
        {
            uint r = a._v - b._v;
            if ((int)r < 0) r += 2 * op.Mod;
            return new MontgomeryModInt<T>(r);
        }
        [凾(256)]
        public static MontgomeryModInt<T> operator *(MontgomeryModInt<T> a, MontgomeryModInt<T> b)
            => new MontgomeryModInt<T>(Reduce((ulong)a._v * b._v));
        [凾(256)]
        public static MontgomeryModInt<T> operator /(MontgomeryModInt<T> a, MontgomeryModInt<T> b)
            => a * b.Inv();

        [凾(256)] public static MontgomeryModInt<T> operator +(MontgomeryModInt<T> a) => a;
        [凾(256)]
        public static MontgomeryModInt<T> operator -(MontgomeryModInt<T> a)
        {
            uint r = (uint)-(int)a._v;
            if ((int)r < 0) r += 2 * op.Mod;
            return new MontgomeryModInt<T>(r);
        }
        [凾(256)]
        public static MontgomeryModInt<T> operator ++(MontgomeryModInt<T> a) => a + 1;
        [凾(256)]
        public static MontgomeryModInt<T> operator --(MontgomeryModInt<T> a) => a - 1;


#if !NET7_0_OR_GREATER
        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [凾(256)]
        public MontgomeryModInt<T> Pow(long n)
        {
            Contract.Assert(0 <= n, $"{nameof(n)} must be positive.");
            MontgomeryModInt<T> x = this, r = 1;

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
#endif

        /// <summary>
        /// 自身を x として、 xy≡1 なる y を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: gcd(x, mod) = 1</para>
        /// </remarks>
        [凾(256)]
        public MontgomeryModInt<T> Inv() => this.Pow(op.Mod - 2);

        [凾(256)] public override bool Equals(object obj) => obj is MontgomeryModInt<T> m && Equals(m);
        [凾(256)]
        public bool Equals(MontgomeryModInt<T> other)
        {
            var v1 = _v;
            var v2 = other._v;

            if (v1 >= op.Mod) v1 -= op.Mod;
            if (v2 >= op.Mod) v2 -= op.Mod;
            return v1 == v2;
        }
        [凾(256)] public static bool operator ==(MontgomeryModInt<T> left, MontgomeryModInt<T> right) => Equals(left, right);
        [凾(256)] public static bool operator !=(MontgomeryModInt<T> left, MontgomeryModInt<T> right) => !Equals(left, right);
        [凾(256)] public override int GetHashCode() => (int)_v;

        public string ToString(string format, IFormatProvider formatProvider) => _v.ToString(format, formatProvider);
#if NET7_0_OR_GREATER
        static int INumberBase<MontgomeryModInt<T>>.Radix => 2;
        static MontgomeryModInt<T> IAdditiveIdentity<MontgomeryModInt<T>, MontgomeryModInt<T>>.AdditiveIdentity => default;
        static MontgomeryModInt<T> IMultiplicativeIdentity<MontgomeryModInt<T>, MontgomeryModInt<T>>.MultiplicativeIdentity => _One;
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.Abs(MontgomeryModInt<T> v) => v;
        static bool INumberBase<MontgomeryModInt<T>>.IsCanonical(MontgomeryModInt<T> v) => true;
        static bool INumberBase<MontgomeryModInt<T>>.IsComplexNumber(MontgomeryModInt<T> v) => false;
        static bool INumberBase<MontgomeryModInt<T>>.IsRealNumber(MontgomeryModInt<T> v) => true;
        static bool INumberBase<MontgomeryModInt<T>>.IsImaginaryNumber(MontgomeryModInt<T> v) => false;
        static bool INumberBase<MontgomeryModInt<T>>.IsEvenInteger(MontgomeryModInt<T> v) => uint.IsEvenInteger(v._v);
        static bool INumberBase<MontgomeryModInt<T>>.IsOddInteger(MontgomeryModInt<T> v) => uint.IsOddInteger(v._v);
        static bool INumberBase<MontgomeryModInt<T>>.IsFinite(MontgomeryModInt<T> v) => true;
        static bool INumberBase<MontgomeryModInt<T>>.IsInfinity(MontgomeryModInt<T> v) => false;
        static bool INumberBase<MontgomeryModInt<T>>.IsInteger(MontgomeryModInt<T> v) => true;
        static bool INumberBase<MontgomeryModInt<T>>.IsPositive(MontgomeryModInt<T> v) => true;
        static bool INumberBase<MontgomeryModInt<T>>.IsNegative(MontgomeryModInt<T> v) => false;
        static bool INumberBase<MontgomeryModInt<T>>.IsPositiveInfinity(MontgomeryModInt<T> v) => false;
        static bool INumberBase<MontgomeryModInt<T>>.IsNegativeInfinity(MontgomeryModInt<T> v) => false;
        static bool INumberBase<MontgomeryModInt<T>>.IsNormal(MontgomeryModInt<T> v) => v._v != 0;
        static bool INumberBase<MontgomeryModInt<T>>.IsSubnormal(MontgomeryModInt<T> v) => false;
        static bool INumberBase<MontgomeryModInt<T>>.IsZero(MontgomeryModInt<T> v) => v._v == 0;
        static bool INumberBase<MontgomeryModInt<T>>.IsNaN(MontgomeryModInt<T> v) => false;
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MaxMagnitude(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(uint.Max(x._v, y._v));
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MaxMagnitudeNumber(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(uint.Max(x._v, y._v));
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MinMagnitude(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(uint.Min(x._v, y._v));
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MinMagnitudeNumber(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(uint.Min(x._v, y._v));
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => long.Parse(s, style, provider);
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.Parse(string s, NumberStyles style, IFormatProvider provider) => long.Parse(s, style, provider);
        static MontgomeryModInt<T> ISpanParsable<MontgomeryModInt<T>>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => long.Parse(s, provider);
        static MontgomeryModInt<T> IParsable<MontgomeryModInt<T>>.Parse(string s, IFormatProvider provider) => long.Parse(s, provider);
        static bool ISpanParsable<MontgomeryModInt<T>>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out MontgomeryModInt<T> result)
        => TryParse(s, NumberStyles.None, provider, out result);
        static bool IParsable<MontgomeryModInt<T>>.TryParse(string s, IFormatProvider provider, out MontgomeryModInt<T> result)
        => TryParse(s, NumberStyles.None, provider, out result);
        static bool INumberBase<MontgomeryModInt<T>>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out MontgomeryModInt<T> result)
        => TryParse(s, style, provider, out result);
        static bool INumberBase<MontgomeryModInt<T>>.TryParse(string s, NumberStyles style, IFormatProvider provider, out MontgomeryModInt<T> result)
        => TryParse(s, style, provider, out result);
        private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out MontgomeryModInt<T> result)
        {
            var b = long.TryParse(s, style, provider, out var r);
            result = r;
            return b;
        }
        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) => _v.TryFormat(destination, out charsWritten, format, provider);


        static bool INumberBase<MontgomeryModInt<T>>.TryConvertFromChecked<TOther>(TOther v, out MontgomeryModInt<T> r)
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
        static bool INumberBase<MontgomeryModInt<T>>.TryConvertFromSaturating<TOther>(TOther v, out MontgomeryModInt<T> r)
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
        static bool INumberBase<MontgomeryModInt<T>>.TryConvertFromTruncating<TOther>(TOther v, out MontgomeryModInt<T> r)
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
        static bool INumberBase<MontgomeryModInt<T>>.TryConvertToChecked<TOther>(MontgomeryModInt<T> v, out TOther r) where TOther : default => WrapChecked(v.Value, out r);
        static bool INumberBase<MontgomeryModInt<T>>.TryConvertToSaturating<TOther>(MontgomeryModInt<T> v, out TOther r) where TOther : default => WrapSaturating(v.Value, out r);
        static bool INumberBase<MontgomeryModInt<T>>.TryConvertToTruncating<TOther>(MontgomeryModInt<T> v, out TOther r) where TOther : default => WrapTruncating(v.Value, out r);

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
#else
        public readonly struct Operator : IArithmeticOperator<MontgomeryModInt<T>>
        {
            public MontgomeryModInt<T> MultiplyIdentity => One;
            [凾(256)] public MontgomeryModInt<T> Add(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => x + y;
            [凾(256)] public MontgomeryModInt<T> Subtract(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => x - y;
            [凾(256)] public MontgomeryModInt<T> Multiply(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => x * y;
            [凾(256)] public MontgomeryModInt<T> Divide(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => x / y;
            [凾(256)] MontgomeryModInt<T> IDivisionOperator<MontgomeryModInt<T>>.Modulo(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => throw new NotSupportedException();
            [凾(256)] public MontgomeryModInt<T> Minus(MontgomeryModInt<T> x) => -x;
            [凾(256)] public MontgomeryModInt<T> Increment(MontgomeryModInt<T> x) => ++x;
            [凾(256)] public MontgomeryModInt<T> Decrement(MontgomeryModInt<T> x) => --x;
        }
#endif
    }
}
