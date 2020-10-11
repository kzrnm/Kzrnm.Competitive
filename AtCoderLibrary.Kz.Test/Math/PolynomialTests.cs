using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class PolynomialTests
    {
        [Fact]
        public void Add()
        {
            (new Polynomial<int, IntOperator>(new int[] { 1, 2, 3 })
                + new Polynomial<int, IntOperator>(new int[] { 0, 5, 0, 2 }))
                .Value.Should().Equal(1, 7, 3, 2);
            (new Polynomial<int, IntOperator>(new int[] { 0, 5, 0, 2 })
                + new Polynomial<int, IntOperator>(new int[] { 1, 2, 3 }))
                .Value.Should().Equal(1, 7, 3, 2);
        }
        [Fact]
        public void Subtract()
        {
            (new Polynomial<int, IntOperator>(new int[] { 1, 2, 3 })
                - new Polynomial<int, IntOperator>(new int[] { 0, 5, 0, 2 }))
                .Value.Should().Equal(1, -3, 3, -2);
            (new Polynomial<int, IntOperator>(new int[] { 0, 5, 0, 2 })
                - new Polynomial<int, IntOperator>(new int[] { 1, 2, 3 }))
                .Value.Should().Equal(-1, 3, -3, 2);
        }
        [Fact]
        public void Multiply()
        {
            (new Polynomial<int, IntOperator>(new int[] { 1, 2, 3 })
                * new Polynomial<int, IntOperator>(new int[] { 0, 5, 0, 2 }))
                .Value.Should().Equal(0, 5, 10, 17, 4, 6);
            (new Polynomial<int, IntOperator>(new int[] { 0, 5, 0, 2 })
                * new Polynomial<int, IntOperator>(new int[] { 1, 2, 3 }))
                .Value.Should().Equal(0, 5, 10, 17, 4, 6);
        }
        [Fact]
        public void Divide()
        {
            (new Polynomial<int, IntOperator>(new int[] { 0, 5, 10, 17, 4, 6 })
                / new Polynomial<int, IntOperator>(new int[] { 0, 5, 0, 2 }))
                .Value.Should().Equal(1, 2, 3);
            (new Polynomial<int, IntOperator>(new int[] { 0, 5, 10, 17, 4, 6 })
                / new Polynomial<int, IntOperator>(new int[] { 1, 2, 3 }))
                .Value.Should().Equal(0, 5, 0, 2);
        }
        [Fact]
        public void Derivative()
        {
            new Polynomial<int, IntOperator>(new int[] { 0, 5, 10, 17, 4, 6 })
                .Derivative()
                .Value
                .Should()
                .Equal(5, 20, 51, 16, 30);
        }
        [Fact]
        public void Integrate()
        {
            new Polynomial<double, DoubleOperator>(new double[] { 0, 5, 10, 17, 4, 6 })
                .Integrate()
                .Value
                .Should()
                .Equal(0, 0, 5 / 2.0, 10 / 3.0, 17 / 4.0, 4 / 5.0, 6 / 6.0);
        }
        [Fact]
        public void Calc()
        {
            new Polynomial<int, IntOperator>(new int[] { 0, 5, 10, 17, 4, 6 })
                .Calc(1)
                .Should()
                .Be(42);
            new Polynomial<int, IntOperator>(new int[] { 0, 5, 10, 17, 4, 6 })
                .Calc(2)
                .Should()
                .Be(442);
        }
    }
}
