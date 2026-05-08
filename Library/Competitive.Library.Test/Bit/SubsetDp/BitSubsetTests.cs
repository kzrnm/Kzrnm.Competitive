using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Kzrnm.Competitive.Testing.Bit.SubsetDp;

[SuppressMessage("Style", "IDE0049", Justification = "いらん")]
public class BitSubsetTests
{
    (T, T)[] Combinations<T>(BitSubsetEnumerator32<T>.Combination e) where T : IBinaryInteger<T>
    {
        var ls = new List<(T, T)>();
        foreach (var tup in e)
            ls.Add(tup);
        return ls.ToArray();
    }
    (T, T)[] Combinations<T>(BitSubsetEnumerator64<T>.Combination e) where T : IBinaryInteger<T>
    {
        var ls = new List<(T, T)>();
        foreach (var tup in e)
            ls.Add(tup);
        return ls.ToArray();
    }
    public static IEnumerable<(Int32, ImmutableArray<Int32>)> Int32_Data()
    {
        yield return (0b0, [0]);
        yield return (0b1, [1, 0]);
        yield return (0b100101, [
            0b100101,
            0b100100,
            0b100001,
            0b100000,
            0b000101,
            0b000100,
            0b000001,
            0b000000,
        ]);
        yield return (0b100000101, [
            0b100000101,
            0b100000100,
            0b100000001,
            0b100000000,
            0b000000101,
            0b000000100,
            0b000000001,
            0b000000000,
        ]);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Int32_Data))]
    public async Task Int32(Int32 num, ImmutableArray<Int32> expected)
    {
        await num.BitSubset(false).Should().BeStrictlyEquivalentTo(expected);
        await num.BitSubset().Should().BeStrictlyEquivalentTo(expected.Skip(1));

        await num.BitSubset(false).ToArray().Should().BeStrictlyEquivalentTo(expected);
        await num.BitSubset().ToArray().Should().BeStrictlyEquivalentTo(expected.Skip(1));

        await Combinations(num.BitSubsetCombination(false)).Should().BeStrictlyEquivalentTo(
            expected.Take(expected.Length / 2).Select(b => (b, num & ~b)));
        await Combinations(num.BitSubsetCombination()).Should().BeStrictlyEquivalentTo(
            expected.Take(expected.Length / 2).Skip(1).Select(b => (b, num & ~b)));
    }


    public static IEnumerable<(UInt32, ImmutableArray<uint>)> UInt32_Data()
    {
        yield return (0b0, [0]);
        yield return (0b1, [1, 0]);
        yield return (0b100101, [
            0b100101,
            0b100100,
            0b100001,
            0b100000,
            0b000101,
            0b000100,
            0b000001,
            0b000000,
        ]);
        yield return (0b10000000001000000000000000000001, [
            0b10000000001000000000000000000001,
            0b10000000001000000000000000000000,
            0b10000000000000000000000000000001,
            0b10000000000000000000000000000000,
            0b00000000001000000000000000000001,
            0b00000000001000000000000000000000,
            0b00000000000000000000000000000001,
            0b00000000000000000000000000000000,
        ]);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(UInt32_Data))]
    public async Task UInt32(UInt32 num, ImmutableArray<uint> expected)
    {
        await num.BitSubset(false).Should().BeStrictlyEquivalentTo(expected);
        await num.BitSubset().Should().BeStrictlyEquivalentTo(expected.Skip(1));

        await num.BitSubset(false).ToArray().Should().BeStrictlyEquivalentTo(expected);
        await num.BitSubset().ToArray().Should().BeStrictlyEquivalentTo(expected.Skip(1));

        await Combinations(num.BitSubsetCombination(false)).Should().BeStrictlyEquivalentTo(
            expected.Take(expected.Length / 2).Select(b => (b, num & ~b)));
        await Combinations(num.BitSubsetCombination()).Should().BeStrictlyEquivalentTo(
            expected.Take(expected.Length / 2).Skip(1).Select(b => (b, num & ~b)));
    }

    public static IEnumerable<(Int64, ImmutableArray<Int64>)> Int64_Data()
    {
        yield return (0b0, [0]);
        yield return (0b1, [1, 0]);
        yield return (0b100101, [
            0b100101,
            0b100100,
            0b100001,
            0b100000,
            0b000101,
            0b000100,
            0b000001,
            0b000000,
        ]);
        yield return (0b1000000000000000000000000100000000000000000001, [
            0b1000000000000000000000000100000000000000000001,
            0b1000000000000000000000000100000000000000000000,
            0b1000000000000000000000000000000000000000000001,
            0b1000000000000000000000000000000000000000000000,
            0b0000000000000000000000000100000000000000000001,
            0b0000000000000000000000000100000000000000000000,
            0b0000000000000000000000000000000000000000000001,
            0b0000000000000000000000000000000000000000000000,
        ]);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Int64_Data))]
    public async Task Int64(Int64 num, ImmutableArray<Int64> expected)
    {
        await num.BitSubset(false).Should().BeStrictlyEquivalentTo(expected);
        await num.BitSubset().Should().BeStrictlyEquivalentTo(expected.Skip(1));

        await num.BitSubset(false).ToArray().Should().BeStrictlyEquivalentTo(expected);
        await num.BitSubset().ToArray().Should().BeStrictlyEquivalentTo(expected.Skip(1));

        await Combinations(num.BitSubsetCombination(false)).Should().BeStrictlyEquivalentTo(
            expected.Take(expected.Length / 2).Select(b => (b, num & ~b)));
        await Combinations(num.BitSubsetCombination()).Should().BeStrictlyEquivalentTo(
            expected.Take(expected.Length / 2).Skip(1).Select(b => (b, num & ~b)));
    }

    public static IEnumerable<(UInt64, ImmutableArray<UInt64>)> UInt64_Data()
    {
        yield return (0b0, [0]);
        yield return (0b1, [1, 0]);
        yield return (0b100101, [
            0b100101,
            0b100100,
            0b100001,
            0b100000,
            0b000101,
            0b000100,
            0b000001,
            0b000000,
        ]);
        yield return (0b1000000000000000000000000000000000000000000100000000000000000001, [
            0b1000000000000000000000000000000000000000000100000000000000000001,
            0b1000000000000000000000000000000000000000000100000000000000000000,
            0b1000000000000000000000000000000000000000000000000000000000000001,
            0b1000000000000000000000000000000000000000000000000000000000000000,
            0b0000000000000000000000000000000000000000000100000000000000000001,
            0b0000000000000000000000000000000000000000000100000000000000000000,
            0b0000000000000000000000000000000000000000000000000000000000000001,
            0b0000000000000000000000000000000000000000000000000000000000000000,
        ]);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(UInt64_Data))]
    public async Task UInt64(UInt64 num, ImmutableArray<UInt64> expected)
    {
        await num.BitSubset(false).Should().BeStrictlyEquivalentTo(expected);
        await num.BitSubset().Should().BeStrictlyEquivalentTo(expected.Skip(1));

        await num.BitSubset(false).ToArray().Should().BeStrictlyEquivalentTo(expected);
        await num.BitSubset().ToArray().Should().BeStrictlyEquivalentTo(expected.Skip(1));

        await Combinations(num.BitSubsetCombination(false)).Should().BeStrictlyEquivalentTo(
            expected.Take(expected.Length / 2).Select(b => (b, num & ~b)));
        await Combinations(num.BitSubsetCombination()).Should().BeStrictlyEquivalentTo(
            expected.Take(expected.Length / 2).Skip(1).Select(b => (b, num & ~b)));
    }
}