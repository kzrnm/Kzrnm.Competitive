namespace Kzrnm.Competitive.Testing.MathNS;

public class PolynomialTests
{
    [Fact]
    public void Add()
    {
        (new IntPolynomial([1, 2, 3])
            + new IntPolynomial([0, 5, 0, 2]))
            .Coefficients.ShouldBe([1, 7, 3, 2]);
        (new IntPolynomial([0, 5, 0, 2])
            + new IntPolynomial([1, 2, 3]))
            .Coefficients.ShouldBe([1, 7, 3, 2]);
    }
    [Fact]
    public void Subtract()
    {
        (new IntPolynomial([1, 2, 3])
            - new IntPolynomial([0, 5, 0, 2]))
            .Coefficients.ShouldBe([1, -3, 3, -2]);
        (new IntPolynomial([0, 5, 0, 2])
            - new IntPolynomial([1, 2, 3]))
            .Coefficients.ShouldBe([-1, 3, -3, 2]);
    }
    [Fact]
    public void Minus()
    {
        (-new IntPolynomial([1, 2, 3]))
            .Coefficients.ShouldBe([-1, -2, -3]);
        (-new IntPolynomial([-1, -2, -3]))
            .Coefficients.ShouldBe([1, 2, 3]);
    }
    [Fact]
    public void Multiply()
    {
        (new IntPolynomial([1, 2, 3])
            * new IntPolynomial([0, 5, 0, 2]))
            .Coefficients.ShouldBe([0, 5, 10, 17, 4, 6]);
        (new IntPolynomial([0, 5, 0, 2])
            * new IntPolynomial([1, 2, 3]))
            .Coefficients.ShouldBe([0, 5, 10, 17, 4, 6]);
    }
    [Fact]
    public void Divide()
    {
        (new IntPolynomial([0, 5, 10, 17, 4, 6])
            / new IntPolynomial([0, 5, 0, 2]))
            .Coefficients.ShouldBe([1, 2, 3]);
        (new IntPolynomial([0, 5, 10, 17, 4, 6])
            / new IntPolynomial([1, 2, 3]))
            .Coefficients.ShouldBe([0, 5, 0, 2]);
        (new IntPolynomial([1, 2, 3])
            / new IntPolynomial([0, 5, 10, 17, 4, 6]))
            .Coefficients.ShouldBeEmpty();
    }
    [Fact]
    public void Derivative()
    {
        new IntPolynomial([0, 5, 10, 17, 4, 6])
            .Derivative()
            .Coefficients
            .ShouldBe([5, 20, 51, 16, 30]);
    }
    [Fact]
    public void Integrate()
    {
        new DoublePolynomial([0, 5, 10, 17, 4, 6])
            .Integrate()
            .Coefficients
            .ShouldBe([0, 0, 5 / 2.0, 10 / 3.0, 17 / 4.0, 4 / 5.0, 6 / 6.0]);
    }
    [Fact]
    public void Eval()
    {
        new IntPolynomial([0, 5, 10, 17, 4, 6])
            .Eval(1)
            .ShouldBe(42);
        new IntPolynomial([0, 5, 10, 17, 4, 6])
            .Eval(2)
            .ShouldBe(442);
    }

    public static TheoryData<(double x, double y)[]> LagrangeInterpolation_Data => new()
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
        polynomial.Coefficients.Length.ShouldBe(data.Length);
        foreach (var (x, y) in data)
        {
            polynomial.Eval(x).ShouldBe(y, 1e-8);
        }
    }
}
