using System;
using System.Collections.Generic;
#region https://algoful.com/Archive/Algorithm/BMSearch
#endregion

using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class BoyerMoore
    {
        /// <summary>
        /// BoyerMoore法で検索するパターンを初期化する
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static BoyerMoore<T> Create<T>(ReadOnlySpan<T> pattern) => new BoyerMoore<T>(pattern);
        /// <summary>
        /// BoyerMoore法で検索するパターンを初期化する
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static BoyerMoore<T> Create<T>(Span<T> pattern) => new BoyerMoore<T>(pattern);
        /// <summary>
        /// BoyerMoore法で検索するパターンを初期化する
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static BoyerMoore<T> Create<T>(T[] pattern) => new BoyerMoore<T>(pattern);
        /// <summary>
        /// BoyerMoore法で検索するパターンを初期化する
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static BoyerMoore<char> Create(string pattern) => new BoyerMoore<char>(pattern);
    }
    public readonly ref struct BoyerMoore<T>
    {
        readonly ReadOnlySpan<T> pattern;
        readonly Dictionary<T, int> table;
        public BoyerMoore(ReadOnlySpan<T> pattern)
        {
            this.pattern = pattern;
            table = CreateTable(pattern);
        }
        [凾(256)]
        static Dictionary<T, int> CreateTable(ReadOnlySpan<T> pattern)
        {
            var table = new Dictionary<T, int>();
            for (int i = 0; i < pattern.Length; i++)
            {
                table[pattern[i]] = pattern.Length - i - 1;
            }
            return table;
        }

        /// <summary>
        /// <paramref name="target"/> の中で pattern と一致するインデックスを1つ返します。見つからなければ -1 を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: 最良 O(|<see cref="pattern"/>| / |<paramref name="target"/>|) , 最悪 O(|<see cref="pattern"/>| × |<paramref name="target"/>|)</para>
        /// </remarks>
        [凾(256)]
        public int Match(ReadOnlySpan<T> target)
        {
            var i = pattern.Length - 1;
            while (i < target.Length)
            {
                var p = pattern.Length - 1;

                while (p >= 0 && i < target.Length)
                {
                    if (EqualityComparer<T>.Default.Equals(target[i], pattern[p]))
                    {
                        i--; p--;
                    }
                    else
                    {
                        break;
                    }
                }
                if (p < 0) return i + 1;

                // 不一致の場合、ずらし表を参照し i を進める
                // ただし、今比較した位置より後の位置とする
                var shift1 = table.TryGetValue(pattern[p], out int value) ? value : pattern.Length;
                var shift2 = pattern.Length - p; // 比較を開始した地点の1つ後ろの文字
                i += Math.Max(shift1, shift2);
            }

            return -1;
        }
    }
}