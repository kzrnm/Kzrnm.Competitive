namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class Matrix4x4Tests
{
    [Test, MultipleAssertions]
    public async Task Property()
    {
        var mat = new LongMatrix4x4((1, 2, 3, 4), (5, 6, 7, 8), (9, 10, 11, 12), (13, 14, 15, 16));
        await mat.Row0.Should().BeEqualTo((1, 2, 3, 4));
        await mat.Row1.Should().BeEqualTo((5, 6, 7, 8));
        await mat.Row2.Should().BeEqualTo((9, 10, 11, 12));
        await mat.Row3.Should().BeEqualTo((13, 14, 15, 16));
    }

    [Test]
    [Property("Category", "Operator")]
    public async Task SingleMinus()
    {
        var mat = -new LongMatrix4x4(
            (1, 2, 3, 4),
            (5, 6, 7, 8),
            (9, 10, 11, 12),
            (13, 14, 15, 16)
        );
        await mat.Should().BeEqualTo(new LongMatrix4x4(
            (-1, -2, -3, -4),
            (-5, -6, -7, -8),
            (-9, -10, -11, -12),
            (-13, -14, -15, -16)
        ));
    }

    public static IEnumerable<(LongMatrix4x4, LongMatrix4x4, LongMatrix4x4)> Add_Data =>
    [
        (
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            ),
            LongMatrix4x4.MultiplicativeIdentity,
            new LongMatrix4x4(
                (2, 2, 3, 4),
                (5, 7, 7, 8),
                (9, 10, 12, 12),
                (13, 14, 15, 17)
            )
        ),
        (
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            ),
            new LongMatrix4x4(
                (1, -2, 3, 4),
                (5, -6, 7, 8),
                (9, -10, 11, 12),
                (13,-14, -15, -16)
            ),
            new LongMatrix4x4(
                (2, 0, 6, 8),
                (10, 0, 14, 16),
                (18, 0, 22, 24),
                (26, 0, 0, 0)
            )
        ),
    ];

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Add_Data))]
    public async Task Add(LongMatrix4x4 mat1, LongMatrix4x4 mat2, LongMatrix4x4 expected)
    {
        await (mat1 + mat2).Should().BeEqualTo(expected);
        await (mat2 + mat1).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(LongMatrix4x4, LongMatrix4x4, LongMatrix4x4)> Subtract_Data =>
    [
        (
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            ),
            new LongMatrix4x4(
                (1, -2, 3, 4),
                (5, -6, 7, 8),
                (9, -10, 11, 12),
                (13,-14, -15, -16)
            ),
            new LongMatrix4x4(
                (0, 4, 0, 0),
                (0, 12, 0, 0),
                (0, 20, 0, 0),
                (0, 28, 30, 32)
            )
        ),
        (
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            ),
            LongMatrix4x4.MultiplicativeIdentity,
            new LongMatrix4x4(
                (0, 2, 3, 4),
                (5, 5, 7, 8),
                (9, 10, 10, 12),
                (13, 14, 15, 15)
            )
        ),
        (
            LongMatrix4x4.MultiplicativeIdentity,
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            ),
            new LongMatrix4x4(
                (0, -2, -3, -4),
                (-5, -5, -7, -8),
                (-9, -10, -10, -12),
                (-13, -14, -15, -15)
            )
        ),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Subtract_Data))]
    public async Task Subtract(LongMatrix4x4 mat1, LongMatrix4x4 mat2, LongMatrix4x4 expected)
    {
        await (mat1 - mat2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(LongMatrix4x4, LongMatrix4x4, LongMatrix4x4)> Multiply_Data =>
    [
        (
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            ),
            new LongMatrix4x4(
                (1, -2, 3, 4),
                (5, -6, 7, 8),
                (9, -10, 11, 12),
                (13,-14, -15, -16)
            ),
            new LongMatrix4x4(
                (90, -100, -10, -8),
                (202, -228, 14, 24),
                (314, -356, 38, 56),
                (426, -484, 62, 88)
            )
        ),
        (
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            ),
            LongMatrix4x4.MultiplicativeIdentity,
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            )
        ),
        (
            LongMatrix4x4.MultiplicativeIdentity,
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            ),
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            )
        ),
    ];

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Multiply_Data))]
    public async Task Multiply(LongMatrix4x4 mat1, LongMatrix4x4 mat2, LongMatrix4x4 expected)
    {
        await (mat1 * mat2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(long, LongMatrix4x4, LongMatrix4x4)> MultiplyScalar_Data =>
    [
        (
            3,
            LongMatrix4x4.MultiplicativeIdentity,
            new LongMatrix4x4(
                (3, 0, 0, 0),
                (0, 3, 0, 0),
                (0, 0, 3, 0),
                (0, 0, 0, 3)
            )
        ),
        (
            3,
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            ),
            new LongMatrix4x4(
                (3, 6, 9, 12),
                (15, 18, 21, 24),
                (27, 30, 33, 36),
                (39, 42, 45, 48)
            )
        ),
    ];

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(MultiplyScalar_Data))]
    public async Task MultiplyScalar(long a, LongMatrix4x4 mat, LongMatrix4x4 expected)
    {
        await (mat * a).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(LongMatrix4x4, (long, long, long, long), (long, long, long, long))> MultiplyVector_Data =>
    [
        (
            new LongMatrix4x4(
                (3, 0, 0, 0),
                (0, 3, 0, 0),
                (0, 0, 3, 0),
                (0, 0, 0, 3)
            ),
            (1,2,3,4),
            (3,6,9,12)
        ),
        (
            new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            ),
            (1,2,3,4),
            (30, 70, 110, 150)
        ),
    ];

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(MultiplyVector_Data))]
    public async Task MultiplyVector(LongMatrix4x4 mat, (long, long, long, long) vector, (long, long, long, long) expected)
    {
        await (mat * vector).Should().BeEqualTo(expected);
        await mat.Multiply(vector).Should().BeEqualTo(expected);
        await mat.Multiply(vector.Item1, vector.Item2, vector.Item3, vector.Item4).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Pow()
    {
        var orig = new LongMatrix4x4(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            );
        await orig.Pow(3).Should().BeEqualTo(new LongMatrix4x4(
                (785, 890, 995, 1100),
                (1817, 2058, 2299, 2540),
                (2849, 3226, 3603, 3980),
                (3881, 4394, 4907, 5420)
            ) * 4);
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
        await new FractionMatrix4x4(
            (10, -9, -12, 6),
            (7, -12, 11, 15),
            (1, 0, 2, 9),
            (-10, 10, 3, 13)
        ).Determinant().Should().BeEqualTo(-10683);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Inv()
    {
        var orig = new FractionMatrix4x4(
            (10, -9, -12, 6),
            (7, -12, 11, 15),
            (1, 0, 2, 9),
            (-10, 10, 3, 13)
        );
        var inv = orig.Inv();
        await inv.Should().BeEqualTo(new FractionMatrix4x4(
            (new Fraction(-78, 1187), new Fraction(-397, 3561), new Fraction(1810, 3561), new Fraction(-229, 1187)),
            (new Fraction(-265, 3561), new Fraction(-1364, 10683), new Fraction(4658, 10683), new Fraction(-428, 3561)),
            (new Fraction(-84, 1187), new Fraction(29, 3561), new Fraction(397, 3561), new Fraction(-64, 1187)),
            (new Fraction(82, 3561), new Fraction(113, 10683), new Fraction(319, 10683), new Fraction(119, 3561))
        ));
        await (orig * inv).Should().BeEqualTo(FractionMatrix4x4.MultiplicativeIdentity);
        await (inv * orig).Should().BeEqualTo(FractionMatrix4x4.MultiplicativeIdentity);
    }

    [Test]
    [Property("Category", "Normal")]
    public async Task AsSpan()
    {
        var mat = new LongMatrix4x4(
            (1, 2, 3, 4),
            (5, 6, 7, 8),
            (9, 10, 11, 12),
            (13, 14, 15, 16)
        );
        await mat.AsSpan().ToArray().Should().BeStrictlyEquivalentTo([
             1L, 2L, 3L, 4L,
             5L, 6L, 7L, 8L,
             9L,10L,11L,12L,
            13L,14L,15L,16L
        ]);
    }

    [Test]
    [Property("Category", "Normal")]
    public async Task AsSpan3Bytes()
    {
        var mat = new Matrix4x4<UInt24>(
            ((UInt24)1, (UInt24)2, (UInt24)3, (UInt24)4),
            ((UInt24)5, (UInt24)6, (UInt24)7, (UInt24)8),
            ((UInt24)9, (UInt24)10, (UInt24)11, (UInt24)12),
            ((UInt24)13, (UInt24)14, (UInt24)15, (UInt24)16)
        );
        await mat.AsSpan().ToArray().Should().BeStrictlyEquivalentTo([
            (UInt24)1, (UInt24)2, (UInt24)3, (UInt24)4,
            (UInt24)5, (UInt24)6, (UInt24)7, (UInt24)8,
            (UInt24)9, (UInt24)10,(UInt24)11,(UInt24)12,
            (UInt24)13,(UInt24)14,(UInt24)15,(UInt24)16
        ]);
    }
}