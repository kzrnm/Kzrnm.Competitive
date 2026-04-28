namespace Kzrnm.Competitive.Testing.MathNS;

public class PolynomialTests
{
    [Test, MultipleAssertions]
    public async Task Add()
    {
        await (new IntPolynomial([1, 2, 3])
            + new IntPolynomial([0, 5, 0, 2]))
            .Coefficients.Should().BeEquivalentOrderTo([1, 7, 3, 2]);
        await (new IntPolynomial([0, 5, 0, 2])
            + new IntPolynomial([1, 2, 3]))
            .Coefficients.Should().BeEquivalentOrderTo([1, 7, 3, 2]);
    }
    [Test, MultipleAssertions]
    public async Task Subtract()
    {
        await (new IntPolynomial([1, 2, 3])
            - new IntPolynomial([0, 5, 0, 2]))
            .Coefficients.Should().BeEquivalentOrderTo([1, -3, 3, -2]);
        await (new IntPolynomial([0, 5, 0, 2])
            - new IntPolynomial([1, 2, 3]))
            .Coefficients.Should().BeEquivalentOrderTo([-1, 3, -3, 2]);
    }
    [Test, MultipleAssertions]
    public async Task Minus()
    {
        await (-new IntPolynomial([1, 2, 3]))
            .Coefficients.Should().BeEquivalentOrderTo([-1, -2, -3]);
        await (-new IntPolynomial([-1, -2, -3]))
            .Coefficients.Should().BeEquivalentOrderTo([1, 2, 3]);
    }
    [Test, MultipleAssertions]
    public async Task Multiply()
    {
        await (new IntPolynomial([1, 2, 3])
            * new IntPolynomial([0, 5, 0, 2]))
            .Coefficients.Should().BeEquivalentOrderTo([0, 5, 10, 17, 4, 6]);
        await (new IntPolynomial([0, 5, 0, 2])
            * new IntPolynomial([1, 2, 3]))
            .Coefficients.Should().BeEquivalentOrderTo([0, 5, 10, 17, 4, 6]);
    }
    [Test, MultipleAssertions]
    public async Task Divide()
    {
        await (new IntPolynomial([0, 5, 10, 17, 4, 6])
            / new IntPolynomial([0, 5, 0, 2]))
            .Coefficients.Should().BeEquivalentOrderTo([1, 2, 3]);
        await (new IntPolynomial([0, 5, 10, 17, 4, 6])
            / new IntPolynomial([1, 2, 3]))
            .Coefficients.Should().BeEquivalentOrderTo([0, 5, 0, 2]);
        await (new IntPolynomial([1, 2, 3])
            / new IntPolynomial([0, 5, 10, 17, 4, 6]))
            .Coefficients.Should().BeEmpty();
    }
    [Test, MultipleAssertions]
    public async Task Derivative()
    {
        await new IntPolynomial([0, 5, 10, 17, 4, 6])
             .Derivative()
             .Coefficients
             .Should().BeEquivalentOrderTo([5, 20, 51, 16, 30]);
    }
    [Test, MultipleAssertions]
    public async Task Integrate()
    {
        await new DoublePolynomial([0, 5, 10, 17, 4, 6])
            .Integrate()
            .Coefficients
            .Should().BeEquivalentOrderTo([0, 0, 5 / 2.0, 10 / 3.0, 17 / 4.0, 4 / 5.0, 6 / 6.0]);
    }
    [Test, MultipleAssertions]
    public async Task Eval()
    {
        await new IntPolynomial([0, 5, 10, 17, 4, 6])
            .Eval(1)
            .Should().BeEqualTo(42);
        await new IntPolynomial([0, 5, 10, 17, 4, 6])
            .Eval(2)
            .Should().BeEqualTo(442);
    }

    public static IEnumerable<(double x, double y)[]> LagrangeInterpolation_Data =>
    [
        [
            (0,1),
            (1,10),
        ],
        [
            (0,1),
            (1,10),
            (2,11),
        ],
        [
            (0,1),
            (1,10),
            (2,11),
            (3,-51),
        ],
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(LagrangeInterpolation_Data))]
    public async Task LagrangeInterpolation((double x, double y)[] data)
    {
        var polynomial = DoublePolynomial.LagrangeInterpolation(data);
        await polynomial.Coefficients.Length.Should().BeEqualTo(data.Length);
        foreach (var (x, y) in data)
        {
            await polynomial.Eval(x).Should().BeCloseTo(y, 1e-8);
        }
    }
}