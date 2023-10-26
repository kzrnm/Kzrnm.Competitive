
using System.Linq;

namespace Kzrnm.Competitive.Testing.Number
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
            BitOperationsEx.Msb(input).Should().Be(msb);
            BitOperationsEx.Lsb(input).Should().Be(lsb);
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
            BitOperationsEx.Msb(input).Should().Be(msb);
            BitOperationsEx.Lsb(input).Should().Be(lsb);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0b1010, 0b10101, 0b00100)]
        [InlineData(0b1101, 0b11110000, 0b11010000)]
        [InlineData(0b1101, 0b11110, 0b11010)]
        public void ParallelBitDepositInt32(int input, int mask, int res)
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
        public void ParallelBitExtractInt32(int input, int mask, int res)
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

        [Theory]
        [InlineData(0b10101010001010101001000111010101, 0b10101011100010010101010001010101)]
        [InlineData(0b01010101000010110101011010101010, 0b01010101011010101101000010101010)]
        [InlineData(0b11010100101000010101001001010111, 0b11101010010010101000010100101011)]
        [InlineData(0b01010101010101010010000101001011, 0b11010010100001001010101010101010)]
        [InlineData(0b10111001010101010100001010010101, 0b10101001010000101010101010011101)]
        [InlineData(0b11110000110001000010100000010010, 0b01001000000101000010001100001111)]
        public void BitReverseUInt32(uint a, uint b)
        {
            new string(System.Convert.ToString(a, 2).PadLeft(32, '0').Reverse().ToArray())
                .Should().Be(System.Convert.ToString(b, 2).PadLeft(32, '0'));

            BitOperationsEx.BitReverse(a).Should().Be(b);
            BitOperationsEx.BitReverse(b).Should().Be(a);
        }

        [Theory]
        [InlineData(0b0101010100001011010101101010101010101010001010101001000111010101, 0b1010101110001001010101000101010101010101011010101101000010101010)]
        [InlineData(0b1010101000101010100100011101010101010101000010110101011010101010, 0b0101010101101010110100001010101010101011100010010101010001010101)]
        [InlineData(0b0101010101010101001000010100101111010100101000010101001001010111, 0b1110101001001010100001010010101111010010100001001010101010101010)]
        [InlineData(0b1011100101010101010000101001010101010101000010110101011010101010, 0b0101010101101010110100001010101010101001010000101010101010011101)]
        public void BitReverseUInt64(ulong a, ulong b)
        {
            new string(System.Convert.ToString((long)a, 2).PadLeft(64, '0').Reverse().ToArray())
                .Should().Be(System.Convert.ToString((long)b, 2).PadLeft(64, '0'));

            BitOperationsEx.BitReverse(a).Should().Be(b);
            BitOperationsEx.BitReverse(b).Should().Be(a);
        }
    }
}
