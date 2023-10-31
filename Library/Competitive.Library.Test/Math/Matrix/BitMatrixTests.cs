using System.Collections;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class BitMatrixTests
    {
        [Fact]
        [Trait("Category", "Normal")]
        public void Construct()
        {
            new BitMatrix(new bool[][]
            {
                new []{ true, false, true},
                new []{ false, true, true },
            }).Value.Should().BeEquivalentTo(new BitArray[]
            {
                new BitArray(new[]{ true, false, true }),
                new BitArray(new[]{ false, true, true }),
            });
            new BitMatrix(new BitArray[]
            {
                new BitArray(new[]{ true, false, true }),
                new BitArray(new[]{ false, true, true }),
            }).Value.Should().BeEquivalentTo(new BitArray[]
            {
                new BitArray(new[]{ true, false, true }),
                new BitArray(new[]{ false, true, true }),
            });
        }

        public static TheoryData Parse_Data => new TheoryData<string, BitMatrix>
        {
            {
                """
                10001
                00110
                """,
                new BitMatrix(new BitArray[]
                {
                    new BitArray(new[]{ true, false, false, false, true }),
                    new BitArray(new[]{ false, false, true, true, false }),
                })
            },
            {
                """
                10101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010
                01010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101
                01000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001
                """,
                new BitMatrix(new BitArray[]
                {
                    new BitArray((int[])(object)new uint[]{ 0b01010101010101010101010101010101, 0b01010101010101010101010101010101,0b01010101010101010101010101010101,0b01010101010101010101010101010101,}),
                    new BitArray((int[])(object)new uint[]{ 0b10101010101010101010101010101010, 0b10101010101010101010101010101010,0b10101010101010101010101010101010,0b10101010101010101010101010101010,}),
                    new BitArray((int[])(object)new uint[]{ 0b10, 0, 0, 0b10000000000000000000000000000000, }),
                })
            },
            {
                $"""
                {new string('0', 130)}
                {new string('1', 130)}
                """,
                new BitMatrix(new BitArray[]
                {
                    new BitArray(130, false),
                    new BitArray(130, true),
                })
            },
        };
        [Theory]
        [Trait("Category", "Normal")]
        [MemberData(nameof(Parse_Data))]
        public void Parse(string text, BitMatrix mat)
        {
            BitMatrix.Parse(text.Split('\n')).Should().BeEquivalentTo(mat);
        }

        public static TheoryData String_Data => new TheoryData<BitMatrix, string>
        {
            {
                new BitMatrix(new[]
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
                new BitMatrix(new[]
                {
                    new[] {  false },
                }),
                "0"
            },
            {
                new BitMatrix(new[]
                {
                    new[] {  true },
                }),
                "1"
            },
        };
        [Theory]
        [Trait("Category", "Normal")]
        [MemberData(nameof(String_Data))]
        public void String(BitMatrix mat, string text)
        {
            mat.ToString().Replace("\r\n", "\n").Should().Be(text);
            BitMatrix.Parse(text.Split('\n')).Should().BeEquivalentTo(mat);
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            var mat = BitMatrix.Parse(new[]
            {
                "101",
                "000",
                "001",
                "111",
            });
            var expected = BitMatrix.Parse(new[]
            {
                "010",
                "111",
                "110",
                "000",
            });
            (-mat).Should().BeEquivalentTo(expected);
            (~mat).Should().BeEquivalentTo(expected);
        }

        public static TheoryData Add_Data => new TheoryData<BitMatrix, BitMatrix, BitMatrix>
        {
            {
                BitMatrix.Zero,
                BitMatrix.Identity,
                BitMatrix.Identity
            },
            {
                BitMatrix.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitMatrix.Parse(new[]
                {
                    "100",
                    "010",
                    "101",
                }),
                BitMatrix.Parse(new[]
                {
                    "001",
                    "010",
                    "100",
                })
            },
            {
                BitMatrix.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitMatrix.Zero,
                BitMatrix.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                })
            },
            {
                BitMatrix.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitMatrix.Identity,
                BitMatrix.Parse(new[]
                {
                    "001",
                    "010",
                    "000",
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void Add(BitMatrix mat1, BitMatrix mat2, BitMatrix expected)
        {
            (mat1 + mat2).Should().BeEquivalentTo(expected);
            (mat2 + mat1).Should().BeEquivalentTo(expected);
            (mat1 - mat2).Should().BeEquivalentTo(expected);
            (mat1 ^ mat2).Should().BeEquivalentTo(expected);
        }

        public static TheoryData Multiply_Data => new TheoryData<BitMatrix, BitMatrix, BitMatrix>
        {
            {
                BitMatrix.Identity,
                BitMatrix.Zero,
                BitMatrix.Zero
            },
            {
                BitMatrix.Parse(new[]
                {
                    "101",
                    "010",
                }),
                BitMatrix.Parse(new[]
                {
                    "10",
                    "11",
                    "10",
                }),
                BitMatrix.Parse(new[]
                {
                    "00",
                    "11",
                })
            },
            {
                BitMatrix.Parse(new[]
                {
                    "00",
                    "11",
                }),
                BitMatrix.Zero,
                BitMatrix.Parse(new[]
                {
                    "00",
                    "00",
                })
            },
            {
                BitMatrix.Parse(new[]
                {
                    "00",
                    "11",
                }),
                BitMatrix.Identity,
                BitMatrix.Parse(new[]
                {
                    "00",
                    "11",
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(BitMatrix mat1, BitMatrix mat2, BitMatrix expected)
        {
            (mat1 * mat2).Should().BeEquivalentTo(expected);
        }

        public static TheoryData MultiplyVector_Data => new TheoryData<BitMatrix, bool[], BitArray>
        {
            {
                BitMatrix.Parse(new[]
                {
                    "101",
                    "010",
                }),
                new[]{ true, false, true},
                new BitArray(new[]{ false, false})
            },
            {
                BitMatrix.Parse(new[]
                {
                    "101",
                    "010",
                }),
                new[]{ true, true, true},
                new BitArray(new[]{ false, true})
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(BitMatrix mat, bool[] vector, BitArray expected)
        {
            (mat * vector).Should().BeEquivalentTo(expected);
            (mat * new BitArray(vector)).Should().BeEquivalentTo(expected);
            mat.Multiply(new BitArray(vector)).Should().BeEquivalentTo(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = BitMatrix.Parse(new[]
            {
                "111",
                "101",
                "011",
            });
            var expecteds = new[]
            {
                BitMatrix.Parse(new[]
                {
                    "100",
                    "010",
                    "001",
                }),
                BitMatrix.Parse(new[]
                {
                    "111",
                    "101",
                    "011",
                }),
                BitMatrix.Parse(new[]
                {
                    "001",
                    "100",
                    "110",
                }),
                BitMatrix.Parse(new[]
                {
                    "011",
                    "111",
                    "010",
                }),
                BitMatrix.Parse(new[]
                {
                    "110",
                    "001",
                    "101",
                }),
                BitMatrix.Parse(new[]
                {
                    "010",
                    "011",
                    "100",
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

        public static TheoryData Inv_Data => new TheoryData<BitMatrix>
        {
            {
                BitMatrix.Parse(new[]
                {
                    "101",
                    "010",
                    "011",
                })
            },
            {
                BitMatrix.Parse(new[]
                {
                    "100",
                    "010",
                    "111",
                })
            },
            {
                BitMatrix.Parse(new[]
                {
                    "001",
                    "010",
                    "111",
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Inv_Data))]
        public void Inv(BitMatrix mat)
        {
            var inv = mat.Inv();
            var expected = BitMatrix.Parse(new[]
            {
                "100",
                "010",
                "001",
            });
            (mat * inv).Should().BeEquivalentTo(expected);
            (inv * mat).Should().BeEquivalentTo(expected);
        }

        public static TheoryData GaussianElimination_Data => new TheoryData<
            bool,
            BitMatrix,
            BitMatrix>
        {
            {
                false,
                BitMatrix.Parse(new[]
                {
                    "1001",
                    "0101",
                    "1111",
                }),
                BitMatrix.Parse(new[]
                {
                    "1001",
                    "0101",
                    "0011",
                })
            },
            {
                false,
                BitMatrix.Parse(new[]
                {
                    "1001",
                    "0101",
                    "1111",
                    "1110",
                    "1100",
                }),
                BitMatrix.Parse(new[]
                {
                    "1001",
                    "0101",
                    "0011",
                    "0001",
                    "0000",
                })
            },
            {
                true,
                BitMatrix.Parse(new[]
                {
                    "1001",
                    "0101",
                    "1111",
                    "1110",
                    "1100",
                }),
                BitMatrix.Parse(new[]
                {
                    "1000",
                    "0100",
                    "0010",
                    "0001",
                    "0000",
                })
            },
        };

        [Theory]
        [MemberData(nameof(GaussianElimination_Data))]
        public void GaussianElimination(bool isReduced, BitMatrix orig, BitMatrix expected)
        {
            var got = orig.GaussianElimination(isReduced);
            got.Should().BeEquivalentTo(expected);
        }

        public static TheoryData LinearSystem_Data => new TheoryData<
            BitMatrix,
            bool[],
            BitArray[]>
        {
            {
                BitMatrix.Parse(new[]
                {
                    "100",
                    "010",
                    "111",
                }),
                new[] { true, true, true, },
                new[]
                {
                    new BitArray(new[]{ true, true, true, })
                }
            },
            {
                BitMatrix.Parse(new[]
                {
                    "100",
                    "010",
                    "101",
                }),
                new[] { true, true, true, },
                new[]
                {
                    new BitArray(new[]{ true, true, false, })
                }
            },
            {
                BitMatrix.Parse(new[]
                {
                    "100",
                    "010",
                    "110",
                }),
                new[] { true, true, true, },
                new BitArray[0]
            },
            {
                BitMatrix.Parse(new[]
                {
                    "100",
                    "010",
                    "110",
                }),
                new[] { true, true, false, },
                new[]
                {
                    new BitArray(new[]{ true, true, false, }),
                    new BitArray(new[]{ false, false, true, }),
                }
            },
            {
                BitMatrix.Parse(new[]
                {
                    "000",
                    "000",
                }),
                new[] { false, false, },
                new[]
                {
                    new BitArray(new[]{ false, false, false, }),
                    new BitArray(new[]{ true, false, false, }),
                    new BitArray(new[]{ false, true, false, }),
                    new BitArray(new[]{ false, false, true, }),
                }
            },
        };

        [Theory]
        [MemberData(nameof(LinearSystem_Data))]
        public void LinearSystem(BitMatrix matrix, bool[] vector, BitArray[] expected)
        {
            var got = matrix.LinearSystem(vector);
            got.Should().HaveSameCount(expected);
            for (int i = 0; i < got.Length; i++)
                got[i].Should().BeEquivalentTo(expected[i], because: "got[{0}]", i);
        }
    }
}
