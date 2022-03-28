using AtCoder;
using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    // verification-helper: SAMEAS Library/run.test.py
    public class Matrix2x2Tests
    {
        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            (-new Matrix2x2<long, LongOperator>(
                (1, 2),
                (5, 6)
            )).Should().Be(new Matrix2x2<long, LongOperator>(
                (-1, -2),
                (-5, -6)
            ));
        }

        public static TheoryData Add_Data = new TheoryData<Matrix2x2<long, LongOperator>, Matrix2x2<long, LongOperator>, Matrix2x2<long, LongOperator>>
        {
            {
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                ),
                Matrix2x2<long, LongOperator>.Identity,
                new Matrix2x2<long, LongOperator>(
                    (2, 2),
                    (5, 7)
                )
            },
            {
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                ),
                new Matrix2x2<long, LongOperator>(
                    (1, -2),
                    (5, -6)
                ),
                new Matrix2x2<long, LongOperator>(
                    (2, 0),
                    (10, 0)
                )
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void Add(Matrix2x2<long, LongOperator> mat1, Matrix2x2<long, LongOperator> mat2, Matrix2x2<long, LongOperator> expected)
        {
            (mat1 + mat2).Should().Be(expected);
            (mat2 + mat1).Should().Be(expected);
            default(Matrix2x2<long, LongOperator>.Operator).Add(mat1, mat2).Should().Be(expected);
            default(Matrix2x2<long, LongOperator>.Operator).Add(mat2, mat1).Should().Be(expected);
        }

        public static TheoryData Subtract_Data = new TheoryData<Matrix2x2<long, LongOperator>, Matrix2x2<long, LongOperator>, Matrix2x2<long, LongOperator>>
        {
            {
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                ),
                new Matrix2x2<long, LongOperator>(
                    (1, -2),
                    (5, -6)
                ),
                new Matrix2x2<long, LongOperator>(
                    (0, 4),
                    (0, 12)
                )
            },
            {
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                ),
                Matrix2x2<long, LongOperator>.Identity,
                new Matrix2x2<long, LongOperator>(
                    (0, 2),
                    (5, 5)
                )
            },
            {
                Matrix2x2<long, LongOperator>.Identity,
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                ),
                new Matrix2x2<long, LongOperator>(
                    (0, -2),
                    (-5, -5)
                )
            },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Subtract_Data))]
        public void Subtract(Matrix2x2<long, LongOperator> mat1, Matrix2x2<long, LongOperator> mat2, Matrix2x2<long, LongOperator> expected)
        {
            (mat1 - mat2).Should().Be(expected);
            default(Matrix2x2<long, LongOperator>.Operator).Subtract(mat1, mat2).Should().Be(expected);
        }

        public static TheoryData Multiply_Data = new TheoryData<Matrix2x2<long, LongOperator>, Matrix2x2<long, LongOperator>, Matrix2x2<long, LongOperator>>
        {
            {
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                ),
                new Matrix2x2<long, LongOperator>(
                    (1, -2),
                    (5, -6)
                ),
                new Matrix2x2<long, LongOperator>(
                    (11, -14),
                    (35, -46)
                )
            },
            {
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                ),
                Matrix2x2<long, LongOperator>.Identity,
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                )
            },
            {
                Matrix2x2<long, LongOperator>.Identity,
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                ),
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                )
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(Matrix2x2<long, LongOperator> mat1, Matrix2x2<long, LongOperator> mat2, Matrix2x2<long, LongOperator> expected)
        {
            (mat1 * mat2).Should().Be(expected);
            default(Matrix2x2<long, LongOperator>.Operator).Multiply(mat1, mat2).Should().Be(expected);
        }

        public static TheoryData MultiplyScalar_Data = new TheoryData<long, Matrix2x2<long, LongOperator>, Matrix2x2<long, LongOperator>>
        {
            {
                3,
                Matrix2x2<long, LongOperator>.Identity,
                new Matrix2x2<long, LongOperator>(
                    (3, 0),
                    (0, 3)
                )
            },
            {
                3,
                new Matrix2x2<long, LongOperator>(
                    (1, 2),
                    (5, 6)
                ),
                new Matrix2x2<long, LongOperator>(
                    (3, 6),
                    (15, 18)
                )
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyScalar_Data))]
        public void MultiplyScalar(long a, Matrix2x2<long, LongOperator> mat, Matrix2x2<long, LongOperator> expected)
        {
            (a * mat).Should().Be(expected);
        }

        public static TheoryData MultiplyVector_Data = new TheoryData<Matrix2x2<long, LongOperator>, (long, long), (long, long)>
        {
            {
                new Matrix2x2<long, LongOperator>(
                    (3, 0),
                    (0, 3)
                ),
                (1,2),
                (3,6)
            },
            {
                new Matrix2x2<long, LongOperator>(
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
        public void MultiplyVector(Matrix2x2<long, LongOperator> mat, (long v0, long v1) vector, (long, long) expected)
        {
            (mat * vector).Should().Be(expected);
            mat.Multiply(vector).Should().Be(expected);
            mat.Multiply(vector.v0, vector.v1).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = new Matrix2x2<long, LongOperator>(
                    (1, -2),
                    (5, 6)
                );
            orig.Pow(7).Should().Be(new Matrix2x2<long, LongOperator>(
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
            new Matrix2x2<Fraction, FractionOperator>(
                (10, -9),
                (7, -12)
            ).Determinant().Should().Be(-57);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Inv()
        {
            var orig = new Matrix2x2<Fraction, FractionOperator>(
                (10, -9),
                (7, -12)
            );
            var inv = orig.Inv();
            inv.Should().Be(new Fraction(1, 57) * new Matrix2x2<Fraction, FractionOperator>(
                (12, -9),
                (7, -10)
            ));
            (orig * inv).Should().Be(Matrix2x2<Fraction, FractionOperator>.Identity);
            (inv * orig).Should().Be(Matrix2x2<Fraction, FractionOperator>.Identity);
        }
    }
}