
namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class Matrix3x3Tests
{
    [Test, MultipleAssertions]
    public async Task Property()
    {
        var mat = new LongMatrix3x3((1, 2, 3), (4, 5, 6), (7, 8, 9));
        await mat.Row0.Should().BeEqualTo((1, 2, 3));
        await mat.Row1.Should().BeEqualTo((4, 5, 6));
        await mat.Row2.Should().BeEqualTo((7, 8, 9));
    }

    [Test]
    [Property("Category", "Operator")]
    public async Task SingleMinus()
    {
        var mat = -new LongMatrix3x3(
            (1, 2, 3),
            (5, 6, 7),
            (9, 10, 11)
        );
        await mat.Should().BeEqualTo(new LongMatrix3x3(
            (-1, -2, -3),
            (-5, -6, -7),
            (-9, -10, -11)
        ));
    }

    public static IEnumerable<(LongMatrix3x3, LongMatrix3x3, LongMatrix3x3)> Add_Data =>
        [
        (
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            ),
            LongMatrix3x3.MultiplicativeIdentity,
            new LongMatrix3x3(
                (2, 2, 3),
                (5, 7, 7),
                (9, 10, 12)
            )
        ),
        (
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            ),
            new LongMatrix3x3(
                (1, -2, 3),
                (5, -6, 7),
                (9, -10, 11)
            ),
            new LongMatrix3x3(
                (2, 0, 6),
                (10, 0, 14),
                (18, 0, 22)
            )
        )
];

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Add_Data))]
    public async Task Add(LongMatrix3x3 mat1, LongMatrix3x3 mat2, LongMatrix3x3 expected)
    {
        await (mat1 + mat2).Should().BeEqualTo(expected);
        await (mat2 + mat1).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(LongMatrix3x3, LongMatrix3x3, LongMatrix3x3)> Subtract_Data =>
    [
        (
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            ),
            new LongMatrix3x3(
                (1, -2, 3),
                (5, -6, 7),
                (9, -10, 11)
            ),
            new LongMatrix3x3(
                (0, 4, 0),
                (0, 12, 0),
                (0, 20, 0)
            )
        ),
        (
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            ),
            LongMatrix3x3.MultiplicativeIdentity,
            new LongMatrix3x3(
                (0, 2, 3),
                (5, 5, 7),
                (9, 10, 10)
            )
        ),
        (
            LongMatrix3x3.MultiplicativeIdentity,
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            ),
            new LongMatrix3x3(
                (0, -2, -3),
                (-5, -5, -7),
                (-9, -10, -10)
            )
        )
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Subtract_Data))]
    public async Task Subtract(LongMatrix3x3 mat1, LongMatrix3x3 mat2, LongMatrix3x3 expected)
    {
        await (mat1 - mat2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(LongMatrix3x3, LongMatrix3x3, LongMatrix3x3)> Multiply_Data =>
    [
        (
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            ),
            new LongMatrix3x3(
                (1, -2, 3),
                (5, -6, -7),
                (9, -10, 11)
            ),
            new LongMatrix3x3(
                (38, -44, 22),
                (98, -116, 50),
                (158, -188, 78)
            )
        ),
        (
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            ),
            LongMatrix3x3.MultiplicativeIdentity,
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            )
        ),
        (
            LongMatrix3x3.MultiplicativeIdentity,
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            ),
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            )
        )
    ];

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Multiply_Data))]
    public async Task Multiply(LongMatrix3x3 mat1, LongMatrix3x3 mat2, LongMatrix3x3 expected)
    {
        await (mat1 * mat2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(long, LongMatrix3x3, LongMatrix3x3)> MultiplyScalar_Data =>
    [
        (
            3,
            LongMatrix3x3.MultiplicativeIdentity,
            new LongMatrix3x3(
                (3, 0, 0),
                (0, 3, 0),
                (0, 0, 3)
            )
        ),
        (
            3,
            new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            ),
            new LongMatrix3x3(
                (3, 6, 9),
                (15, 18, 21),
                (27, 30, 33)
            )
        )
    ];

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(MultiplyScalar_Data))]
    public async Task MultiplyScalar(long a, LongMatrix3x3 mat, LongMatrix3x3 expected)
    {
        await (mat * a).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(LongMatrix3x3, (long, long, long), (long, long, long))> MultiplyVector_Data =>
    [
        (
            new LongMatrix3x3(
                (3, 0, 0),
                (0, 3, 0),
                (0, 0, 3)
            ),
            (1,2,3),
            (3,6,9)
        ),
        (
            new LongMatrix3x3(
                (1, 2, 3),
                (4, 5, 6),
                (7, 8, 9)
            ),
            (1,2,3),
            (14, 32, 50)
        )
    ];

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(MultiplyVector_Data))]
    public async Task MultiplyVector(LongMatrix3x3 mat, (long, long, long) vector, (long, long, long) expected)
    {
        await (mat * vector).Should().BeEqualTo(expected);
        await mat.Multiply(vector).Should().BeEqualTo(expected);
        await mat.Multiply(vector.Item1, vector.Item2, vector.Item3).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Pow()
    {
        var orig = new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            );
        await orig.Pow(5).Should().BeEqualTo(new LongMatrix3x3(
                (1825, 2162, 2499),
                (4847, 5742, 6637),
                (7869, 9322, 10775)
            ) * 144);
        var cur = orig;
        for (int i = 1; i < 10; i++)
        {
            await orig.Pow(i).Should().BeEqualTo(cur);
            cur *= orig;
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Determinant()
    {
        await new FractionMatrix3x3(
            (10, -9, -12),
            (7, -12, 11),
            (-10, 10, 3)
        ).Determinant().Should().BeEqualTo(319);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Inv()
    {
        var orig = new FractionMatrix3x3(
            (10, -9, -12),
            (7, -12, 11),
            (-10, 10, 3)
        );
        var inv = orig.Inv();
        await inv.Should().BeEqualTo(new FractionMatrix3x3(
            (new Fraction(-146, 319), new Fraction(-93, 319), new Fraction(-243, 319)),
            (new Fraction(-131, 319), new Fraction(-90, 319), new Fraction(-194, 319)),
            (new Fraction(-50, 319), new Fraction(-10, 319), new Fraction(-57, 319))
        ));
        await (orig * inv).Should().BeEqualTo(FractionMatrix3x3.MultiplicativeIdentity);
        await (inv * orig).Should().BeEqualTo(FractionMatrix3x3.MultiplicativeIdentity);
    }

    [Test]
    [Property("Category", "Normal")]
    public async Task AsSpan()
    {
        var mat = new LongMatrix3x3(
            (1, 2, 3),
            (4, 5, 6),
            (7, 8, 9)
        );
        await mat.AsSpan().ToArray().Should().BeStrictlyEquivalentTo([
            1L, 2L, 3L,
            4L, 5L, 6L,
            7L, 8L, 9L
        ]);
    }

    [Test]
    [Property("Category", "Normal")]
    public async Task AsSpan3Bytes()
    {
        var mat = new Matrix3x3<UInt24>(
            ((UInt24)1, (UInt24)2, (UInt24)3),
            ((UInt24)4, (UInt24)5, (UInt24)6),
            ((UInt24)7, (UInt24)8, (UInt24)9)
        );
        await mat.AsSpan().ToArray().Should().BeStrictlyEquivalentTo([
            (UInt24)1, (UInt24)2, (UInt24)3,
            (UInt24)4, (UInt24)5, (UInt24)6,
            (UInt24)7, (UInt24)8, (UInt24)9
        ]);
    }
}