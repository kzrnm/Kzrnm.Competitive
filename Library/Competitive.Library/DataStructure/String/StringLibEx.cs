using AtCoder;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class StringLibEx
    {
        /// <inheritdoc cref="LcsTable{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
        [凾(256)]
        public static int[][] LcsTable(string s, string t) => LcsTable(s.AsSpan(), t);
        /// <inheritdoc cref="LcsTable{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
        [凾(256)]
        public static int[][] LcsTable<T>(T[] s, T[] t) => LcsTable((ReadOnlySpan<T>)s, t);
        /// <inheritdoc cref="LcsTable{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
        [凾(256)]
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

        /// <inheritdoc cref="Lcs{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
        [凾(256)]
        public static char[] Lcs(string s, string t) => Lcs(s.AsSpan(), t);
        /// <inheritdoc cref="Lcs{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
        [凾(256)]
        public static T[] Lcs<T>(T[] s, T[] t) => Lcs((ReadOnlySpan<T>)s, t);
        /// <inheritdoc cref="Lcs{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
        [凾(256)]
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


        /// <inheritdoc cref="RunEnumerate{T}(ReadOnlySpan{T})"/>
        [凾(256)]
        public static (int From, int ToExclusive)[][] RunEnumerate(string s)
            => RunEnumerate((ReadOnlySpan<char>)s);
        /// <inheritdoc cref="RunEnumerate{T}(ReadOnlySpan{T})"/>
        [凾(256)]
        public static (int From, int ToExclusive)[][] RunEnumerate<T>(T[] s)
            => RunEnumerate((ReadOnlySpan<T>)s);
        /// <inheritdoc cref="RunEnumerate{T}(ReadOnlySpan{T})"/>
        [凾(256)]
        public static (int From, int ToExclusive)[][] RunEnumerate<T>(Span<T> s)
            => RunEnumerate((ReadOnlySpan<T>)s);

        /// <summary>
        /// <paramref name="s"/> の run を返します。
        /// </summary>
        /// <remarks>
        /// <para>戻り値を run とします。</para>
        /// <para>run[i] は長さ i の文字列の繰り返しの範囲を保持します。</para>
        /// <para>繰り返しは途中で終わることもできます。</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// RunEnumerate("mississippi") == new (int From, int ToExclusive)[]{
        ///   [],
        ///   [(2,4),(5,7),(8,10)],
        ///   [],
        ///   [(1,8)], // iss の繰り返し: ississi
        /// }
        /// </code>
        /// </example>
        [凾(256)]
        public static (int From, int ToExclusive)[][] RunEnumerate<T>(ReadOnlySpan<T> s)
        {
            var a = s.ToArray();
            Array.Reverse(a);
            var tmp = new RunTmp<T>(a);
            tmp.Run(0, a.Length, 0);
            foreach (var run in tmp.runs)
                if (run != null)
                    foreach (ref var tup in run.AsSpan())
                        tup = (a.Length - tup.ToExclusive, a.Length - tup.From);
            Array.Reverse(a);
            tmp.Run(0, a.Length, 1);

            var runs = tmp.runs;
            var vis = new HashSet<int>[a.Length];
            var rt = new (int From, int ToExclusive)[runs.Length][];
            rt[0] = Array.Empty<(int, int)>();
            for (int ph = 1; ph < rt.Length; ph++)
            {
                if (runs[ph] == null)
                {
                    rt[ph] = Array.Empty<(int, int)>();
                    continue;
                }
                var run = runs[ph].AsSpan();
                run.Select(t => ((ulong)t.From << 32) | (uint)~t.ToExclusive).AsSpan().Sort(run);

                var res = new List<(int, int)>();
                var prevT = -1;
                foreach (var (f, t) in run)
                {
                    if (t <= prevT) continue;
                    prevT = t;
                    if ((vis[f] ??= new()).Add(t))
                    {
                        res.Add((f, t));
                    }
                }

                rt[ph] = res.ToArray();
            }
            return rt;
        }
        readonly ref struct RunTmp<T>
        {
            readonly Span<T> a;
            public readonly List<(int From, int ToExclusive)>[] runs;
            public RunTmp(Span<T> s)
            {
                a = s;
                runs = new List<(int From, int ToExclusive)>[s.Length / 2 + 1];
            }
            public void Run(int l, int r, byte f)
            {
                if (l + 1 >= r) return;
                int m = (l + r + f) / 2;
                Run(l, m, f);
                Run(m, r, f);

                int[] z1, z2;
                var zs1 = a[l..m];
                zs1.Reverse();
                z1 = StringLib.ZAlgorithm<T>(zs1);
                zs1.Reverse();

                {
                    var zs2b = ArrayPool<T>.Shared.Rent(r - m + r - l);
                    var zs2 = zs2b.AsSpan(0, r - m + r - l);
                    a[m..r].CopyTo(zs2);
                    a[l..r].CopyTo(zs2[(r - m)..]);
                    z2 = StringLib.ZAlgorithm<T>(zs2);
                    ArrayPool<T>.Shared.Return(zs2b);
                }

                for (int i = m - 1; i >= l; i--)
                {
                    int z1d = 0; int z2d = 0;
                    {
                        int d1 = m - i;
                        int d2 = r - l - d1;
                        if ((uint)d1 < (uint)z1.Length)
                            z1d = z1[d1];
                        if ((uint)d2 < (uint)z2.Length)
                            z2d = z2[d2];
                    }
                    int l1 = Math.Min(i - l, z1d);
                    int l2 = Math.Min(r - m, z2d);
                    int le = i - l1, ri = m + l2, peri = m - i;
                    if (ri - le >= 2 * peri)
                    {
                        ref var rt = ref runs.AsSpan()[m - i];
                        (rt ??= new()).Add((i - l1, m + l2));
                    }
                }
            }
        }
    }
}