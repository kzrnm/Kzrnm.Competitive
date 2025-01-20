namespace Kzrnm.Competitive.Testing.Algorithm
{
    public class LongestIncreasingSubsequenceTests
    {
        [Fact]
        public void Strictly()
        {
            var a = new int[]
            {
                9,1,3,1,2,3,4,4,5,6,
            };

            var (lis, idxs) = LongestIncreasingSubsequence.Lis(a);
            lis.ShouldBe([1, 2, 3, 4, 5, 6]);
            idxs.ShouldBe([3, 4, 5, 7, 8, 9]);
        }
        [Fact]
        public void NotStrictly()
        {
            var a = new int[]
            {
                9,1,3,1,2,3,4,4,5,6,
            };

            var (lis, idxs) = LongestIncreasingSubsequence.Lis(a, false);
            lis.ShouldBe([1, 1, 2, 3, 4, 4, 5, 6]);
            idxs.ShouldBe([1, 3, 4, 5, 6, 7, 8, 9]);
        }
    }
}
