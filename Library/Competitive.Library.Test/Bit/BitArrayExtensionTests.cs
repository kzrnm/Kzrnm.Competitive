using Kzrnm.Competitive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Bit;

public class BitArrayExtensionTests
{
    [Fact]
    public void GetUnsageIntArray()
    {
        var b = new BitArray(70);
        b.SetAll(true);
        b[0] = false;
        b[64] = false;

        var arr = b.GetArray();
        arr.Length.ShouldBe(3);
        arr[0].ShouldBe(unchecked((uint)-2));
        arr[1].ShouldBe(unchecked((uint)-1));
        (arr[2] & 0b111111).ShouldBe(0b111110u);
    }

    [Fact]
    public void EmptyCopyTo()
    {
        var b = new BitArray(0);
        b.CopyTo(Array.Empty<int>());
    }
    [Theory]
    [MemberData(nameof(BitArrayCase.RandomCases), MemberType = typeof(BitArrayCase), DisableDiscoveryEnumeration = true)]
    public void CopyToInt(BitArrayCase c)
    {
        var b = c.ToBitArray();
        var dst = new int[(b.Length + 31) / 32];
        b.CopyTo(dst);

        var exp = new int[(b.Length + 31) / 32];
        var exps = exp.AsSpan();
        for (int i = 0; i < b.Length;)
        {
            for (int j = 0; j < 32 && i < b.Length; j++, i++)
                if (b[i])
                {
                    exps[0] |= 1 << j;
                }
            exps = exps[1..];
        }

        dst.ShouldBe(exp);
    }
    [Theory]
    [MemberData(nameof(BitArrayCase.RandomCases), MemberType = typeof(BitArrayCase), DisableDiscoveryEnumeration = true)]
    public void CopyToUInt(BitArrayCase c)
    {
        var b = c.ToBitArray();
        var dst = new uint[(b.Length + 31) / 32];
        b.CopyTo(dst);

        var exp = new uint[(b.Length + 31) / 32];
        var exps = exp.AsSpan();
        for (int i = 0; i < b.Length;)
        {
            for (int j = 0; j < 32 && i < b.Length; j++, i++)
                if (b[i])
                {
                    exps[0] |= 1u << j;
                }
            exps = exps[1..];
        }
        dst.ShouldBe(exp);
    }
    [Theory]
    [MemberData(nameof(BitArrayCase.RandomCases), MemberType = typeof(BitArrayCase), DisableDiscoveryEnumeration = true)]
    public void CopyToBool(BitArrayCase c)
    {
        var exp = c.ToBoolArray();
        var b = c.ToBitArray();
        var dst = new bool[b.Length];
        b.CopyTo(dst);
        dst.ShouldBe(exp);
    }
    [Theory]
    [MemberData(nameof(BitArrayCase.RandomCases), MemberType = typeof(BitArrayCase), DisableDiscoveryEnumeration = true)]
    public void ToUInt32Array(BitArrayCase c)
    {
        var b = c.ToBitArray();
        var other = new BitArray((int[])(object)b.ToUInt32Array());
        other.Length.ShouldBe((b.Length + 31) / 32 * 32);
        b.Cast<bool>().ShouldBe(other.Cast<bool>().Take(b.Length));
    }

    [Theory]
    [MemberData(nameof(BitArrayCase.LongBinaryTexts), MemberType = typeof(BitArrayCase), DisableDiscoveryEnumeration = true)]
    public void BitString(string input)
    {
        var bits = new BitArray(input.Length);
        for (int i = 0; i < input.Length; i++)
            bits[i] = input[i] != '0';

        bits.ToBitString().ShouldBe(input);
    }

    [Theory]
    [MemberData(nameof(BitArrayCase.RandomCases), MemberType = typeof(BitArrayCase), DisableDiscoveryEnumeration = true)]
    public void SequenceEqual(BitArrayCase c)
    {
        var a = c.ToBitArray();
        var b = new BitArray(a);

        a.SequenceEqual(b).ShouldBeTrue();
        a[^1] = true;
        b[^1] = false;
        a.Length--;
        a.SequenceEqual(b).ShouldBeFalse();
        b.Length--;
        a.SequenceEqual(b).ShouldBeTrue();
        if (a.Length == 0) return;
        a[^1] = true;
        b[^1] = false;
        a.Length--;
        a.SequenceEqual(b).ShouldBeFalse();
    }

    [Theory]
    [MemberData(nameof(BitArrayCase.RandomCases), MemberType = typeof(BitArrayCase), DisableDiscoveryEnumeration = true)]
    public void OnBits(BitArrayCase c)
    {
        var b = c.ToBitArray();
        var expected = new List<int>();
        for (int i = 0; i < b.Length; i++)

            if (b[i])
                expected.Add(i);
        b.OnBits().ShouldBe(expected);
        ++b.Length;
        b[^1] = true;
        expected.Add(b.Length - 1);
        b.OnBits().ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(BitArrayCase.RandomCases), MemberType = typeof(BitArrayCase), DisableDiscoveryEnumeration = true)]
    public void PopCount(BitArrayCase c)
    {
        var b = c.ToBitArray();
        b.PopCount().ShouldBe(c.ToBoolArray().Count(c => c));
    }

    public static TheoryData<int> Bits_Data => new(new int[]
    {
        20,
        32,
        32*2,
        32*3,
    }.SelectMany(n => Enumerable.Range(-5, 11).Select(i => n + i)));

    [Theory]
    [MemberData(nameof(Bits_Data), DisableDiscoveryEnumeration = true)]
    public void Lsb(int len)
    {
        var b = new BitArray(Enumerable.Repeat(false, len).Concat(Enumerable.Repeat(true, 2)).ToArray());
        b.Lsb().ShouldBe(len);

        b = new BitArray(Enumerable.Repeat(false, len).Concat(Enumerable.Repeat(true, 2)).Concat(Enumerable.Repeat(false, 2)).ToArray());
        b.Lsb().ShouldBe(len);

        b = new BitArray(Enumerable.Repeat(false, len).Prepend(true).ToArray());
        b.Lsb().ShouldBe(0);

        b = new BitArray(Enumerable.Repeat(false, len).Append(true).ToArray());
        b.Lsb().ShouldBe(len);
        b.Length -= 2;
        b.Lsb().ShouldBe(len - 1);
    }

    [Theory]
    [MemberData(nameof(Bits_Data), DisableDiscoveryEnumeration = true)]
    public void Msb(int len)
    {
        var b = new BitArray(Enumerable.Repeat(false, len).Concat(Enumerable.Repeat(true, 1)).ToArray());
        b.Msb().ShouldBe(len);

        b = new BitArray(Enumerable.Repeat(false, len).Concat(Enumerable.Repeat(true, 1)).Concat(Enumerable.Repeat(false, 2)).ToArray());
        b.Msb().ShouldBe(len);

        b = new BitArray(Enumerable.Repeat(false, len).Prepend(true).ToArray());
        b.Msb().ShouldBe(0);

        b = new BitArray(Enumerable.Repeat(false, len).Append(true).ToArray());
        b.Msb().ShouldBe(len);
        b.Length -= 2;
        b.Msb().ShouldBe(len - 1);
    }
}
