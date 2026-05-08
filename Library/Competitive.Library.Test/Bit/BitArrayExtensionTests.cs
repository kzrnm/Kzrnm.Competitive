using Kzrnm.Competitive;
using System.Collections;

namespace Kzrnm.Competitive.Testing.Bit;

public class BitArrayExtensionTests
{
    [Test, MultipleAssertions]
    public async Task GetUnsageIntArray()
    {
        var b = new BitArray(70);
        b.SetAll(true);
        b[0] = false;
        b[64] = false;

        await b.AsSpan().ToArray().Should().BeStrictlyEquivalentTo([unchecked((uint)-2), unchecked((uint)-1), 0b111110u]);

        b.Length = 32 * 3;
        await b.AsSpan().ToArray().Should().BeStrictlyEquivalentTo([unchecked((uint)-2), unchecked((uint)-1), 0b111110u]);

        b.Length = 32 * 3 + 1;
        await b.AsSpan().ToArray().Should().BeStrictlyEquivalentTo([unchecked((uint)-2), unchecked((uint)-1), 0b111110u, 0u]);
    }

    [Test]
    public async Task EmptyCopyTo()
    {
        var b = new BitArray(0);
        b.CopyTo(Array.Empty<int>());
    }

    [ThousandOfTestcases]
    [Test]
    [MethodDataSource(typeof(BitArrayCase), nameof(BitArrayCase.RandomCases))]
    public async Task CopyToInt(BitArrayCase c)
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

        await dst.Should().BeStrictlyEquivalentTo(exp);
    }
    [Test, MultipleAssertions]
    [MethodDataSource(typeof(BitArrayCase), nameof(BitArrayCase.RandomCases))]
    public async Task CopyToUInt(BitArrayCase c)
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
        await dst.Should().BeStrictlyEquivalentTo(exp);
    }
    [Test, MultipleAssertions]
    [MethodDataSource(typeof(BitArrayCase), nameof(BitArrayCase.RandomCases))]
    public async Task CopyToBool(BitArrayCase c)
    {
        var exp = c.ToBoolArray();
        var b = c.ToBitArray();
        var dst = new bool[b.Length];
        b.CopyTo(dst);
        await dst.Should().BeStrictlyEquivalentTo(exp);
    }
    [Test, MultipleAssertions]
    [MethodDataSource(typeof(BitArrayCase), nameof(BitArrayCase.RandomCases))]
    public async Task ToUInt32Array(BitArrayCase c)
    {
        var b = c.ToBitArray();
        var other = new BitArray((int[])(object)b.ToUInt32Array());
        await other.Length.Should().BeEqualTo((b.Length + 31) / 32 * 32);
        await b.Cast<bool>().Should().BeStrictlyEquivalentTo(other.Cast<bool>().Take(b.Length));
    }

    [Test, MultipleAssertions]
    [MethodDataSource(typeof(BitArrayCase), nameof(BitArrayCase.LongBinaryTexts))]
    public async Task BitString(string input)
    {
        var bits = new BitArray(input.Length);
        for (int i = 0; i < input.Length; i++)
            bits[i] = input[i] != '0';

        await bits.ToBitString().Should().BeEqualTo(input);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(typeof(BitArrayCase), nameof(BitArrayCase.RandomCases))]
    public async Task SequenceEqual(BitArrayCase c)
    {
        var a = c.ToBitArray();
        var b = new BitArray(a);

        await a.SequenceEqual(b).Should().BeTrue();
        a[^1] = true;
        b[^1] = false;
        a.Length--;
        await a.SequenceEqual(b).Should().BeFalse();
        b.Length--;
        await a.SequenceEqual(b).Should().BeTrue();
        if (a.Length == 0) return;
        a[^1] = true;
        b[^1] = false;
        a.Length--;
        await a.SequenceEqual(b).Should().BeFalse();
    }

    [Test, MultipleAssertions]
    [MethodDataSource(typeof(BitArrayCase), nameof(BitArrayCase.RandomCases))]
    public async Task OnBits(BitArrayCase c)
    {
        var b = c.ToBitArray();
        var expected = new List<int>();
        for (int i = 0; i < b.Length; i++)

            if (b[i])
                expected.Add(i);
        await b.OnBits().Should().BeStrictlyEquivalentTo(expected);
        ++b.Length;
        b[^1] = true;
        expected.Add(b.Length - 1);
        await b.OnBits().Should().BeStrictlyEquivalentTo(expected);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(typeof(BitArrayCase), nameof(BitArrayCase.RandomCases))]
    public async Task PopCount(BitArrayCase c)
    {
        var b = c.ToBitArray();
        await b.PopCount().Should().BeEqualTo(c.ToBoolArray().Count(c => c));
    }

    public static IEnumerable<int> Bits_Data => new int[]
    {
        20,
        32,
        32*2,
        32*3,
    }.SelectMany(n => Enumerable.Range(-5, 11).Select(i => n + i));

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Bits_Data))]
    public async Task Lsb(int len)
    {
        var b = new BitArray(Enumerable.Repeat(false, len).Concat(Enumerable.Repeat(true, 2)).ToArray());
        await b.Lsb().Should().BeEqualTo(len);

        b = new BitArray(Enumerable.Repeat(false, len).Concat(Enumerable.Repeat(true, 2)).Concat(Enumerable.Repeat(false, 2)).ToArray());
        await b.Lsb().Should().BeEqualTo(len);

        b = new BitArray(Enumerable.Repeat(false, len).Prepend(true).ToArray());
        await b.Lsb().Should().BeEqualTo(0);

        b = new BitArray(Enumerable.Repeat(false, len).Append(true).ToArray());
        await b.Lsb().Should().BeEqualTo(len);
        b.Length -= 2;
        await b.Lsb().Should().BeEqualTo(len - 1);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Bits_Data))]
    public async Task Msb(int len)
    {
        var b = new BitArray(Enumerable.Repeat(false, len).Concat(Enumerable.Repeat(true, 1)).ToArray());
        await b.Msb().Should().BeEqualTo(len);

        b = new BitArray(Enumerable.Repeat(false, len).Concat(Enumerable.Repeat(true, 1)).Concat(Enumerable.Repeat(false, 2)).ToArray());
        await b.Msb().Should().BeEqualTo(len);

        b = new BitArray(Enumerable.Repeat(false, len).Prepend(true).ToArray());
        await b.Msb().Should().BeEqualTo(0);

        b = new BitArray(Enumerable.Repeat(false, len).Append(true).ToArray());
        await b.Msb().Should().BeEqualTo(len);
        b.Length -= 2;
        await b.Msb().Should().BeEqualTo(len - 1);
    }
}