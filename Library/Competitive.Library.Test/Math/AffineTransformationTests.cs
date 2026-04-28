namespace Kzrnm.Competitive.Testing.MathNS;

public class AffineTransformationTests
{
    public static IEnumerable<(DoubleAffineTransformation, DoubleAffineTransformation, DoubleAffineTransformation)> ApplyOther_Data =>
        [
            (
                new (2.0, 3.0),
                new (-5, 7.0),
                new (-10, -8)
            ),
            (
                new (0, 3.0),
                new (-3, 7.0),
                new (0, -2)
            ),
        ];

    [Test]
    [MethodDataSource(nameof(ApplyOther_Data))]
    public async Task ApplyOther(
        DoubleAffineTransformation a,
        DoubleAffineTransformation b,
        DoubleAffineTransformation expected)
    {
        await b.Apply(a).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(DoubleAffineTransformation, double, double)> ApplyNumber_Data =>
    [
        (new (2.0, 3.0), 1, 5),
        (new (-5, 7.0), 1, 2),
        (new (-10, -8), 1, -18),
        (new (0, 3.0), 1, 3),
        (new (-3, 7.0), 1, 4),
        (new (0, -2), 1, -2),
        (new (2.0, 3.0), -1.5, 0),
        (new (-5, 7.0), -1.5, 14.5),
        (new (-10, -8), -1.5, 7),
        (new (0, 3.0), -1.5, 3),
        (new (-3, 7.0), -1.5, 11.5),
        (new (0, -2), -1.5, -2),
    ];
    [Test]
    [MethodDataSource(nameof(ApplyNumber_Data))]
    public async Task ApplyNumber(
        DoubleAffineTransformation a, double x, double expected)
    {
        await a.Apply(x).Should().BeEqualTo(expected);
    }
}