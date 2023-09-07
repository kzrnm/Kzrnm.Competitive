using System.Linq;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class BitMatrix64Tests
    {
        [Fact]
        [Trait("Category", "Normal")]
        public void Construct()
        {
            new BitMatrix64(new bool[][]
            {
                new []{ true, false, true},
                new []{ false, true, true },
            }).Value.Should().BeEquivalentTo(new ulong[]
            {
                0b101ul,
                0b110ul,
            });
            new BitMatrix64(new ulong[]
            {
                0b101ul,
                0b110ul,
            }).Value.Should().BeEquivalentTo(new ulong[]
            {
                0b101ul,
                0b110ul,
            });
        }

        public static TheoryData Parse_Data => new TheoryData<string, BitMatrix64>
        {
            {
                """
                1010101010101010101010101010101010101010101010101010101010101010
                0101010101010101010101010101010101010101010101010101010101010101
                """,
                new BitMatrix64(new ulong[]
                {
                    0b0101010101010101010101010101010101010101010101010101010101010101,
                    0b1010101010101010101010101010101010101010101010101010101010101010,
                })
            },
            {
                """
                11001
                10010
                """,
                new BitMatrix64(new ulong[]
                {
                    0b10011,
                    0b01001,
                })
            },
        };
        [Theory]
        [Trait("Category", "Normal")]
        [MemberData(nameof(Parse_Data))]
        public void Parse(string text, BitMatrix64 mat)
        {
            BitMatrix64.Parse(text.Split('\n')).Should().BeEquivalentTo(mat);
        }

        public static TheoryData String_Data => new TheoryData<BitMatrix64, string>
        {
            {
                new BitMatrix64(new[]
                {
                    new[] {  true, false,  true },
                    new[] { false, false, false },
                    new[] { false, false,  true },
                    new[] {  true,  true,  true },
                }),
                $$$"""
                {{{"101".PadRight(64, '0')}}}
                {{{"000".PadRight(64, '0')}}}
                {{{"001".PadRight(64, '0')}}}
                {{{"111".PadRight(64, '0')}}}
                """.Replace("\r\n", "\n")
            },
            {
                new BitMatrix64(new[]
                {
                    new[] {  false },
                }),
                "0".PadRight(64, '0')
            },
            {
                new BitMatrix64(new[]
                {
                    new[] {  true },
                }),
                "1".PadRight(64, '0')
            },
            {
                new BitMatrix64(new[]
                {
                    0b1100111ul,
                }),
                "1110011000000000000000000000000000000000000000000000000000000000"
            },
        };
        [Theory]
        [Trait("Category", "Normal")]
        [MemberData(nameof(String_Data))]
        public void String(BitMatrix64 mat, string text)
        {
            mat.ToString().Replace("\r\n", "\n").Should().Be(text);
            BitMatrix64.Parse(text.Split('\n')).Should().BeEquivalentTo(mat);
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            var mat = BitMatrix64.Parse(new[]
            {
                "101",
                "000",
                "001",
                "111",
            });
            var expected = BitMatrix64.Parse(new[]
            {
                "010".PadRight(64, '1'),
                "111".PadRight(64, '1'),
                "110".PadRight(64, '1'),
                "000".PadRight(64, '1'),
            });
            (-mat).Should().BeEquivalentTo(expected);
            (~mat).Should().BeEquivalentTo(expected);
        }

        public static TheoryData Add_Data => new TheoryData<BitMatrix64, BitMatrix64, BitMatrix64>
        {
            {
                BitMatrix64.Zero,
                BitMatrix64.Identity,
                BitMatrix64.Identity
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitMatrix64.Parse(new[]
                {
                    "100",
                    "010",
                    "101",
                }),
                BitMatrix64.Parse(new[]
                {
                    "001",
                    "010",
                    "100",
                })
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitMatrix64.Zero,
                BitMatrix64.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                })
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitMatrix64.Identity,
                BitMatrix64.Parse(new[]
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
        public void Add(BitMatrix64 mat1, BitMatrix64 mat2, BitMatrix64 expected)
        {
            (mat1 + mat2).Should().BeEquivalentTo(expected);
            (mat2 + mat1).Should().BeEquivalentTo(expected);
            (mat1 - mat2).Should().BeEquivalentTo(expected);
            (mat1 ^ mat2).Should().BeEquivalentTo(expected);
#if !NET7_0_OR_GREATER
            default(BitMatrix64.Operator).Add(mat1, mat2).Should().BeEquivalentTo(expected);
            default(BitMatrix64.Operator).Add(mat2, mat1).Should().BeEquivalentTo(expected);
            default(BitMatrix64.Operator).Subtract(mat1, mat2).Should().BeEquivalentTo(expected);
#endif
        }

        public static TheoryData Multiply_Data => new TheoryData<BitMatrix64, BitMatrix64, BitMatrix64>
        {
            {
                BitMatrix64.Identity,
                BitMatrix64.Zero,
                BitMatrix64.Zero
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "101",
                    "010",
                }),
                BitMatrix64.Parse(new[]
                {
                    "10",
                    "11",
                    "10",
                }),
                BitMatrix64.Parse(new[]
                {
                    "00",
                    "11",
                })
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "00",
                    "11",
                }),
                BitMatrix64.Zero,
                BitMatrix64.Parse(new[]
                {
                    "00",
                    "00",
                })
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "00",
                    "11",
                }),
                BitMatrix64.Identity,
                BitMatrix64.Parse(new[]
                {
                    "00",
                    "11",
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(BitMatrix64 mat1, BitMatrix64 mat2, BitMatrix64 expected)
        {
            (mat1 * mat2).Should().BeEquivalentTo(expected);
#if !NET7_0_OR_GREATER
            default(BitMatrix64.Operator).Multiply(mat1, mat2).Should().BeEquivalentTo(expected);
#endif
        }

        public static TheoryData MultiplyVector_Data => new TheoryData<BitMatrix64, bool[], ulong, ulong>
        {
            {
                BitMatrix64.Parse(new[]
                {
                    "101",
                    "010",
                }),
                new[]{ true, false, true},
                0b101ul,
                0b0ul
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "101",
                    "010",
                }),
                new[]{ true, true, true},
                0b111ul,
                0b10ul
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "101",
                    "010",
                }),
                new[]{ false, true, true},
                0b110ul,
                0b11ul
            },
            {
                BitMatrix64.Parse(
                    Enumerable.Repeat(new string('0', 64), 64)
                    .Select((s, i) => s.Remove(i, 1).Insert(i, "1"))
                    .ToArray()
                ),
                new[] { true, false, false, true, false, true, false, false, true, true, false, true, false, true, true, true, true, true, false, true, false, false, false, false, true, true, true, false, true, false, true, true, false, false, false, false, true, false, true, false, true, false, true, true, false, true, false, false, true, true, true, false, false, true, false, true, false, true, false, true, false, false, true, true },
                0b1100101010100111001011010101000011010111000010111110101100101001ul,
                0b1100101010100111001011010101000011010111000010111110101100101001ul
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(BitMatrix64 mat, bool[] vector, ulong vectorArray, ulong expected)
        {
            (mat * vector).Should().Be(expected);
            (mat * vectorArray).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = BitMatrix64.Parse(new[]
            {
                "111",
                "101",
                "011",
            });
            var expecteds = new[]
            {
                BitMatrix64.Parse(new[]
                {
                    "100",
                    "010",
                    "001",
                }),
                BitMatrix64.Parse(new[]
                {
                    "111",
                    "101",
                    "011",
                }),
                BitMatrix64.Parse(new[]
                {
                    "001",
                    "100",
                    "010",
                }),
                BitMatrix64.Parse(new[]
                {
                    "111",
                    "011",
                    "101",
                }),
                BitMatrix64.Parse(new[]
                {
                    "001",
                    "010",
                    "100",
                }),
                BitMatrix64.Parse(new[]
                {
                    "111",
                    "101",
                    "011",
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

        public static TheoryData GaussianElimination_Data => new TheoryData<
            bool,
            BitMatrix64,
            BitMatrix64>
        {
            {
                false,
                BitMatrix64.Parse(new[]
                {
                    "1001",
                    "0101",
                    "1111",
                }),
                BitMatrix64.Parse(new[]
                {
                    "1001",
                    "0101",
                    "0011",
                })
            },
            {
                false,
                BitMatrix64.Parse(new[]
                {
                    "1001",
                    "0101",
                    "1111",
                    "1110",
                    "1100",
                }),
                BitMatrix64.Parse(new[]
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
                BitMatrix64.Parse(new[]
                {
                    "1001",
                    "0101",
                    "1111",
                    "1110",
                    "1100",
                }),
                BitMatrix64.Parse(new[]
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
        public void GaussianElimination(bool isReduced, BitMatrix64 orig, BitMatrix64 expected)
        {
            var got = orig.GaussianElimination(isReduced);
            got.Should().BeEquivalentTo(expected);
        }

        public static TheoryData LinearSystem_Data => new TheoryData<BitMatrix64, ulong, ulong[]>
        {
            {
                BitMatrix64.Parse(new[]
                {
                    "100",
                    "010",
                    "111",
                }),
                0b111ul,
                new[]
                {
                    0b111ul
                }
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "100",
                    "010",
                    "101",
                }),
                0b111ul,
                new[]
                {
                    0b011ul
                }
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "100",
                    "010",
                    "110",
                }),
                0b111ul,
                new ulong[0]
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "100",
                    "010",
                    "110",
                }),
                0b110,
                new[]
                {
                    0b010ul,
                    0b100ul,
                }
            },
            {
                BitMatrix64.Parse(new[]
                {
                    "000",
                    "000",
                }),
                0ul,
                new[]
                {
                    0ul,
                    1ul,
                }
            },
        };

        [Theory]
        [MemberData(nameof(LinearSystem_Data))]
        public void LinearSystem(BitMatrix64 matrix, ulong vector, ulong[] expected)
        {
            var got = matrix.LinearSystem(vector);
            got.Should().HaveSameCount(expected);
            for (int i = 0; i < got.Length; i++)
                got[i].Should().Be(expected[i], because: "got[{0}]", i);
        }
    }
}
