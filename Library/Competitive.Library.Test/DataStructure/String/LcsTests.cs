
namespace Kzrnm.Competitive.Testing.DataStructure.String;

public class LcsTests
{
    [Theory]
    [InlineData("fdsayusyhfsda", "fdsayusyhfsda", "fdsayusyhfsda")]
    [InlineData("ababcsd", "bacds", "bacs")]
    [InlineData("bacds", "ababcsd", "bacd")]
    public void String(string s, string t, string expected)
    {
        StringLibEx.Lcs(s, t).ShouldBe(expected.ToCharArray());
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 1, 2, 3, 4, 5, 6, 7 })]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 2, 3, 7, 6, 5, 4, 1 }, new[] { 2, 3, 4 })]
    [InlineData(new[] { 2, 3, 7, 6, 5, 4 }, new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 2, 3, 7 })]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7 }, new[] { 2, 3, 7, 6, 5, 4 }, new[] { 2, 3, 4 })]
    public void Int(int[] s, int[] t, int[] expected)
    {
        StringLibEx.Lcs(s, t).ShouldBe(expected);
    }
}
