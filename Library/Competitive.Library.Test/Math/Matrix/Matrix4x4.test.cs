using AtCoder;
using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class Matrix4x4Tests
    {
        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            (-new Matrix4x4<long, LongOperator>(
                (1, 2, 3, 4),
                (5, 6, 7, 8),
                (9, 10, 11, 12),
                (13, 14, 15, 16)
            )).Should().Be(new Matrix4x4<long, LongOperator>(
                (-1, -2, -3, -4),
                (-5, -6, -7, -8),
                (-9, -10, -11, -12),
                (-13, -14, -15, -16)
            ));
        }

        public static TheoryData Add_Data = new TheoryData<Matrix4x4<long, LongOperator>, Matrix4x4<long, LongOperator>, Matrix4x4<long, LongOperator>>
        {
            {
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                ),
                Matrix4x4<long, LongOperator>.Identity,
                new Matrix4x4<long, LongOperator>(
                    (2, 2, 3, 4),
                    (5, 7, 7, 8),
                    (9, 10, 12, 12),
                    (13, 14, 15, 17)
                )
            },
            {
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                ),
                new Matrix4x4<long, LongOperator>(
                    (1, -2, 3, 4),
                    (5, -6, 7, 8),
                    (9, -10, 11, 12),
                    (13,-14, -15, -16)
                ),
                new Matrix4x4<long, LongOperator>(
                    (2, 0, 6, 8),
                    (10, 0, 14, 16),
                    (18, 0, 22, 24),
                    (26, 0, 0, 0)
                )
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void Add(Matrix4x4<long, LongOperator> mat1, Matrix4x4<long, LongOperator> mat2, Matrix4x4<long, LongOperator> expected)
        {
            (mat1 + mat2).Should().Be(expected);
            (mat2 + mat1).Should().Be(expected);
            default(Matrix4x4<long, LongOperator>.Operator).Add(mat1, mat2).Should().Be(expected);
            default(Matrix4x4<long, LongOperator>.Operator).Add(mat2, mat1).Should().Be(expected);
        }

        public static TheoryData Subtract_Data = new TheoryData<Matrix4x4<long, LongOperator>, Matrix4x4<long, LongOperator>, Matrix4x4<long, LongOperator>>
        {
            {
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                ),
                new Matrix4x4<long, LongOperator>(
                    (1, -2, 3, 4),
                    (5, -6, 7, 8),
                    (9, -10, 11, 12),
                    (13,-14, -15, -16)
                ),
                new Matrix4x4<long, LongOperator>(
                    (0, 4, 0, 0),
                    (0, 12, 0, 0),
                    (0, 20, 0, 0),
                    (0, 28, 30, 32)
                )
            },
            {
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                ),
                Matrix4x4<long, LongOperator>.Identity,
                new Matrix4x4<long, LongOperator>(
                    (0, 2, 3, 4),
                    (5, 5, 7, 8),
                    (9, 10, 10, 12),
                    (13, 14, 15, 15)
                )
            },
            {
                Matrix4x4<long, LongOperator>.Identity,
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                ),
                new Matrix4x4<long, LongOperator>(
                    (0, -2, -3, -4),
                    (-5, -5, -7, -8),
                    (-9, -10, -10, -12),
                    (-13, -14, -15, -15)
                )
            },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Subtract_Data))]
        public void Subtract(Matrix4x4<long, LongOperator> mat1, Matrix4x4<long, LongOperator> mat2, Matrix4x4<long, LongOperator> expected)
        {
            (mat1 - mat2).Should().Be(expected);
            default(Matrix4x4<long, LongOperator>.Operator).Subtract(mat1, mat2).Should().Be(expected);
        }

        public static TheoryData Multiply_Data = new TheoryData<Matrix4x4<long, LongOperator>, Matrix4x4<long, LongOperator>, Matrix4x4<long, LongOperator>>
        {
            {
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                ),
                new Matrix4x4<long, LongOperator>(
                    (1, -2, 3, 4),
                    (5, -6, 7, 8),
                    (9, -10, 11, 12),
                    (13,-14, -15, -16)
                ),
                new Matrix4x4<long, LongOperator>(
                    (90, -100, -10, -8),
                    (202, -228, 14, 24),
                    (314, -356, 38, 56),
                    (426, -484, 62, 88)
                )
            },
            {
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                ),
                Matrix4x4<long, LongOperator>.Identity,
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                )
            },
            {
                Matrix4x4<long, LongOperator>.Identity,
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                ),
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                )
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(Matrix4x4<long, LongOperator> mat1, Matrix4x4<long, LongOperator> mat2, Matrix4x4<long, LongOperator> expected)
        {
            (mat1 * mat2).Should().Be(expected);
            default(Matrix4x4<long, LongOperator>.Operator).Multiply(mat1, mat2).Should().Be(expected);
        }

        public static TheoryData MultiplyScalar_Data = new TheoryData<long, Matrix4x4<long, LongOperator>, Matrix4x4<long, LongOperator>>
        {
            {
                3,
                Matrix4x4<long, LongOperator>.Identity,
                new Matrix4x4<long, LongOperator>(
                    (3, 0, 0, 0),
                    (0, 3, 0, 0),
                    (0, 0, 3, 0),
                    (0, 0, 0, 3)
                )
            },
            {
                3,
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                ),
                new Matrix4x4<long, LongOperator>(
                    (3, 6, 9, 12),
                    (15, 18, 21, 24),
                    (27, 30, 33, 36),
                    (39, 42, 45, 48)
                )
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyScalar_Data))]
        public void MultiplyScalar(long a, Matrix4x4<long, LongOperator> mat, Matrix4x4<long, LongOperator> expected)
        {
            (a * mat).Should().Be(expected);
        }

        public static TheoryData MultiplyVector_Data = new TheoryData<Matrix4x4<long, LongOperator>, (long, long, long, long), (long, long, long, long)>
        {
            {
                new Matrix4x4<long, LongOperator>(
                    (3, 0, 0, 0),
                    (0, 3, 0, 0),
                    (0, 0, 3, 0),
                    (0, 0, 0, 3)
                ),
                (1,2,3,4),
                (3,6,9,12)
            },
            {
                new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                ),
                (1,2,3,4),
                (30, 70, 110, 150)
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(Matrix4x4<long, LongOperator> mat, (long v0, long v1, long v2, long v3) vector, (long, long, long, long) expected)
        {
            (mat * vector).Should().Be(expected);
            mat.Multiply(vector).Should().Be(expected);
            mat.Multiply(vector.v0, vector.v1, vector.v2, vector.v3).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = new Matrix4x4<long, LongOperator>(
                    (1, 2, 3, 4),
                    (5, 6, 7, 8),
                    (9, 10, 11, 12),
                    (13, 14, 15, 16)
                );
            orig.Pow(3).Should().Be(4 * new Matrix4x4<long, LongOperator>(
                    (785, 890, 995, 1100),
                    (1817, 2058, 2299, 2540),
                    (2849, 3226, 3603, 3980),
                    (3881, 4394, 4907, 5420)
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
            new Matrix4x4<Fraction, FractionOperator>(
                (10, -9, -12, 6),
                (7, -12, 11, 15),
                (1, 0, 2, 9),
                (-10, 10, 3, 13)
            ).Determinant().Should().Be(-10683);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Inv()
        {
            var orig = new Matrix4x4<Fraction, FractionOperator>(
                (10, -9, -12, 6),
                (7, -12, 11, 15),
                (1, 0, 2, 9),
                (-10, 10, 3, 13)
            );
            var inv = orig.Inv();
            inv.Should().Be(new Matrix4x4<Fraction, FractionOperator>(
                (new Fraction(-78, 1187), new Fraction(-397, 3561), new Fraction(1810, 3561), new Fraction(-229, 1187)),
                (new Fraction(-265, 3561), new Fraction(-1364, 10683), new Fraction(4658, 10683), new Fraction(-428, 3561)),
                (new Fraction(-84, 1187), new Fraction(29, 3561), new Fraction(397, 3561), new Fraction(-64, 1187)),
                (new Fraction(82, 3561), new Fraction(113, 10683), new Fraction(319, 10683), new Fraction(119, 3561))
            ));
            (orig * inv).Should().Be(Matrix4x4<Fraction, FractionOperator>.Identity);
            (inv * orig).Should().Be(Matrix4x4<Fraction, FractionOperator>.Identity);
        }
    }
}