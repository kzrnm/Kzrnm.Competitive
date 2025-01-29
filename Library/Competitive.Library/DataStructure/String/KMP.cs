using System;
using System.Collections.Generic;
#region https://algoful.com/Archive/Algorithm/KMPSearch
#endregion

using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class KMP
    {
        /// <summary>
        /// KMP法で検索するパターンを初期化します。
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static KMP<T> Create<T>(ReadOnlySpan<T> pattern) => new KMP<T>(pattern);
        /// <summary>
        /// KMP法で検索するパターンを初期化します。
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static KMP<T> Create<T>(Span<T> pattern) => new KMP<T>(pattern);
        /// <summary>
        /// KMP法で検索するパターンを初期化します。
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static KMP<T> Create<T>(T[] pattern) => new KMP<T>(pattern);
        /// <summary>
        /// KMP法で検索するパターンを初期化します。
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static KMP<char> Create(string pattern) => new KMP<char>(pattern);
    }
    public readonly ref struct KMP<T>
    {
        readonly ReadOnlySpan<T> pattern;
        readonly int[] table;
        public KMP(ReadOnlySpan<T> pattern) { this.pattern = pattern; table = CreateTable(pattern); }
        [凾(256)]
        static int[] CreateTable(ReadOnlySpan<T> pattern)
        {
            var table = new int[pattern.Length + 1];
            table[0] = -1;
            int j = -1;
            for (int i = 0; i < pattern.Length; i++)
            {
                while (j >= 0 && !EqualityComparer<T>.Default.Equals(pattern[i], pattern[j])) j = table[j];
                table[i + 1] = ++j;
            }
            return table;
        }

        /// <summary>
        /// <paramref name="target"/> の中で pattern と一致するインデックスを返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(|<paramref name="target"/>|)</para>
        /// </remarks>
        [凾(256)]
        public Enumerator Matches(ReadOnlySpan<T> target) => new Enumerator(pattern, target, table);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
        public ref struct Enumerator
        {
            readonly ReadOnlySpan<T> pattern, target;
            readonly int[] table;
            int index, p;
            public Enumerator(ReadOnlySpan<T> pattern, ReadOnlySpan<T> target, int[] table)
            {
                this.pattern = pattern;
                this.target = target;
                this.table = table;
                p = index = 0;
            }
            public int Current => index - pattern.Length;
            public Enumerator GetEnumerator() => this;
            public List<int> ToList()
            {
                var list = new List<int>();
                foreach (var item in this)
                    list.Add(item);
                return list;
            }
            [凾(256)]
            public bool MoveNext()
            {
                while (index < target.Length)
                {
                    if (EqualityComparer<T>.Default.Equals(target[index], pattern[p]))
                    {
                        index++;
                        p++;
                    }
                    else if (p == 0)
                        index++;
                    else
                        p = table[p];

                    if (p == pattern.Length)
                    {
                        p = table[p];
                        return true;
                    }
                }
                return false;
            }
        }
    }
}