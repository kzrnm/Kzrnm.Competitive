using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    [Verify] // verification-helper: PROBLEM https://judge.yosupo.jp/problem/aplusb
    public class LCSTests
    {
        [Theory]
        [InlineData("fdsayusyhfsda", "fdsayusyhfsda", "fdsayusyhfsda")]
        [InlineData("ababcsd", "bacds", "bacs")]
        public void String(string s, string t, string expected)
        {
            StringLibEx.LCS(s, t).Should().Equal(expected.ToCharArray());
        }


        public static TheoryData Int_Data = new TheoryData<int[], int[], int[]>
        {
            {
                new[]{1,2,3,4,5,6,7},
                new[]{1,2,3,4,5,6,7},
                new[]{1,2,3,4,5,6,7}
            },
            {
                new[]{1,2,3,4,5,6,7},
                new[]{2,3,7,6,5,4,1},
                new[]{2,3,4}
            },
        };
        [Theory]
        [MemberData(nameof(Int_Data))]
        public void Int(int[] s, int[] t, int[] expected)
        {
            StringLibEx.LCS(s, t).Should().Equal(expected);
        }
    }
}
