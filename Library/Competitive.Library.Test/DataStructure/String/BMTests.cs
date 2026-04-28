
namespace Kzrnm.Competitive.Testing.DataStructure.String;

public class BMTests
{
    public static IEnumerable<(string, string, int)> Match_Data =>
    [
        ("ab", new string('q',1998)+"ab", 1998),
        ("abc", "abd", -1),
    ];

    [Test]
    [MethodDataSource(nameof(Match_Data))]
    public async Task Match(string pattern, string target, int index)
    {
        var bm = BoyerMoore.Create(pattern);
        await bm.Match(target).Should().BeEqualTo(index);
    }
}