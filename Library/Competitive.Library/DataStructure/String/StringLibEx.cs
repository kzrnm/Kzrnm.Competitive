using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class StringLibEx
    {
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を求めます。
        /// </summary>
        public static char[] Lcs(string s, string t) => Lcs(s.AsSpan(), t);
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を求めます。
        /// </summary>
        public static T[] Lcs<T>(T[] s, T[] t) => Lcs((ReadOnlySpan<T>)s, t);
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を求めます。
        /// </summary>
        public static T[] Lcs<T>(Span<T> s, Span<T> t) => Lcs((ReadOnlySpan<T>)s, t);
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を求めます。
        /// </summary>
        [凾(256)]
        public static T[] Lcs<T>(ReadOnlySpan<T> s, ReadOnlySpan<T> t)
        {
            if (s.Length < t.Length) return Lcs(t, s);

            var dp = new int[s.Length + 1][];
            dp[0] = new int[s.Length + 1];
            int i, j;
            for (i = 0; i < s.Length; i++)
            {
                var crr = dp[i];
                var nrr = dp[i + 1] = new int[s.Length + 1];
                for (j = 0; j < t.Length; j++)
                {
                    if (EqualityComparer<T>.Default.Equals(s[i], t[j]))
                        nrr[j + 1] = crr[j] + 1;
                    else
                        nrr[j + 1] = Math.Max(crr[j + 1], nrr[j]);
                }
            }
            var list = LCSSearch(dp, s, s.Length, t.Length);
            list.Reverse();
            return list.ToArray();

            static List<T> LCSSearch(int[][] dp, ReadOnlySpan<T> s, int i, int j)
            {
                var list = new List<T>();
                while (i > 0 && j > 0)
                {
                    while (dp[i][j] == dp[i - 1][j])
                    {
                        if (--i <= 0) return list;
                    }
                    while (dp[i][j] == dp[i][j - 1])
                    {
                        if (--j <= 0) return list;
                    }

                    list.Add(s[i - 1]);
                    i--; j--;
                }
                return list;
            }
        }
    }
}