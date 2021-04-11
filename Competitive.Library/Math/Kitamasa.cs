using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kzrnm.Competitive
{
    public static partial class Kitamasa
    {
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        public static uint FastKitamasa(uint[] a, uint[] c, long n, int mod)
            => FastKitamasa((ReadOnlySpan<uint>)a, c, n, mod);
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        public static uint FastKitamasa(Span<uint> a, Span<uint> c, long n, int mod)
            => FastKitamasa((ReadOnlySpan<uint>)a, c, n, mod);
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        public static uint FastKitamasa(ReadOnlySpan<uint> a, ReadOnlySpan<uint> c, long n, int mod)
        {
            Contract.Assert(a.Length == c.Length, reason: "漸化式の係数 c と数列 a の数が違います");
            return new Inner(a, c, mod).Calculate(n);
        }


        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        public static uint FastKitamasa<TMod>(uint[] a, uint[] c, long n) where TMod : struct, IStaticMod
            => FastKitamasa<TMod>((ReadOnlySpan<uint>)a, c, n);
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        public static uint FastKitamasa<TMod>(Span<uint> a, Span<uint> c, long n) where TMod : struct, IStaticMod
            => FastKitamasa<TMod>((ReadOnlySpan<uint>)a, c, n);
        /// <summary>
        /// <para><paramref name="a"/>[n+k] = <paramref name="c"/>[0]*<paramref name="a"/>[n+k-1]+...+<paramref name="c"/>[k-1]*<paramref name="a"/>[n] である漸化式 <paramref name="a"/>(0-based) の <paramref name="n"/> を求めます。</para>
        /// <para>計算量: O(k logk logn)</para>
        /// </summary>
        public static uint FastKitamasa<TMod>(ReadOnlySpan<uint> a, ReadOnlySpan<uint> c, long n) where TMod : struct, IStaticMod
        {
            Contract.Assert(a.Length == c.Length, reason: "漸化式の係数 c と数列 a の数が違います");
            return new Inner<TMod>(a, c).Calculate(n);
        }
    }
}
