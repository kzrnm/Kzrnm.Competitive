using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    public class KMPTests
    {
        public static TheoryData<string, string, IEnumerable<int>> Match_Data => new()
        {
            { "ab", new string('q',1998)+"ab", [1998] },
            { "abc", "abd", Array.Empty<int>() },
        };

        [Theory]
        [MemberData(nameof(Match_Data))]
        public void Matches(string pattern, string target, IEnumerable<int> indexes)
        {
            var kmp = KMP.Create(pattern);
            kmp.Matches(target).ToList().ShouldBe(indexes);
        }
    }
}
