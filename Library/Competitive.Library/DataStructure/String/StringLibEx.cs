using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class StringLibEx
    {
        /// <summary>
        /// <paramref name="s"/>[..i] と <paramref name="t"/>[..j] の LCS(最長共通部分列)長を table[i][j] に保持する2次元配列を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>| |<paramref name="t"/>|)</para>
        /// </remarks>
        public static int[][] LcsTable(string s, string t) => LcsTable(s.AsSpan(), t);
        /// <summary>
        /// <paramref name="s"/>[..i] と <paramref name="t"/>[..j] の LCS(最長共通部分列)長を table[i][j] に保持する2次元配列を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>| |<paramref name="t"/>|)</para>
        /// </remarks>
        public static int[][] LcsTable<T>(T[] s, T[] t) => LcsTable((ReadOnlySpan<T>)s, t);
        /// <summary>
        /// <paramref name="s"/>[..i] と <paramref name="t"/>[..j] の LCS(最長共通部分列)長を table[i][j] に保持する2次元配列を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>| |<paramref name="t"/>|)</para>
        /// </remarks>
        public static int[][] LcsTable<T>(Span<T> s, Span<T> t) => LcsTable((ReadOnlySpan<T>)s, t);
        /// <summary>
        /// <paramref name="s"/>[..i] と <paramref name="t"/>[..j] の LCS(最長共通部分列)長を table[i][j] に保持する2次元配列を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>| |<paramref name="t"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static int[][] LcsTable<T>(ReadOnlySpan<T> s, ReadOnlySpan<T> t)
        {
            // dp[i][j] := s[..i] と t[..j] の LCS 長
            var dp = new int[s.Length + 1][];
            dp[0] = new int[t.Length + 1];
            int i, j;
            for (i = 0; i < s.Length; i++)
            {
                var crr = dp[i];
                var nrr = dp[i + 1] = new int[t.Length + 1];
                for (j = 0; j < t.Length; j++)
                {
                    if (EqualityComparer<T>.Default.Equals(s[i], t[j]))
                        nrr[j + 1] = crr[j] + 1;
                    else
                        nrr[j + 1] = Math.Max(crr[j + 1], nrr[j]);
                }
            }
            return dp;
        }

        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>| |<paramref name="t"/>|)</para>
        /// </remarks>
        public static char[] Lcs(string s, string t) => Lcs(s.AsSpan(), t);
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>| |<paramref name="t"/>|)</para>
        /// </remarks>
        public static T[] Lcs<T>(T[] s, T[] t) => Lcs((ReadOnlySpan<T>)s, t);
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>| |<paramref name="t"/>|)</para>
        /// </remarks>
        public static T[] Lcs<T>(Span<T> s, Span<T> t) => Lcs((ReadOnlySpan<T>)s, t);
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>| |<paramref name="t"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static T[] Lcs<T>(ReadOnlySpan<T> s, ReadOnlySpan<T> t)
        {
            var dp = LcsTable(s, t);
            var i = s.Length;
            var j = t.Length;
            var list = new List<T>();
            while (i > 0 && j > 0)
            {
                while (dp[i][j] == dp[i - 1][j])
                {
                    if (--i <= 0)
                        goto R;
                }
                while (dp[i][j] == dp[i][j - 1])
                {
                    if (--j <= 0)
                        goto R;
                }

                list.Add(s[i - 1]);
                i--; j--;
            }
        R:
            list.Reverse();
            return list.ToArray();
        }

        /// <summary>
        /// <paramref name="s"/>の i 文字目を中心とした回文の半径を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        /// <example>abcbaba → 1131121</example>
        public static int[] Manacher(string s) => Manacher(s.AsSpan());
        /// <summary>
        /// <paramref name="s"/>の i 文字目を中心とした回文の半径を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        /// <example>abcbaba → 1131121</example>
        public static int[] Manacher<T>(T[] s) => Manacher((ReadOnlySpan<T>)s);
        /// <summary>
        /// <paramref name="s"/>の i 文字目を中心とした回文の半径を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        /// <example>abcbaba → 1131121</example>
        public static int[] Manacher<T>(Span<T> s) => Manacher((ReadOnlySpan<T>)s);
        /// <summary>
        /// <paramref name="s"/>の i 文字目を中心とした回文の半径を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        /// <example>abcbaba → 1131221</example>
        [凾(256)]
        public static int[] Manacher<T>(ReadOnlySpan<T> s)
        {
            var rt = new int[s.Length];
            for (int i = 0, j = 0, k; i < s.Length; i += k, j -= k)
            {
                while (
                    (i - j) is var d && (uint)d < (uint)s.Length
                    && (i + j) is var e && (uint)e < (uint)s.Length
                    && EqualityComparer<T>.Default.Equals(s[d], s[e]))
                    ++j;
                rt[i] = j;

                k = 1;
                while (
                    (i - k) is var d && (uint)d < (uint)s.Length
                    && (i + k) is var e && (uint)e < (uint)s.Length
                    && k + rt[d] < j)
                {
                    rt[e] = rt[d];
                    ++k;
                }
            }
            return rt;
        }
    }
}