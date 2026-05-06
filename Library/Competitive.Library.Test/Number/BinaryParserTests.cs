namespace Kzrnm.Competitive.Testing.Number;

[ThousandOfTestcases]
public class BinaryParserTests
{
    public static IEnumerable<uint> ParseUInt32_Data()
    {
        for (uint i = 0; i < 100; i++)
        {
            yield return i;
            yield return uint.MaxValue - i;
        }
        var rnd = new Xoshiro256(227);
        for (int i = 0; i < 100; i++)
        {
            yield return rnd.NextUInt32();
        }
    }

    [Test]
    [MethodDataSource(nameof(ParseUInt32_Data))]
    public async Task ParseUInt32(uint num)
    {
        var str = System.Convert.ToString(num, 2);
        await BinaryParser.ParseUInt32(str).Should().BeEqualTo(num);
    }

    public static IEnumerable<ulong> ParseUInt64_Data()
    {
        for (ulong i = 0; i < 100; i++)
        {
            yield return i;
            yield return ulong.MaxValue - i;
        }
        var rnd = new Xoshiro256(227);
        for (int i = 0; i < 100; i++)
        {
            yield return rnd.NextUInt64();
        }
    }

    [Test]
    [MethodDataSource(nameof(ParseUInt64_Data))]
    public async Task ParseUInt64(ulong num)
    {
        var str = System.Convert.ToString((long)num, 2);
        await BinaryParser.ParseUInt64(str).Should().BeEqualTo(num);
    }

    [Test, MultipleAssertions]
    [MethodDataSource<BitArrayCase>(nameof(BitArrayCase.LongBinaryTexts))]
    public async Task ParseBitArray(string input)
    {
        var bits = BinaryParser.ParseBitArray(input);
        await bits.Cast<bool>().Should().BeEquivalentOrderTo(input.Select(c => c != '0').ToArray());

        await bits.Length.Should().BeEqualTo(input.Length);

        for (int i = 0; i < bits.Length; i++)
        {
            await bits[i].Should().BeEqualTo(input[i] != '0');
        }
    }
}