using AtCoder;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

using ArrayMatrix = ArrayMatrix<MontgomeryModInt<Mod998244353>>;
using MontgomeryModInt = MontgomeryModInt<Mod998244353>;
using SimdModMatrix = SimdModMatrix<Mod998244353>;
public class SimdModMatrixTests
{
    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Construct()
    {
        await new SimdModMatrix(
        [
            [1, 2, 3],
            [4, 5, 6],
        ]).ToArray().Should().BeEquivalentOrderTo((MontgomeryModInt[][])[
            [1, 2, 3],
            [4, 5, 6],
        ]);
        await new SimdModMatrix(new MontgomeryModInt[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
        }).ToArray().Should().BeEquivalentOrderTo((MontgomeryModInt[][])[
            [1, 2, 3],
            [4, 5, 6],
        ]);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task NormalIdentity()
    {
        await SimdModMatrix.NormalIdentity(2).Should().BeEqualTo(new SimdModMatrix(
        [
            [1, 0],
            [0, 1],
        ]));
        await SimdModMatrix.NormalIdentity(3).Should().BeEqualTo(new SimdModMatrix(
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
        await new SimdModMatrix(
        [
            [1, 2, 3],
            [4, 5, 6],
        ]).Should().BeEqualTo(new SimdModMatrix(new MontgomeryModInt[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
        }));
        await new SimdModMatrix(
        [
            [1, 2, 3],
            [4, 5, 6],
        ]).Should().BeEqualTo(new SimdModMatrix([1, 2, 3, 4, 5, 6], 2, 3));
        await new SimdModMatrix(
        [
            [1, 2,],
            [3, 4,],
            [5, 6,],
        ]).Should().NotBeEqualTo(new SimdModMatrix(new MontgomeryModInt[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
        }));
    }

    [Test]
    [Property("Category", "Operator")]
    public async Task SingleMinus()
    {
        await (-new SimdModMatrix(new MontgomeryModInt[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
        })).ToArray().Should().BeEquivalentOrderTo((MontgomeryModInt[][])[
            [-1, -2, -3],
            [-4, -5, -6],
        ]);
    }

    public static IEnumerable<(SimdModMatrix, SimdModMatrix, SimdModMatrix)> Add_Data =>
    [
        (
            SimdModMatrix.Zero,
            SimdModMatrix.Identity,
            SimdModMatrix.Identity
        ),
        (
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }),
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 2, 2, 2 },
                { 2, 2, 2 },
            }),
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 3, 4, 5 },
                { 6, 7, 8 },
            })
        ),
        (
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            }),
            SimdModMatrix.Zero,
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            })
        ),
        (
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            }),
            SimdModMatrix.Identity,
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 2, 2 },
                { 3, 5 },
            })
        ),
    ];

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Add_Data))]
    public async Task Add(SimdModMatrix mat1, SimdModMatrix mat2, SimdModMatrix expected)
    {
        await (mat1 + mat2).Should().BeEqualTo(expected);
        await (mat2 + mat1).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(int, int)> AddRandom_Data()
    {
        var nums = new int[]
        {
            1,2,3,5,7,8,9,
            159,160,161
        };
        foreach (var a in nums)
            foreach (var b in nums)
                yield return (a, b);
    }
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(AddRandom_Data))]
    public async Task AddRandom(int h, int w)
    {
        var rnd = new Random(227);
        var a = Enumerable.Repeat(rnd, h * w).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();
        var b = Enumerable.Repeat(rnd, h * w).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();

        for (var i = 0; i < 2; i++)
        {
            var got = new SimdModMatrix(a, h, w) + new SimdModMatrix(b, h, w);
            var expected = new ArrayMatrix(a, h, w) + new ArrayMatrix(b, h, w);

            await got.Height.Should().BeEqualTo(expected.Height);
            await got.Width.Should().BeEqualTo(expected.Width);
            await got._v.Should().BeEquivalentOrderTo(expected._v);

            (a, b) = (b, a);
        }
    }

    public static IEnumerable<(SimdModMatrix, SimdModMatrix, SimdModMatrix)> Subtract_Data =>
    [
        (
            SimdModMatrix.Identity,
            SimdModMatrix.Zero,
            SimdModMatrix.Identity
        ),
        (
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }),
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 2, 2, 2 },
                { 2, 2, 2 },
            }),
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { -1, 0, 1 },
                { 2, 3, 4 },
            })
        ),
        (
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            }),
            SimdModMatrix.Zero,
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            })
        ),
        (
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            }),
            SimdModMatrix.Identity,
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 0, 2 },
                { 3, 3 },
            })
        ),
        (
            SimdModMatrix.Zero,
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            }),
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { -1, -2 },
                { -3, -4 },
            })
        ),
        (
            SimdModMatrix.Identity,
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            }),
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 0,  -2 },
                { -3, -3 },
            })
        ),
    ];
    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Subtract_Data))]
    public async Task Subtract(SimdModMatrix mat1, SimdModMatrix mat2, SimdModMatrix expected)
    {
        await (mat1 - mat2).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(AddRandom_Data))]
    public async Task SubtractRandom(int h, int w)
    {
        var rnd = new Random(227);
        var a = Enumerable.Repeat(rnd, h * w).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();
        var b = Enumerable.Repeat(rnd, h * w).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();

        for (int i = 0; i < 2; i++)
        {
            var got = new SimdModMatrix(a, h, w) - new SimdModMatrix(b, h, w);
            var expected = new ArrayMatrix(a, h, w) - new ArrayMatrix(b, h, w);

            await got.Height.Should().BeEqualTo(expected.Height);
            await got.Width.Should().BeEqualTo(expected.Width);
            await got._v.Should().BeEquivalentOrderTo(expected._v);

            (a, b) = (b, a);
        }
    }

    public static IEnumerable<(SimdModMatrix, SimdModMatrix, SimdModMatrix)> Multiply_Data =>
    [
        (
            SimdModMatrix.Identity,
            SimdModMatrix.Zero,
            SimdModMatrix.Zero
        ),
        (
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }),
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            }),
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 22, 28 },
                { 49, 64 },
            })
        ),
        (
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            }),
            SimdModMatrix.Zero,
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 0, 0 },
                { 0, 0 },
            })
        ),
        (
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            }),
            SimdModMatrix.Identity,
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            })
        ),
    ];

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(Multiply_Data))]
    public async Task Multiply(SimdModMatrix mat1, SimdModMatrix mat2, SimdModMatrix expected)
    {
        await (mat1 * mat2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<ValueTuple<int, int, int>> MultiplyRandom_Data()
    {
        yield return (130, 130, 130);
        yield return (130, 1, 130);
        yield return (1, 130, 130);
        yield return (130, 130, 1);
        yield return (64, 130, 130);
        yield return (130, 64, 130);
        yield return (130, 130, 64);
    }


    [Test, MultipleAssertions]
    [MethodDataSource(nameof(MultiplyRandom_Data))]
    public async Task MultiplyRandom(int h, int w, int mid)
    {
        var rnd = new Random(227);
        var a = Enumerable.Repeat(rnd, h * mid).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();
        var b = Enumerable.Repeat(rnd, mid * w).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();

        for (var i = 0; i < 2; i++)
        {
            var got = new SimdModMatrix(a, h, mid) * new SimdModMatrix(b, mid, w);
            var expected = new ArrayMatrix(a, h, mid) * new ArrayMatrix(b, mid, w);

            await got.Height.Should().BeEqualTo(expected.Height);
            await got.Width.Should().BeEqualTo(expected.Width);
            await got._v.Should().BeEquivalentOrderTo(expected._v);
        }
    }

    public static IEnumerable<(int, SimdModMatrix, SimdModMatrix)> MultiplyScalar_Data =>
    [
        (
            2,
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }),
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 2, 4, 6 },
                { 8, 10, 12 },
            })
        ),
        (
            3,
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            }),
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 3, 6 },
                { 9, 12 },
            })
        ),
    ];

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(nameof(MultiplyScalar_Data))]
    public async Task MultiplyScalar(int a, SimdModMatrix mat, SimdModMatrix expected)
    {
        await (mat * a).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(SimdModMatrix, MontgomeryModInt[], MontgomeryModInt[])> MultiplyVector_Data =>
    [
        (
            new SimdModMatrix(new MontgomeryModInt[,]{
                {3, 0, 0, 0},
                {0, 3, 0, 0},
                {0, 0, 3, 0},
                {0, 0, 0, 3}
            }),
            [1,2,3,4],
            [3,6,9,12]
        ),
        (
            new SimdModMatrix(new MontgomeryModInt[,]{
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
    public async Task MultiplyVector(SimdModMatrix mat, MontgomeryModInt[] vector, MontgomeryModInt[] expected)
    {
        await (mat * vector).Should().BeEquivalentOrderTo(expected);
        await mat.Multiply(vector).Should().BeEquivalentOrderTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Pow()
    {
        var orig = new SimdModMatrix(new MontgomeryModInt[,]
        {
            { 1, 2 },
            { 3, 4 },
        });
        await orig.Pow(5).ToArray().Should().BeEquivalentOrderTo((MontgomeryModInt[][])[
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

    public static IEnumerable<(MontgomeryModInt[,], MontgomeryModInt)> Determinant_Data =>
    [
        (
            new MontgomeryModInt[,]
            {
                {10, -9},
                { 7, -12},
            },
            -57
        ),
        (
            new MontgomeryModInt[,]
            {
                {10, -9, -12},
                {7, -12, 11},
                {-10, 10, 3},
            },
            319
        ),
        (
            new MontgomeryModInt[,]
            {
                {10, -9, -12, 6 },
                {7, -12, 11, 15},
                {1, 0, 2, 9},
                {-10, 10, 3, 13},
            },
            -10683
        ),
        (
            new MontgomeryModInt[,]
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
    public async Task Determinant(MontgomeryModInt[,] array, MontgomeryModInt expected)
    {
        await new SimdModMatrix(array).Determinant().Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Determinant2()
    {
        for (int n = 3; n < 20; n++)
        {
            var array = new MontgomeryModInt[n, n];
            for (int i = 0; i < n; i++)
            {
                array[i, 0] = array[0, i] = array[i, i] = 1;
            }
            await new SimdModMatrix(array).Determinant().Should().BeEqualTo(-(MontgomeryModInt)(n - 2));
        }
    }

    [Test]
    [Property("Category", "Normal")]
    public async Task Determinant3()
    {
        int n = 60;
        var array = new MontgomeryModInt[n, n];
        for (int i = 0; i < n; i++)
        {
            array[i, 0] = array[0, i] = array[i, i] = 1;
        }
        await new SimdModMatrix(array).Determinant().Should().BeEqualTo(-(MontgomeryModInt)(n - 2));
    }

    public static IEnumerable<(SimdModMatrix matrix, int i, int j, MontgomeryModInt expected)> Cofactor_Data()
    {
        {
            var mt = new MontgomeryModInt[,]
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
            var mt = new MontgomeryModInt[,]
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
    public async Task Cofactor(SimdModMatrix matrix, int i, int j, MontgomeryModInt expected)
    {
        await matrix.Cofactor(i, j).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    public async Task Inv()
    {
        var orig = new SimdModMatrix(new MontgomeryModInt[,]
        {
            {10, -9, -12},
            {7, -12, 11},
            {-10, 10, 3},
        });
        var inv = orig.Inv();
        await inv.Should().BeEqualTo(new SimdModMatrix(new MontgomeryModInt[,]
          {
            { -(MontgomeryModInt)146/ 319, -(MontgomeryModInt)93/319, -(MontgomeryModInt)243/319 },
            { -(MontgomeryModInt)131/319, -(MontgomeryModInt)90/319, -(MontgomeryModInt)194/319 },
            { -(MontgomeryModInt)50/319, -(MontgomeryModInt)10/319, -(MontgomeryModInt)57/319 },
          }));
        MontgomeryModInt[][] id = [
            [1,0,0],
            [0,1,0],
            [0,0,1],
        ];
        await (orig * inv).ToArray().Should().BeEquivalentOrderTo(id);
        await (inv * orig).ToArray().Should().BeEquivalentOrderTo(id);
    }


    public static IEnumerable<(SimdModMatrix, SimdModMatrix)> Transpose_Data =>
    [
        (
            new(new MontgomeryModInt[2,3]{
                { 1,-4,3 },
                { 3,2,2 },
            }),
            new(new MontgomeryModInt[3,2]{
                { 1,3 },
                {-4,2 },
                { 3,2 },
            })
        ),
        (
            new(new MontgomeryModInt[2,2]{
                { 1,2 },
                { 3,4 },
            }),
            new(new MontgomeryModInt[2,2]{
                { 1,3 },
                { 2,4 },
            })
        ),
    ];

    [Test]
    [MethodDataSource(nameof(Transpose_Data))]
    [Property("Category", "Normal")]
    public async Task Transpose(SimdModMatrix orig, SimdModMatrix expected)
    {
        await orig.Transpose().Should().BeEqualTo(expected);
    }


    public static IEnumerable<(SimdModMatrix, SimdModMatrix)> GaussianElimination_Data =>
    [
        (
            new(new MontgomeryModInt[2,3]{
                { 1,-4,3 },
                { 3,2,2 },
            }),
            new(new MontgomeryModInt[2,3]{
                { 1,0,1 },
                { 0,1,-(MontgomeryModInt)1/2 },
            })
        ),
        (
            new (new MontgomeryModInt[5, 3]{
                { 1,-4,3 },
                { 3,2,2 },
                { 1,2,0 },
                { 1,0,1 },
                { 0,-1,(MontgomeryModInt)1/2 },
            }),
            new(new MontgomeryModInt[5, 3]{
                { 1,0,(MontgomeryModInt)1/1 },
                { 0,1,-(MontgomeryModInt)1/2 },
                { 0,0,0 },
                { 0,0,0 },
                { 0,0,0 },
            })
        ),
        (
            new(new MontgomeryModInt[3, 3]{
                { 1,-4,3 },
                { 3,2,2 },
                { 1,0,0 },
            }),
            new(new MontgomeryModInt[3, 3]{
                { 1,0,0 },
                { 0,1,0 },
                { 0,0,1 },
            })
        ),
        (
            new(new MontgomeryModInt[3, 4]{
                { 1,-1,2,0 },
                { 4,2,-3,2 },
                { 1,0,1,1 },
            }),
            new(new MontgomeryModInt[3, 4]{
                { 1,0,0,(MontgomeryModInt)1/5 },
                { 0,1,0,(MontgomeryModInt)9/5 },
                { 0,0,1,(MontgomeryModInt)4/5 },
            })
        ),
        (
            new(new MontgomeryModInt[3, 4]{
                { 1,2,3,4 },
                { 1,2,3,4 },
                { 1,2,3,4 },
            }),
            new(new MontgomeryModInt[3, 4]{
                { 1,2,3,4 },
                { 0,0,0,0 },
                { 0,0,0,0 },
            })
        ),
        (
            new(new MontgomeryModInt[3, 4]{
                { 1,2,3,4 },
                { 1,2,3,5 },
                { 1,2,3,4 },
            }),
            new(new MontgomeryModInt[3, 4]{
                { 1,2,3,0 },
                { 0,0,0,1 },
                { 0,0,0,0 },
            })
        ),
    ];

    [Test]
    [MethodDataSource(nameof(GaussianElimination_Data))]
    public async Task GaussianElimination(SimdModMatrix orig, SimdModMatrix expected)
    {
        var got = orig.GaussianElimination();
        await got.Should().BeEqualTo(expected);
    }

    public static IEnumerable<(SimdModMatrix, MontgomeryModInt[], MontgomeryModInt[][])> LinearSystem_Data =>
    [
        (
            new(new MontgomeryModInt[2,3]{
                { 1,-4,3 },
                { 3,2,2 },
            }),
            [7, 2],
            [
                [(MontgomeryModInt)11/7, -(MontgomeryModInt)19/14, 0],
                [-1, (MontgomeryModInt)1/2, 1],
            ]
        ),
        (
            new(new MontgomeryModInt[2,2]{
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
            new(new MontgomeryModInt[2,2]{
                { 1,2 },
                { 2,4 },
            }),
            [1, 3],
            []
        ),
        (
            new(new MontgomeryModInt[2,2]{
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

    [Test]
    [MethodDataSource(nameof(LinearSystem_Data))]
    public async Task LinearSystem(SimdModMatrix matrix, MontgomeryModInt[] vector, MontgomeryModInt[][] expected)
    {
        var got = matrix.LinearSystem(vector);
        await got.Length.Should().BeEqualTo(expected.Length);
        for (int i = 0; i < got.Length; i++)
            await got[i].Should().BeEquivalentOrderTo(expected[i]);
    }
}