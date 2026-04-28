namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class Matrix2x2Tests
{
    [Test, MultipleAssertions]
    public async Task Property()
    {
        var mat = new LongMatrix2x2((1, 2), (3, 4));
        await mat.Row0.Should().BeEqualTo((1, 2));
        await mat.Row1.Should().BeEqualTo((3, 4));
    }

    [Test]
    [Property("Category", "Operator")]
    public async Task SingleMinus()
    {
        var mat = -new LongMatrix2x2(
            (1, 2),
            (5, 6)
        );
        await mat.Should().BeEqualTo(new LongMatrix2x2(
            (-1, -2),
            (-5, -6)
        ));
    }

    public static IEnumerable<(LongMatrix2x2, LongMatrix2x2, LongMatrix2x2)> Add_Data =>
        [
        (
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (2, 2),
                (5, 7)
            )
        ),
        (
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            new LongMatrix2x2(
                (1, -2),
                (5, -6)
            ),
            new LongMatrix2x2(
                (2, 0),
                (10, 0)
            )
        ),
    ];

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Add_Data))]
    public async Task Add(LongMatrix2x2 mat1, LongMatrix2x2 mat2, LongMatrix2x2 expected)
    {
        await (mat1 + mat2).Should().BeEqualTo(expected);
        await (mat2 + mat1).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(LongMatrix2x2, LongMatrix2x2, LongMatrix2x2)> Subtract_Data =>
    [
        (
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            new LongMatrix2x2(
                (1, -2),
                (5, -6)
            ),
            new LongMatrix2x2(
                (0, 4),
                (0, 12)
            )
        ),
        (
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (0, 2),
                (5, 5)
            )
        ),
        (
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            new LongMatrix2x2(
                (0, -2),
                (-5, -5)
            )
        ),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Subtract_Data))]
    public async Task Subtract(LongMatrix2x2 mat1, LongMatrix2x2 mat2, LongMatrix2x2 expected)
    {
        await (mat1 - mat2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(LongMatrix2x2, LongMatrix2x2, LongMatrix2x2)> Multiply_Data =>
    [
        (
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            new LongMatrix2x2(
                (1, -2),
                (5, -6)
            ),
            new LongMatrix2x2(
                (11, -14),
                (35, -46)
            )
        ),
        (
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            )
        ),
        (
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            )
        ),
    ];

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Multiply_Data))]
    public async Task Multiply(LongMatrix2x2 mat1, LongMatrix2x2 mat2, LongMatrix2x2 expected)
    {
        await (mat1 * mat2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(long, LongMatrix2x2, LongMatrix2x2)> MultiplyScalar_Data =>
    [
        (
            3,
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (3, 0),
                (0, 3)
            )
        ),
        (
            3,
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            new LongMatrix2x2(
                (3, 6),
                (15, 18)
            )
        ),
    ];

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(MultiplyScalar_Data))]
    public async Task MultiplyScalar(long a, LongMatrix2x2 mat, LongMatrix2x2 expected)
    {
        await (mat * a).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(LongMatrix2x2, (long, long), (long, long))> MultiplyVector_Data =>
    [
        (
            new LongMatrix2x2(
                (3, 0),
                (0, 3)
            ),
            (1,2),
            (3,6)
        ),
        (
            new LongMatrix2x2(
                (1, 2),
                (3, 4)
            ),
            (1,2),
            (5, 11)
        ),
    ];

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(MultiplyVector_Data))]
    public async Task MultiplyVector(LongMatrix2x2 mat, (long, long) vector, (long, long) expected)
    {
        await (mat * vector).Should().BeEqualTo(expected);
        await mat.Multiply(vector).Should().BeEqualTo(expected);
        await mat.Multiply(vector.Item1, vector.Item2).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Pow()
    {
        var orig = new LongMatrix2x2(
                (1, -2),
                (5, 6)
            );
        await orig.Pow(7).Should().BeEqualTo(new LongMatrix2x2(
                (-6959, 6526),
                (-16315, -23274)
            ));
        var cur = orig;
        for (int i = 1; i < 10; i++)
        {
            await orig.Pow(i).Should().BeEqualTo(cur);
            cur *= orig;
        }
    }

    [Test]
    [Property("Category", "Normal")]
    public async Task Determinant()
    {
        await new FractionMatrix2x2(
            (10, -9),
            (7, -12)
        ).Determinant().Should().BeEqualTo(-57);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Inv()
    {
        var orig = new FractionMatrix2x2(
            (10, -9),
            (7, -12)
        );
        var inv = orig.Inv();
        await inv.Should().BeEqualTo(new FractionMatrix2x2(
            (12, -9),
            (7, -10)
        ) * new Fraction(1, 57));
        await (orig * inv).Should().BeEqualTo(FractionMatrix2x2.MultiplicativeIdentity);
        await (inv * orig).Should().BeEqualTo(FractionMatrix2x2.MultiplicativeIdentity);
    }

    [Test]
    [Property("Category", "Normal")]
    public async Task AsSpan()
    {
        var mat = new LongMatrix2x2(
            (1, 2),
            (3, 4)
        );
        await mat.AsSpan().ToArray().Should().BeEquivalentOrderTo([
            1L, 2L,
            3L, 4L
        ]);
    }

    [Test]
    [Property("Category", "Normal")]
    public async Task AsSpan3Bytes()
    {
        var mat = new Matrix2x2<UInt24>(
            ((UInt24)1, (UInt24)2),
            ((UInt24)3, (UInt24)4)
        );
        await mat.AsSpan().ToArray().Should().BeEquivalentOrderTo([
            (UInt24)1, (UInt24)2,
            (UInt24)3, (UInt24)4
        ]);
    }
}