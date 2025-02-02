using AtCoder;
using System;

namespace Kzrnm.Competitive.Testing.Number;

public class StaticModIntFactorTests
{
    [Fact]
    public void Combination()
    {
        var factor = new StaticModInt1000000007Factor(10);
        for (int i = 0; i <= 10; i++)
            for (int j = 0; j <= 10; j++)
                factor.Combination(i, j).Value.ShouldBe((int)MathLibEx.Combination(i, j));
        Should.Throw<Exception>(() => factor.Combination(11, 0));
    }

    [Fact]
    public void Homogeneous()
    {
        var factor = new StaticModInt1000000007Factor(30);
        for (int i = 0; i <= 10; i++)
            for (int j = 0; j <= 20; j++)
                factor.Homogeneous(i, j).ShouldBe(factor.Combination(i + j - 1, j));
        Should.Throw<Exception>(() => factor.Homogeneous(10, 22));
    }

    [Fact]
    public void Permutation()
    {
        var fact = new int[11];
        fact[0] = 1;
        for (int i = 1; i < fact.Length; i++)
            fact[i] = fact[i - 1] * i;

        var factor = new StaticModInt1000000007Factor(10);
        for (int i = 0; i <= 10; i++)
            for (int j = 0; j <= 10; j++)
            {
                int expected = 1;
                for (int k = 0; k < j; k++)
                    expected *= i - k;
                factor.Permutation(i, j).Value.ShouldBe(expected);
            }
        Should.Throw<Exception>(() => factor.Permutation(11, 0));
    }

    [Fact]
    public void Factorial()
    {
        var factor = new StaticModInt1000000007Factor(10);
        int expected = 1;
        factor.Factorial(0).Value.ShouldBe(1);
        factor.FactorialInverse(0).Value.ShouldBe(1);
        for (int i = 1; i <= 10; i++)
        {
            expected *= i;
            factor.Factorial(i).Value.ShouldBe(expected);
            factor.FactorialInverse(i).ShouldBe(StaticModInt<Mod1000000007>.One / expected);
        }
        Should.Throw<Exception>(() => factor.Factorial(11));
    }

    [Fact]
    public void DoubleFactorialOdd()
    {
        var factor = new StaticModInt1000000007Factor(10);
        int expected = 1;
        for (int i = 1; i <= 10; i += 2)
        {
            expected *= i;
            factor.DoubleFactorial(i).Value.ShouldBe(expected);
        }
        Should.Throw<Exception>(() => factor.Factorial(11));
    }

    [Fact]
    public void DoubleFactorialEven()
    {
        var factor = new StaticModInt1000000007Factor(10);
        int expected = 1;
        factor.DoubleFactorial(0).Value.ShouldBe(1);
        for (int i = 2; i <= 10; i += 2)
        {
            expected *= i;
            factor.DoubleFactorial(i).Value.ShouldBe(expected);
        }
        Should.Throw<Exception>(() => factor.Factorial(12));
    }
}
