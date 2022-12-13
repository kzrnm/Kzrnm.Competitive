using FluentAssertions;

namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    public class BMTests
    {
        public static TheoryData Match_Data => new TheoryData<string, string, int>
        {
            { "ab", new string('q',1998)+"ab", 1998 },
            { "abc", "abd", -1 },
        };

        [Theory]
        [MemberData(nameof(Match_Data))]
        public void Match(string pattern, string target, int index)
        {
            var bm = BoyerMoore.Create(pattern);
            bm.Match(target).Should().Be(index);
        }
    }
}
