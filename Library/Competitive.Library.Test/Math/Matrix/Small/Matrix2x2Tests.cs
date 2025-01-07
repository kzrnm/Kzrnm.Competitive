
namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class Matrix2x2Tests
    {
        [Fact]
        public void Property()
        {
            var mat = new LongMatrix2x2((1, 2), (3, 4));
            mat.Row0.Should().Be((1, 2));
            mat.Row1.Should().Be((3, 4));
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            (-new LongMatrix2x2(
                (1, 2),
                (5, 6)
            )).Should().Be(new LongMatrix2x2(
                (-1, -2),
                (-5, -6)
            ));
        }

        public static TheoryData Add_Data => new TheoryData<LongMatrix2x2, LongMatrix2x2, LongMatrix2x2>
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
            (mat1 + mat2).Should().Be(expected);
            (mat2 + mat1).Should().Be(expected);
        }

        public static TheoryData Subtract_Data => new TheoryData<LongMatrix2x2, LongMatrix2x2, LongMatrix2x2>
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
            (mat1 - mat2).Should().Be(expected);
        }

        public static TheoryData Multiply_Data => new TheoryData<LongMatrix2x2, LongMatrix2x2, LongMatrix2x2>
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
            (mat1 * mat2).Should().Be(expected);
        }

        public static TheoryData MultiplyScalar_Data => new TheoryData<long, LongMatrix2x2, LongMatrix2x2>
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
            (mat * a).Should().Be(expected);
        }

        public static TheoryData MultiplyVector_Data => new TheoryData<LongMatrix2x2, (long, long), (long, long)>
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
        public void MultiplyVector(LongMatrix2x2 mat, (long v0, long v1) vector, (long, long) expected)
        {
            (mat * vector).Should().Be(expected);
            mat.Multiply(vector).Should().Be(expected);
            mat.Multiply(vector.v0, vector.v1).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = new LongMatrix2x2(
                    (1, -2),
                    (5, 6)
                );
            orig.Pow(7).Should().Be(new LongMatrix2x2(
                    (-6959, 6526),
                    (-16315, -23274)
                ));
            var cur = orig;
            for (int i = 1; i < 10; i++)
            {
                orig.Pow(i).Should().Be(cur);
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
            ).Determinant().Should().Be(-57);
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
            inv.Should().Be(new FractionMatrix2x2(
                (12, -9),
                (7, -10)
            ) * new Fraction(1, 57));
            (orig * inv).Should().Be(FractionMatrix2x2.MultiplicativeIdentity);
            (inv * orig).Should().Be(FractionMatrix2x2.MultiplicativeIdentity);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void AsSpan()
        {
            var mat = new LongMatrix2x2(
                (1, 2),
                (3, 4)
            );
            mat.AsSpan().ToArray().Should().Equal([
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
            mat.AsSpan().ToArray().Should().Equal([
                (UInt24)1, (UInt24)2,
                (UInt24)3, (UInt24)4
            ]);
        }
    }
}