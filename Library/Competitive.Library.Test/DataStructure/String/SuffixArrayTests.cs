using System.Diagnostics.CodeAnalysis;

namespace Kzrnm.Competitive.Testing.DataStructure.String;

public class SuffixArrayTests
{
    [Test]
    [SuppressMessage("Usage", "TUnitAssertions0002")]
    public async Task LCPInt()
    {
        var rnd = new Random();
        for (int n = 1; n < 50; n++)
        {
            var str = rnd.NextIntArray(n, -5, 5);
            var sa = SuffixArray.Create(str);
            var saNative = GetNative((ReadOnlySpan<int>)str);

            await using var multi = Assert.MultipleAsync();
            for (int i = 0; i < str.Length; i++)
                for (int j = i; j < str.Length; j++)
                {
                    multi.Add(sa.LongestCommonPrefix(i, j).Should().BeEqualTo(saNative.GetLCP(i, j)));
                    multi.Add(sa.LongestCommonPrefix(j, i).Should().BeEqualTo(saNative.GetLCP(i, j)));
                }
        }
        {
            var str = Enumerable.Repeat(1, 50).ToArray();
            var sa = SuffixArray.Create(str);
            var saNative = GetNative((ReadOnlySpan<int>)str);

            await using var multi = Assert.MultipleAsync();
            for (int i = 0; i < str.Length; i++)
                for (int j = i; j < str.Length; j++)
                {
                    multi.Add(sa.LongestCommonPrefix(i, j).Should().BeEqualTo(saNative.GetLCP(i, j)));
                    multi.Add(sa.LongestCommonPrefix(j, i).Should().BeEqualTo(saNative.GetLCP(i, j)));
                }
        }
        {
            var str = Enumerable.Range(0, 50).ToArray();
            var sa = SuffixArray.Create(str);
            var saNative = GetNative((ReadOnlySpan<int>)str);

            await using var multi = Assert.MultipleAsync();
            for (int i = 0; i < str.Length; i++)
                for (int j = i; j < str.Length; j++)
                {
                    multi.Add(sa.LongestCommonPrefix(i, j).Should().BeEqualTo(saNative.GetLCP(i, j)));
                    multi.Add(sa.LongestCommonPrefix(j, i).Should().BeEqualTo(saNative.GetLCP(i, j)));
                }
        }
        {
            var str = new[] { -4210, 4219014, -5, -4210, -4210, 4219014, -5, -4210 };
            var sa = SuffixArray.Create(str);
            var saNative = GetNative((ReadOnlySpan<int>)str);

            await using var multi = Assert.MultipleAsync();

            multi.Add(sa.LongestCommonPrefix(0, 3).Should().BeEqualTo(1));
            multi.Add(sa.LongestCommonPrefix(0, 4).Should().BeEqualTo(4));

            for (int i = 0; i < str.Length; i++)
                for (int j = i; j < str.Length; j++)
                {
                    multi.Add(sa.LongestCommonPrefix(i, j).Should().BeEqualTo(saNative.GetLCP(i, j)));
                    multi.Add(sa.LongestCommonPrefix(j, i).Should().BeEqualTo(saNative.GetLCP(i, j)));
                }
        }
    }

    [Test]
    [SuppressMessage("Usage", "TUnitAssertions0002")]
    public async Task LCPString()
    {
        var rnd = new Random();
        for (int n = 1; n < 50; n++)
        {
            var str = rnd.NextString(n);
            var sa = SuffixArray.Create(str);
            var saNative = GetNative(str.AsSpan());

            await using var multi = Assert.MultipleAsync();
            for (int i = 0; i < str.Length; i++)
                for (int j = i; j < str.Length; j++)
                {
                    multi.Add(sa.LongestCommonPrefix(i, j).Should().BeEqualTo(saNative.GetLCP(i, j)));
                    multi.Add(sa.LongestCommonPrefix(j, i).Should().BeEqualTo(saNative.GetLCP(i, j)));
                }
        }
        {
            var str = "abcaabca";
            var sa = SuffixArray.Create(str);
            var saNative = GetNative(str.AsSpan());
            
            await using var multi = Assert.MultipleAsync();
            multi.Add(sa.LongestCommonPrefix(0, 3).Should().BeEqualTo(1));
            multi.Add(sa.LongestCommonPrefix(0, 4).Should().BeEqualTo(4));

            multi.Add(sa.SA.Should().BeEquivalentOrderTo([
                7, // a
                3, // aabca
                4, // abca
                0, // abcaabca
                5, // bca
                1, // bcaabca
                6, // ca
                2,  // caabca
            ]));
            multi.Add(sa.LcpArray.Should().BeEquivalentOrderTo([
                1, // a - aabca
                1, // aabca - abca
                4, // abca - abcaabca
                0, // abcaabca - bca
                3, // bca - bcaabca
                0, // bcaabca - ca
                2,  // ca - caabca
            ]));
            multi.Add(sa.Rank.Should().BeEquivalentOrderTo([3, 5, 7, 1, 2, 4, 6, 0]));

            for (int i = 0; i < str.Length; i++)
                for (int j = i; j < str.Length; j++)
                {
                    multi.Add(sa.LongestCommonPrefix(i, j).Should().BeEqualTo(saNative.GetLCP(i, j)));
                    multi.Add(sa.LongestCommonPrefix(j, i).Should().BeEqualTo(saNative.GetLCP(i, j)));
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