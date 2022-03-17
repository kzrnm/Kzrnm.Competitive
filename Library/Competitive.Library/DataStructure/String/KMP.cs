using AtCoder.Internal;
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
        /// KMP法で検索するパターンを初期化する
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static KMP<T> Create<T>(ReadOnlySpan<T> pattern) => new KMP<T>(pattern);
        /// <summary>
        /// KMP法で検索するパターンを初期化する
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static KMP<T> Create<T>(Span<T> pattern) => new KMP<T>(pattern);
        /// <summary>
        /// KMP法で検索するパターンを初期化する
        /// </summary>
        /// <param name="pattern">検索したいパターン</param>
        [凾(256)]
        public static KMP<T> Create<T>(T[] pattern) => new KMP<T>(pattern);
        /// <summary>
        /// KMP法で検索するパターンを初期化する
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
        /// <paramref name="target"/> の中で pattern と一致するインデックスを返す
        /// </summary>
        [凾(256)]
        public Enumerator Matches(ReadOnlySpan<T> target) => new Enumerator(pattern, target, table);

        public ref struct Enumerator
        {
            private readonly ReadOnlySpan<T> pattern;
            private readonly ReadOnlySpan<T> target;
            private readonly int[] table;
            private int index;
            private int p;
            public Enumerator(ReadOnlySpan<T> pattern, ReadOnlySpan<T> target, int[] table)
            {
                this.pattern = pattern;
                this.target = target;
                this.table = table;
                p = index = 0;
            }
            public int Current => index - pattern.Length;
            public Enumerator GetEnumerator() => this;
            public SimpleList<int> ToList()
            {
                var list = new SimpleList<int>();
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