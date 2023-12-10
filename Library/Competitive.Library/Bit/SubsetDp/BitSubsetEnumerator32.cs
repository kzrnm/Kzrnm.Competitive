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
    public static class __BitSubset32_Ext
    {
        /// <summary>
        /// <para><paramref name="n"/> をビット集合としたときの部分集合を列挙します。</para>
        /// <para><paramref name="proper"/> が <see langword="true"/> のときは真部分集合を列挙します。</para>
        /// </summary>
        [凾(256)]
        public static BitSubsetEnumerator32<int> BitSubset(this int n, bool proper = true) => new(n, proper);
        /// <summary>
        /// <para><paramref name="n"/> をビット集合としたときの部分集合を列挙します。</para>
        /// <para><paramref name="proper"/> が <see langword="true"/> のときは真部分集合を列挙します。</para>
        /// </summary>
        [凾(256)]
        public static BitSubsetEnumerator32<uint> BitSubset(this uint n, bool proper = true) => new(n, proper);
    }
    /// <summary>
    /// 整数をビット集合としたときの部分集合を列挙します。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
    public struct BitSubsetEnumerator32<T> : IEnumerable<T>, IEnumerator<T> where T : IBinaryInteger<T>
    {
        uint msk, n;
        byte p;
        /// <summary>
        /// <para><paramref name="v"/> をビット集合としたときの部分集合を列挙します。</para>
        /// <para><paramref name="proper"/> が <see langword="true"/> のときは真部分集合を列挙します。</para>
        /// <para>直接呼び出しは危険。</para>
        /// </summary>
        [凾(256)]
        internal BitSubsetEnumerator32(T v, bool proper)
        {
            Debug.Assert(typeof(T) == typeof(int) || typeof(T) == typeof(uint));
            var u = Unsafe.As<T, uint>(ref v);
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
        public T Current => Unsafe.As<uint, T>(ref n);
        public BitSubsetEnumerator32<T> GetEnumerator() => this;
        [凾(256)]
        void IDisposable.Dispose() { }
        void IEnumerator.Reset() => throw new NotSupportedException();
        object IEnumerator.Current => Current;
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
