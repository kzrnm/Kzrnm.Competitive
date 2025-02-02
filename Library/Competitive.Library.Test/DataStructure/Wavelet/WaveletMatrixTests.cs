using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure;

public class WaveletMatrixTests
{
    public static readonly long[] orig = [
        1,
        -1,
        2,
        -2,
        3,
        3,
        -3,
        -2,
        4,
        5,
        6
    ];
    public WaveletMatrix<long> matrix = new(orig);

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    [InlineData(2, 2)]
    [InlineData(3, -2)]
    [InlineData(4, 3)]
    [InlineData(5, 3)]
    [InlineData(6, -3)]
    [InlineData(7, -2)]
    [InlineData(8, 4)]
    [InlineData(9, 5)]
    [InlineData(10, 6)]
    public void Indexer(int index, long expected)
    {
        matrix[index].ShouldBe(expected);
    }


    [Fact]
    public void Rank()
    {
        static int Native(int r, long x)
        {
            int cnt = 0;
            for (int i = 0; i < r; i++)
            {
                if (orig[i] == x)
                    ++cnt;
            }
            return cnt;
        }
        for (long x = -8; x <= 8; x++)
        {
            for (int r = 0; r <= orig.Length; r++)
            {
                matrix.Rank(r, x).ShouldBe(Native(r, x));
                for (int l = 0; l < r; l++)
                    matrix.Rank(l, r, x).ShouldBe(Native(r, x) - Native(l, x));
            }
        }
    }

    [Fact]
    public void KthSmallest()
    {
        for (int l = 0; l < orig.Length; l++)
            for (int r = l + 1; r <= orig.Length; r++)
            {
                var rangeItems = orig.AsSpan()[l..r].ToArray();
                Array.Sort(rangeItems);
                for (int k = 0; k < r - l; k++)
                {
                    matrix.KthSmallest(l, r, k).ShouldBe(rangeItems[k]);
                }
            }
    }

    [Fact]
    public void KthLargest()
    {
        for (int l = 0; l < orig.Length; l++)
            for (int r = l + 1; r <= orig.Length; r++)
            {
                var rangeItems = orig.AsSpan()[l..r].ToArray();
                Array.Sort(rangeItems);
                Array.Reverse(rangeItems);
                for (int k = 0; k < r - l; k++)
                {
                    matrix.KthLargest(l, r, k).ShouldBe(rangeItems[k]);
                }
            }
    }

    [Fact]
    public void RangeFreq()
    {
        static int Native(int l, int r, long lower, long upper)
        {
            int cnt = 0;
            for (int i = l; i < r; i++)
            {
                if (lower <= orig[i] && orig[i] < upper)
                    ++cnt;
            }
            return cnt;
        }
        for (int l = 0; l < orig.Length; l++)
            for (int r = l + 1; r <= orig.Length; r++)
                for (int upper = -8; upper <= 8; upper++)
                {
                    matrix.RangeFreq(l, r, upper).ShouldBe(Native(l, r, long.MinValue, upper));
                    for (int lower = -8; lower < upper; lower++)
                        matrix.RangeFreq(l, r, lower, upper).ShouldBe(Native(l, r, lower, upper), $"{l}, {r}, {lower}, {upper}");
                }
    }

    [Fact]
    public void PrevValue()
    {
        for (int l = 0; l < orig.Length; l++)
            for (int r = l + 1; r <= orig.Length; r++)
                for (int upper = -8; upper <= 8; upper++)
                {
                    matrix.PrevValue(l, r, upper).ShouldBe(
                        orig.AsSpan()[l..r].ToArray().Where(v => v < upper)
                        .Select(v => (long?)v)
                        .DefaultIfEmpty(null).Max(), $"{l}, {r}, {upper}");
                }
    }

    [Fact]
    public void NextValue()
    {
        for (int l = 0; l < orig.Length; l++)
            for (int r = l + 1; r <= orig.Length; r++)
                for (int lower = -8; lower <= 8; lower++)
                {
                    matrix.NextValue(l, r, lower).ShouldBe(
                        orig.AsSpan()[l..r].ToArray().Where(v => v >= lower)
                        .Select(v => (long?)v)
                        .DefaultIfEmpty(null).Min(), $"{l}, {r}, {lower}");
                }
    }
}
