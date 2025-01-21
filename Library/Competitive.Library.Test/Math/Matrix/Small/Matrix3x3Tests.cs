
namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class Matrix3x3Tests
    {
        [Fact]
        public void Property()
        {
            var mat = new LongMatrix3x3((1, 2, 3), (4, 5, 6), (7, 8, 9));
            mat.Row0.ShouldBe((1, 2, 3));
            mat.Row1.ShouldBe((4, 5, 6));
            mat.Row2.ShouldBe((7, 8, 9));
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            (-new LongMatrix3x3(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            )).ShouldBe(new LongMatrix3x3(
                (-1, -2, -3),
                (-5, -6, -7),
                (-9, -10, -11)
            ));
        }

        public static TheoryData<LongMatrix3x3, LongMatrix3x3, LongMatrix3x3> Add_Data => new()
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
            (mat1 + mat2).ShouldBe(expected);
            (mat2 + mat1).ShouldBe(expected);
        }

        public static TheoryData<LongMatrix3x3, LongMatrix3x3, LongMatrix3x3> Subtract_Data => new()
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
            (mat1 - mat2).ShouldBe(expected);
        }

        public static TheoryData<LongMatrix3x3, LongMatrix3x3, LongMatrix3x3> Multiply_Data => new()
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
            (mat1 * mat2).ShouldBe(expected);
        }

        public static TheoryData<long, LongMatrix3x3, LongMatrix3x3> MultiplyScalar_Data => new()
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
            (mat * a).ShouldBe(expected);
        }

        public static TheoryData<LongMatrix3x3, SerializableTuple<long, long, long>, SerializableTuple<long, long, long>> MultiplyVector_Data => new()
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
        public void MultiplyVector(LongMatrix3x3 mat, SerializableTuple<long, long, long> vector, SerializableTuple<long, long, long> expected)
        {
            Inner(mat, vector, expected);
            static void Inner(LongMatrix3x3 mat, (long, long, long) vector, (long, long, long) expected)
            {
                (mat * vector).ShouldBe(expected);
                mat.Multiply(vector).ShouldBe(expected);
                mat.Multiply(vector.Item1, vector.Item2, vector.Item3).ShouldBe(expected);
            }
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
            orig.Pow(5).ShouldBe(new LongMatrix3x3(
                    (1825, 2162, 2499),
                    (4847, 5742, 6637),
                    (7869, 9322, 10775)
                ) * 144);
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
            new FractionMatrix3x3(
                (10, -9, -12),
                (7, -12, 11),
                (-10, 10, 3)
            ).Determinant().ShouldBe(319);
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
            inv.ShouldBe(new FractionMatrix3x3(
                (new Fraction(-146, 319), new Fraction(-93, 319), new Fraction(-243, 319)),
                (new Fraction(-131, 319), new Fraction(-90, 319), new Fraction(-194, 319)),
                (new Fraction(-50, 319), new Fraction(-10, 319), new Fraction(-57, 319))
            ));
            (orig * inv).ShouldBe(FractionMatrix3x3.MultiplicativeIdentity);
            (inv * orig).ShouldBe(FractionMatrix3x3.MultiplicativeIdentity);
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
            mat.AsSpan().ToArray().ShouldBe([
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
            mat.AsSpan().ToArray().ShouldBe([
                (UInt24)1, (UInt24)2, (UInt24)3,
                (UInt24)4, (UInt24)5, (UInt24)6,
                (UInt24)7, (UInt24)8, (UInt24)9
            ]);
        }
    }
}