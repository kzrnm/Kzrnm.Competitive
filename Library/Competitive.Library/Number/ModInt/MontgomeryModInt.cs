// https://nyaannyaan.github.io/library/modint/montgomery-modint.hpp
using AtCoder;
using AtCoder.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 奇数オンリーの ModInt
    /// </summary>
    public readonly struct MontgomeryModInt<T> : IUtf8ConsoleWriterFormatter, IEquatable<MontgomeryModInt<T>>, IFormattable,
        INumKz<MontgomeryModInt<T>>,
        IModInt<MontgomeryModInt<T>>
        where T : struct, IStaticMod
    {
        internal readonly uint _v;

        static readonly T op = new T();
        internal static readonly uint n2 = (uint)(((ulong)-op.Mod) % op.Mod);
        internal static readonly uint r = GetR();
        /// <summary>
        /// 1
        /// </summary>
        static readonly MontgomeryModInt<T> _One = 1;
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
        /// <summary>
        /// <para>計算済みの <paramref name="a"/> から <see cref="MontgomeryModInt{T}"/> を生成します。</para>
        /// </summary>
        [凾(256)]
        internal static MontgomeryModInt<T> Raw(uint a) => new(a);

        [凾(256)] public static implicit operator MontgomeryModInt<T>(ulong n) => new MontgomeryModInt<T>(n);
        [凾(256)] public static implicit operator MontgomeryModInt<T>(uint n) => new MontgomeryModInt<T>((ulong)n);
        [凾(256)] public static implicit operator MontgomeryModInt<T>(long n) => new MontgomeryModInt<T>(n);
        [凾(256)] public static implicit operator MontgomeryModInt<T>(int n) => new MontgomeryModInt<T>(n);

        [凾(256)] public static explicit operator ulong(MontgomeryModInt<T> n) => (ulong)n.Value;
        [凾(256)] public static explicit operator uint(MontgomeryModInt<T> n) => (uint)n.Value;
        [凾(256)] public static explicit operator long(MontgomeryModInt<T> n) => n.Value;
        [凾(256)] public static explicit operator int(MontgomeryModInt<T> n) => n.Value;

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
            var mm = 2 * op.Mod;
            var r = a._v + b._v - mm;
            if ((int)r < 0) r += mm;
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
        public static MontgomeryModInt<T> operator ++(MontgomeryModInt<T> a) => a + One;
        [凾(256)]
        public static MontgomeryModInt<T> operator --(MontgomeryModInt<T> a) => a - One;

        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [凾(256)]
        public MontgomeryModInt<T> Pow(ulong n)
        {
            MontgomeryModInt<T> x = this, r = One;

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
        public MontgomeryModInt<T> Pow(long n)
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
        public MontgomeryModInt<T> Inv() => Pow(op.Mod - 2);

        [凾(256)] public override bool Equals(object obj) => obj is MontgomeryModInt<T> m && Equals(m);
        [凾(256)]
        public bool Equals(MontgomeryModInt<T> other) => GetHashCode() == other.GetHashCode();

        [凾(256)] public static bool operator ==(MontgomeryModInt<T> left, MontgomeryModInt<T> right) => left.Equals(right);
        [凾(256)] public static bool operator !=(MontgomeryModInt<T> left, MontgomeryModInt<T> right) => !left.Equals(right);
        [凾(256)]
        public override int GetHashCode()
        {
            var v = _v;
            if (v >= op.Mod) v -= op.Mod;
            return (int)v;
        }

        public static bool TryParse(ReadOnlySpan<char> s, out MontgomeryModInt<T> result)
        {
            result = Zero;
            MontgomeryModInt<T> ten = 10u;
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
        public static MontgomeryModInt<T> Parse(ReadOnlySpan<char> s)
            => TryParse(s, out var r) ? r : throw new FormatException();

        [SourceExpander.NotEmbeddingSource] // for xUnit
        public static MontgomeryModInt<T> Parse(string s, IFormatProvider provider) => Parse(s);

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) => Value.TryFormat(destination, out charsWritten, format, provider);
        public string ToString(string format, IFormatProvider formatProvider) => Value.ToString(format, formatProvider);
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.Abs(MontgomeryModInt<T> v) => v;
        static bool INumberBase<MontgomeryModInt<T>>.IsEvenInteger(MontgomeryModInt<T> v) => int.IsEvenInteger(v.Value);
        static bool INumberBase<MontgomeryModInt<T>>.IsOddInteger(MontgomeryModInt<T> v) => int.IsOddInteger(v.Value);
        static bool INumberBase<MontgomeryModInt<T>>.IsInteger(MontgomeryModInt<T> v) => true;
        static bool INumberBase<MontgomeryModInt<T>>.IsPositive(MontgomeryModInt<T> v) => true;
        static bool INumberBase<MontgomeryModInt<T>>.IsNegative(MontgomeryModInt<T> v) => false;
        static bool INumberBase<MontgomeryModInt<T>>.IsNormal(MontgomeryModInt<T> v) => v.Value != 0;
        static bool INumberBase<MontgomeryModInt<T>>.IsNaN(MontgomeryModInt<T> v) => false;
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MaxMagnitude(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(int.Max(x.Value, y.Value));
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MaxMagnitudeNumber(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(int.Max(x.Value, y.Value));
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MinMagnitude(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(int.Min(x.Value, y.Value));
        static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MinMagnitudeNumber(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(int.Min(x.Value, y.Value));

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
    }
}
