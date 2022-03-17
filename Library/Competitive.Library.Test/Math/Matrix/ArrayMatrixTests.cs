using AtCoder;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.MathNS.Matrix
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
                ArrayMatrix<int, IntOperator>.Zero,
                ArrayMatrix<int, IntOperator>.Identity,
                ArrayMatrix<int, IntOperator>.Identity
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
                ArrayMatrix<int, IntOperator>.Zero,
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
                ArrayMatrix<int, IntOperator>.Identity,
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
                ArrayMatrix<int, IntOperator>.Identity,
                ArrayMatrix<int, IntOperator>.Zero,
                ArrayMatrix<int, IntOperator>.Identity
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
                ArrayMatrix<int, IntOperator>.Zero,
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
                ArrayMatrix<int, IntOperator>.Identity,
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
                ArrayMatrix<int, IntOperator>.Identity,
                ArrayMatrix<int, IntOperator>.Zero,
                ArrayMatrix<int, IntOperator>.Zero
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
                ArrayMatrix<int, IntOperator>.Zero,
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
                ArrayMatrix<int, IntOperator>.Identity,
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

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void MultiplyModInt(ArrayMatrix<int, IntOperator> matInt1, ArrayMatrix<int, IntOperator> matInt2, ArrayMatrix<int, IntOperator> expectedInt)
        {
            var mat1 = Int2ModInt(matInt1);
            var mat2 = Int2ModInt(matInt2);
            var expected = Int2ModInt(expectedInt);
            (mat1 * mat2).Value.Should().BeEquivalentTo(expected.Value);
            mat1.Strassen(mat2).Value.Should().BeEquivalentTo(expected.Value);
            default(ArrayMatrixOperator<StaticModInt<Mod1000000007>, StaticModIntOperator<Mod1000000007>>)
                .Multiply(mat1, mat2).Value.Should().BeEquivalentTo(expected.Value);
        }
        private static ArrayMatrix<StaticModInt<Mod1000000007>, StaticModIntOperator<Mod1000000007>> Int2ModInt(ArrayMatrix<int, IntOperator> mat)
            => mat.kind switch
            {
                ArrayMatrixKind.Normal
                    => new(mat.Value.Select(arr => arr.Select(n => new StaticModInt<Mod1000000007>(n)).ToArray()).ToArray()),
                _ => new ArrayMatrix<StaticModInt<Mod1000000007>, StaticModIntOperator<Mod1000000007>>(mat.kind),
            };

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

        public static TheoryData MultiplyVector_Data = new TheoryData<ArrayMatrix<long, LongOperator>, long[], long[]>
        {
            {
                new ArrayMatrix<long, LongOperator>(new long[,]{
                    {3, 0, 0, 0},
                    {0, 3, 0, 0},
                    {0, 0, 3, 0},
                    {0, 0, 0, 3}
                }),
                new long[]{1,2,3,4},
                new long[]{3,6,9,12}
            },
            {
                new ArrayMatrix<long, LongOperator>(new long[,]{
                    {1, 2, 3, 4},
                    {5, 6, 7, 8},
                    {9, 10, 11, 12},
                    {13, 14, 15, 16}
                }),
                new long[]{1,2,3,4},
                new long[]{30, 70, 110, 150}
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(ArrayMatrix<long, LongOperator> mat, long[] vector, long[] expected)
        {
            (mat * vector).Should().Equal(expected);
            mat.Multiply(vector).Should().Equal(expected);
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

        public static TheoryData Determinant_Data = new TheoryData<Fraction[,], Fraction>
        {
            {
                new Fraction[,]
                {
                    {10, -9},
                    { 7, -12}
                },
                -57
            },
            {
new Fraction[,]
{
                    {10, -9, -12},
                    {7, -12, 11},
                    {-10, 10, 3}
                },
                319
            },
            {
                new Fraction[,]
                {
                    {10, -9, -12, 6 },
                    {7, -12, 11, 15},
                    {1, 0, 2, 9},
                    {-10, 10, 3, 13}
                },
                -10683
            },
            {
                new Fraction[,]
                {
                    { 4, 6, 5,-2,2},
                    { 1,-2,-1, 2,4},
                    { 0, 3, 1, 4,3},
                    {-7, 5,-1, 3,5},
                    {-1, 3, 2,-2,2}
                },
                -881
            }
        };

        [Theory]
        [MemberData(nameof(Determinant_Data))]
        [Trait("Category", "Normal")]
        public void Determinant(Fraction[,] array, Fraction expected)
        {
            new ArrayMatrix<Fraction, FractionOperator>(array).Determinant().Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Determinant2()
        {
            for (int n = 3; n < 20; n++)
            {
                var array = new Fraction[n, n];
                for (int i = 0; i < n; i++)
                {
                    array[i, 0] = array[0, i] = array[i, i] = 1;
                }
                new ArrayMatrix<Fraction, FractionOperator>(array).Determinant().Should().Be(-(n - 2));
            }
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Determinant3()
        {
            int n = 300;
            var array = new Fraction[n, n];
            for (int i = 0; i < n; i++)
            {
                array[i, 0] = array[0, i] = array[i, i] = 1;
            }
            new ArrayMatrix<Fraction, FractionOperator>(array).Determinant().Should().Be(-(n - 2));
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

        public static TheoryData GaussianElimination_Data = new TheoryData<ArrayMatrix<Fraction, FractionOperator>, ArrayMatrix<Fraction, FractionOperator>>
        {
            {
                new(new Fraction[2,3]{
                    { 1,-4,3 },
                    { 3,2,2 },
                }),
                new(new Fraction[2,3]{
                    { 1,0,new(1,1) },
                    { 0,1,new(-1,2) },
                })
            },
            {
                new(new Fraction[5,3]{
                    { 1,-4,3 },
                    { 3,2,2 },
                    { 1,2,0 },
                    { 1,0,1 },
                    { 0,-1,new(1,2) },
                }),
                new(new Fraction[5,3]{
                    { 1,0,new(1,1) },
                    { 0,1,new(-1,2) },
                    { 0,0,0 },
                    { 0,0,0 },
                    { 0,0,0 },
                })
            },
            {
                new(new Fraction[3,3]{
                    { 1,-4,3 },
                    { 3,2,2 },
                    { 1,0,0 },
                }),
                new(new Fraction[3,3]{
                    { 1,0,0 },
                    { 0,1,0 },
                    { 0,0,1 },
                })
            },
            {
                new(new Fraction[3,4]{
                    { 1,-1,2,0 },
                    { 4,2,-3,2 },
                    { 1,0,1,1 },
                }),
                new(new Fraction[3,4]{
                    { 1,0,0,new(1,5) },
                    { 0,1,0,new(9,5) },
                    { 0,0,1,new(4,5) },
                })
            },
            {
                new(new Fraction[3,4]{
                    { 1,2,3,4 },
                    { 1,2,3,4 },
                    { 1,2,3,4 },
                }),
                new(new Fraction[3,4]{
                    { 1,2,3,4 },
                    { 0,0,0,0 },
                    { 0,0,0,0 },
                })
            },
            {
                new(new Fraction[3,4]{
                    { 1,2,3,4 },
                    { 1,2,3,5 },
                    { 1,2,3,4 },
                }),
                new(new Fraction[3,4]{
                    { 1,2,3,4 },
                    { 0,0,0,1 },
                    { 0,0,0,0 },
                })
            },
        };

        [Theory]
        [MemberData(nameof(GaussianElimination_Data))]
        public void GaussianElimination(ArrayMatrix<Fraction, FractionOperator> orig, ArrayMatrix<Fraction, FractionOperator> expected)
        {
            var got = orig.GaussianElimination();
            got.Value.Should().HaveSameCount(expected.Value);
            for (int i = 0; i < got.Value.Length; i++)
                got.Value[i].Should().Equal(expected.Value[i], because: "row {0}", i);
        }
    }
}