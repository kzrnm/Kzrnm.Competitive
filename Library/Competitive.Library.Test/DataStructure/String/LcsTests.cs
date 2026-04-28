
namespace Kzrnm.Competitive.Testing.DataStructure.String;

public class LcsTests
{
    [Test]
    [Arguments("fdsayusyhfsda", "fdsayusyhfsda", "fdsayusyhfsda")]
    [Arguments("ababcsd", "bacds", "bacs")]
    [Arguments("bacds", "ababcsd", "bacd")]
    public async Task String(string s, string t, string expected)
    {
        await StringLibEx.Lcs(s, t).Should().BeEquivalentOrderTo(expected.ToCharArray());
    }

    [Test]
    [Arguments(new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 1, 2, 3, 4, 5, 6, 7 })]
    [Arguments(new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 2, 3, 7, 6, 5, 4, 1 }, new[] { 2, 3, 4 })]
    [Arguments(new[] { 2, 3, 7, 6, 5, 4 }, new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 2, 3, 7 })]
    [Arguments(new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 2, 3, 7, 6, 5, 4 }, new[] { 2, 3, 4 })]
    public async Task Int(int[] s, int[] t, int[] expected)
    {
        await StringLibEx.Lcs(s, t).Should().BeEquivalentOrderTo(expected);
    }
}