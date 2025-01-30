using AtCoder.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 最長増加部分列
    /// </summary>
    public static class LongestIncreasingSubsequence
    {
        /// <summary>
        /// <para><paramref name="s"/> のある1つの最長増加部分列と使用したインデックスを返します。</para>
        /// <para><paramref name="strict"/> が <see langword="true"/> ならば狭義単調増加、 <see langword="false"/> ならば広義単調増加な列を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        /// <param name="s">元となるリスト</param>
        /// <param name="strict">狭義単調増加か否か</param>
        [凾(256)] public static (T[] Lis, int[] Indexes) Lis<T>(T[] s, bool strict = true) where T : IComparable<T> => Lis((ReadOnlySpan<T>)s, strict);
        /// <summary>
        /// <para><paramref name="s"/> のある1つの最長増加部分列と使用したインデックスを返します。</para>
        /// <para><paramref name="strict"/> が <see langword="true"/> ならば狭義単調増加、 <see langword="false"/> ならば広義単調増加な列を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        /// <param name="s">元となるリスト</param>
        /// <param name="strict">狭義単調増加か否か</param>
        [凾(256)] public static (T[] Lis, int[] Indexes) Lis<T>(Span<T> s, bool strict = true) where T : IComparable<T> => Lis((ReadOnlySpan<T>)s, strict);
        /// <summary>
        /// <para><paramref name="s"/> のある1つの最長増加部分列と使用したインデックスを返します。</para>
        /// <para><paramref name="strict"/> が <see langword="true"/> ならば狭義単調増加、 <see langword="false"/> ならば広義単調増加な列を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        /// <param name="s">元となるリスト</param>
        /// <param name="strict">狭義単調増加か否か</param>
        [凾(256)]
        public static (T[] Lis, int[] Indexes) Lis<T>(ReadOnlySpan<T> s, bool strict = true) where T : IComparable<T>
            => strict
            ? Lis<T, Vs<T>>(s)
            : Lis<T, Vl<T>>(s);


        /// <summary>
        /// <para><paramref name="s"/> のある1つの最長増加部分列と使用したインデックスを返します。</para>
        /// <para><paramref name="strict"/> が <see langword="true"/> ならば狭義単調増加、 <see langword="false"/> ならば広義単調増加な列を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        /// <param name="s">元となるリスト</param>
        /// <param name="cmp"><see cref="IComparer{T}"/>の実装</param>
        /// <param name="strict">狭義単調増加か否か</param>
        [凾(256)] public static (T[] Lis, int[] Indexes) Lis<T, TCmp>(T[] s, TCmp cmp, bool strict = true) where TCmp : IComparer<T> => Lis((ReadOnlySpan<T>)s, cmp, strict);
        /// <summary>
        /// <para><paramref name="s"/> のある1つの最長増加部分列と使用したインデックスを返します。</para>
        /// <para><paramref name="strict"/> が <see langword="true"/> ならば狭義単調増加、 <see langword="false"/> ならば広義単調増加な列を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        /// <param name="s">元となるリスト</param>
        /// <param name="cmp"><see cref="IComparer{T}"/>の実装</param>
        /// <param name="strict">狭義単調増加か否か</param>
        [凾(256)] public static (T[] Lis, int[] Indexes) Lis<T, TCmp>(Span<T> s, TCmp cmp, bool strict = true) where TCmp : IComparer<T> => Lis((ReadOnlySpan<T>)s, cmp, strict);
        /// <summary>
        /// <para><paramref name="s"/> のある1つの最長増加部分列と使用したインデックスを返します。</para>
        /// <para><paramref name="strict"/> が <see langword="true"/> ならば狭義単調増加、 <see langword="false"/> ならば広義単調増加な列を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        /// <param name="s">元となるリスト</param>
        /// <param name="cmp"><see cref="IComparer{T}"/>の実装</param>
        /// <param name="strict">狭義単調増加か否か</param>
        [凾(256)]
        public static (T[] Lis, int[] Indexes) Lis<T, TCmp>(ReadOnlySpan<T> s, TCmp cmp, bool strict = true) where TCmp : IComparer<T>
            => strict
            ? Lis(s, new Cs<T, TCmp> { cp = cmp })
            : Lis(s, new Cl<T, TCmp> { cp = cmp });

        interface ICp<T>
        {
            int Search(Span<T> s, T v);
        }
#pragma warning disable IDE0251 // メンバーを 'readonly' にする
        /// <summary>
        /// <see cref="IComparable{T}"/> の狭義単調増加列を考える
        /// </summary>
        struct Vs<T> : ICp<T> where T : IComparable<T>
        {
            [凾(256)]
            public int Search(Span<T> s, T v)
            {
                var i = s.LowerBound(v);
                return i < s.Length && s[i].CompareTo(v) == 0 ? -1 : i;
            }
        }
        /// <summary>
        /// <see cref="IComparable{T}"/> の広義単調増加列を考える
        /// </summary>
        struct Vl<T> : ICp<T> where T : IComparable<T>
        {
            [凾(256)] public int Search(Span<T> s, T v) => s.UpperBound(v);
        }

        /// <summary>
        /// <see cref="IComparer{T}"/> による狭義単調増加列を考える
        /// </summary>
        struct Cs<T, TCmp> : ICp<T> where TCmp : IComparer<T>
        {
            public TCmp cp;
            [凾(256)]
            public int Search(Span<T> s, T v)
            {
                var i = s.LowerBound(v, cp);
                return i < s.Length && cp.Compare(s[i], v) == 0 ? -1 : i;
            }
        }
        /// <summary>
        /// <see cref="IComparer{T}"/> による広義単調増加列を考える
        /// </summary>
        struct Cl<T, TCmp> : ICp<T> where TCmp : IComparer<T>
        {
            public TCmp cp;
            [凾(256)] public int Search(Span<T> s, T v) => s.UpperBound(v, cp);
        }
#pragma warning restore IDE0251 // メンバーを 'readonly' にする

        [凾(256)]
        static (T[] Lis, int[] Indexes) Lis<T, Cp>(ReadOnlySpan<T> s, Cp op = default) where Cp : ICp<T>
        {
            if (s.IsEmpty) return (Array.Empty<T>(), Array.Empty<int>());

            // s[i] を最長増加部分列で使うときに使う直前の値のインデックス
            var prevs = new int[s.Length];

            // ls[i]: 長さ i+1 の最長増加部分列の末尾の値
            // ls が常に単調増加になるようにする
            // 
            // li[i]: ls[i] がヒットしたインデックス
            var ls = new List<T>(s.Length) { s[0] };
            var li = new List<int>(s.Length) { 0 };
            prevs[0] = -1;

            for (int i = 1; i < s.Length; i++)
            {
                var sp = ls.AsSpan();
                var ix = op.Search(sp, s[i]);
                if ((uint)ix < (uint)sp.Length)
                {
                    sp[ix] = s[i];
                    li[ix] = i;
                    prevs[i] = ix > 0 ? li[ix - 1] : -1;
                }
                else if (ix == sp.Length)
                {
                    ls.Add(s[i]);

                    prevs[i] = li[^1];
                    li.Add(i);
                }
            }

            var lis = new T[ls.Count];
            var idxs = new int[ls.Count];
            var c = li[^1];
            for (int i = lis.Length - 1; i >= 0; i--)
            {
                lis[i] = s[c];
                idxs[i] = c;
                c = prevs[c];
            }
            Debug.Assert(c == -1);
            return (lis, idxs);
        }
    }
}
