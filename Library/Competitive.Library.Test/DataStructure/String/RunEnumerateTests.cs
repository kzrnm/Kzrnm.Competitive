using System.Collections.Immutable;

namespace Kzrnm.Competitive.Testing.DataStructure.String;

public class RunEnumerateTests
{
    public static IEnumerable<(string, ImmutableArray<ImmutableArray<(int From, int ToExclusive)>>)> RunEnumerate_Data()
    {
        yield return ("",
        [
            [],
        ]);
        yield return ("mississippi",
        [
            [],
            [(2,4),(5,7),(8,10)],
            [],
            [(1,8)],
            [],
            [],
        ]);
        yield return ("aaaaaaa",
        [
            [],
            [(0,7)],
            [],
            [],
        ]);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(RunEnumerate_Data))]
    public async Task RunEnumerate(string s, ImmutableArray<ImmutableArray<(int From, int ToExclusive)>> expected)
    {
        await ShouldEqual(StringLibEx.RunEnumerate(s), expected);
        await ShouldEqual(StringLibEx.RunEnumerate(s.Select(t => (int)t).ToArray()), expected);

        static async Task ShouldEqual((int From, int ToExclusive)[][] got, ImmutableArray<ImmutableArray<(int From, int ToExclusive)>> expected)
        {
            await got.Length.Should().BeEqualTo(expected.Length);
            for (int i = 0; i < got.Length; i++)
            {
                await got[i].Should().BeEquivalentOrderTo(expected[i]);
            }
        }
    }
}