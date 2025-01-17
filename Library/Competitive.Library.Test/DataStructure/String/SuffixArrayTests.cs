using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    public class SuffixArrayTests
    {
        [Fact]
        public void LCPInt()
        {
            var rnd = new Random();
            for (int n = 1; n < 50; n++)
            {
                var str = rnd.NextIntArray(n, -5, 5);
                var sa = SuffixArray.Create(str);
                var saNative = GetNative((ReadOnlySpan<int>)str);

                for (int i = 0; i < str.Length; i++)
                    for (int j = i; j < str.Length; j++)
                    {
                        sa.LongestCommonPrefix(i, j).ShouldBe(saNative.GetLCP(i, j));
                        sa.LongestCommonPrefix(j, i).ShouldBe(saNative.GetLCP(i, j));
                    }
            }
            {
                var str = Enumerable.Repeat(1, 50).ToArray();
                var sa = SuffixArray.Create(str);
                var saNative = GetNative((ReadOnlySpan<int>)str);

                for (int i = 0; i < str.Length; i++)
                    for (int j = i; j < str.Length; j++)
                    {
                        sa.LongestCommonPrefix(i, j).ShouldBe(saNative.GetLCP(i, j));
                        sa.LongestCommonPrefix(j, i).ShouldBe(saNative.GetLCP(i, j));
                    }
            }
            {
                var str = Enumerable.Range(0, 50).ToArray();
                var sa = SuffixArray.Create(str);
                var saNative = GetNative((ReadOnlySpan<int>)str);

                for (int i = 0; i < str.Length; i++)
                    for (int j = i; j < str.Length; j++)
                    {
                        sa.LongestCommonPrefix(i, j).ShouldBe(saNative.GetLCP(i, j));
                        sa.LongestCommonPrefix(j, i).ShouldBe(saNative.GetLCP(i, j));
                    }
            }
            {
                var str = new[] { -4210, 4219014, -5, -4210, -4210, 4219014, -5, -4210 };
                var sa = SuffixArray.Create(str);
                var saNative = GetNative((ReadOnlySpan<int>)str);
                sa.LongestCommonPrefix(0, 3).ShouldBe(1);
                sa.LongestCommonPrefix(0, 4).ShouldBe(4);

                for (int i = 0; i < str.Length; i++)
                    for (int j = i; j < str.Length; j++)
                    {
                        sa.LongestCommonPrefix(i, j).ShouldBe(saNative.GetLCP(i, j));
                        sa.LongestCommonPrefix(j, i).ShouldBe(saNative.GetLCP(i, j));
                    }
            }
        }

        [Fact]
        public void LCPString()
        {
            var rnd = new Random();
            for (int n = 1; n < 50; n++)
            {
                var str = rnd.NextString(n);
                var sa = SuffixArray.Create(str);
                var saNative = GetNative(str.AsSpan());

                for (int i = 0; i < str.Length; i++)
                    for (int j = i; j < str.Length; j++)
                    {
                        sa.LongestCommonPrefix(i, j).ShouldBe(saNative.GetLCP(i, j));
                        sa.LongestCommonPrefix(j, i).ShouldBe(saNative.GetLCP(i, j));
                    }
            }
            {
                var str = "abcaabca";
                var sa = SuffixArray.Create(str);
                var saNative = GetNative(str.AsSpan());
                sa.LongestCommonPrefix(0, 3).ShouldBe(1);
                sa.LongestCommonPrefix(0, 4).ShouldBe(4);

                sa.SA.ShouldBe([
                    7, // a
                    3, // aabca
                    4, // abca
                    0, // abcaabca
                    5, // bca
                    1, // bcaabca
                    6, // ca
                    2,  // caabca
                ]);
                sa.LcpArray.ShouldBe([
                    1, // a - aabca
                    1, // aabca - abca
                    4, // abca - abcaabca
                    0, // abcaabca - bca
                    3, // bca - bcaabca
                    0, // bcaabca - ca
                    2,  // ca - caabca
                ]);
                sa.Rank.ShouldBe([3, 5, 7, 1, 2, 4, 6, 0]);

                for (int i = 0; i < str.Length; i++)
                    for (int j = i; j < str.Length; j++)
                    {
                        sa.LongestCommonPrefix(i, j).ShouldBe(saNative.GetLCP(i, j));
                        sa.LongestCommonPrefix(j, i).ShouldBe(saNative.GetLCP(i, j));
                    }
            }
        }

        static SuffixArrayNative<T> GetNative<T>(ReadOnlySpan<T> str) => new(str);
        private readonly ref struct SuffixArrayNative<T>(ReadOnlySpan<T> str)
        {
            private readonly ReadOnlySpan<T> str = str;

            public int GetLCP(int i, int j)
            {
                if (i > j) (i, j) = (j, i);

                for (int l = 0; j + l < str.Length; l++)
                {
                    if (!EqualityComparer<T>.Default.Equals(str[i + l], str[j + l]))
                        return l;
                }
                return str.Length - j;
            }
        }
    }
}
