namespace Kzrnm.Competitive.Testing.DataStructure.String;

public class KMPTests
{
    public static IEnumerable<(string, string, int[])> Match_Data =>
    [
        ("ab", new string('q',1998)+"ab", [1998]),
        ("abc", "abd", []),
    ];

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Match_Data))]
    public async Task Matches(string pattern, string target, int[] indexes)
    {
        var kmp = KMP.Create(pattern);
        await kmp.Matches(target).ToList().Should().BeEquivalentOrderTo(indexes);
    }
}