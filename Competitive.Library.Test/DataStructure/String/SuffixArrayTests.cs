using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.Competitive.DataStructure.String
{
    public class SuffixArrayTests
    {
        [Fact]
        public void LCPInt()
        {
            var rnd = new Random();
            for (int n = 0; n < 100; n++)
            {
                var str = rnd.NextIntArray(n, -5, 5);
                var sa = SuffixArray.Create(str);
                var saNative = GetNative((ReadOnlySpan<int>)str);

                for (int i = 0; i < str.Length; i++)
                    for (int j = i; j < str.Length; j++)
                        sa.GetLCP(i, j).Should().Be(sa.GetLCP(j, i)).And.Be(saNative.GetLCP(i, j));
            }
            {
                var str = new[] { -4210, 4219014, -5, -4210, -4210, 4219014, -5, -4210 };
                var sa = SuffixArray.Create(str);
                var saNative = GetNative((ReadOnlySpan<int>)str);
                sa.GetLCP(0, 3).Should().Be(1);
                sa.GetLCP(0, 4).Should().Be(4);

                for (int i = 0; i < str.Length; i++)
                    for (int j = i; j < str.Length; j++)
                        sa.GetLCP(i, j).Should().Be(sa.GetLCP(j, i)).And.Be(saNative.GetLCP(i, j));
            }
        }

        [Fact]
        public void LCPString()
        {
            var rnd = new Random();
            for (int n = 0; n < 100; n++)
            {
                var str = rnd.NextString(n);
                var sa = SuffixArray.Create(str);
                var saNative = GetNative(str.AsSpan());

                for (int i = 0; i < str.Length; i++)
                    for (int j = i; j < str.Length; j++)
                        sa.GetLCP(i, j).Should().Be(sa.GetLCP(j, i)).And.Be(saNative.GetLCP(i, j));
            }
            {
                var str = "abcaabca";
                var sa = SuffixArray.Create(str);
                var saNative = GetNative(str.AsSpan());
                sa.GetLCP(0, 3).Should().Be(1);
                sa.GetLCP(0, 4).Should().Be(4);

                for (int i = 0; i < str.Length; i++)
                    for (int j = i; j < str.Length; j++)
                        sa.GetLCP(i, j).Should().Be(sa.GetLCP(j, i)).And.Be(saNative.GetLCP(i, j));
            }
        }

        static SuffixArrayNative<T> GetNative<T>(ReadOnlySpan<T> str) => new SuffixArrayNative<T>(str);
        private ref struct SuffixArrayNative<T>
        {
            private readonly ReadOnlySpan<T> str;
            public SuffixArrayNative(ReadOnlySpan<T> str)
            {
                this.str = str;
            }

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
