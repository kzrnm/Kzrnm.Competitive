using System.Collections.Immutable;

namespace Kzrnm.Competitive.Testing.DataStructure.String;

public class PreriodTests
{
    public static IEnumerable<(ImmutableArray<int>, int)> PeriodInt_Data()
    {
        for (int i = 1; i < 10; i++)
        {
            var loop = Enumerable.Range(1, i);
            for (int r = 1; r < 5; r++)
            {
                yield return (Enumerable.Repeat(loop, r).SelectMany(a => a).ToImmutableArray(), i);
            }
        }
        yield return ([1, 2, 3, 1, 2,], 5);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(PeriodInt_Data))]
    public async Task PeriodInt(ImmutableArray<int> input, int expected)
    {
        var s = input.ToArray();
        await StringLibEx.Period(s).Should().BeEqualTo(expected);
        var ss = s[..expected];
        for (int i = 0; i < expected; i++)
        {
            if (s.Length % expected == 0)

                for (int j = expected; j < s.Length; j += expected)
                {
                    await s.AsSpan(j, expected).StartsWith(ss).Should().BeTrue();
                }
        }
    }

    [Test, MultipleAssertions]
    public async Task PeriodString()
    {
        await StringLibEx.Period("aaa").Should().BeEqualTo(1);
        await StringLibEx.Period("ababababab").Should().BeEqualTo(2);
        await StringLibEx.Period("abc").Should().BeEqualTo(3);
        await StringLibEx.Period("abcabc").Should().BeEqualTo(3);
        await StringLibEx.Period("ababa").Should().BeEqualTo(5);
    }
}