using FluentAssertions;
using System.Linq;
using Xunit;

namespace AtCoderLib.範囲演算
{
    public class BinaryIndexedTreeRangeTests
    {
        [Fact]
        public void AddAndSum()
        {
            var bit = new BinaryIndexedTreeRange(10);
            bit.Sum(10).Should().Be(0);
            bit.Sum(0, 10).Should().Be(0);
            bit[0..10].Should().Be(0);

            bit.Add(1, 3, 2);
            bit.Sum(10).Should().Be(4);
            bit.Sum(0, 10).Should().Be(4);
            bit[0..2].Should().Be(2);
            bit[0..3].Should().Be(4);
            bit[0..10].Should().Be(4);

            bit.Add(2, 4, 7);
            bit.Sum(10).Should().Be(18);
            bit.Sum(0, 10).Should().Be(18);
            bit[0..2].Should().Be(2);
            bit[0..3].Should().Be(11);
            bit[0..10].Should().Be(18);
        }
    }
}