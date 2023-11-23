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
        /// <para><paramref name="s"/> の最長増加部分列を1つ返します。</para>
        /// <para><paramref name="s"/> が <see langword="true"/> ならば狭義単調増加、 <see langword="false"/> ならば広義単調増加な列を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        [凾(256)] public static (T[] Lis, int[] Indexes) Lis<T>(T[] s, bool strict = true) where T : IComparable<T> => Lis((ReadOnlySpan<T>)s, strict);
        /// <summary>
        /// <para><paramref name="s"/> の最長増加部分列を1つ返します。</para>
        /// <para><paramref name="s"/> が <see langword="true"/> ならば狭義単調増加、 <see langword="false"/> ならば広義単調増加な列を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        [凾(256)] public static (T[] Lis, int[] Indexes) Lis<T>(Span<T> s, bool strict = true) where T : IComparable<T> => Lis((ReadOnlySpan<T>)s, strict);
        /// <summary>
        /// <para><paramref name="s"/> の最長増加部分列を1つ返します。</para>
        /// <para><paramref name="s"/> が <see langword="true"/> ならば狭義単調増加、 <see langword="false"/> ならば広義単調増加な列を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: |<paramref name="s"/>| log |<paramref name="s"/>|</para>
        /// </remarks>
        [凾(256)]
        public static (T[] Lis, int[] Indexes) Lis<T>(ReadOnlySpan<T> s, bool strict = true) where T : IComparable<T>
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

            for (int i = 1; i < s.Length; i++)
            {
                var sp = ls.AsSpan();
                var ix = strict
                    ? sp.LowerBound(s[i])
                    : sp.UpperBound(s[i]);
                if ((uint)ix < (uint)sp.Length)
                {
                    sp[ix] = s[i];
                    li[ix] = i;

                    prevs[i] = ix > 0 ? li[ix - 1] : -1;

                }
                else
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
