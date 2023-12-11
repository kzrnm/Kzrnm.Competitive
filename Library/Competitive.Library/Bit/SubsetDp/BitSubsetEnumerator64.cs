using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using static BitOperationsEx;
    public static class __BitSubset64_Ext
    {
        /// <summary>
        /// <para><paramref name="n"/> をビット集合としたときの部分集合を列挙します。</para>
        /// <para><paramref name="proper"/> が <see langword="true"/> のときは真部分集合を列挙します。</para>
        /// </summary>
        [凾(256)]
        public static BitSubsetEnumerator64<long> BitSubset(this long n, bool proper = true) => new(n, proper);
        /// <summary>
        /// <para><paramref name="n"/> をビット集合としたときの部分集合を列挙します。</para>
        /// <para><paramref name="proper"/> が <see langword="true"/> のときは真部分集合を列挙します。</para>
        /// </summary>
        [凾(256)]
        public static BitSubsetEnumerator64<ulong> BitSubset(this ulong n, bool proper = true) => new(n, proper);

        /// <summary>
        /// <para><paramref name="n"/> をビット集合としたときの部分集合とその補集合を列挙します。</para>
        /// <para><paramref name="proper"/> が <see langword="true"/> のときは真部分集合を列挙します。</para>
        /// </summary>
        /// <remarks>
        /// <para>列挙する部分集合は常にその補集合よりも大きい値になります。</para>
        /// </remarks>
        [凾(256)]
        public static BitSubsetEnumerator64<long>.Combination BitSubsetCombination(this long n, bool proper = true) => new(n, proper);
        /// <summary>
        /// <para><paramref name="n"/> をビット集合としたときの部分集合とその補集合を列挙します。</para>
        /// <para><paramref name="proper"/> が <see langword="true"/> のときは真部分集合を列挙します。</para>
        /// </summary>
        /// <remarks>
        /// <para>列挙する部分集合は常にその補集合よりも大きい値になります。</para>
        /// </remarks>
        [凾(256)]
        public static BitSubsetEnumerator64<ulong>.Combination BitSubsetCombination(this ulong n, bool proper = true) => new(n, proper);
    }
    /// <summary>
    /// 整数をビット集合としたときの部分集合を列挙します。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
    public struct BitSubsetEnumerator64<T> : IEnumerable<T>, IEnumerator<T> where T : IBinaryInteger<T>
    {
        ulong msk, n;
        byte p;
        /// <summary>
        /// <para><paramref name="v"/> をビット集合としたときの部分集合を列挙します。</para>
        /// <para><paramref name="proper"/> が <see langword="true"/> のときは真部分集合を列挙します。</para>
        /// <para>直接呼び出しは危険。</para>
        /// </summary>
        [凾(256)]
        internal BitSubsetEnumerator64(T v, bool proper)
        {
            Debug.Assert(typeof(T) == typeof(long) || typeof(T) == typeof(ulong));
            var u = Unsafe.As<T, ulong>(ref v);
            msk = u;
            n = u;
            if (proper)
                p = 1;
            else
                ++n;
        }
        [凾(256)]
        public bool MoveNext()
        {
            if (--n >= msk)
            {
                if (p != 0) return false;
                p = 1;
            }
            else
                n &= msk;
            return true;
        }

        /// <summary>
        /// 配列を返します。当然ながら列挙中に呼び出すと壊れます。
        /// </summary>
        public T[] ToArray()
        {
            var res = new T[(1 << PopCount(msk)) - p];
            for (var i = 0; i < res.Length; i++)
            {
                MoveNext();
                res[i] = Current;
            }
            return res;
        }
        public T Current => Unsafe.As<ulong, T>(ref n);
        /// <summary>
        /// <see cref="Current"/> の補集合
        /// </summary>
        public T Complement
        {
            [凾(256)]
            get
            {
                var c = ~n & msk;
                return Unsafe.As<ulong, T>(ref c);
            }
        }
        public BitSubsetEnumerator64<T> GetEnumerator() => this;
        void IDisposable.Dispose() { }
        void IEnumerator.Reset() => throw new NotSupportedException();
        object IEnumerator.Current => Current;
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
        /// <summary>
        /// 補集合の組み合わせを列挙します。
        /// </summary>
        public ref struct Combination
        {
            BitSubsetEnumerator64<T> impl;
            public Combination(T v, bool proper)
            {
                impl = new(v, proper);
            }
            public Combination GetEnumerator() => this;

            [凾(256)]
            public bool MoveNext()
            {
                if (!impl.MoveNext()) return false;
                T a = impl.Current, b = impl.Complement;
                Current = (a, b);
                return a > b;
            }

            /// <summary>
            ///　v = a|b, a&amp;b==0 かつ a > b をみたすタプル (a,b)
            /// </summary>
            public (T, T) Current { get; private set; }
        }
    }
}
