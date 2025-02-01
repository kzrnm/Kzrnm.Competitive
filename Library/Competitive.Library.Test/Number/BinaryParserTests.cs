using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Number;

public class BinaryParserTests
{
    public static IEnumerable<TheoryDataRow<uint>> ParseUInt32_Data()
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

    [Theory]
    [MemberData(nameof(ParseUInt32_Data), DisableDiscoveryEnumeration = true)]
    public void ParseUInt32(uint num)
    {
        var str = System.Convert.ToString(num, 2);
        BinaryParser.ParseUInt32(str).ShouldBe(num);
    }

    public static IEnumerable<TheoryDataRow<ulong>> ParseUInt64_Data()
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

    [Theory]
    [MemberData(nameof(ParseUInt64_Data), DisableDiscoveryEnumeration = true)]
    public void ParseUInt64(ulong num)
    {
        var str = System.Convert.ToString((long)num, 2);
        BinaryParser.ParseUInt64(str).ShouldBe(num);
    }

    [Theory]
    [MemberData(nameof(BitArrayCase.LongBinaryTexts), MemberType = typeof(BitArrayCase), DisableDiscoveryEnumeration = true)]
    public void ParseBitArray(string input)
    {
        var bits = BinaryParser.ParseBitArray(input);
        bits.Cast<bool>().ShouldBe(input.Select(c => c != '0').ToArray());

        bits.Length.ShouldBe(input.Length);

        for (int i = 0; i < bits.Length; i++)
        {
            bits[i].ShouldBe(input[i] != '0');
        }
    }
}
