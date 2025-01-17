using System.Numerics;

namespace Kzrnm.Competitive.Testing.Number
{
    public class ArbitraryModIntFactorTests
    {
        static BigInteger[][] table = CombinationTable(200);
        static BigInteger[][] CombinationTable(int maxSize)
        {
            var c = new BigInteger[++maxSize][];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = new BigInteger[i + 1];
                c[i][0] = c[i][^1] = 1;
                for (int j = 1; j + 1 < c[i].Length; ++j)
                {
                    c[i][j] = c[i - 1][j - 1] + c[i - 1][j];
                }
            }
            return c;
        }

        [Theory]
        [InlineData(2)]
        [InlineData(8)]
        [InlineData(30)]
        [InlineData(50)]
        [InlineData(900)]
        [InlineData(1000)]
        [InlineData(367519)]
        [InlineData(73513440)]
        [InlineData(998244353)]
        public void Mod(int mod)
        {
            var amf = new ArbitraryModIntFactor(mod);
            for (int n = 0; n < table.Length; n++)
                for (int k = 0; k <= n; k++)
                    amf.Combination(n, k).ShouldBe((uint)(table[n][k] % mod), $"Combination(n={n}, k={k})");
        }
    }
}
