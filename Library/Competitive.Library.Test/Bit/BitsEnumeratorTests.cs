
using System.Collections.Immutable;

namespace Kzrnm.Competitive.Testing.Bit;

public class BitsEnumeratorTests
{
    public static IEnumerable<(byte, ImmutableArray<int>)> BitEnumerateByte_Data()
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
                                        yield return (num, lst.ToImmutableArray());
                                    }
    }

    [ThousandOfTestcases]
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(BitEnumerateByte_Data))]
    public async Task BitEnumerateByte(byte num, ImmutableArray<int> expected)
    {
        await new BitsEnumerator<byte>(num).Should().BeEquivalentOrderTo(expected);
        await new BitsEnumerator<byte>(num).ToArray().Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(Int128, int[])> BitEnumerateInt128_Data =>
    [
        (-1, Enumerable.Range(0,128).ToArray() ),
        (Int128.MaxValue, Enumerable.Range(0,127).ToArray() ),
        (Int128.MinValue, [127] ),
        (1, [0] ),
        (3, [0, 1 ] ),
        (10, [1, 3] ),
        (1 << 20, [20] ),
        (Int128.One << 120, [120] ),
        (new(1, 1), [0, 64] ),
        (0, [] ),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(BitEnumerateInt128_Data))]
    public async Task BitEnumerateInt128(Int128 num, int[] expected)
    {
        await new BitsEnumerator<Int128>(num).Should().BeEquivalentOrderTo(expected);
        await new BitsEnumerator<Int128>(num).ToArray().Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(UInt128, int[])> BitEnumerateUInt128_Data =>
    [
        (UInt128.MaxValue, Enumerable.Range(0,128).ToArray() ),
        (1, [0] ),
        (3, [0, 1 ] ),
        (10, [1, 3] ),
        (1 << 20, [20] ),
        (UInt128.One << 120, [120] ),
        (new(1, 1), [0, 64] ),
        (0, [] ),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(BitEnumerateUInt128_Data))]
    public async Task BitEnumerateUInt128(UInt128 num, int[] expected)
    {
        await new BitsEnumerator<UInt128>(num).Should().BeEquivalentOrderTo(expected);
        await new BitsEnumerator<UInt128>(num).ToArray().Should().BeEquivalentOrderTo(expected);
    }
}