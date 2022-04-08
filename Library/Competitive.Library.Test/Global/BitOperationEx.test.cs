using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.GlobalNS
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
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

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0b1010, 0b10101, 0b00100)]
        [InlineData(0b1101, 0b11110000, 0b11010000)]
        [InlineData(0b1101, 0b11110, 0b11010)]
        public void ParallelBitDepositInt32(int input, uint mask, int res)
        {
            BitOperationsEx.ParallelBitDeposit(input, mask).Should().Be(res);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0b1010, 0b10101, 0b00100)]
        [InlineData(0b1101, 0b11110000, 0b11010000)]
        [InlineData(0b1101, 0b11110, 0b11010)]
        public void ParallelBitDepositUInt32(uint input, uint mask, uint res)
        {
            BitOperationsEx.ParallelBitDeposit(input, mask).Should().Be(res);
            BitOperationsEx.ParallelBitDepositLogic(input, mask).Should().Be(res);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0b10101, 0b10101, 0b111)]
        [InlineData(0b1010, 0b10101, 0)]
        [InlineData(0b101101, 0b011110, 0b110)]
        public void ParallelBitExtractInt32(int input, uint mask, int res)
        {
            BitOperationsEx.ParallelBitExtract(input, mask).Should().Be(res);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0b10101, 0b10101, 0b111)]
        [InlineData(0b1010, 0b10101, 0)]
        [InlineData(0b101101, 0b011110, 0b110)]
        public void ParallelBitExtractUInt32(uint input, uint mask, uint res)
        {
            BitOperationsEx.ParallelBitExtract(input, mask).Should().Be(res);
            BitOperationsEx.ParallelBitExtractLogic(input, mask).Should().Be(res);
        }
    }
}
