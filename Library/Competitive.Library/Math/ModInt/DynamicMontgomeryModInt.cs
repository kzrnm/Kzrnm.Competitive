// https://nyaannyaan.github.io/library/modint/montgomery-modint.hpp
using AtCoder.Internal;
using AtCoder.Operators;
using Kzrnm.Competitive.IO;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using static Internal.Montgomery;
    /// <summary>
    /// 奇数オンリーの ModInt
    /// </summary>
    public struct DynamicMontgomeryModInt<T> : IUtf8ConsoleWriterFormatter, IEquatable<DynamicMontgomeryModInt<T>> where T : struct
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
        [凾(256)] public static implicit operator DynamicMontgomeryModInt<T>(PropertyConsoleReader r) => new DynamicMontgomeryModInt<T>(r.Long);


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
        public DynamicMontgomeryModInt<T> Pow(long n)
        {
            Contract.Assert(0 <= n, $"{nameof(n)} must be positive.");
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
        {
            var v1 = _v;
            var v2 = other._v;

            if (v1 >= _mod) v1 -= _mod;
            if (v2 >= _mod) v2 -= _mod;
            return v1 == v2;
        }
        [凾(256)] public static bool operator ==(DynamicMontgomeryModInt<T> left, DynamicMontgomeryModInt<T> right) => Equals(left, right);
        [凾(256)] public static bool operator !=(DynamicMontgomeryModInt<T> left, DynamicMontgomeryModInt<T> right) => !Equals(left, right);
        [凾(256)] public override int GetHashCode() => (int)_v;


        public readonly struct Operator : IArithmeticOperator<DynamicMontgomeryModInt<T>>
        {
            public DynamicMontgomeryModInt<T> MultiplyIdentity => One;
            [凾(256)] public DynamicMontgomeryModInt<T> Add(DynamicMontgomeryModInt<T> x, DynamicMontgomeryModInt<T> y) => x + y;
            [凾(256)] public DynamicMontgomeryModInt<T> Subtract(DynamicMontgomeryModInt<T> x, DynamicMontgomeryModInt<T> y) => x - y;
            [凾(256)] public DynamicMontgomeryModInt<T> Multiply(DynamicMontgomeryModInt<T> x, DynamicMontgomeryModInt<T> y) => x * y;
            [凾(256)] public DynamicMontgomeryModInt<T> Divide(DynamicMontgomeryModInt<T> x, DynamicMontgomeryModInt<T> y) => x / y;
            [凾(256)] DynamicMontgomeryModInt<T> IDivisionOperator<DynamicMontgomeryModInt<T>>.Modulo(DynamicMontgomeryModInt<T> x, DynamicMontgomeryModInt<T> y) => throw new NotSupportedException();
            [凾(256)] public DynamicMontgomeryModInt<T> Minus(DynamicMontgomeryModInt<T> x) => -x;
            [凾(256)] public DynamicMontgomeryModInt<T> Increment(DynamicMontgomeryModInt<T> x) => ++x;
            [凾(256)] public DynamicMontgomeryModInt<T> Decrement(DynamicMontgomeryModInt<T> x) => --x;
        }
    }
}
