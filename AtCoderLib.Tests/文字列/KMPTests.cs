using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AtCoderLib.文字列
{
    public class KMPTests
    {
        public static TheoryData Match_Data = new TheoryData<string, string, IEnumerable<int>>
        {
            { Util.Strs[0], Util.Str, Enumerable.Range(0,1000) },
            { Util.Strs[19], Util.Str, new int[]{ 19000 } },
            { Util.Strs[25], Util.Str, new int[]{ 25000 } },
            { "ab", Util.Str, new int[]{ 1998 } },
            { "abc", Util.Str, Array.Empty<int>() },
        };

        [Theory]
        [MemberData(nameof(Match_Data))]
        public void Matches(string pattern, string target, IEnumerable<int> indexes)
        {
            var kmp = new KMP(pattern);
            kmp.Matches(target).Should().Equal(indexes);
        }
    }
}
