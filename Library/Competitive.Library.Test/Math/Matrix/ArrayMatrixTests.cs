using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class ArrayMatrixTests
    {
        [Fact]
        [Trait("Category", "Normal")]
        public void Construct()
        {
            new ArrayMatrix<int>(new int[][]
            {
                new int[]{ 1, 2, 3 },
                new int[]{ 4, 5, 6 },
            }).ToArray().Should().BeEquivalentTo(new int[][]
            {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
            });
            new ArrayMatrix<int>(new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }).ToArray().Should().BeEquivalentTo(new int[][]
            {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
            });
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Equal()
        {
            new ArrayMatrix<int>(new int[][]
            {
                new int[]{ 1, 2, 3 },
                new int[]{ 4, 5, 6 },
            }).Should().Be(new ArrayMatrix<int>(new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }));
            new ArrayMatrix<int>(new int[][]
            {
                new int[]{ 1, 2, 3 },
                new int[]{ 4, 5, 6 },
            }).Should().Be(new ArrayMatrix<int>(new int[] { 1, 2, 3, 4, 5, 6 }, 2, 3));
            new ArrayMatrix<int>(new int[][]
            {
                new int[]{ 1, 2, },
                new int[]{ 3, 4, },
                new int[]{ 5, 6, },
            }).Should().NotBe(new ArrayMatrix<int>(new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }));
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            (-new ArrayMatrix<int>(new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            })).ToArray().Should().BeEquivalentTo(new int[][]
            {
                new int[] { -1, -2, -3 },
                new int[] { -4, -5, -6 },
            });
        }

        public static TheoryData Add_Data => new TheoryData<ArrayMatrix<int>, ArrayMatrix<int>, ArrayMatrix<int>>
        {
            {
                ArrayMatrix<int>.Zero,
                ArrayMatrix<int>.Identity,
                ArrayMatrix<int>.Identity
            },
            {
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                }),
                new ArrayMatrix<int>(new int[,]
                {
                    { 2, 2, 2 },
                    { 2, 2, 2 },
                }),
                new ArrayMatrix<int>(new int[,]
                {
                    { 3, 4, 5 },
                    { 6, 7, 8 },
                })
            },
            {
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                ArrayMatrix<int>.Zero,
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                })
            },
            {
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                ArrayMatrix<int>.Identity,
                new ArrayMatrix<int>(new int[,]
                {
                    { 2, 2 },
                    { 3, 5 },
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void Add(ArrayMatrix<int> mat1, ArrayMatrix<int> mat2, ArrayMatrix<int> expected)
        {
            (mat1 + mat2).Should().Be(expected);
            (mat2 + mat1).Should().Be(expected);
        }

        public static TheoryData Subtract_Data => new TheoryData<ArrayMatrix<int>, ArrayMatrix<int>, ArrayMatrix<int>>
        {
            {
                ArrayMatrix<int>.Identity,
                ArrayMatrix<int>.Zero,
                ArrayMatrix<int>.Identity
            },
            {
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                }),
                new ArrayMatrix<int>(new int[,]
                {
                    { 2, 2, 2 },
                    { 2, 2, 2 },
                }),
                new ArrayMatrix<int>(new int[,]
                {
                    { -1, 0, 1 },
                    { 2, 3, 4 },
                })
            },
            {
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                ArrayMatrix<int>.Zero,
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                })
            },
            {
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                ArrayMatrix<int>.Identity,
                new ArrayMatrix<int>(new int[,]
                {
                    { 0, 2 },
                    { 3, 3 },
                })
            },
            {
                ArrayMatrix<int>.Zero,
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                new ArrayMatrix<int>(new int[,]
                {
                    { -1, -2 },
                    { -3, -4 },
                })
            },
            {
                ArrayMatrix<int>.Identity,
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                new ArrayMatrix<int>(new int[,]
                {
                    { 0,  -2 },
                    { -3, -3 },
                })
            },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Subtract_Data))]
        public void Subtract(ArrayMatrix<int> mat1, ArrayMatrix<int> mat2, ArrayMatrix<int> expected)
        {
            (mat1 - mat2).Should().Be(expected);
        }

        public static TheoryData Multiply_Data => new TheoryData<ArrayMatrix<int>, ArrayMatrix<int>, ArrayMatrix<int>>
        {
            {
                ArrayMatrix<int>.Identity,
                ArrayMatrix<int>.Zero,
                ArrayMatrix<int>.Zero
            },
            {
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                }),
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                    { 5, 6 },
                }),
                new ArrayMatrix<int>(new int[,]
                {
                    { 22, 28 },
                    { 49, 64 },
                })
            },
            {
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                ArrayMatrix<int>.Zero,
                new ArrayMatrix<int>(new int[,]
                {
                    { 0, 0 },
                    { 0, 0 },
                })
            },
            {
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                ArrayMatrix<int>.Identity,
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(ArrayMatrix<int> mat1, ArrayMatrix<int> mat2, ArrayMatrix<int> expected)
        {
            (mat1 * mat2).Should().Be(expected);
        }

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void MultiplyModInt(ArrayMatrix<int> matInt1, ArrayMatrix<int> matInt2, ArrayMatrix<int> expectedInt)
        {
            var mat1 = Int2ModInt(matInt1);
            var mat2 = Int2ModInt(matInt2);
            var expected = Int2ModInt(expectedInt);
            (mat1 * mat2).Should().Be(expected);
            mat1.Strassen(mat2).Should().Be(expected);
        }

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void MultiplyMontgomeryModInt(ArrayMatrix<int> matInt1, ArrayMatrix<int> matInt2, ArrayMatrix<int> expectedInt)
        {
            var mat1 = Int2MontgomeryModInt(matInt1);
            var mat2 = Int2MontgomeryModInt(matInt2);
            var expected = Int2MontgomeryModInt(expectedInt);
            (mat1 * mat2).Should().Be(expected);
            mat1.Strassen(mat2).Should().Be(expected);
        }


        static ArrayMatrix<StaticModInt<Mod1000000007>> Int2ModInt(ArrayMatrix<int> mat)
           => mat.kind switch
           {
               ArrayMatrixKind.Normal
                   => new(mat.ToArray().Select(arr => arr.Select(n => (StaticModInt<Mod1000000007>)n).ToArray()).ToArray()),
               _ => new(mat.kind),
           };

        static ArrayMatrix<MontgomeryModInt<Mod1000000007>> Int2MontgomeryModInt(ArrayMatrix<int> mat)
           => mat.kind switch
           {
               ArrayMatrixKind.Normal
                   => new(mat.ToArray().Select(arr => arr.Select(n => (MontgomeryModInt<Mod1000000007>)n).ToArray()).ToArray()),
               _ => new(mat.kind),
           };


        public static TheoryData MultiplyScalar_Data => new TheoryData<int, ArrayMatrix<int>, ArrayMatrix<int>>
        {
            {
                2,
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                }),
                new ArrayMatrix<int>(new int[,]
                {
                    { 2, 4, 6 },
                    { 8, 10, 12 },
                })
            },
            {
                3,
                new ArrayMatrix<int>(new int[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                }),
                new ArrayMatrix<int>(new int[,]
                {
                    { 3, 6 },
                    { 9, 12 },
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyScalar_Data))]
        public void MultiplyScalar(int a, ArrayMatrix<int> mat, ArrayMatrix<int> expected)
        {
            (mat * a).Should().BeEquivalentTo(expected);
        }

        public static TheoryData MultiplyVector_Data => new TheoryData<ArrayMatrix<long>, long[], long[]>
        {
            {
                new ArrayMatrix<long>(new long[,]{
                    {3, 0, 0, 0},
                    {0, 3, 0, 0},
                    {0, 0, 3, 0},
                    {0, 0, 0, 3}
                }),
                new long[]{1,2,3,4},
                new long[]{3,6,9,12}
            },
            {
                new ArrayMatrix<long>(new long[,]{
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
        public void MultiplyVector(ArrayMatrix<long> mat, long[] vector, long[] expected)
        {
            (mat * vector).Should().Equal(expected);
            mat.Multiply(vector).Should().Equal(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = new ArrayMatrix<int>(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });
            orig.Pow(5).ToArray().Should().BeEquivalentTo(new int[][]
            {
                new[]{ 1069, 1558},
                new[]{ 2337, 3406 },
            });
            var cur = orig;
            for (int i = 1; i < 10; i++)
            {
                orig.Pow(i).Should().BeEquivalentTo(cur);
                cur *= orig;
            }
        }

        public static TheoryData Determinant_Data => new TheoryData<Fraction[,], Fraction>
        {
            {
                new Fraction[,]
                {
                    {10, -9},
                    { 7, -12},
                },
                -57
            },
            {
                new Fraction[,]
                {
                    {10, -9, -12},
                    {7, -12, 11},
                    {-10, 10, 3},
                },
                319
            },
            {
                new Fraction[,]
                {
                    {10, -9, -12, 6 },
                    {7, -12, 11, 15},
                    {1, 0, 2, 9},
                    {-10, 10, 3, 13},
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
                    {-1, 3, 2,-2,2},
                },
                -881
            }
        };

        [Theory]
        [MemberData(nameof(Determinant_Data))]
        [Trait("Category", "Normal")]
        public void Determinant(Fraction[,] array, Fraction expected)
        {
            new ArrayMatrix<Fraction>(array).Determinant().Should().Be(expected);
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
                new ArrayMatrix<Fraction>(array).Determinant().Should().Be(-(n - 2));
            }
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Determinant3()
        {
            int n = 60;
            var array = new Fraction[n, n];
            for (int i = 0; i < n; i++)
            {
                array[i, 0] = array[0, i] = array[i, i] = 1;
            }
            new ArrayMatrix<Fraction>(array).Determinant().Should().Be(-(n - 2));
        }

        public static IEnumerable<(Fraction[,] array, int i, int j, Fraction expected)> Cofactor_Data()
        {
            {
                var mt = new Fraction[,]
                {
                    {10, -9},
                    { 7, -12},
                };

                yield return (mt, 0, 0, -12);
                yield return (mt, 0, 1, -7);
                yield return (mt, 1, 0, 9);
                yield return (mt, 1, 1, 10);
            }
            {
                var mt = new Fraction[,]
                {
                    {10, -9, -12},
                    {7, -12, 11},
                    {-10, 10, 3},
                };

                yield return (mt, 0, 0, -146);
                yield return (mt, 0, 1, -131);
                yield return (mt, 0, 2, -50);
                yield return (mt, 1, 0, -93);
                yield return (mt, 1, 1, -90);
                yield return (mt, 1, 2, -10);
                yield return (mt, 2, 0, -243);
                yield return (mt, 2, 1, -194);
                yield return (mt, 2, 2, -57);
            }
        }

        [Theory]
        [TupleMemberData(nameof(Cofactor_Data))]
        [Trait("Category", "Normal")]
        public void Cofactor(Fraction[,] array, int i, int j, Fraction expected)
        {
            new ArrayMatrix<Fraction>(array).Cofactor(i, j).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Inv()
        {
            var orig = new ArrayMatrix<Fraction>(new Fraction[,]
            {
                {10, -9, -12},
                {7, -12, 11},
                {-10, 10, 3}
            });
            var inv = orig.Inv();
            inv.ToArray().Should().BeEquivalentTo(new Fraction[][]
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
            (orig * inv).ToArray().Should().BeEquivalentTo(id);
            (inv * orig).ToArray().Should().BeEquivalentTo(id);
        }

        public static TheoryData GaussianElimination_Data => new TheoryData<
            ArrayMatrix<Fraction>,
            ArrayMatrix<Fraction>>
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
                    { 1,2,3,0 },
                    { 0,0,0,1 },
                    { 0,0,0,0 },
                })
            },
        };

        [Theory]
        [MemberData(nameof(GaussianElimination_Data))]
        public void GaussianElimination(ArrayMatrix<Fraction> orig, ArrayMatrix<Fraction> expected)
        {
            var got = orig.GaussianElimination();
            got.Should().Be(expected);
        }

        public static TheoryData LinearSystem_Data => new TheoryData<
            ArrayMatrix<Fraction>,
            Fraction[],
            Fraction[][]>
        {
            {
                new(new Fraction[2,3]{
                    { 1,-4,3 },
                    { 3,2,2 },
                }),
                new Fraction[2] { 7, 2 },
                new Fraction[][]
                {
                    new Fraction[]{ new(11,7), new(-19,14), 0 },
                    new Fraction[]{-1, new(1,2), 1},
                }
            },
            {
                new(new Fraction[2,2]{
                    { 1,2 },
                    { 2,4 },
                }),
                new Fraction[2] { 1, 2 },
                new Fraction[][]
                {
                    new Fraction[]{ 1, 0 },
                    new Fraction[]{-2, 1 },
                }
            },
            {
                new(new Fraction[2,2]{
                    { 1,2 },
                    { 2,4 },
                }),
                new Fraction[2] { 1, 3 },
                new Fraction[0][]
            },
            {
                new(new Fraction[2,2]{
                    { 0,0 },
                    { 0,0 },
                }),
                new Fraction[2] { 0, 0 },
                new Fraction[][]
                {
                    new Fraction[]{ 0, 0 },
                    new Fraction[]{ 1, 0 },
                    new Fraction[]{ 0, 1 },
                }
            },
        };

        [Theory]
        [MemberData(nameof(LinearSystem_Data))]
        public void LinearSystem(ArrayMatrix<Fraction> matrix, Fraction[] vector, Fraction[][] expected)
        {
            var got = matrix.LinearSystem(vector);
            got.Should().HaveSameCount(expected);
            for (int i = 0; i < got.Length; i++)
                got[i].Should().Equal(expected[i], because: "got[{0}]", i);
        }
    }
}