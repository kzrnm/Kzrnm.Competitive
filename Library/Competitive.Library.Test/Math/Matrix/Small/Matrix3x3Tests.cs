
namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class Matrix3x3Tests
    {
        [Fact]
        public void Property()
        {
            var mat = new LongMatrix3x3((1, 2, 3), (4, 5, 6), (7, 8, 9));
            mat.Row0.Should().Be((1, 2, 3));
            mat.Row1.Should().Be((4, 5, 6));
            mat.Row2.Should().Be((7, 8, 9));
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            (-new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            )).Should().Be(new LongMatrix3x3(
                (-1, -2, -3),
                (-5, -6, -7),
                (-9, -10, -11)
            ));
        }

        public static TheoryData Add_Data => new TheoryData<LongMatrix3x3, LongMatrix3x3, LongMatrix3x3>
        {
            {
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
            },
            {
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
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void Add(LongMatrix3x3 mat1, LongMatrix3x3 mat2, LongMatrix3x3 expected)
        {
            (mat1 + mat2).Should().Be(expected);
            (mat2 + mat1).Should().Be(expected);
        }

        public static TheoryData Subtract_Data => new TheoryData<LongMatrix3x3, LongMatrix3x3, LongMatrix3x3>
        {
            {
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
            },
            {
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
            },
            {
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
            },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Subtract_Data))]
        public void Subtract(LongMatrix3x3 mat1, LongMatrix3x3 mat2, LongMatrix3x3 expected)
        {
            (mat1 - mat2).Should().Be(expected);
        }

        public static TheoryData Multiply_Data => new TheoryData<LongMatrix3x3, LongMatrix3x3, LongMatrix3x3>
        {
            {
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
            },
            {
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
            },
            {
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
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(LongMatrix3x3 mat1, LongMatrix3x3 mat2, LongMatrix3x3 expected)
        {
            (mat1 * mat2).Should().Be(expected);
        }

        public static TheoryData MultiplyScalar_Data => new TheoryData<long, LongMatrix3x3, LongMatrix3x3>
        {
            {
                3,
                LongMatrix3x3.MultiplicativeIdentity,
                new LongMatrix3x3(
                    (3, 0, 0),
                    (0, 3, 0),
                    (0, 0, 3)
                )
            },
            {
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
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyScalar_Data))]
        public void MultiplyScalar(long a, LongMatrix3x3 mat, LongMatrix3x3 expected)
        {
            (mat * a).Should().Be(expected);
        }

        public static TheoryData MultiplyVector_Data => new TheoryData<LongMatrix3x3, (long, long, long), (long, long, long)>
        {
            {
                new LongMatrix3x3(
                    (3, 0, 0),
                    (0, 3, 0),
                    (0, 0, 3)
                ),
                (1,2,3),
                (3,6,9)
            },
            {
                new LongMatrix3x3(
                    (1, 2, 3),
                    (4, 5, 6),
                    (7, 8, 9)
                ),
                (1,2,3),
                (14, 32, 50)
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(LongMatrix3x3 mat, (long v0, long v1, long v2) vector, (long, long, long) expected)
        {
            (mat * vector).Should().Be(expected);
            mat.Multiply(vector).Should().Be(expected);
            mat.Multiply(vector.v0, vector.v1, vector.v2).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = new LongMatrix3x3(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                );
            orig.Pow(5).Should().Be(new LongMatrix3x3(
                    (1825, 2162, 2499),
                    (4847, 5742, 6637),
                    (7869, 9322, 10775)
                ) * 144);
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
            new FractionMatrix3x3(
                (10, -9, -12),
                (7, -12, 11),
                (-10, 10, 3)
            ).Determinant().Should().Be(319);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Inv()
        {
            var orig = new FractionMatrix3x3(
                (10, -9, -12),
                (7, -12, 11),
                (-10, 10, 3)
            );
            var inv = orig.Inv();
            inv.Should().Be(new FractionMatrix3x3(
                (new Fraction(-146, 319), new Fraction(-93, 319), new Fraction(-243, 319)),
                (new Fraction(-131, 319), new Fraction(-90, 319), new Fraction(-194, 319)),
                (new Fraction(-50, 319), new Fraction(-10, 319), new Fraction(-57, 319))
            ));
            (orig * inv).Should().Be(FractionMatrix3x3.MultiplicativeIdentity);
            (inv * orig).Should().Be(FractionMatrix3x3.MultiplicativeIdentity);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void AsSpan()
        {
            var mat = new LongMatrix3x3(
                (1, 2, 3),
                (4, 5, 6),
                (7, 8, 9)
            );
            mat.AsSpan().ToArray().Should().Equal([
                1, 2, 3,
                4, 5, 6,
                7, 8, 9
            ]);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void AsSpan3Bytes()
        {
            var mat = new Matrix3x3<UInt24>(
                ((UInt24)1, (UInt24)2, (UInt24)3),
                ((UInt24)4, (UInt24)5, (UInt24)6),
                ((UInt24)7, (UInt24)8, (UInt24)9)
            );
            mat.AsSpan().ToArray().Should().Equal([
                (UInt24)1, (UInt24)2, (UInt24)3,
                (UInt24)4, (UInt24)5, (UInt24)6,
                (UInt24)7, (UInt24)8, (UInt24)9
            ]);
        }
    }
}