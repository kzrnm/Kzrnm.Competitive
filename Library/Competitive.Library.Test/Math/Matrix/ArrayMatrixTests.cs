using AtCoder;
using Kzrnm.Competitive.Internal;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class ArrayMatrixTests
{
    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Construct()
    {
        await new ArrayMatrix<int>(
        [
            [1, 2, 3],
            [4, 5, 6],
        ]).ToArray().Should().BeEquivalentOrderTo((int[][])[
            [1, 2, 3],
            [4, 5, 6],
        ]);
        await new ArrayMatrix<int>(new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
        }).ToArray().Should().BeEquivalentOrderTo((int[][])[
            [1, 2, 3],
            [4, 5, 6],
        ]);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task NormalIdentity()
    {
        await ArrayMatrix<int>.NormalIdentity(2).Should().BeEqualTo(new ArrayMatrix<int>(
        [
            [1, 0],
            [0, 1],
        ]));
        await ArrayMatrix<int>.NormalIdentity(3).Should().BeEqualTo(new ArrayMatrix<int>(
        [
            [1, 0, 0],
            [0, 1, 0],
            [0, 0, 1],
        ]));

        await ArrayMatrix<MontgomeryModInt<Mod1000000007>>.NormalIdentity(3).Should().BeEqualTo(new ArrayMatrix<MontgomeryModInt<Mod1000000007>>(
        [
            [1, 0, 0],
            [0, 1, 0],
            [0, 0, 1],
        ]));
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Equal()
    {
        await new ArrayMatrix<int>(
        [
            [1, 2, 3],
            [4, 5, 6],
        ]).Should().BeEqualTo(new ArrayMatrix<int>(new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
        }));
        await new ArrayMatrix<int>(
        [
            [1, 2, 3],
            [4, 5, 6],
        ]).Should().BeEqualTo(new ArrayMatrix<int>([1, 2, 3, 4, 5, 6], 2, 3));
        await new ArrayMatrix<int>(
        [
            [1, 2,],
            [3, 4,],
            [5, 6,],
        ]).Should().NotBeEqualTo(new ArrayMatrix<int>(new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
        }));
    }

    [Test]
    [Property("Category", "Operator")]
    public async Task SingleMinus()
    {
        await (-new ArrayMatrix<int>(new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
        })).ToArray().Should().BeEquivalentOrderTo((int[][])[
            [-1, -2, -3],
            [-4, -5, -6],
        ]);
    }

    public static IEnumerable<(ArrayMatrix<int>, ArrayMatrix<int>, ArrayMatrix<int>)> Add_Data =>
    [
        (
            ArrayMatrix<int>.Zero,
            ArrayMatrix<int>.Identity,
            ArrayMatrix<int>.Identity
        ),
        (
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
        ),
        (
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
        ),
        (
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
        ),
    ];

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Add_Data))]
    public async Task Add(ArrayMatrix<int> mat1, ArrayMatrix<int> mat2, ArrayMatrix<int> expected)
    {
        await (mat1 + mat2).Should().BeEqualTo(expected);
        await (mat2 + mat1).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(ArrayMatrix<int>, ArrayMatrix<int>, ArrayMatrix<int>)> Subtract_Data =>
    [
        (
            ArrayMatrix<int>.Identity,
            ArrayMatrix<int>.Zero,
            ArrayMatrix<int>.Identity
        ),
        (
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
        ),
        (
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
        ),
        (
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
        ),
        (
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
        ),
        (
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
        ),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Subtract_Data))]
    public async Task Subtract(ArrayMatrix<int> mat1, ArrayMatrix<int> mat2, ArrayMatrix<int> expected)
    {
        await (mat1 - mat2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(ArrayMatrix<int>, ArrayMatrix<int>, ArrayMatrix<int>)> Multiply_Data =>
    [
        (
            ArrayMatrix<int>.Identity,
            ArrayMatrix<int>.Zero,
            ArrayMatrix<int>.Zero
        ),
        (
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
        ),
        (
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
        ),
        (
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
        ),
    ];

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Multiply_Data))]
    public async Task Multiply(ArrayMatrix<int> mat1, ArrayMatrix<int> mat2, ArrayMatrix<int> expected)
    {
        await (mat1 * mat2).Should().BeEqualTo(expected);
    }

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Multiply_Data))]
    public async Task MultiplyModInt(ArrayMatrix<int> matInt1, ArrayMatrix<int> matInt2, ArrayMatrix<int> expectedInt)
    {
        var mat1 = Int2ModInt(matInt1);
        var mat2 = Int2ModInt(matInt2);
        var expected = Int2ModInt(expectedInt);
        await (mat1 * mat2).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Multiply_Data))]
    public async Task MultiplyMontgomeryModInt(ArrayMatrix<int> matInt1, ArrayMatrix<int> matInt2, ArrayMatrix<int> expectedInt)
    {
        var mat1 = Int2MontgomeryModInt(matInt1);
        var mat2 = Int2MontgomeryModInt(matInt2);
        var expected = Int2MontgomeryModInt(expectedInt);
        await (mat1 * mat2).Should().BeEqualTo(expected);
        await mat1.Strassen(mat2).Should().BeEqualTo(expected);
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


    public static IEnumerable<(int, ArrayMatrix<int>, ArrayMatrix<int>)> MultiplyScalar_Data =>
    [
        (
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
        ),
        (
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
        ),
    ];

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(MultiplyScalar_Data))]
    public async Task MultiplyScalar(int a, ArrayMatrix<int> mat, ArrayMatrix<int> expected)
    {
        await (mat * a).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(ArrayMatrix<long>, long[], long[])> MultiplyVector_Data =>
    [
        (
            new ArrayMatrix<long>(new long[,]{
                {3, 0, 0, 0},
                {0, 3, 0, 0},
                {0, 0, 3, 0},
                {0, 0, 0, 3}
            }),
            [1,2,3,4],
            [3,6,9,12]
        ),
        (
            new ArrayMatrix<long>(new long[,]{
                {1, 2, 3, 4},
                {5, 6, 7, 8},
                {9, 10, 11, 12},
                {13, 14, 15, 16}
            }),
            [1,2,3,4],
            [30, 70, 110, 150]
        ),
    ];

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(MultiplyVector_Data))]
    public async Task MultiplyVector(ArrayMatrix<long> mat, long[] vector, long[] expected)
    {
        await (mat * vector).Should().BeEquivalentOrderTo(expected);
        await mat.Multiply(vector).Should().BeEquivalentOrderTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Pow()
    {
        var orig = new ArrayMatrix<int>(new int[,]
        {
            { 1, 2 },
            { 3, 4 },
        });
        await orig.Pow(5).ToArray().Should().BeEquivalentOrderTo((int[][])[
            [1069, 1558],
            [2337, 3406],
        ]);
        var cur = orig;
        for (int i = 1; i < 10; i++)
        {
            await orig.Pow(i).Should().BeEqualTo(cur);
            cur *= orig;
        }
    }

    public static IEnumerable<(Fraction[,], Fraction)> Determinant_Data =>
    [
        (
            new Fraction[,]
            {
                {10, -9},
                { 7, -12},
            },
            -57
        ),
        (
            new Fraction[,]
            {
                {10, -9, -12},
                {7, -12, 11},
                {-10, 10, 3},
            },
            319
        ),
        (
            new Fraction[,]
            {
                {10, -9, -12, 6 },
                {7, -12, 11, 15},
                {1, 0, 2, 9},
                {-10, 10, 3, 13},
            },
            -10683
        ),
        (
            new Fraction[,]
            {
                { 4, 6, 5,-2,2},
                { 1,-2,-1, 2,4},
                { 0, 3, 1, 4,3},
                {-7, 5,-1, 3,5},
                {-1, 3, 2,-2,2},
            },
            -881
        ),
    ];

    [Test]
    [MethodDataSource(nameof(Determinant_Data))]
    [Property("Category", "Normal")]
    public async Task Determinant(Fraction[,] array, Fraction expected)
    {
        await new ArrayMatrix<Fraction>(array).Determinant().Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Determinant2()
    {
        for (int n = 3; n < 20; n++)
        {
            var array = new Fraction[n, n];
            for (int i = 0; i < n; i++)
            {
                array[i, 0] = array[0, i] = array[i, i] = 1;
            }
            await new ArrayMatrix<Fraction>(array).Determinant().Should().BeEqualTo(-(n - 2));
        }
    }

    [Test]
    [Property("Category", "Normal")]
    public async Task Determinant3()
    {
        int n = 60;
        var array = new Fraction[n, n];
        for (int i = 0; i < n; i++)
        {
            array[i, 0] = array[0, i] = array[i, i] = 1;
        }
        await new ArrayMatrix<Fraction>(array).Determinant().Should().BeEqualTo(-(n - 2));
    }

    public static IEnumerable<(ArrayMatrix<Fraction> matrix, int i, int j, Fraction expected)> Cofactor_Data()
    {
        {
            var mt = new Fraction[,]
            {
                {10, -9},
                { 7, -12},
            };

            yield return (new(mt), 0, 0, -12);
            yield return (new(mt), 0, 1, -7);
            yield return (new(mt), 1, 0, 9);
            yield return (new(mt), 1, 1, 10);
        }
        {
            var mt = new Fraction[,]
            {
                {10, -9, -12},
                {7, -12, 11},
                {-10, 10, 3},
            };

            yield return (new(mt), 0, 0, -146);
            yield return (new(mt), 0, 1, -131);
            yield return (new(mt), 0, 2, -50);
            yield return (new(mt), 1, 0, -93);
            yield return (new(mt), 1, 1, -90);
            yield return (new(mt), 1, 2, -10);
            yield return (new(mt), 2, 0, -243);
            yield return (new(mt), 2, 1, -194);
            yield return (new(mt), 2, 2, -57);
        }
    }

    [Test]
    [MethodDataSource(nameof(Cofactor_Data))]
    [Property("Category", "Normal")]
    public async Task Cofactor(ArrayMatrix<Fraction> matrix, int i, int j, Fraction expected)
    {
        await matrix.Cofactor(i, j).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Inv()
    {
        var orig = new ArrayMatrix<Fraction>(new Fraction[,]
        {
            {10, -9, -12},
            {7, -12, 11},
            {-10, 10, 3}
        });
        var inv = orig.Inv();
        await inv.ToArray().Should().BeEquivalentOrderTo((Fraction[][])[
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
        await (orig * inv).ToArray().Should().BeEquivalentOrderTo(id);
        await (inv * orig).ToArray().Should().BeEquivalentOrderTo(id);
    }


    public static IEnumerable<(ArrayMatrix<int>, ArrayMatrix<int>)> Transpose_Data =>
    [
        (
            new(new int[2,3]{
                { 1,-4,3 },
                { 3,2,2 },
            }),
            new(new int[3,2]{
                { 1,3 },
                {-4,2 },
                { 3,2 },
            })
        ),
        (
            new(new int[2,2]{
                { 1,2 },
                { 3,4 },
            }),
            new(new int[2,2]{
                { 1,3 },
                { 2,4 },
            })
        ),
    ];

    [Test]
    [MethodDataSource(nameof(Transpose_Data))]
    [Property("Category", "Normal")]
    public async Task Transpose(ArrayMatrix<int> orig, ArrayMatrix<int> expected)
    {
        await orig.Transpose().Should().BeEqualTo(expected);
    }


    public static IEnumerable<(ArrayMatrix<Fraction>, ArrayMatrix<Fraction>)> GaussianElimination_Data =>
    [
        (
            new(new Fraction[2,3]{
                { 1,-4,3 },
                { 3,2,2 },
            }),
            new(new Fraction[2,3]{
                { 1,0,new(1,1) },
                { 0,1,new(-1,2) },
            })
        ),
        (
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
        ),
        (
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
        ),
        (
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
        ),
        (
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
        ),
        (
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
        ),
    ];

    [Test]
    [MethodDataSource(nameof(GaussianElimination_Data))]
    public async Task GaussianElimination(ArrayMatrix<Fraction> orig, ArrayMatrix<Fraction> expected)
    {
        var got = orig.GaussianElimination();
        await got.Should().BeEqualTo(expected);
    }

    public static IEnumerable<(ArrayMatrix<Fraction>, Fraction[], Fraction[][])> LinearSystem_Data =>
    [
        (
            new(new Fraction[2,3]{
                { 1,-4,3 },
                { 3,2,2 },
            }),
            [7, 2],
            [
                [new(11,7), new(-19,14), 0],
                [-1, new(1,2), 1],
            ]
        ),
        (
            new(new Fraction[2,2]{
                { 1,2 },
                { 2,4 },
            }),
            [1, 2],
            [
                [1, 0],
                [-2, 1],
            ]
        ),
        (
            new(new Fraction[2,2]{
                { 1,2 },
                { 2,4 },
            }),
            [1, 3],
            []
        ),
        (
            new(new Fraction[2,2]{
                { 0,0 },
                { 0,0 },
            }),
            [0, 0],
            [
                [0, 0],
                [1, 0],
                [0, 1],
            ]
        ),
    ];

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(LinearSystem_Data))]
    public async Task LinearSystem(ArrayMatrix<Fraction> matrix, Fraction[] vector, Fraction[][] expected)
    {
        var got = matrix.LinearSystem(vector);
        await got.Length.Should().BeEqualTo(expected.Length);
        for (int i = 0; i < got.Length; i++)
            await got[i].Should().BeEquivalentOrderTo(expected[i]);
    }
}