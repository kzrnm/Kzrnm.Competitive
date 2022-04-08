using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class WaveletMatrixTests
    {
        public static long[] orig = new long[] {
            1,-1,2,-2,3,3,-3,-2,4,5,6
        };
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
            matrix[index].Should().Be(expected);
        }


        [Fact]
        public void Rank()
        {
            static int Native(long x, int r)
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
                    matrix.Rank(x, r).Should().Be(Native(x, r));
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
                        matrix.KthSmallest(l, r, k).Should().Be(rangeItems[k]);
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
                        matrix.KthLargest(l, r, k).Should().Be(rangeItems[k]);
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
                        matrix.RangeFreq(l, r, upper).Should().Be(Native(l, r, long.MinValue, upper));
                        for (int lower = -8; lower < upper; lower++)
                            matrix.RangeFreq(l, r, lower, upper).Should().Be(Native(l, r, lower, upper), "{0}, {1}, {2}, {3}", l, r, lower, upper);
                    }
        }

        [Fact]
        public void PrevValue()
        {
            for (int l = 0; l < orig.Length; l++)
                for (int r = l + 1; r <= orig.Length; r++)
                    for (int upper = -8; upper <= 8; upper++)
                    {
                        matrix.PrevValue(l, r, upper).Should().Be(
                            orig.AsSpan()[l..r].ToArray().Where(v => v < upper)
                            .Select(v => (long?)v)
                            .DefaultIfEmpty(null).Max(), "{0}, {1}, {2}", l, r, upper);
                    }
        }

        [Fact]
        public void NextValue()
        {
            for (int l = 0; l < orig.Length; l++)
                for (int r = l + 1; r <= orig.Length; r++)
                    for (int lower = -8; lower <= 8; lower++)
                    {
                        matrix.NextValue(l, r, lower).Should().Be(
                            orig.AsSpan()[l..r].ToArray().Where(v => v >= lower)
                            .Select(v => (long?)v)
                            .DefaultIfEmpty(null).Min(), "{0}, {1}, {2}", l, r, lower);
                    }
        }
    }
}
