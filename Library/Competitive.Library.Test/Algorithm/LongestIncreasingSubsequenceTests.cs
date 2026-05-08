using System.Collections.Immutable;

namespace Kzrnm.Competitive.Testing.Algorithm;

public class LongestIncreasingSubsequenceTests
{
    record struct CaseData(
        ImmutableArray<int> Input,
        ImmutableArray<(int, int)> Strictly,
        ImmutableArray<(int, int)> NotStrictly,
        ImmutableArray<(int, int)> ReverseStrictly,
        ImmutableArray<(int, int)> ReverseNotStrictly
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

    public static IEnumerable<(ImmutableArray<int> input, ImmutableArray<LongestIncreasingSubsequence.Result<int>> expected)> Strictly_Data() =>
        ToParams(Cases, t => t.Strictly);

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Strictly_Data))]
    public async Task Strictly(ImmutableArray<int> input, ImmutableArray<LongestIncreasingSubsequence.Result<int>> expected)
    {
        var s = input.ToArray();
        await LongestIncreasingSubsequence.Lis(s).Should().BeStrictlyEquivalentTo(expected);
    }

    public static IEnumerable<(ImmutableArray<int> input, ImmutableArray<LongestIncreasingSubsequence.Result<int>> expected)> NotStrictly_Data() =>
        ToParams(Cases, t => t.NotStrictly);

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(NotStrictly_Data))]
    public async Task NotStrictly(ImmutableArray<int> input, ImmutableArray<LongestIncreasingSubsequence.Result<int>> expected)
    {
        var s = input.ToArray();
        await LongestIncreasingSubsequence.Lis(s, false).Should().BeStrictlyEquivalentTo(expected);
    }

    public static IEnumerable<(ImmutableArray<int> input, ImmutableArray<LongestIncreasingSubsequence.Result<int>> expected)> ReverseStrictly_Data() =>
        ToParams(Cases, t => t.ReverseStrictly);

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(ReverseStrictly_Data))]
    public async Task ReverseStrictly(ImmutableArray<int> input, ImmutableArray<LongestIncreasingSubsequence.Result<int>> expected)
    {
        var s = input.ToArray();
        await LongestIncreasingSubsequence.Lis(s, new ReverseComparer<int>()).Should().BeStrictlyEquivalentTo(expected);
    }

    public static IEnumerable<(ImmutableArray<int> input, ImmutableArray<LongestIncreasingSubsequence.Result<int>> expected)> ReverseNotStrictly_Data() =>
        ToParams(Cases, t => t.ReverseNotStrictly);

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(ReverseNotStrictly_Data))]
    public async Task ReverseNotStrictly(ImmutableArray<int> input, ImmutableArray<LongestIncreasingSubsequence.Result<int>> expected)
    {
        var s = input.ToArray();
        await LongestIncreasingSubsequence.Lis(s, new ReverseComparer<int>(), false).Should().BeStrictlyEquivalentTo(expected);
    }

    static (ImmutableArray<int> input, ImmutableArray<LongestIncreasingSubsequence.Result<int>> expected)[] ToParams(IEnumerable<CaseData> d, Func<CaseData, ImmutableArray<(int Value, int Index)>> expectedFunc)
        => d.Select(cd => (cd.Input, expectedFunc(cd).Select(t => new LongestIncreasingSubsequence.Result<int>(t.Value, t.Index)).ToImmutableArray())).ToArray();
}