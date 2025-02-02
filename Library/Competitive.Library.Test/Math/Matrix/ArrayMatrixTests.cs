using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class ArrayMatrixTests
{
    [Fact]
    [Trait("Category", "Normal")]
    public void Construct()
    {
        new ArrayMatrix<int>(
        [
            [1, 2, 3],
            [4, 5, 6],
        ]).ToArray().ShouldBe([
            [1, 2, 3],
            [4, 5, 6],
        ]);
        new ArrayMatrix<int>(new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
        }).ToArray().ShouldBe([
            [1, 2, 3],
            [4, 5, 6],
        ]);
    }

    [Fact]
    [Trait("Category", "Normal")]
    public void NormalIdentity()
    {
        ArrayMatrix<int>.NormalIdentity(2).ShouldBe(new ArrayMatrix<int>(
        [
            [1, 0],
            [0, 1],
        ]));
        ArrayMatrix<int>.NormalIdentity(3).ShouldBe(new ArrayMatrix<int>(
        [
            [1, 0, 0],
            [0, 1, 0],
            [0, 0, 1],
        ]));

        ArrayMatrix<MontgomeryModInt<Mod1000000007>>.NormalIdentity(3).ShouldBe(new ArrayMatrix<MontgomeryModInt<Mod1000000007>>(
        [
            [1, 0, 0],
            [0, 1, 0],
            [0, 0, 1],
        ]));
    }

    [Fact]
    [Trait("Category", "Normal")]
    public void Equal()
    {
        new ArrayMatrix<int>(
        [
            [1, 2, 3],
            [4, 5, 6],
        ]).ShouldBe(new ArrayMatrix<int>(new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
        }));
        new ArrayMatrix<int>(
        [
            [1, 2, 3],
            [4, 5, 6],
        ]).ShouldBe(new ArrayMatrix<int>([1, 2, 3, 4, 5, 6], 2, 3));
        new ArrayMatrix<int>(
        [
            [1, 2,],
            [3, 4,],
            [5, 6,],
        ]).ShouldNotBe(new ArrayMatrix<int>(new int[,]
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
        })).ToArray().ShouldBe([
            [-1, -2, -3],
            [-4, -5, -6],
        ]);
    }

    public static TheoryData<ArrayMatrix<int>, ArrayMatrix<int>, ArrayMatrix<int>> Add_Data => new()
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
        (mat1 + mat2).ShouldBe(expected);
        (mat2 + mat1).ShouldBe(expected);
    }

    public static TheoryData<ArrayMatrix<int>, ArrayMatrix<int>, ArrayMatrix<int>> Subtract_Data => new()
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
        (mat1 - mat2).ShouldBe(expected);
    }

    public static TheoryData<ArrayMatrix<int>, ArrayMatrix<int>, ArrayMatrix<int>> Multiply_Data => new()
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
        (mat1 * mat2).ShouldBe(expected);
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(Multiply_Data))]
    public void MultiplyModInt(ArrayMatrix<int> matInt1, ArrayMatrix<int> matInt2, ArrayMatrix<int> expectedInt)
    {
        var mat1 = Int2ModInt(matInt1);
        var mat2 = Int2ModInt(matInt2);
        var expected = Int2ModInt(expectedInt);
        (mat1 * mat2).ShouldBe(expected);
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(Multiply_Data))]
    public void MultiplyMontgomeryModInt(ArrayMatrix<int> matInt1, ArrayMatrix<int> matInt2, ArrayMatrix<int> expectedInt)
    {
        var mat1 = Int2MontgomeryModInt(matInt1);
        var mat2 = Int2MontgomeryModInt(matInt2);
        var expected = Int2MontgomeryModInt(expectedInt);
        (mat1 * mat2).ShouldBe(expected);
        mat1.Strassen(mat2).ShouldBe(expected);
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


    public static TheoryData<int, ArrayMatrix<int>, ArrayMatrix<int>> MultiplyScalar_Data => new()
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
        (mat * a).ShouldBe(expected);
    }

    public static TheoryData<ArrayMatrix<long>, long[], long[]> MultiplyVector_Data => new()
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
        (mat * vector).ShouldBe(expected);
        mat.Multiply(vector).ShouldBe(expected);
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
        orig.Pow(5).ToArray().ShouldBe([
            [1069, 1558],
            [2337, 3406],
        ]);
        var cur = orig;
        for (int i = 1; i < 10; i++)
        {
            orig.Pow(i).ShouldBe(cur);
            cur *= orig;
        }
    }

    public static TheoryData<Fraction[,], Fraction> Determinant_Data => new()
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
        new ArrayMatrix<Fraction>(array).Determinant().ShouldBe(expected);
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
            new ArrayMatrix<Fraction>(array).Determinant().ShouldBe(-(n - 2));
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
        new ArrayMatrix<Fraction>(array).Determinant().ShouldBe(-(n - 2));
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
    [MemberData(nameof(Cofactor_Data))]
    [Trait("Category", "Normal")]
    public void Cofactor(Fraction[,] array, int i, int j, Fraction expected)
    {
        new ArrayMatrix<Fraction>(array).Cofactor(i, j).ShouldBe(expected);
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
        inv.ToArray().ShouldBe([
            [new Fraction(-146,319), new Fraction(-93,319), new Fraction(-243,319)],
            [new Fraction(-131,319), new Fraction(-90,319), new Fraction(-194,319)],
            [new Fraction(-50,319), new Fraction(-10,319), new Fraction(-57,319)],
        ]);
        var id = new Fraction[][]
        {
            [1,0,0],
            [0,1,0],
            [0,0,1],
        };
        (orig * inv).ToArray().ShouldBe(id);
        (inv * orig).ToArray().ShouldBe(id);
    }


    public static TheoryData<ArrayMatrix<int>, ArrayMatrix<int>> Transpose_Data => new()
    {
        {
            new(new int[2,3]{
                { 1,-4,3 },
                { 3,2,2 },
            }),
            new(new int[3,2]{
                { 1,3 },
                {-4,2 },
                { 3,2 },
            })
        },
        {
            new(new int[2,2]{
                { 1,2 },
                { 3,4 },
            }),
            new(new int[2,2]{
                { 1,3 },
                { 2,4 },
            })
        },
    };

    [Theory]
    [MemberData(nameof(Transpose_Data))]
    [Trait("Category", "Normal")]
    public void Transpose(ArrayMatrix<int> orig, ArrayMatrix<int> expected)
    {
        orig.Transpose().ShouldBe(expected);
    }


    public static TheoryData<ArrayMatrix<Fraction>, ArrayMatrix<Fraction>> GaussianElimination_Data => new()
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
        got.ShouldBe(expected);
    }

    public static TheoryData<ArrayMatrix<Fraction>, Fraction[], Fraction[][]> LinearSystem_Data => new()
    {
        {
            new(new Fraction[2,3]{
                { 1,-4,3 },
                { 3,2,2 },
            }),
            new Fraction[2] { 7, 2 },
            new Fraction[][]
            {
                [new(11,7), new(-19,14), 0],
                [-1, new(1,2), 1],
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
                [1, 0],
                [-2, 1],
            }
        },
        {
            new(new Fraction[2,2]{
                { 1,2 },
                { 2,4 },
            }),
            new Fraction[2] { 1, 3 },
            []
        },
        {
            new(new Fraction[2,2]{
                { 0,0 },
                { 0,0 },
            }),
            new Fraction[2] { 0, 0 },
            new Fraction[][]
            {
                [0, 0],
                [1, 0],
                [0, 1],
            }
        },
    };

    [Theory]
    [MemberData(nameof(LinearSystem_Data))]
    public void LinearSystem(ArrayMatrix<Fraction> matrix, Fraction[] vector, Fraction[][] expected)
    {
        var got = matrix.LinearSystem(vector);
        got.Length.ShouldBe(expected.Length);
        for (int i = 0; i < got.Length; i++)
            got[i].ShouldBe(expected[i], $"got[{i}]");
    }
}