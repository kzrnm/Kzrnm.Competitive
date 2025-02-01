using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Sdk;

namespace Kzrnm.Competitive.Testing.Bit;

public class BitsEnumeratorTests
{
    public static IEnumerable<TheoryDataRow<byte, int[]>> BitEnumerateByte_Data()
    {
        var s = new int[8];
        for (s[0] = 0; s[0] < 2; s[0]++)
            for (s[1] = 0; s[1] < 2; s[1]++)
                for (s[2] = 0; s[2] < 2; s[2]++)
                    for (s[3] = 0; s[3] < 2; s[3]++)
                        for (s[4] = 0; s[4] < 2; s[4]++)
                            for (s[5] = 0; s[5] < 2; s[5]++)
                                for (s[6] = 0; s[6] < 2; s[6]++)
                                    for (s[7] = 0; s[7] < 2; s[7]++)
                                    {
                                        byte num = 0;
                                        var lst = new List<int>(8);
                                        for (int i = 0; i < s.Length; i++)
                                            if (s[i] != 0)
                                            {
                                                num |= (byte)(1 << i);
                                                lst.Add(i);
                                            }
                                        yield return (num, lst.ToArray());
                                    }
    }
    [Theory]
    [MemberData(nameof(BitEnumerateByte_Data))]
    public void BitEnumerateByte(byte num, int[] expected)
    {
        new BitsEnumerator<byte>(num).ShouldBe(expected);
        new BitsEnumerator<byte>(num).ToArray().ShouldBe(expected);
    }

    public static TheoryData<Int128, int[]> BitEnumerateInt128_Data => new()
    {
        { -1, Enumerable.Range(0,128).ToArray() },
        { Int128.MaxValue, Enumerable.Range(0,127).ToArray() },
        { Int128.MinValue, [127] },
        { 1, [0] },
        { 3, [0, 1 ] },
        { 10, [1, 3] },
        { 1 << 20, [20] },
        { Int128.One << 120, [120] },
        { new(1, 1), [0, 64] },
        { 0, [] },
    };
    struct IInt128 : IXunitSerializable
    {
        public void Deserialize(IXunitSerializationInfo info)
        {
            throw new NotImplementedException();
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            throw new NotImplementedException();
        }
    }
    [Theory]
    [MemberData(nameof(BitEnumerateInt128_Data))]
    public void BitEnumerateInt128(Int128 num, int[] expected)
    {
        new BitsEnumerator<Int128>(num).ShouldBe(expected);
        new BitsEnumerator<Int128>(num).ToArray().ShouldBe(expected);
    }

    public static TheoryData<UInt128, int[]> BitEnumerateUInt128_Data => new()
    {
        { UInt128.MaxValue, Enumerable.Range(0,128).ToArray() },
        { 1, [0] },
        { 3, [0, 1 ] },
        { 10, [1, 3] },
        { 1 << 20, [20] },
        { UInt128.One << 120, [120] },
        { new(1, 1), [0, 64] },
        { 0, [] },
    };
    [Theory]
    [MemberData(nameof(BitEnumerateUInt128_Data))]
    public void BitEnumerateUInt128(UInt128 num, int[] expected)
    {
        new BitsEnumerator<UInt128>(num).ShouldBe(expected);
        new BitsEnumerator<UInt128>(num).ToArray().ShouldBe(expected);
    }
}
