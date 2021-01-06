using FluentAssertions;
using Xunit;

namespace AtCoder.MathNS.Matrix
{
    public class ArrayMatrixTests
    {
        [Fact]
        [Trait("Category", "Normal")]
        public void Construct()
        {
            new ArrayMatrix<int, IntOperator>(new int[][]
            {
                new int[]{ 1, 2, 3 },
                new int[]{ 4, 5, 6 },
            }).Value.Should().BeEquivalentTo(new int[][]
            {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
            });
            new ArrayMatrix<int, IntOperator>(new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }).Value.Should().BeEquivalentTo(new int[][]
            {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
            });
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            (-new ArrayMatrix<int, IntOperator>(new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            })).Value.Should().BeEquivalentTo(new int[][]
            {
                new int[] { -1, -2, -3 },
                new int[] { -4, -5, -6 },
            });
        }

        public static TheoryData Add_Data = new TheoryData<ArrayMatrix<int, IntOperator>, ArrayMatrix<int, IntOperator>, ArrayMatrix<int, IntOperator>>
        {
            {
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Zero),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Identity),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Identity)
            },
            {
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                }),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 2, 2, 2 },
                    { 2, 2, 2 },
                }),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 3, 4, 5 },
                    { 6, 7, 8 },
                })
            },
            {
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Zero),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                })
            },
            {
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Identity),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 2, 2 },
                    { 3, 5 },
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void Add(ArrayMatrix<int, IntOperator> mat1, ArrayMatrix<int, IntOperator> mat2, ArrayMatrix<int, IntOperator> expected)
        {
            (mat1 + mat2).Value.Should().BeEquivalentTo(expected.Value);
            (mat2 + mat1).Value.Should().BeEquivalentTo(expected.Value);
            default(ArrayMatrixOperator<int, IntOperator>).Add(mat1, mat2).Value.Should().BeEquivalentTo(expected.Value);
            default(ArrayMatrixOperator<int, IntOperator>).Add(mat2, mat1).Value.Should().BeEquivalentTo(expected.Value);
        }

        public static TheoryData Subtract_Data = new TheoryData<ArrayMatrix<int, IntOperator>, ArrayMatrix<int, IntOperator>, ArrayMatrix<int, IntOperator>>
        {
            {
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Identity),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Zero),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Identity)
            },
            {
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                }),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 2, 2, 2 },
                    { 2, 2, 2 },
                }),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { -1, 0, 1 },
                    { 2, 3, 4 },
                })
            },
            {
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Zero),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                })
            },
            {
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Identity),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 0, 2 },
                    { 3, 3 },
                })
            },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Subtract_Data))]
        public void Subtract(ArrayMatrix<int, IntOperator> mat1, ArrayMatrix<int, IntOperator> mat2, ArrayMatrix<int, IntOperator> expected)
        {
            (mat1 - mat2).Value.Should().BeEquivalentTo(expected.Value);
            default(ArrayMatrixOperator<int, IntOperator>).Subtract(mat1, mat2).Value.Should().BeEquivalentTo(expected.Value);
        }

        public static TheoryData Multiply_Data = new TheoryData<ArrayMatrix<int, IntOperator>, ArrayMatrix<int, IntOperator>, ArrayMatrix<int, IntOperator>>
        {
            {
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Identity),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Zero),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Zero)
            },
            {
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                }),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                    { 5, 6 },
                }),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 22, 28 },
                    { 49, 64 },
                })
            },
            {
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Zero),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 0, 0 },
                    { 0, 0 },
                })
            },
            {
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                new ArrayMatrix<int, IntOperator>(ArrayMatrixKind.Identity),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(ArrayMatrix<int, IntOperator> mat1, ArrayMatrix<int, IntOperator> mat2, ArrayMatrix<int, IntOperator> expected)
        {
            (mat1 * mat2).Value.Should().BeEquivalentTo(expected.Value);
            default(ArrayMatrixOperator<int, IntOperator>).Multiply(mat1, mat2).Value.Should().BeEquivalentTo(expected.Value);
        }


        public static TheoryData MultiplyScalar_Data = new TheoryData<int, ArrayMatrix<int, IntOperator>, ArrayMatrix<int, IntOperator>>
        {
            {
                2,
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                }),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 2, 4, 6 },
                    { 8, 10, 12 },
                })
            },
            {
                3,
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                new ArrayMatrix<int, IntOperator>(new int[,]
                {
                    { 3, 6 },
                    { 9, 12 },
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyScalar_Data))]
        public void MultiplyScalar(int a, ArrayMatrix<int, IntOperator> mat, ArrayMatrix<int, IntOperator> expected)
        {
            (a * mat).Value.Should().BeEquivalentTo(expected.Value);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = new ArrayMatrix<int, IntOperator>(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });
            orig.Pow(5).Value.Should().BeEquivalentTo(new int[][]
            {
                new[]{ 1069, 1558},
                new[]{ 2337, 3406 },
            });
            var cur = orig;
            for (int i = 1; i < 10; i++)
            {
                orig.Pow(i).Value.Should().BeEquivalentTo(cur.Value);
                cur *= orig;
            }
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Inv()
        {
            var orig = new ArrayMatrix<Fraction, FractionOperator>(new Fraction[,]
            {
                {10, -9, -12},
                {7, -12, 11},
                {-10, 10, 3}
            });
            var inv = orig.Inv();
            inv.Value.Should().BeEquivalentTo(new Fraction[][]
            {
                new[]{ new Fraction(-146,319), new Fraction(-93,319), new Fraction(-243,319)},
                new[]{ new Fraction(-131,319), new Fraction(-90,319), new Fraction(-194,319)},
                new[]{ new Fraction(-50,319), new Fraction(-10,319), new Fraction(-57,319)},
            });
            var id = new Fraction[][]
            {
                new Fraction[]{1,0,0},
                new Fraction[]{0,1,0},
                new Fraction[]{0,0,1},
            };
            (orig * inv).Value.Should().BeEquivalentTo(id);
            (inv * orig).Value.Should().BeEquivalentTo(id);
        }
    }
}