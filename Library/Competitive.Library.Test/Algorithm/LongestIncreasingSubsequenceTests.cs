using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Algorithm;

public class LongestIncreasingSubsequenceTests
{
    record struct CaseData(int[] Input,
        SerializableTuple<int, int>[] Strictly,
        SerializableTuple<int, int>[] NotStrictly,
        SerializableTuple<int, int>[] ReverseStrictly,
        SerializableTuple<int, int>[] ReverseNotStrictly
    );
    static CaseData[] Cases = [
        new()
        {
            Input = [],
            Strictly = [],
            NotStrictly = [],
            ReverseStrictly = [],
            ReverseNotStrictly = [],
        },
        new()
        {
            Input = [-4, 4, 2, 4, 3, 2, 1, 1, 0, -1],
            Strictly = [(-4, 0), (2, 2), (3, 4)],
            NotStrictly = [(-4, 0), (1, 6), (1, 7)],
            ReverseStrictly = [(4, 1), (3, 4), (2, 5), (1, 6), (0, 8), (-1, 9)],
            ReverseNotStrictly = [(4, 1), (4, 3), (3, 4), (2, 5), (1, 6), (1, 7), (0, 8), (-1, 9)],
        },
        new()
        {
            Input = [9, 1, 3, 1, 2, 3, 4, 4, 5, 6],
            Strictly = [(1, 1), (2, 4), (3, 5), (4, 6), (5, 8), (6, 9)],
            NotStrictly = [(1, 1), (1, 3), (2, 4), (3, 5), (4, 6), (4, 7), (5, 8), (6, 9)],
            ReverseStrictly = [(9, 0), (3, 2), (2, 4)],
            ReverseNotStrictly = [(9, 0), (4, 6), (4, 7)],
        },
        new()
        {
            Input = [1,6,1,3,3,5,4,2,3,5,4,3,7,5,1,4,2],
            Strictly = [(1, 0), (2, 7), (3, 8), (4, 10), (5, 13)],
            NotStrictly = [(1, 0), (1, 2), (3, 3), (3, 4), (3, 8), (3, 11), (4, 15)],
            ReverseStrictly = [(6, 1), (5, 5), (4, 6), (3, 8), (2, 16)],
            ReverseNotStrictly = [(6, 1), (5, 5), (5, 9), (5, 13), (4, 15), (2, 16)],
        },
    ];

    public static IEnumerable<TheoryDataRow<int[], SerializableTuple<int, int>[]>> Strictly_Data() =>
        Cases.Select(t => new TheoryDataRow<int[], SerializableTuple<int, int>[]>(t.Input, t.Strictly));

    [Theory]
    [MemberData(nameof(Strictly_Data))]
    public void Strictly(int[] input, SerializableTuple<int, int>[] expected)
    {
        LongestIncreasingSubsequence.Lis(input).ShouldBe(
            expected.Select(t => new LongestIncreasingSubsequence.Result<int>(t.Item1, t.Item2)));
    }

    public static IEnumerable<TheoryDataRow<int[], SerializableTuple<int, int>[]>> NotStrictly_Data() =>
        Cases.Select(t => new TheoryDataRow<int[], SerializableTuple<int, int>[]>(t.Input, t.NotStrictly));

    [Theory]
    [MemberData(nameof(NotStrictly_Data))]
    public void NotStrictly(int[] input, SerializableTuple<int, int>[] expected)
    {
        LongestIncreasingSubsequence.Lis(input, false).ShouldBe(
            expected.Select(t => new LongestIncreasingSubsequence.Result<int>(t.Item1, t.Item2)));
    }

    public static IEnumerable<TheoryDataRow<int[], SerializableTuple<int, int>[]>> ReverseStrictly_Data() =>
        Cases.Select(t => new TheoryDataRow<int[], SerializableTuple<int, int>[]>(t.Input, t.ReverseStrictly));

    [Theory]
    [MemberData(nameof(ReverseStrictly_Data))]
    public void ReverseStrictly(int[] input, SerializableTuple<int, int>[] expected)
    {
        LongestIncreasingSubsequence.Lis(input, new ReverseComparer<int>()).ShouldBe(
            expected.Select(t => new LongestIncreasingSubsequence.Result<int>(t.Item1, t.Item2)));
    }

    public static IEnumerable<TheoryDataRow<int[], SerializableTuple<int, int>[]>> ReverseNotStrictly_Data() =>
        Cases.Select(t => new TheoryDataRow<int[], SerializableTuple<int, int>[]>(t.Input, t.ReverseNotStrictly));

    [Theory]
    [MemberData(nameof(ReverseNotStrictly_Data))]
    public void ReverseNotStrictly(int[] input, SerializableTuple<int, int>[] expected)
    {
        LongestIncreasingSubsequence.Lis(input, new ReverseComparer<int>(), false).ShouldBe(
            expected.Select(t => new LongestIncreasingSubsequence.Result<int>(t.Item1, t.Item2)));
    }
}
