using System.Collections;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class BitOrMatrixTests
    {
        [Fact]
        [Trait("Category", "Normal")]
        public void Construct()
        {
            new BitOrMatrix(new bool[][]
            {
                new []{ true, false, true},
                new []{ false, true, true },
            }).Value.Should().BeEquivalentTo(new BitArray[]
            {
                new BitArray(new[]{ true, false, true }),
                new BitArray(new[]{ false, true, true }),
            });
            new BitOrMatrix(new BitArray[]
            {
                new BitArray(new[]{ true, false, true }),
                new BitArray(new[]{ false, true, true }),
            }).Value.Should().BeEquivalentTo(new BitArray[]
            {
                new BitArray(new[]{ true, false, true }),
                new BitArray(new[]{ false, true, true }),
            });
        }


        public static TheoryData String_Data => new TheoryData<BitOrMatrix, string>
        {
            {
                new BitOrMatrix(new[]
                {
                    new[] {  true, false,  true },
                    new[] { false, false, false },
                    new[] { false, false,  true },
                    new[] {  true,  true,  true },
                }),
                """
                101
                000
                001
                111
                """.Replace("\r\n", "\n")
            },
            {
                new BitOrMatrix(new[]
                {
                    new[] {  false },
                }),
                "0"
            },
            {
                new BitOrMatrix(new[]
                {
                    new[] {  true },
                }),
                "1"
            },
        };
        [Theory]
        [Trait("Category", "Normal")]
        [MemberData(nameof(String_Data))]
        public void String(BitOrMatrix mat, string text)
        {
            mat.ToString().Replace("\r\n", "\n").Should().Be(text);
            BitOrMatrix.Parse(text.Split('\n')).Should().BeEquivalentTo(mat);
        }

        public static TheoryData Add_Data => new TheoryData<BitOrMatrix, BitOrMatrix, BitOrMatrix>
        {
            {
                BitOrMatrix.Zero,
                BitOrMatrix.Identity,
                BitOrMatrix.Identity
            },
            {
                BitOrMatrix.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitOrMatrix.Parse(new[]
                {
                    "100",
                    "010",
                    "101",
                }),
                BitOrMatrix.Parse(new[]
                {
                    "101",
                    "010",
                    "101",
                })
            },
            {
                BitOrMatrix.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitOrMatrix.Zero,
                BitOrMatrix.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                })
            },
            {
                BitOrMatrix.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitOrMatrix.Identity,
                BitOrMatrix.Parse(new[]
                {
                    "101",
                    "010",
                    "001",
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void Add(BitOrMatrix mat1, BitOrMatrix mat2, BitOrMatrix expected)
        {
            (mat1 + mat2).Should().BeEquivalentTo(expected);
            (mat2 + mat1).Should().BeEquivalentTo(expected);
#if !NET7_0_OR_GREATER
            default(BitOrMatrix.Operator).Add(mat1, mat2).Should().BeEquivalentTo(expected);
            default(BitOrMatrix.Operator).Add(mat2, mat1).Should().BeEquivalentTo(expected);
#endif
        }

        public static TheoryData Multiply_Data => new TheoryData<BitOrMatrix, BitOrMatrix, BitOrMatrix>
        {
            {
                BitOrMatrix.Identity,
                BitOrMatrix.Zero,
                BitOrMatrix.Zero
            },
            {
                BitOrMatrix.Parse(new[]
                {
                    "101",
                    "010",
                }),
                BitOrMatrix.Parse(new[]
                {
                    "10",
                    "11",
                    "10",
                }),
                BitOrMatrix.Parse(new[]
                {
                    "10",
                    "11",
                })
            },
            {
                BitOrMatrix.Parse(new[]
                {
                    "00",
                    "01",
                }),
                BitOrMatrix.Parse(new[]
                {
                    "00",
                    "01",
                }),
                BitOrMatrix.Parse(new[]
                {
                    "00",
                    "01",
                })
            },
            {
                BitOrMatrix.Parse(new[]
                {
                    "11",
                    "11",
                }),
                BitOrMatrix.Zero,
                BitOrMatrix.Parse(new[]
                {
                    "00",
                    "00",
                })
            },
            {
                BitOrMatrix.Parse(new[]
                {
                    "00",
                    "11",
                }),
                BitOrMatrix.Identity,
                BitOrMatrix.Parse(new[]
                {
                    "00",
                    "11",
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(BitOrMatrix mat1, BitOrMatrix mat2, BitOrMatrix expected)
        {
            (mat1 * mat2).Should().BeEquivalentTo(expected);
#if !NET7_0_OR_GREATER
            default(BitOrMatrix.Operator).Multiply(mat1, mat2).Should().BeEquivalentTo(expected);
#endif
        }

        public static TheoryData MultiplyVector_Data => new TheoryData<BitOrMatrix, bool[], BitArray>
        {
            {
                BitOrMatrix.Parse(new[]
                {
                    "101",
                    "010",
                }),
                new[]{ true, false, true},
                new BitArray(new[]{ true, false})
            },
            {
                BitOrMatrix.Parse(new[]
                {
                    "101",
                    "010",
                }),
                new[]{ true, true, true},
                new BitArray(new[]{ true, true})
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(BitOrMatrix mat, bool[] vector, BitArray expected)
        {
            (mat * vector).Should().BeEquivalentTo(expected);
            (mat * new BitArray(vector)).Should().BeEquivalentTo(expected);
            mat.Multiply(new BitArray(vector)).Should().BeEquivalentTo(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = BitOrMatrix.Parse(new[]
            {
                "111",
                "101",
                "011",
            });
            var expecteds = new[]
            {
                BitOrMatrix.Parse(new[]
                {
                    "100",
                    "010",
                    "001",
                }),
                BitOrMatrix.Parse(new[]
                {
                    "111",
                    "101",
                    "011",
                }),
                BitOrMatrix.Parse(new[]
                {
                    "111",
                    "111",
                    "111",
                }),
                BitOrMatrix.Parse(new[]
                {
                    "111",
                    "111",
                    "111",
                }),
            };
            var cur = orig;
            for (int i = 1; i < expecteds.Length; i++)
            {
                cur.Should().BeEquivalentTo(expecteds[i]);
                orig.Pow(i).Should().BeEquivalentTo(cur);
                cur *= orig;
            }
        }
    }
}
