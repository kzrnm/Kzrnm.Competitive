using AtCoder;
using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class StaticModIntFactorTests
    {
        [Fact]
        public void Combination()
        {
            var factor = new StaticModIntFactor<Mod1000000007>(10);
            for (int i = 0; i <= 10; i++)
                for (int j = 0; j <= 10; j++)
                    factor.Combination(i, j).Value.Should().Be((int)MathLibEx.Combination(i, j));
            factor.Invoking(factor => factor.Combination(11, 0)).Should().Throw<Exception>();
        }

        [Fact]
        public void Homogeneous()
        {
            var factor = new StaticModIntFactor<Mod1000000007>(30);
            for (int i = 0; i <= 10; i++)
                for (int j = 0; j <= 20; j++)
                    factor.Homogeneous(i, j).Should().Be(factor.Combination(i + j - 1, j));
            factor.Invoking(factor => factor.Homogeneous(10, 22)).Should().Throw<Exception>();
        }

        [Fact]
        public void Permutation()
        {
            var fact = new int[11];
            fact[0] = 1;
            for (int i = 1; i < fact.Length; i++)
                fact[i] = fact[i - 1] * i;

            var factor = new StaticModIntFactor<Mod1000000007>(10);
            for (int i = 0; i <= 10; i++)
                for (int j = 0; j <= 10; j++)
                {
                    int expected = 1;
                    for (int k = 0; k < j; k++)
                        expected *= i - k;
                    factor.Permutation(i, j).Value.Should().Be(expected);
                }
            factor.Invoking(factor => factor.Permutation(11, 0)).Should().Throw<Exception>();
        }

        [Fact]
        public void Factorial()
        {
            var factor = new StaticModIntFactor<Mod1000000007>(10);
            int expected = 1;
            factor.Factorial(0).Value.Should().Be(1);
            factor.FactorialInvers(0).Value.Should().Be(1);
            for (int i = 1; i <= 10; i++)
            {
                expected *= i;
                factor.Factorial(i).Value.Should().Be(expected);
                factor.FactorialInvers(i).Should().Be(new StaticModInt<Mod1000000007>(1) / expected);
            }
            factor.Invoking(factor => factor.Factorial(11)).Should().Throw<Exception>();
        }

        [Fact]
        public void DoubleFactorialOdd()
        {
            var factor = new StaticModIntFactor<Mod1000000007>(10);
            int expected = 1;
            for (int i = 1; i <= 10; i += 2)
            {
                expected *= i;
                factor.DoubleFactorial(i).Value.Should().Be(expected);
            }
            factor.Invoking(factor => factor.Factorial(11)).Should().Throw<Exception>();
        }

        [Fact]
        public void DoubleFactorialEven()
        {
            var factor = new StaticModIntFactor<Mod1000000007>(10);
            int expected = 1;
            factor.DoubleFactorial(0).Value.Should().Be(1);
            for (int i = 2; i <= 10; i += 2)
            {
                expected *= i;
                factor.DoubleFactorial(i).Value.Should().Be(expected);
            }
            factor.Invoking(factor => factor.Factorial(12)).Should().Throw<Exception>();
        }
    }
}
