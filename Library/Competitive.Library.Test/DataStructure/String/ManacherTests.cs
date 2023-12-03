
namespace Kzrnm.Competitive.Testing.DataStructure.String
{
    public class ManacherTests
    {
        [Theory]
        [InlineData("abcbaba", new int[] { 1, 1, 3, 1, 2, 2, 1, })]
        [InlineData("aaaaa", new int[] { 1, 2, 3, 2, 1, })]
        public void Manacher(string s, int[] expected)
        {
            StringLibEx.Manacher(s).Should()
                .HaveCount(s.Length)
                .And.Equal(expected);
        }
    }
}
