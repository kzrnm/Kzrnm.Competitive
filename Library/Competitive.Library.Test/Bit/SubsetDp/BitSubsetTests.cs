using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;

namespace Kzrnm.Competitive.Testing.Bit.SubsetDp
{
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
        public static IEnumerable<(Int32 num, Int32[] expected)> Int32_Data()
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

        [Theory]
        [TupleMemberData(nameof(Int32_Data))]
        public void Int32(Int32 num, Int32[] expected)
        {
            num.BitSubset(false).ShouldBe(expected);
            num.BitSubset().ShouldBe(expected.Skip(1));

            num.BitSubset(false).ToArray().ShouldBe(expected);
            num.BitSubset().ToArray().ShouldBe(expected.Skip(1));

            Combinations(num.BitSubsetCombination(false)).ShouldBe(
                expected.Take(expected.Length / 2).Select(b => (b, num & ~b)));
            Combinations(num.BitSubsetCombination()).ShouldBe(
                expected.Take(expected.Length / 2).Skip(1).Select(b => (b, num & ~b)));
        }


        public static IEnumerable<(UInt32 num, UInt32[] expected)> UInt32_Data()
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

        [Theory]
        [TupleMemberData(nameof(UInt32_Data))]
        public void UInt32(UInt32 num, UInt32[] expected)
        {
            num.BitSubset(false).ShouldBe(expected);
            num.BitSubset().ShouldBe(expected.Skip(1));

            num.BitSubset(false).ToArray().ShouldBe(expected);
            num.BitSubset().ToArray().ShouldBe(expected.Skip(1));

            Combinations(num.BitSubsetCombination(false)).ShouldBe(
                expected.Take(expected.Length / 2).Select(b => (b, num & ~b)));
            Combinations(num.BitSubsetCombination()).ShouldBe(
                expected.Take(expected.Length / 2).Skip(1).Select(b => (b, num & ~b)));
        }

        public static IEnumerable<(Int64 num, Int64[] expected)> Int64_Data()
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

        [Theory]
        [TupleMemberData(nameof(Int64_Data))]
        public void Int64(Int64 num, Int64[] expected)
        {
            num.BitSubset(false).ShouldBe(expected);
            num.BitSubset().ShouldBe(expected.Skip(1));

            num.BitSubset(false).ToArray().ShouldBe(expected);
            num.BitSubset().ToArray().ShouldBe(expected.Skip(1));

            Combinations(num.BitSubsetCombination(false)).ShouldBe(
                expected.Take(expected.Length / 2).Select(b => (b, num & ~b)));
            Combinations(num.BitSubsetCombination()).ShouldBe(
                expected.Take(expected.Length / 2).Skip(1).Select(b => (b, num & ~b)));
        }

        public static IEnumerable<(UInt64 num, UInt64[] expected)> UInt64_Data()
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

        [Theory]
        [TupleMemberData(nameof(UInt64_Data))]
        public void UInt64(UInt64 num, UInt64[] expected)
        {
            num.BitSubset(false).ShouldBe(expected);
            num.BitSubset().ShouldBe(expected.Skip(1));

            num.BitSubset(false).ToArray().ShouldBe(expected);
            num.BitSubset().ToArray().ShouldBe(expected.Skip(1));

            Combinations(num.BitSubsetCombination(false)).ShouldBe(
                expected.Take(expected.Length / 2).Select(b => (b, num & ~b)));
            Combinations(num.BitSubsetCombination()).ShouldBe(
                expected.Take(expected.Length / 2).Skip(1).Select(b => (b, num & ~b)));
        }
    }
}