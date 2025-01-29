using AtCoder;
using AtCoder.Internal;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class SuffixArray
    {
        /// <inheritdoc cref="Create{T}(ReadOnlySpan{T})"/>
        [凾(256)]
        public static SuffixArray Create(string s)
            => Create<char>(s);

        /// <inheritdoc cref="Create{T}(ReadOnlySpan{T})"/>
        [凾(256)]
        public static SuffixArray Create<T>(Span<T> s)
            => Create((ReadOnlySpan<T>)s);

        /// <inheritdoc cref="Create{T}(ReadOnlySpan{T})"/>
        [凾(256)]
        public static SuffixArray Create<T>(T[] s)
            => Create((ReadOnlySpan<T>)s);

        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array を構築します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="s"/>|log|<paramref name="s"/>|), 空間O(|<paramref name="s"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static SuffixArray Create<T>(ReadOnlySpan<T> s)
        {
            var sa = StringLib.SuffixArray(s);
            var lcp = StringLib.LcpArray(s, sa);
            return new SuffixArray(sa, lcp);
        }

        /// <summary>
        /// <para>Suffix Array: [0...n-1] の順列で s[SA[i]..n) &lt; s[SA[i+1]..n) を満たすもの</para>
        /// <para>言い換えると、s[i...] の辞書順に i が入っている</para>
        /// </summary>
        public int[] SA { get; }
        /// <summary>
        /// <para>Rank: Suffix Array の逆引き。</para>
        /// <para>言い換えると、s[i...] が辞書順で何番目かを格納している。</para>
        /// </summary>
        public int[] Rank { get; }
        /// <summary>
        /// <para>LCP Array: s[SA[i]..n), s[SA[i+1]..n) の LCP(Longest Common Prefix) の長さ。</para>
        /// <para>言い換えると、辞書順で次に来るものとの LCP の長さ。</para>
        /// </summary>
        public int[] LcpArray { get; }
        /// <summary>
        /// Suffix Array の長さ = LCP Array の長さ + 1
        /// </summary>
        int Length { get; }

        /// <summary>
        /// s[<paramref name="i"/>:] と s[<paramref name="j"/>:] の LCP(Longest Common Prefix) の長さ。
        /// </summary>
        [凾(256)]
        public int LongestCommonPrefix(int i, int j)
        {
            if (i == j) return Length - i;
            i = Rank[i]; j = Rank[j];
            if (i > j) (i, j) = (j, i);
            return rmq.Prod(i, j);
        }

        /// <summary>
        /// LCP Array の最小値を取得する RMQ
        /// </summary>
        readonly SparseTable<int, MinOp> rmq;
        SuffixArray(int[] suffixArray, int[] lcpArray)
        {
            Contract.Assert(suffixArray.Length == lcpArray.Length + 1);
            Length = suffixArray.Length;
            SA = suffixArray;
            LcpArray = lcpArray;
            Rank = new int[suffixArray.Length];
            for (int i = 0; i < suffixArray.Length; i++)
                Rank[suffixArray[i]] = i;

            var h = LcpArray;
            if (h.Length == 0)
                h = new int[1];
            rmq = new SparseTable<int, MinOp>(h);
        }
        readonly struct MinOp : ISparseTableOperator<int>
        {
            [凾(256)]
            public int Operate(int x, int y) => Math.Min(x, y);
        }
    }
}