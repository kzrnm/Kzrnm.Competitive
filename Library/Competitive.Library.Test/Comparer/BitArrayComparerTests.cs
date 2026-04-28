using System.Collections;

namespace Kzrnm.Competitive.Testing.Comparer;

public class BitArrayComparerTests
{
    [Test, MultipleAssertions]
    public async Task Compare()
    {
        var arr = new BitArray[]
        {
            new BitArray((int[])(object)new uint[]{0xFF153215,0x1}),
            new BitArray((int[])(object)new uint[]{0xFF153215}),
            new BitArray((int[])(object)new uint[]{0xFF153215,0x0}),
            new BitArray((int[])(object)new uint[]{0x0F153215}),
        };
        Array.Sort(arr, BitArrayComparer.Default);

        uint[][] expected = [
            [0x0F153215],
            [0xFF153215],
            [0xFF153215, 0x0],
            [0xFF153215, 0x1],
        ];
        await arr.Should().HaveCount(expected.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            await arr[i].ToUInt32Array().Should().BeEquivalentOrderTo(expected[i]);
        }
    }

    [Test, MultipleAssertions]
    public async Task Reverse()
    {
        var arr = new BitArray[]
        {
            new BitArray((int[])(object)new uint[]{0xFF153215,0x1}),
            new BitArray((int[])(object)new uint[]{0xFF153215}),
            new BitArray((int[])(object)new uint[]{0xFF153215,0x0}),
            new BitArray((int[])(object)new uint[]{0x0F153215}),
        };
        Array.Sort(arr, BitArrayComparer.Reverse);

        uint[][] expected = [
            [0xFF153215, 0x1],
            [0xFF153215, 0x0],
            [0xFF153215],
            [0x0F153215],
        ];
        await arr.Should().HaveCount(expected.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            await arr[i].ToUInt32Array().Should().BeEquivalentOrderTo(expected[i]);
        }
    }
}