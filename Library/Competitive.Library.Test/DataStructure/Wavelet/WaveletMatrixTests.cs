using TUnit.Assertions.Core;

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

    [Test, MultipleAssertions]
    [Arguments(0, 1)]
    [Arguments(1, -1)]
    [Arguments(2, 2)]
    [Arguments(3, -2)]
    [Arguments(4, 3)]
    [Arguments(5, 3)]
    [Arguments(6, -3)]
    [Arguments(7, -2)]
    [Arguments(8, 4)]
    [Arguments(9, 5)]
    [Arguments(10, 6)]
    public async Task Indexer(int index, long expected)
    {
        await matrix[index].Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    public async Task Rank()
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
                await matrix.Rank(r, x).Should().BeEqualTo(Native(r, x));
                for (int l = 0; l < r; l++)
                    await matrix.Rank(l, r, x).Should().BeEqualTo(Native(r, x) - Native(l, x));
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task KthSmallest()
    {
        for (int l = 0; l < orig.Length; l++)
            for (int r = l + 1; r <= orig.Length; r++)
            {
                var rangeItems = orig.AsSpan()[l..r].ToArray();
                Array.Sort(rangeItems);
                for (int k = 0; k < r - l; k++)
                {
                    await matrix.KthSmallest(l, r, k).Should().BeEqualTo(rangeItems[k]);
                }
            }
    }

    [Test, MultipleAssertions]
    public async Task KthLargest()
    {
        for (int l = 0; l < orig.Length; l++)
            for (int r = l + 1; r <= orig.Length; r++)
            {
                var rangeItems = orig.AsSpan()[l..r].ToArray();
                Array.Sort(rangeItems);
                Array.Reverse(rangeItems);
                for (int k = 0; k < r - l; k++)
                {
                    await matrix.KthLargest(l, r, k).Should().BeEqualTo(rangeItems[k]);
                }
            }
    }

    [Test, MultipleAssertions]
    public async Task RangeFreq()
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
                    await matrix.RangeFreq(l, r, upper).Should().BeEqualTo(Native(l, r, long.MinValue, upper));
                    for (int lower = -8; lower < upper; lower++)
                        await matrix.RangeFreq(l, r, lower, upper).Should().BeEqualTo(Native(l, r, lower, upper));
                }
    }

    [Test, MultipleAssertions]
    public async Task PrevValue()
    {
        for (int l = 0; l < orig.Length; l++)
            for (int r = l + 1; r <= orig.Length; r++)
                for (int upper = -8; upper <= 8; upper++)
                {
                    await matrix.PrevValue(l, r, upper).Should().BeEqualTo(
                        orig.AsSpan()[l..r].ToArray().Where(v => v < upper)
                        .Select(v => (long?)v)
                        .DefaultIfEmpty(null).Max());
                }
    }

    [Test, MultipleAssertions]
    public async Task NextValue()
    {
        for (int l = 0; l < orig.Length; l++)
            for (int r = l + 1; r <= orig.Length; r++)
                for (int lower = -8; lower <= 8; lower++)
                {
                    await matrix.NextValue(l, r, lower).Should().BeEqualTo(
                        orig.AsSpan()[l..r].ToArray().Where(v => v >= lower)
                        .Select(v => (long?)v)
                        .DefaultIfEmpty(null).Min());
                }
    }
}