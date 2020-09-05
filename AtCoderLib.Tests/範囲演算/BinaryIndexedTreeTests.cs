using FluentAssertions;
using System.Linq;
using Xunit;

namespace AtCoderLib.範囲演算
{
    public class BinaryIndexedTreeTests
    {
        readonly BinaryIndexedTree bit = new BinaryIndexedTree(Enumerable.Range(0, 100).Select(i => (long)i).ToArray());

        public static TheoryData Query_Data = new TheoryData<int, int, long>
        {
            {0, 100, 4950 },
            {1, 100, 4950 },
            {2, 100, 4949 },
            {3, 100, 4947 },
            {1, 99, 4851},
            {2, 99, 4850 },
        };

        [Fact]
        public void QueryOne()
        {
            long sum = 0;
            for (int i = 0; i <= 100; i++)
            {
                bit.Query(i).Should().Be(sum);
                sum += i;
            }
        }

        [Theory]
        [MemberData(nameof(Query_Data))]
        public void Query(int from, int toExclusive, long expected)
        {
            bit.Query(from, toExclusive).Should().Be(expected);
            bit[from..toExclusive].Should().Be(expected);
        }

        [Fact]
        public void LowerBound()
        {
            long sum = 0;
            bit.LowerBound(0).Should().Be(0);
            for (int i = 1; i < 100; i++)
            {
                sum += i;
                bit.LowerBound(sum).Should().Be(i);
                bit.LowerBound(sum + 1).Should().Be(i + 1);
            }
            bit.LowerBound(long.MaxValue).Should().Be(100);
        }
        [Fact]
        public void AddAndQuery()
        {
            var bit = new BinaryIndexedTree(10);
            bit[0..10].Should().Be(0);
            bit.Add(1, 10);
            bit[0..1].Should().Be(0);
            bit[2..10].Should().Be(0);
            bit[0..10].Should().Be(10);
            bit.Query(10).Should().Be(10);
            bit.Add(2, 7);
            bit[0..1].Should().Be(0);
            bit[2..10].Should().Be(7);
            bit[0..10].Should().Be(17);
            bit.Query(10).Should().Be(17);
            bit.Add(1, 1);
            bit[0..1].Should().Be(0);
            bit[2..10].Should().Be(7);
            bit[0..10].Should().Be(18);
            bit.Query(10).Should().Be(18);
        }
    }
}
