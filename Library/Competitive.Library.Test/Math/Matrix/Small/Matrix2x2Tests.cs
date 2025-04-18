namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class Matrix2x2Tests
{
    [Fact]
    public void Property()
    {
        var mat = new LongMatrix2x2((1, 2), (3, 4));
        mat.Row0.ShouldBe((1, 2));
        mat.Row1.ShouldBe((3, 4));
    }

    [Fact]
    [Trait("Category", "Operator")]
    public void SingleMinus()
    {
        (-new LongMatrix2x2(
            (1, 2),
            (5, 6)
        )).ShouldBe(new LongMatrix2x2(
            (-1, -2),
            (-5, -6)
        ));
    }

    public static TheoryData<LongMatrix2x2, LongMatrix2x2, LongMatrix2x2> Add_Data => new()
    {
        {
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (2, 2),
                (5, 7)
            )
        },
        {
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
        },
    };

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(Add_Data))]
    public void Add(LongMatrix2x2 mat1, LongMatrix2x2 mat2, LongMatrix2x2 expected)
    {
        (mat1 + mat2).ShouldBe(expected);
        (mat2 + mat1).ShouldBe(expected);
    }

    public static TheoryData<LongMatrix2x2, LongMatrix2x2, LongMatrix2x2> Subtract_Data => new()
    {
        {
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
        },
        {
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (0, 2),
                (5, 5)
            )
        },
        {
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            new LongMatrix2x2(
                (0, -2),
                (-5, -5)
            )
        },
    };
    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(Subtract_Data))]
    public void Subtract(LongMatrix2x2 mat1, LongMatrix2x2 mat2, LongMatrix2x2 expected)
    {
        (mat1 - mat2).ShouldBe(expected);
    }

    public static TheoryData<LongMatrix2x2, LongMatrix2x2, LongMatrix2x2> Multiply_Data => new()
    {
        {
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
        },
        {
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            )
        },
        {
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            )
        },
    };

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(Multiply_Data))]
    public void Multiply(LongMatrix2x2 mat1, LongMatrix2x2 mat2, LongMatrix2x2 expected)
    {
        (mat1 * mat2).ShouldBe(expected);
    }

    public static TheoryData<long, LongMatrix2x2, LongMatrix2x2> MultiplyScalar_Data => new()
    {
        {
            3,
            LongMatrix2x2.MultiplicativeIdentity,
            new LongMatrix2x2(
                (3, 0),
                (0, 3)
            )
        },
        {
            3,
            new LongMatrix2x2(
                (1, 2),
                (5, 6)
            ),
            new LongMatrix2x2(
                (3, 6),
                (15, 18)
            )
        },
    };

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(MultiplyScalar_Data))]
    public void MultiplyScalar(long a, LongMatrix2x2 mat, LongMatrix2x2 expected)
    {
        (mat * a).ShouldBe(expected);
    }

    public static TheoryData<LongMatrix2x2, SerializableTuple<long, long>, SerializableTuple<long, long>> MultiplyVector_Data => new()
    {
        {
            new LongMatrix2x2(
                (3, 0),
                (0, 3)
            ),
            (1,2),
            (3,6)
        },
        {
            new LongMatrix2x2(
                (1, 2),
                (3, 4)
            ),
            (1,2),
            (5, 11)
        },
    };

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(MultiplyVector_Data))]
    public void MultiplyVector(LongMatrix2x2 mat, SerializableTuple<long, long> vector, SerializableTuple<long, long> expected)
    {
        Inner(mat, vector, expected);
        static void Inner(LongMatrix2x2 mat, (long, long) vector, (long, long) expected)
        {
            (mat * vector).ShouldBe(expected);
            mat.Multiply(vector).ShouldBe(expected);
            mat.Multiply(vector.Item1, vector.Item2).ShouldBe(expected);
        }
    }

    [Fact]
    [Trait("Category", "Normal")]
    public void Pow()
    {
        var orig = new LongMatrix2x2(
                (1, -2),
                (5, 6)
            );
        orig.Pow(7).ShouldBe(new LongMatrix2x2(
                (-6959, 6526),
                (-16315, -23274)
            ));
        var cur = orig;
        for (int i = 1; i < 10; i++)
        {
            orig.Pow(i).ShouldBe(cur);
            cur *= orig;
        }
    }

    [Fact]
    [Trait("Category", "Normal")]
    public void Determinant()
    {
        new FractionMatrix2x2(
            (10, -9),
            (7, -12)
        ).Determinant().ShouldBe(-57);
    }

    [Fact]
    [Trait("Category", "Normal")]
    public void Inv()
    {
        var orig = new FractionMatrix2x2(
            (10, -9),
            (7, -12)
        );
        var inv = orig.Inv();
        inv.ShouldBe(new FractionMatrix2x2(
            (12, -9),
            (7, -10)
        ) * new Fraction(1, 57));
        (orig * inv).ShouldBe(FractionMatrix2x2.MultiplicativeIdentity);
        (inv * orig).ShouldBe(FractionMatrix2x2.MultiplicativeIdentity);
    }

    [Fact]
    [Trait("Category", "Normal")]
    public void AsSpan()
    {
        var mat = new LongMatrix2x2(
            (1, 2),
            (3, 4)
        );
        mat.AsSpan().ToArray().ShouldBe([
            1, 2,
            3, 4
        ]);
    }

    [Fact]
    [Trait("Category", "Normal")]
    public void AsSpan3Bytes()
    {
        var mat = new Matrix2x2<UInt24>(
            ((UInt24)1, (UInt24)2),
            ((UInt24)3, (UInt24)4)
        );
        mat.AsSpan().ToArray().ShouldBe([
            (UInt24)1, (UInt24)2,
            (UInt24)3, (UInt24)4
        ]);
    }
}