using AtCoder;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class PolynomialTests
    {
        [Fact]
        public void Add()
        {
            (new IntPolynomial(new int[] { 1, 2, 3 })
                + new IntPolynomial(new int[] { 0, 5, 0, 2 }))
                .Coefficients.Should().Equal(1, 7, 3, 2);
            (new IntPolynomial(new int[] { 0, 5, 0, 2 })
                + new IntPolynomial(new int[] { 1, 2, 3 }))
                .Coefficients.Should().Equal(1, 7, 3, 2);
        }
        [Fact]
        public void Subtract()
        {
            (new IntPolynomial(new int[] { 1, 2, 3 })
                - new IntPolynomial(new int[] { 0, 5, 0, 2 }))
                .Coefficients.Should().Equal(1, -3, 3, -2);
            (new IntPolynomial(new int[] { 0, 5, 0, 2 })
                - new IntPolynomial(new int[] { 1, 2, 3 }))
                .Coefficients.Should().Equal(-1, 3, -3, 2);
        }
        [Fact]
        public void Minus()
        {
            (-new IntPolynomial(new int[] { 1, 2, 3 }))
                .Coefficients.Should().Equal(-1, -2, -3);
            (-new IntPolynomial(new int[] { -1, -2, -3 }))
                .Coefficients.Should().Equal(1, 2, 3);
        }
        [Fact]
        public void Multiply()
        {
            (new IntPolynomial(new int[] { 1, 2, 3 })
                * new IntPolynomial(new int[] { 0, 5, 0, 2 }))
                .Coefficients.Should().Equal(0, 5, 10, 17, 4, 6);
            (new IntPolynomial(new int[] { 0, 5, 0, 2 })
                * new IntPolynomial(new int[] { 1, 2, 3 }))
                .Coefficients.Should().Equal(0, 5, 10, 17, 4, 6);
        }
        [Fact]
        public void Divide()
        {
            (new IntPolynomial(new int[] { 0, 5, 10, 17, 4, 6 })
                / new IntPolynomial(new int[] { 0, 5, 0, 2 }))
                .Coefficients.Should().Equal(1, 2, 3);
            (new IntPolynomial(new int[] { 0, 5, 10, 17, 4, 6 })
                / new IntPolynomial(new int[] { 1, 2, 3 }))
                .Coefficients.Should().Equal(0, 5, 0, 2);
            (new IntPolynomial(new int[] { 1, 2, 3 })
                / new IntPolynomial(new int[] { 0, 5, 10, 17, 4, 6 }))
                .Coefficients.Should().BeEmpty();
        }
        [Fact]
        public void Derivative()
        {
            new IntPolynomial(new int[] { 0, 5, 10, 17, 4, 6 })
                .Derivative()
                .Coefficients
                .Should()
                .Equal(5, 20, 51, 16, 30);
        }
        [Fact]
        public void Integrate()
        {
            new DoublePolynomial(new double[] { 0, 5, 10, 17, 4, 6 })
                .Integrate()
                .Coefficients
                .Should()
                .Equal(0, 0, 5 / 2.0, 10 / 3.0, 17 / 4.0, 4 / 5.0, 6 / 6.0);
        }
        [Fact]
        public void Eval()
        {
            new IntPolynomial(new int[] { 0, 5, 10, 17, 4, 6 })
                .Eval(1)
                .Should()
                .Be(42);
            new IntPolynomial(new int[] { 0, 5, 10, 17, 4, 6 })
                .Eval(2)
                .Should()
                .Be(442);
        }

        public static TheoryData LagrangeInterpolation_Data => new TheoryData<(double, double)[]>
        {
            new (double, double)[]
            {
                (0,1),
                (1,10),
            },
            new (double, double)[]
            {
                (0,1),
                (1,10),
                (2,11),
            },
            new (double, double)[]
            {
                (0,1),
                (1,10),
                (2,11),
                (3,-51),
            }
        };
        [Theory]
        [MemberData(nameof(LagrangeInterpolation_Data))]
        public void LagrangeInterpolation((double x, double y)[] data)
        {
            var polynomial = DoublePolynomial.LagrangeInterpolation(data);
            polynomial.Coefficients.Should().HaveCount(data.Length);
            foreach (var (x, y) in data)
            {
                polynomial.Eval(x).Should().BeApproximately(y, 1e-8);
            }
        }
    }
}
