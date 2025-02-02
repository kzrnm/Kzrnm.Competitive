namespace Kzrnm.Competitive.Testing.MathNS;

public class AffineTransformationTests
{
    public static TheoryData<
        DoubleAffineTransformation,
        DoubleAffineTransformation,
        DoubleAffineTransformation> ApplyOther_Data => new()
        {
            {
                new DoubleAffineTransformation(2.0, 3.0),
                new DoubleAffineTransformation(-5, 7.0),
                new DoubleAffineTransformation(-10, -8)
            },
            {
                new DoubleAffineTransformation(0, 3.0),
                new DoubleAffineTransformation(-3, 7.0),
                new DoubleAffineTransformation(0, -2)
            },
        };

    [Theory]
    [MemberData(nameof(ApplyOther_Data))]
    public void ApplyOther(
        DoubleAffineTransformation a,
        DoubleAffineTransformation b,
        DoubleAffineTransformation expected)
    {
        b.Apply(a).ShouldBe(expected);
    }

    public static TheoryData<DoubleAffineTransformation, double, double> ApplyNumber_Data => new()
    {
        { new DoubleAffineTransformation(2.0, 3.0), 1, 5 },
        { new DoubleAffineTransformation(-5, 7.0), 1, 2 },
        { new DoubleAffineTransformation(-10, -8), 1, -18 },
        { new DoubleAffineTransformation(0, 3.0), 1, 3 },
        { new DoubleAffineTransformation(-3, 7.0), 1, 4 },
        { new DoubleAffineTransformation(0, -2), 1, -2 },

        { new DoubleAffineTransformation(2.0, 3.0), -1.5, 0 },
        { new DoubleAffineTransformation(-5, 7.0), -1.5, 14.5 },
        { new DoubleAffineTransformation(-10, -8), -1.5, 7 },
        { new DoubleAffineTransformation(0, 3.0), -1.5, 3 },
        { new DoubleAffineTransformation(-3, 7.0), -1.5, 11.5 },
        { new DoubleAffineTransformation(0, -2), -1.5, -2 },
    };
    [Theory]
    [MemberData(nameof(ApplyNumber_Data))]
    public void ApplyNumber(
        DoubleAffineTransformation a, double x, double expected)
    {
        a.Apply(x).ShouldBe(expected);
    }
}
