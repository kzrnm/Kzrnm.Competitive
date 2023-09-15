namespace Kzrnm.Competitive.Testing.Number
{
    public class PrimitiveRootTests
    {
        [Theory]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(5, 2)]
        [InlineData(7, 3)]
        [InlineData(11, 2)]
        [InlineData(13, 2)]
        [InlineData(37, 2)]
        [InlineData(211, 2)]
        [InlineData(998244353, 3)]
        [InlineData(1000000007, 5)]
        public void Solve(int p, int expected)
        {
            PrimitiveRoot.Solve(p).Should().Be(expected);
            PrimitiveRoot.Solve((uint)p).Should().Be((uint)expected);
        }
    }
}
