using AtCoder;

namespace Kzrnm.Competitive.Testing.Number;

public class MontgomeryModIntFactorTests
{
    [Test, MultipleAssertions]
    public async Task Combination()
    {
        var factor = new MontgomeryModInt1000000007Factor(10);
        for (int i = 0; i <= 10; i++)
            for (int j = 0; j <= 10; j++)
                await factor.Combination(i, j).Value.Should().BeEqualTo((int)MathLibEx.Combination(i, j));
        Assert.Throws<Exception>(() => factor.Combination(11, 0));
    }

    [Test, MultipleAssertions]
    public async Task Homogeneous()
    {
        var factor = new MontgomeryModInt1000000007Factor(30);
        for (int i = 0; i <= 10; i++)
            for (int j = 0; j <= 20; j++)
                await factor.Homogeneous(i, j).Should().BeEqualTo(factor.Combination(i + j - 1, j));
        Assert.Throws<Exception>(() => factor.Homogeneous(10, 22));
    }

    [Test, MultipleAssertions]
    public async Task Permutation()
    {
        var fact = new int[11];
        fact[0] = 1;
        for (int i = 1; i < fact.Length; i++)
            fact[i] = fact[i - 1] * i;

        var factor = new MontgomeryModInt1000000007Factor(10);
        for (int i = 0; i <= 10; i++)
            for (int j = 0; j <= 10; j++)
            {
                int expected = 1;
                for (int k = 0; k < j; k++)
                    expected *= i - k;
                await factor.Permutation(i, j).Value.Should().BeEqualTo(expected);
            }
        Assert.Throws<Exception>(() => factor.Permutation(11, 0));
    }

    [Test, MultipleAssertions]
    public async Task Factorial()
    {
        var factor = new MontgomeryModInt1000000007Factor(10);
        int expected = 1;
        await factor.Factorial(0).Value.Should().BeEqualTo(1);
        await factor.FactorialInverse(0).Value.Should().BeEqualTo(1);
        for (int i = 1; i <= 10; i++)
        {
            expected *= i;
            await factor.Factorial(i).Value.Should().BeEqualTo(expected);
            await factor.FactorialInverse(i).Should().BeEqualTo(MontgomeryModInt<Mod1000000007>.One / expected);
        }
        Assert.Throws<Exception>(() => factor.Factorial(11));
    }

    [Test, MultipleAssertions]
    public async Task DoubleFactorialOdd()
    {
        var factor = new MontgomeryModInt1000000007Factor(10);
        int expected = 1;
        for (int i = 1; i <= 10; i += 2)
        {
            expected *= i;
            await factor.DoubleFactorial(i).Value.Should().BeEqualTo(expected);
        }
        Assert.Throws<Exception>(() => factor.Factorial(11));
    }

    [Test, MultipleAssertions]
    public async Task DoubleFactorialEven()
    {
        var factor = new MontgomeryModInt1000000007Factor(10);
        int expected = 1;
        await factor.DoubleFactorial(0).Value.Should().BeEqualTo(1);
        for (int i = 2; i <= 10; i += 2)
        {
            expected *= i;
            await factor.DoubleFactorial(i).Value.Should().BeEqualTo(expected);
        }
        Assert.Throws<Exception>(() => factor.Factorial(12));
    }
}