namespace Kzrnm.Competitive.Testing.Number;

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

    [Test, MultipleAssertions]
    [Arguments(2)]
    [Arguments(8)]
    [Arguments(30)]
    [Arguments(50)]
    [Arguments(900)]
    [Arguments(1000)]
    [Arguments(367519)]
    [Arguments(73513440)]
    [Arguments(998244353)]
    public async Task Mod(int mod)
    {
        var amf = new ArbitraryModIntFactor(mod);
        for (int n = 0; n < table.Length; n++)
            for (int k = 0; k <= n; k++)
                await amf.Combination(n, k).Should().BeEqualTo((uint)(table[n][k] % mod));
    }
}