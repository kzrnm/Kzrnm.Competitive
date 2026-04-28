namespace Kzrnm.Competitive.Testing.Number;

public class BitOperationsExTests
{
    [Test, MultipleAssertions]
    [Arguments(int.MaxValue, 31, 30, 0)]
    [Arguments(int.MinValue, 1, 31, 31)]
    [Arguments(-1, 32, 31, 0)]
    [Arguments(1, 1, 0, 0)]
    [Arguments(0, 0, 0, 32)]
    public async Task BitOperationInt32(int input, int popCount, int msb, int lsb)
    {
        await BitOperationsEx.PopCount(input).Should().BeEqualTo(popCount);
        await BitOperationsEx.Msb(input).Should().BeEqualTo(msb);
        await BitOperationsEx.Lsb(input).Should().BeEqualTo(lsb);
    }

    [Test, MultipleAssertions]
    [Arguments(long.MaxValue, 63, 62, 0)]
    [Arguments(long.MinValue, 1, 63, 63)]
    [Arguments(-1, 64, 63, 0)]
    [Arguments(1, 1, 0, 0)]
    [Arguments(0, 0, 0, 64)]
    public async Task BitOperationInt64(long input, int popCount, int msb, int lsb)
    {
        await BitOperationsEx.PopCount(input).Should().BeEqualTo(popCount);
        await BitOperationsEx.Msb(input).Should().BeEqualTo(msb);
        await BitOperationsEx.Lsb(input).Should().BeEqualTo(lsb);
    }

    [Test]
    [Arguments(0, 0, 0)]
    [Arguments(0b1010, 0b10101, 0b00100)]
    [Arguments(0b1101, 0b11110000, 0b11010000)]
    [Arguments(0b1101, 0b11110, 0b11010)]
    public async Task ParallelBitDepositInt32(int input, int mask, int res)
    {
        await BitOperationsEx.ParallelBitDeposit(input, mask).Should().BeEqualTo(res);
    }

    [Test, MultipleAssertions]
    [Arguments(0, 0, 0)]
    [Arguments(0b1010, 0b10101, 0b00100)]
    [Arguments(0b1101, 0b11110000, 0b11010000)]
    [Arguments(0b1101, 0b11110, 0b11010)]
    public async Task ParallelBitDepositUInt32(uint input, uint mask, uint res)
    {
        await BitOperationsEx.ParallelBitDeposit(input, mask).Should().BeEqualTo(res);
        await BitOperationsEx.ParallelBitDepositLogic(input, mask).Should().BeEqualTo(res);
    }

    [Test]
    [Arguments(0, 0, 0)]
    [Arguments(0b10101, 0b10101, 0b111)]
    [Arguments(0b1010, 0b10101, 0)]
    [Arguments(0b101101, 0b011110, 0b110)]
    public async Task ParallelBitExtractInt32(int input, int mask, int res)
    {
        await BitOperationsEx.ParallelBitExtract(input, mask).Should().BeEqualTo(res);
    }

    [Test, MultipleAssertions]
    [Arguments(0, 0, 0)]
    [Arguments(0b10101, 0b10101, 0b111)]
    [Arguments(0b1010, 0b10101, 0)]
    [Arguments(0b101101, 0b011110, 0b110)]
    public async Task ParallelBitExtractUInt32(uint input, uint mask, uint res)
    {
        await BitOperationsEx.ParallelBitExtract(input, mask).Should().BeEqualTo(res);
        await BitOperationsEx.ParallelBitExtractLogic(input, mask).Should().BeEqualTo(res);
    }

    [Test, MultipleAssertions]
    [Arguments(0b10101010001010101001000111010101, 0b10101011100010010101010001010101)]
    [Arguments(0b01010101000010110101011010101010, 0b01010101011010101101000010101010)]
    [Arguments(0b11010100101000010101001001010111, 0b11101010010010101000010100101011)]
    [Arguments(0b01010101010101010010000101001011, 0b11010010100001001010101010101010)]
    [Arguments(0b10111001010101010100001010010101, 0b10101001010000101010101010011101)]
    [Arguments(0b11110000110001000010100000010010, 0b01001000000101000010001100001111)]
    public async Task BitReverseUInt32(uint a, uint b)
    {
        await new string(System.Convert.ToString(a, 2).PadLeft(32, '0').Reverse().ToArray())
            .Should().BeEqualTo(System.Convert.ToString(b, 2).PadLeft(32, '0'));

        await BitOperationsEx.BitReverse(a).Should().BeEqualTo(b);
        await BitOperationsEx.BitReverse(b).Should().BeEqualTo(a);
    }

    [Test, MultipleAssertions]
    [Arguments(0b0101010100001011010101101010101010101010001010101001000111010101, 0b1010101110001001010101000101010101010101011010101101000010101010)]
    [Arguments(0b1010101000101010100100011101010101010101000010110101011010101010, 0b0101010101101010110100001010101010101011100010010101010001010101)]
    [Arguments(0b0101010101010101001000010100101111010100101000010101001001010111, 0b1110101001001010100001010010101111010010100001001010101010101010)]
    [Arguments(0b1011100101010101010000101001010101010101000010110101011010101010, 0b0101010101101010110100001010101010101001010000101010101010011101)]
    public async Task BitReverseUInt64(ulong a, ulong b)
    {
        await new string(System.Convert.ToString((long)a, 2).PadLeft(64, '0').Reverse().ToArray())
             .Should().BeEqualTo(System.Convert.ToString((long)b, 2).PadLeft(64, '0'));

        await BitOperationsEx.BitReverse(a).Should().BeEqualTo(b);
        await BitOperationsEx.BitReverse(b).Should().BeEqualTo(a);
    }
}