using System;
using System.Collections;

namespace Kzrnm.Competitive.Testing.Comparer;

public class BitArrayComparerTests
{
    [Fact]
    public void Compare()
    {
        var arr = new BitArray[]
        {
            new BitArray((int[])(object)new uint[]{0xFF153215,0x1}),
            new BitArray((int[])(object)new uint[]{0xFF153215}),
            new BitArray((int[])(object)new uint[]{0xFF153215,0x0}),
            new BitArray((int[])(object)new uint[]{0x0F153215}),
        };
        Array.Sort(arr, BitArrayComparer.Default);

        var expected = new BitArray[] {
            new BitArray((int[])(object)new uint[] { 0x0F153215 }),
            new BitArray((int[])(object)new uint[] { 0xFF153215 }),
            new BitArray((int[])(object)new uint[] { 0xFF153215, 0x0 }),
            new BitArray((int[])(object)new uint[] { 0xFF153215, 0x1 }),
        };
        arr.Length.ShouldBe(expected.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].ShouldBe(expected[i], $"Index: {i}");
        }
    }

    [Fact]
    public void Reverse()
    {
        var arr = new BitArray[]
        {
            new BitArray((int[])(object)new uint[]{0xFF153215,0x1}),
            new BitArray((int[])(object)new uint[]{0xFF153215}),
            new BitArray((int[])(object)new uint[]{0xFF153215,0x0}),
            new BitArray((int[])(object)new uint[]{0x0F153215}),
        };
        Array.Sort(arr, BitArrayComparer.Reverse);

        var expected = new BitArray[] {
            new BitArray((int[])(object)new uint[] { 0xFF153215, 0x1 }),
            new BitArray((int[])(object)new uint[] { 0xFF153215, 0x0 }),
            new BitArray((int[])(object)new uint[] { 0xFF153215 }),
            new BitArray((int[])(object)new uint[] { 0x0F153215 }),
        };
        arr.Length.ShouldBe(expected.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].ShouldBe(expected[i], $"Index: {i}");
        }
    }
}
