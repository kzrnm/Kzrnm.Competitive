// https://nyaannyaan.github.io/library/modint/montgomery-modint.hpp
using AtCoder;
using AtCoder.Internal;
using AtCoder.Operators;
using Kzrnm.Competitive.IO;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 奇数オンリーの ModInt
    /// </summary>
    public struct LazyMontgomeryModInt<T> : IUtf8ConsoleWriterFormatter, IEquatable<LazyMontgomeryModInt<T>> where T : struct, IStaticMod
    {
        static readonly T op = new T();
        internal static readonly uint n2 = (uint)(((ulong)-op.Mod) % op.Mod);
        internal static readonly uint r = GetR();
        /// <summary>
        /// 1
        /// </summary>
        private static readonly LazyMontgomeryModInt<T> One = 1;
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

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        public LazyMontgomeryModInt(long v) : this(Reduce((ulong)(v % op.Mod + op.Mod) * n2)) { }
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        public LazyMontgomeryModInt(ulong v) : this(Reduce(v % op.Mod * n2)) { }
        LazyMontgomeryModInt(uint a)
        {
            _v = a;
        }

        [凾(256)] public static implicit operator LazyMontgomeryModInt<T>(long value) => new LazyMontgomeryModInt<T>(value);

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
        [凾(256)] public static implicit operator ConsoleOutput(LazyMontgomeryModInt<T> m) => m.ToConsoleOutput();
        [凾(256)] public static implicit operator LazyMontgomeryModInt<T>(PropertyConsoleReader r) => new LazyMontgomeryModInt<T>(r.Long);



        [凾(256)] internal static uint Reduce(ulong b) => (uint)((b + (ulong)((uint)b * (uint)-r) * op.Mod) >> 32);



        [凾(256)]
        public static LazyMontgomeryModInt<T> operator +(LazyMontgomeryModInt<T> a, LazyMontgomeryModInt<T> b)
        {
            uint r = a._v + b._v - 2 * op.Mod;
            if ((int)r < 0) r += 2 * op.Mod;
            return new LazyMontgomeryModInt<T>(r);
        }
        [凾(256)]
        public static LazyMontgomeryModInt<T> operator -(LazyMontgomeryModInt<T> a, LazyMontgomeryModInt<T> b)
        {
            uint r = a._v - b._v;
            if ((int)r < 0) r += 2 * op.Mod;
            return new LazyMontgomeryModInt<T>(r);
        }
        [凾(256)]
        public static LazyMontgomeryModInt<T> operator *(LazyMontgomeryModInt<T> a, LazyMontgomeryModInt<T> b)
            => new LazyMontgomeryModInt<T>(Reduce((ulong)a._v * b._v));
        [凾(256)]
        public static LazyMontgomeryModInt<T> operator /(LazyMontgomeryModInt<T> a, LazyMontgomeryModInt<T> b)
            => a * b.Inv();

        [凾(256)] public static LazyMontgomeryModInt<T> operator +(LazyMontgomeryModInt<T> a) => a;
        [凾(256)]
        public static LazyMontgomeryModInt<T> operator -(LazyMontgomeryModInt<T> a)
        {
            uint r = (uint)-a._v;
            if ((int)r < 0) r += 2 * op.Mod;
            return new LazyMontgomeryModInt<T>(r);
        }
        [凾(256)]
        public static LazyMontgomeryModInt<T> operator ++(LazyMontgomeryModInt<T> a) => a + 1;
        [凾(256)]
        public static LazyMontgomeryModInt<T> operator --(LazyMontgomeryModInt<T> a) => a - 1;



        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [凾(256)]
        public LazyMontgomeryModInt<T> Pow(long n)
        {
            Contract.Assert(0 <= n, $"{nameof(n)} must be positive.");
            LazyMontgomeryModInt<T> x = this, r = 1;

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
        public LazyMontgomeryModInt<T> Inv() => Pow(op.Mod - 2);

        [凾(256)] public override bool Equals(object obj) => obj is LazyMontgomeryModInt<T> m && Equals(m);
        [凾(256)]
        public bool Equals(LazyMontgomeryModInt<T> other)
        {
            var v1 = _v;
            var v2 = other._v;

            if (v1 >= op.Mod) v1 -= op.Mod;
            if (v2 >= op.Mod) v2 -= op.Mod;
            return v1 == v2;
        }
        [凾(256)] public static bool operator ==(LazyMontgomeryModInt<T> left, LazyMontgomeryModInt<T> right) => Equals(left, right);
        [凾(256)] public static bool operator !=(LazyMontgomeryModInt<T> left, LazyMontgomeryModInt<T> right) => !Equals(left, right);
        [凾(256)] public override int GetHashCode() => (int)_v;


        public readonly struct Operator : IArithmeticOperator<LazyMontgomeryModInt<T>>
        {
            public LazyMontgomeryModInt<T> MultiplyIdentity => One;
            [凾(256)] public LazyMontgomeryModInt<T> Add(LazyMontgomeryModInt<T> x, LazyMontgomeryModInt<T> y) => x + y;
            [凾(256)] public LazyMontgomeryModInt<T> Subtract(LazyMontgomeryModInt<T> x, LazyMontgomeryModInt<T> y) => x - y;
            [凾(256)] public LazyMontgomeryModInt<T> Multiply(LazyMontgomeryModInt<T> x, LazyMontgomeryModInt<T> y) => x * y;
            [凾(256)] public LazyMontgomeryModInt<T> Divide(LazyMontgomeryModInt<T> x, LazyMontgomeryModInt<T> y) => x / y;
            [凾(256)] LazyMontgomeryModInt<T> IDivisionOperator<LazyMontgomeryModInt<T>>.Modulo(LazyMontgomeryModInt<T> x, LazyMontgomeryModInt<T> y) => throw new NotSupportedException();
            [凾(256)] public LazyMontgomeryModInt<T> Minus(LazyMontgomeryModInt<T> x) => -x;
            [凾(256)] public LazyMontgomeryModInt<T> Increment(LazyMontgomeryModInt<T> x) => ++x;
            [凾(256)] public LazyMontgomeryModInt<T> Decrement(LazyMontgomeryModInt<T> x) => --x;
        }
    }
}
