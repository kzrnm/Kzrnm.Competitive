using FluentAssertions;
using Xunit;

namespace AtCoderLib.整数
{
    public class その他Tests
    {
        [Theory]
        [InlineData(2, 11, 3, 6, 0)]
        [InlineData(3, 11, 3, 6, 1)]
        [InlineData(4, 11, 3, 6, 2)]
        [InlineData(9, 11, 3, 6, 10)]
        [InlineData(10, 11, 3, 6, 13)]
        [InlineData(11, 11, 3, 6, 16)]
        public void FloorSumTest(long n, long m, long a, long b, long expected)
        {
            その他.FloorSum(n, m, a, b).Should().Be(expected);
        }
    }
}
