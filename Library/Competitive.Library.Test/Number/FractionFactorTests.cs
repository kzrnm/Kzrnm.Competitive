namespace Kzrnm.Competitive.Testing.Number;

public class FractionFactorTests
{
    [Test]
    public async Task Factorial()
    {
        var fac = new Internal.FractionFactor();
        long n = 1;
        await fac.Factorial(0).Should().BeEqualTo(1);
        for (int i = 1; i <= 20; i++)
        {
            n *= i;
            await fac.Factorial(i).Should().BeEqualTo(n);
            await fac.FactorialInverse(i).Should().BeEqualTo(new Fraction(1, n));
        }
    }

    [Test]
    public async Task Combination()
    {
        var fac = new Internal.FractionFactor();
        for (int n = 0; n <= 20; n++)
            for (int k = 0; k <= 20; k++)
                await fac.Combination(n, k).Should().BeEqualTo(MathLibEx.Combination(n, k));
    }
}
