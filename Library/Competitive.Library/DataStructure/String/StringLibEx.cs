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
    }
}