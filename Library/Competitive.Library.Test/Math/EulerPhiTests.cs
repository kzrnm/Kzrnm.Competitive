namespace Kzrnm.Competitive.Testing.MathNS;

public class EulerPhiTests
{
    [Fact]
    public void Table()
    {
        var t = EulerPhi.Table(20);
        t.ShouldBe([0, 1, 1, 2, 2, 4, 2, 6, 4, 6, 4, 10, 4, 12, 6, 8, 8, 16, 6, 18, 8]);
        for (int i = 1; i < t.Length; i++)
        {
            EulerPhi.Solve(i).ShouldBe(t[i]);
        }
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2 * 3 * 5 * 7, 48)]
    [InlineData(2 * 3 * 5 * 7 * 4, 192)]
    [InlineData(43243242343, 37065636288)]
    public void F(long n, long expected)
    {
        EulerPhi.Solve(n).ShouldBe(expected);
    }
}
