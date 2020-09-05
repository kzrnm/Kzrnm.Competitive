using FluentAssertions;
using System.Linq;
using Xunit;

namespace AtCoderLib.範囲演算
{
    public class SegmentTreeLazyTests
    {
        const int N = 100000;
        readonly SegmentTreeLazy segMax = new SegmentTreeLazy(Enumerable.Range(0, N).Select(i => (long)i).ToArray());

        public static TheoryData QueryMax_Data = new TheoryData<int, int, long>
        {
            {0, N, N - 1},
            {0, 60420, 60419 },
            {60421, N, N - 1 },
        };
        [Theory]
        [MemberData(nameof(QueryMax_Data))]
        public void MaxQuery(int from, int toExclusive, long expected)
        {
            segMax.Query(from, toExclusive).Should().Be(expected);
            segMax[from..toExclusive].Should().Be(expected);
        }
        [Fact]
        public void MaxSeg()
        {
            segMax.Apply(1, 3, 2);
            segMax[..N].Should().Be(N - 1);
            segMax[..1].Should().Be(0);
            segMax[..2].Should().Be(3);
            segMax[..3].Should().Be(4);
            segMax[2..3].Should().Be(4);

            segMax.Apply(0, N, 10);
            segMax[..1].Should().Be(10);
            segMax[..2].Should().Be(13);
            segMax[..3].Should().Be(14);
            segMax[2..3].Should().Be(14);
            segMax[..N].Should().Be(N + 9);
        }
    }
}
