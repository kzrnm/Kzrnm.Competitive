namespace Kzrnm.Competitive.Testing.MathNS;

public class EulerPhiTests
{
    [Test, MultipleAssertions]
    public async Task Table()
    {
        var t = EulerPhi.Table(20);
        await t.Should().BeEquivalentOrderTo([0, 1, 1, 2, 2, 4, 2, 6, 4, 6, 4, 10, 4, 12, 6, 8, 8, 16, 6, 18, 8]);
        for (int i = 1; i < t.Length; i++)
        {
            await EulerPhi.Solve(i).Should().BeEqualTo(t[i]);
        }
    }

    [Test]
    [Arguments(1, 1)]
    [Arguments(2 * 3 * 5 * 7, 48)]
    [Arguments(2 * 3 * 5 * 7 * 4, 192)]
    [Arguments(43243242343, 37065636288)]
    public async Task F(long n, long expected)
    {
        await EulerPhi.Solve(n).Should().BeEqualTo(expected);
    }
}