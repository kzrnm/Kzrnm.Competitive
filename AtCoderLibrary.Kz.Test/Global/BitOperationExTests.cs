using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class BitOperationsExTests
    {


        [Theory]
        [InlineData(int.MaxValue, 31, 30, 0)]
        [InlineData(int.MinValue, 1, 31, 31)]
        [InlineData(-1, 32, 31, 0)]
        [InlineData(1, 1, 0, 0)]
        [InlineData(0, 0, 0, 32)]
        public void BitOperationInt32(int input, int popCount, int msb, int lsb)
        {
            BitOperationsEx.PopCount(input).Should().Be(popCount);
            BitOperationsEx.MSB(input).Should().Be(msb);
            BitOperationsEx.LSB(input).Should().Be(lsb);
        }

        [Theory]
        [InlineData(long.MaxValue, 63, 62, 0)]
        [InlineData(long.MinValue, 1, 63, 63)]
        [InlineData(-1, 64, 63, 0)]
        [InlineData(1, 1, 0, 0)]
        [InlineData(0, 0, 0, 64)]
        public void BitOperationInt64(long input, int popCount, int msb, int lsb)
        {
            BitOperationsEx.PopCount(input).Should().Be(popCount);
            BitOperationsEx.MSB(input).Should().Be(msb);
            BitOperationsEx.LSB(input).Should().Be(lsb);
        }
    }
}
