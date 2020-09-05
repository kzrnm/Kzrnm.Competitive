using FluentAssertions;
using Xunit;

namespace AtCoderLib.文字列
{
    public class BMTests
    {
        public static TheoryData Match_Data = new TheoryData<string, string, int>
        {
            { Util.Strs[0], Util.Str, 0 },
            { Util.Strs[19], Util.Str, 19000 },
            { Util.Strs[25], Util.Str, 25000 },
            { "ab", Util.Str, 1998 },
            { "abc", Util.Str, -1 },
        };

        [Theory]
        [MemberData(nameof(Match_Data))]
        public void Match(string pattern, string target, int index)
        {
            var bm = new BoyerMoore(pattern);
            bm.Match(target).Should().Be(index);
        }
    }
}
