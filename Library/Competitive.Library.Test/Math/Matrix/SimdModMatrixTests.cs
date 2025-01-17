using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    using ArrayMatrix = ArrayMatrix<MontgomeryModInt<Mod998244353>>;
    using MontgomeryModInt = MontgomeryModInt<Mod998244353>;
    using SimdModMatrix = SimdModMatrix<Mod998244353>;
    public class SimdModMatrixTests
    {
        [Fact]
        [Trait("Category", "Normal")]
        public void Construct()
        {
            new SimdModMatrix(
            [
                [1, 2, 3],
                [4, 5, 6],
            ]).ToArray().ShouldBe(new MontgomeryModInt[][]
            {
                [1, 2, 3],
                [4, 5, 6],
            });
            new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }).ToArray().ShouldBe(new MontgomeryModInt[][]
            {
                [1, 2, 3],
                [4, 5, 6],
            });
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void NormalIdentity()
        {
            SimdModMatrix.NormalIdentity(2).ShouldBe(new SimdModMatrix(
            [
                [1, 0],
                [0, 1],
            ]));
            SimdModMatrix.NormalIdentity(3).ShouldBe(new SimdModMatrix(
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
            new SimdModMatrix(
            [
                [1, 2, 3],
                [4, 5, 6],
            ]).ShouldBe(new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }));
            new SimdModMatrix(
            [
                [1, 2, 3],
                [4, 5, 6],
            ]).ShouldBe(new SimdModMatrix(new MontgomeryModInt[] { 1, 2, 3, 4, 5, 6 }, 2, 3));
            new SimdModMatrix(
            [
                [1, 2,],
                [3, 4,],
                [5, 6,],
            ]).ShouldNotBe(new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            }));
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            (-new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            })).ToArray().ShouldBe(new MontgomeryModInt[][]
            {
                [-1, -2, -3],
                [-4, -5, -6],
            });
        }

        public static TheoryData Add_Data => new TheoryData<SimdModMatrix, SimdModMatrix, SimdModMatrix>
        {
            {
                SimdModMatrix.Zero,
                SimdModMatrix.Identity,
                SimdModMatrix.Identity
            },
            {
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
            },
            {
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
            },
            {
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
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void Add(SimdModMatrix mat1, SimdModMatrix mat2, SimdModMatrix expected)
        {
            (mat1 + mat2).ShouldBe(expected);
            (mat2 + mat1).ShouldBe(expected);
        }

        public static IEnumerable<ValueTuple<int, int>> AddRandom_Data()
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
        [Theory]
        [TupleMemberData(nameof(AddRandom_Data))]
        public void AddRandom(int h, int w)
        {
            var rnd = new Random(227);
            var a = Enumerable.Repeat(rnd, h * w).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();
            var b = Enumerable.Repeat(rnd, h * w).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();

            Impl(a, b);
            Impl(b, a);

            void Impl(MontgomeryModInt[] a, MontgomeryModInt[] b)
            {
                var got = new SimdModMatrix(a, h, w) + new SimdModMatrix(b, h, w);
                var expected = new ArrayMatrix(a, h, w) + new ArrayMatrix(b, h, w);

                got.Height.ShouldBe(expected.Height);
                got.Width.ShouldBe(expected.Width);
                got._v.ShouldBe(expected._v);
            }
        }

        public static TheoryData Subtract_Data => new TheoryData<SimdModMatrix, SimdModMatrix, SimdModMatrix>
        {
            {
                SimdModMatrix.Identity,
                SimdModMatrix.Zero,
                SimdModMatrix.Identity
            },
            {
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
            },
            {
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
            },
            {
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
            },
            {
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
            },
            {
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
            },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Subtract_Data))]
        public void Subtract(SimdModMatrix mat1, SimdModMatrix mat2, SimdModMatrix expected)
        {
            (mat1 - mat2).ShouldBe(expected);
        }

        [Theory]
        [TupleMemberData(nameof(AddRandom_Data))]
        public void SubtractRandom(int h, int w)
        {
            var rnd = new Random(227);
            var a = Enumerable.Repeat(rnd, h * w).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();
            var b = Enumerable.Repeat(rnd, h * w).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();

            Impl(a, b);
            Impl(b, a);

            void Impl(MontgomeryModInt[] a, MontgomeryModInt[] b)
            {
                var got = new SimdModMatrix(a, h, w) - new SimdModMatrix(b, h, w);
                var expected = new ArrayMatrix(a, h, w) - new ArrayMatrix(b, h, w);

                got.Height.ShouldBe(expected.Height);
                got.Width.ShouldBe(expected.Width);
                got._v.ShouldBe(expected._v);
            }
        }

        public static TheoryData Multiply_Data => new TheoryData<SimdModMatrix, SimdModMatrix, SimdModMatrix>
        {
            {
                SimdModMatrix.Identity,
                SimdModMatrix.Zero,
                SimdModMatrix.Zero
            },
            {
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
            },
            {
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
            },
            {
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
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(SimdModMatrix mat1, SimdModMatrix mat2, SimdModMatrix expected)
        {
            (mat1 * mat2).ShouldBe(expected);
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


        [Theory]
        [TupleMemberData(nameof(MultiplyRandom_Data))]
        public void MultiplyRandom(int h, int w, int mid)
        {
            var rnd = new Random(227);
            var a = Enumerable.Repeat(rnd, h * mid).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();
            var b = Enumerable.Repeat(rnd, mid * w).Select(r => (MontgomeryModInt)r.Next(998244353)).ToArray();

            Impl(a, b);

            void Impl(MontgomeryModInt[] a, MontgomeryModInt[] b)
            {
                var got = new SimdModMatrix(a, h, mid) * new SimdModMatrix(b, mid, w);
                var expected = new ArrayMatrix(a, h, mid) * new ArrayMatrix(b, mid, w);

                got.Height.ShouldBe(expected.Height);
                got.Width.ShouldBe(expected.Width);
                got._v.ShouldBe(expected._v);
            }
        }

        public static TheoryData MultiplyScalar_Data => new TheoryData<int, SimdModMatrix, SimdModMatrix>
        {
            {
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
            },
            {
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
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyScalar_Data))]
        public void MultiplyScalar(int a, SimdModMatrix mat, SimdModMatrix expected)
        {
            (mat * a).ShouldBe(expected);
        }

        public static TheoryData MultiplyVector_Data => new TheoryData<SimdModMatrix, MontgomeryModInt[], MontgomeryModInt[]>
        {
            {
                new SimdModMatrix(new MontgomeryModInt[,]{
                    {3, 0, 0, 0},
                    {0, 3, 0, 0},
                    {0, 0, 3, 0},
                    {0, 0, 0, 3}
                }),
                new MontgomeryModInt[]{1,2,3,4},
                new MontgomeryModInt[]{3,6,9,12}
            },
            {
                new SimdModMatrix(new MontgomeryModInt[,]{
                    {1, 2, 3, 4},
                    {5, 6, 7, 8},
                    {9, 10, 11, 12},
                    {13, 14, 15, 16}
                }),
                new MontgomeryModInt[]{1,2,3,4},
                new MontgomeryModInt[]{30, 70, 110, 150}
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(SimdModMatrix mat, MontgomeryModInt[] vector, MontgomeryModInt[] expected)
        {
            (mat * vector).ShouldBe(expected);
            mat.Multiply(vector).ShouldBe(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = new SimdModMatrix(new MontgomeryModInt[,]
            {
                { 1, 2 },
                { 3, 4 },
            });
            orig.Pow(5).ToArray().ShouldBe(new MontgomeryModInt[][]
            {
                [1069, 1558],
                [2337, 3406],
            });
            var cur = orig;
            for (int i = 1; i < 10; i++)
            {
                orig.Pow(i).ShouldBe(cur);
                cur *= orig;
            }
        }

        public static TheoryData Determinant_Data => new TheoryData<MontgomeryModInt[,], MontgomeryModInt>
        {
            {
                new MontgomeryModInt[,]
                {
                    {10, -9},
                    { 7, -12},
                },
                -57
            },
            {
                new MontgomeryModInt[,]
                {
                    {10, -9, -12},
                    {7, -12, 11},
                    {-10, 10, 3},
                },
                319
            },
            {
                new MontgomeryModInt[,]
                {
                    {10, -9, -12, 6 },
                    {7, -12, 11, 15},
                    {1, 0, 2, 9},
                    {-10, 10, 3, 13},
                },
                -10683
            },
            {
                new MontgomeryModInt[,]
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
        public void Determinant(MontgomeryModInt[,] array, MontgomeryModInt expected)
        {
            new SimdModMatrix(array).Determinant().ShouldBe(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Determinant2()
        {
            for (int n = 3; n < 20; n++)
            {
                var array = new MontgomeryModInt[n, n];
                for (int i = 0; i < n; i++)
                {
                    array[i, 0] = array[0, i] = array[i, i] = 1;
                }
                new SimdModMatrix(array).Determinant().ShouldBe(-(MontgomeryModInt)(n - 2));
            }
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Determinant3()
        {
            int n = 60;
            var array = new MontgomeryModInt[n, n];
            for (int i = 0; i < n; i++)
            {
                array[i, 0] = array[0, i] = array[i, i] = 1;
            }
            new SimdModMatrix(array).Determinant().ShouldBe(-(MontgomeryModInt)(n - 2));
        }

        public static IEnumerable<(MontgomeryModInt[,] array, int i, int j, MontgomeryModInt expected)> Cofactor_Data()
        {
            {
                var mt = new MontgomeryModInt[,]
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
                var mt = new MontgomeryModInt[,]
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
        public void Cofactor(MontgomeryModInt[,] array, int i, int j, MontgomeryModInt expected)
        {
            new SimdModMatrix(array).Cofactor(i, j).ShouldBe(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Inv()
        {
            var orig = new SimdModMatrix(new MontgomeryModInt[,]
            {
                {10, -9, -12},
                {7, -12, 11},
                {-10, 10, 3},
            });
            var inv = orig.Inv();
            inv.ShouldBe(new SimdModMatrix(new MontgomeryModInt[,]
            {
                { -(MontgomeryModInt)146/ 319, -(MontgomeryModInt)93/319, -(MontgomeryModInt)243/319 },
                { -(MontgomeryModInt)131/319, -(MontgomeryModInt)90/319, -(MontgomeryModInt)194/319 },
                { -(MontgomeryModInt)50/319, -(MontgomeryModInt)10/319, -(MontgomeryModInt)57/319 },
            }));
            var id = new MontgomeryModInt[][]
            {
                [1,0,0],
                [0,1,0],
                [0,0,1],
            };
            (orig * inv).ToArray().ShouldBe(id);
            (inv * orig).ToArray().ShouldBe(id);
        }


        public static TheoryData Transpose_Data => new TheoryData<SimdModMatrix, SimdModMatrix>
        {
            {
                new(new MontgomeryModInt[2,3]{
                    { 1,-4,3 },
                    { 3,2,2 },
                }),
                new(new MontgomeryModInt[3,2]{
                    { 1,3 },
                    {-4,2 },
                    { 3,2 },
                })
            },
            {
                new(new MontgomeryModInt[2,2]{
                    { 1,2 },
                    { 3,4 },
                }),
                new(new MontgomeryModInt[2,2]{
                    { 1,3 },
                    { 2,4 },
                })
            },
        };

        [Theory]
        [MemberData(nameof(Transpose_Data))]
        [Trait("Category", "Normal")]
        public void Transpose(SimdModMatrix orig, SimdModMatrix expected)
        {
            orig.Transpose().ShouldBe(expected);
        }


        public static TheoryData GaussianElimination_Data => new TheoryData<
            SimdModMatrix,
            SimdModMatrix>
        {
            {
                new(new MontgomeryModInt[2,3]{
                    { 1,-4,3 },
                    { 3,2,2 },
                }),
                new(new MontgomeryModInt[2,3]{
                    { 1,0,1 },
                    { 0,1,-(MontgomeryModInt)1/2 },
                })
            },
            {
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
            },
            {
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
            },
            {
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
            },
            {
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
            },
            {
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
            },
        };

        [Theory]
        [MemberData(nameof(GaussianElimination_Data))]
        public void GaussianElimination(SimdModMatrix orig, SimdModMatrix expected)
        {
            var got = orig.GaussianElimination();
            got.ShouldBe(expected);
        }

        public static TheoryData LinearSystem_Data => new TheoryData<
            SimdModMatrix,
            MontgomeryModInt[],
            MontgomeryModInt[][]>
        {
            {
                new(new MontgomeryModInt[2,3]{
                    { 1,-4,3 },
                    { 3,2,2 },
                }),
                new MontgomeryModInt[2] { 7, 2 },
                new MontgomeryModInt[][]
                {
                    [(MontgomeryModInt)11/7, -(MontgomeryModInt)19/14, 0],
                    [-1, (MontgomeryModInt)1/2, 1],
                }
            },
            {
                new(new MontgomeryModInt[2,2]{
                    { 1,2 },
                    { 2,4 },
                }),
                new MontgomeryModInt[2] { 1, 2 },
                new MontgomeryModInt[][]
                {
                    [1, 0],
                    [-2, 1],
                }
            },
            {
                new(new MontgomeryModInt[2,2]{
                    { 1,2 },
                    { 2,4 },
                }),
                new MontgomeryModInt[2] { 1, 3 },
                []
            },
            {
                new(new MontgomeryModInt[2,2]{
                    { 0,0 },
                    { 0,0 },
                }),
                new MontgomeryModInt[2] { 0, 0 },
                new MontgomeryModInt[][]
                {
                    [0, 0],
                    [1, 0],
                    [0, 1],
                }
            },
        };

        [Theory]
        [MemberData(nameof(LinearSystem_Data))]
        public void LinearSystem(SimdModMatrix matrix, MontgomeryModInt[] vector, MontgomeryModInt[][] expected)
        {
            var got = matrix.LinearSystem(vector);
            got.Length.ShouldBe(expected.Length);
            for (int i = 0; i < got.Length; i++)
                got[i].ShouldBe(expected[i], $"got[{i}]");
        }
    }
}