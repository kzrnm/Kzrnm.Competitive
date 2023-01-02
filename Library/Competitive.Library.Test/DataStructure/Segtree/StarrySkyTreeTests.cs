using Kzrnm.Competitive.DataStructure;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class StarrySkyTreeTests
    {
        const int N = 1000;
        readonly StarrySkyTree<long, StarrySkyTreeOperator> segMax = new(N);
        public StarrySkyTreeTests()
        {
            for (int i = 0; i < N; i++)
                segMax.Apply(i, i + 1, i);
        }

        public static TheoryData QueryMax_Data => new TheoryData<int, int, long>
        {
            {0, N, N - 1},
            {0, 420, 419 },
            {421, N, N - 1 },
        };
        [Theory]
        [MemberData(nameof(QueryMax_Data))]
        public void MaxQuery(int from, int toExclusive, long expected)
        {
            segMax.Prod(from, toExclusive).Should().Be(expected);
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
