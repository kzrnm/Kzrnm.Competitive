using AtCoder;
using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    // verification-helper: SAMEAS Library/run.test.py
    public class PolynomialModIntTests
    {
        [Fact]
        public void Add()
        {
            (new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 1, 2, 3 })
                + new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 0, 2 }))
                .Coefficients.Should().Equal(1, 7, 3, 2);
            (new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 0, 2 })
                + new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 1, 2, 3 }))
                .Coefficients.Should().Equal(1, 7, 3, 2);
        }
        [Fact]
        public void Subtract()
        {
            (new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 1, 2, 3 })
                - new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 0, 2 }))
                .Coefficients.Should().Equal(1, -3, 3, -2);
            (new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 0, 2 })
                - new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 1, 2, 3 }))
                .Coefficients.Should().Equal(-1, 3, -3, 2);
        }
        [Fact]
        public void Minus()
        {
            (-new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 1, 2, 3 }))
                .Coefficients.Should().Equal(-1, -2, -3);
            (-new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { -1, -2, -3 }))
                .Coefficients.Should().Equal(1, 2, 3);
        }
        [Fact]
        public void Multiply()
        {
            (new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 1, 2, 3 })
                * new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 0, 2 }))
                .Coefficients.Should().Equal(0, 5, 10, 17, 4, 6);
            (new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 0, 2 })
                * new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 1, 2, 3 }))
                .Coefficients.Should().Equal(0, 5, 10, 17, 4, 6);
        }
        [Fact]
        public void Divide()
        {
            (new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 10, 17, 4, 6 })
                / new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 0, 2 }))
                .Coefficients.Should().Equal(1, 2, 3);
            (new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 10, 17, 4, 6 })
                / new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 1, 2, 3 }))
                .Coefficients.Should().Equal(0, 5, 0, 2);
            (new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 1, 2, 3 })
                / new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 10, 17, 4, 6 }))
                .Coefficients.Should().BeEmpty();
        }
        [Fact]
        public void Derivative()
        {
            new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 10, 17, 4, 6 })
                .Derivative()
                .Coefficients
                .Should()
                .Equal(5, 20, 51, 16, 30);
        }
        [Fact]
        public void Integrate()
        {
            new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 10, 17, 4, 6 })
                .Integrate()
                .Coefficients
                .Should()
                .Equal(0, 0, 499122179, 332748121, 748683269, 399297742, 1);
        }
        [Fact]
        public void Calc()
        {
            new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 10, 17, 4, 6 })
                .Calc(1)
                .Should()
                .Be((StaticModInt<Mod998244353>)42);
            new PolynomialModInt<Mod998244353>(new StaticModInt<Mod998244353>[] { 0, 5, 10, 17, 4, 6 })
                .Calc(2)
                .Should()
                .Be((StaticModInt<Mod998244353>)442);
        }

        public static TheoryData LagrangeInterpolation_Data = new TheoryData<(StaticModInt<Mod998244353>, StaticModInt<Mod998244353>)[]>
        {
            new (StaticModInt<Mod998244353>, StaticModInt<Mod998244353>)[]
            {
                (0,1),
                (1,10),
            },
            new (StaticModInt<Mod998244353>, StaticModInt<Mod998244353>)[]
            {
                (0,1),
                (1,10),
                (2,11),
            },
            new (StaticModInt<Mod998244353>, StaticModInt<Mod998244353>)[]
            {
                (0,1),
                (1,10),
                (2,11),
                (3,-51),
            }
        };
        [Theory]
        [MemberData(nameof(LagrangeInterpolation_Data))]
        public void LagrangeInterpolation((StaticModInt<Mod998244353> x, StaticModInt<Mod998244353> y)[] data)
        {
            var polynomial = PolynomialModInt<Mod998244353>.LagrangeInterpolation(data);
            polynomial.Coefficients.Should().HaveCount(data.Length);
            foreach (var (x, y) in data)
            {
                polynomial.Calc(x).Should().Be(y);
            }
        }
    }
}
