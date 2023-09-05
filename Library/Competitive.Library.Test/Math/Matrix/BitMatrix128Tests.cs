#if NET7_0_OR_GREATER
using System;
using BitMatrix128 = Kzrnm.Competitive.BitMatrix<System.UInt128>;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class BitMatrix128Tests
    {
        [Fact]
        [Trait("Category", "Normal")]
        public void Construct()
        {
            new BitMatrix128(new bool[][]
            {
                new []{ true, false, true},
                new []{ false, true, true },
            }).Value.Should().BeEquivalentTo(new UInt128[]
            {
                0b101ul,
                0b110ul,
            });
            new BitMatrix128(new UInt128[]
            {
                0b101ul,
                0b110ul,
            }).Value.Should().BeEquivalentTo(new UInt128[]
            {
                0b101ul,
                0b110ul,
            });
        }

        public static TheoryData Parse_Data => new TheoryData<string, BitMatrix128>
        {
            {
                """
                10101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010
                01010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101
                01000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001
                """,
                new BitMatrix128(new UInt128[]
                {
                    new UInt128(0b1010101010101010101010101010101010101010101010101010101010101010,0b1010101010101010101010101010101010101010101010101010101010101010),
                    new UInt128(0b0101010101010101010101010101010101010101010101010101010101010101,0b0101010101010101010101010101010101010101010101010101010101010101),
                    new UInt128(0b0100000000000000000000000000000000000000000000000000000000000000,0b1),
                })
            },
        };
        [Theory]
        [Trait("Category", "Normal")]
        [MemberData(nameof(Parse_Data))]
        public void Parse(string text, BitMatrix128 mat)
        {
            BitMatrix128.Parse(text.Split('\n')).Should().BeEquivalentTo(mat);
        }

        public static TheoryData String_Data => new TheoryData<BitMatrix128, string>
        {
            {
                new BitMatrix128(new[]
                {
                    new[] {  true, false,  true },
                    new[] { false, false, false },
                    new[] { false, false,  true },
                    new[] {  true,  true,  true },
                }),
                $$$"""
                {{{"101".PadLeft(128, '0')}}}
                {{{"000".PadLeft(128, '0')}}}
                {{{"100".PadLeft(128, '0')}}}
                {{{"111".PadLeft(128, '0')}}}
                """.Replace("\r\n", "\n")
            },
            {
                new BitMatrix128(new[]
                {
                    new[] {  false },
                }),
                "0".PadLeft(128, '0')
            },
            {
                new BitMatrix128(new[]
                {
                    new[] {  true },
                }),
                "1".PadLeft(128, '0')
            },
        };
        [Theory]
        [Trait("Category", "Normal")]
        [MemberData(nameof(String_Data))]
        public void String(BitMatrix128 mat, string text)
        {
            mat.ToString().Replace("\r\n", "\n").Should().Be(text);
            BitMatrix128.Parse(text.Split('\n')).Should().BeEquivalentTo(mat);
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            var mat = BitMatrix128.Parse(new[]
            {
                "101",
                "000",
                "001",
                "111",
            });
            var expected = BitMatrix128.Parse(new[]
            {
                "010".PadLeft(128, '1'),
                "111".PadLeft(128, '1'),
                "110".PadLeft(128, '1'),
                "000".PadLeft(128, '1'),
            });
            (-mat).Should().BeEquivalentTo(expected);
            (~mat).Should().BeEquivalentTo(expected);
        }

        public static TheoryData Add_Data => new TheoryData<BitMatrix128, BitMatrix128, BitMatrix128>
        {
            {
                BitMatrix128.Zero,
                BitMatrix128.Identity,
                BitMatrix128.Identity
            },
            {
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitMatrix128.Parse(new[]
                {
                    "100",
                    "010",
                    "101",
                }),
                BitMatrix128.Parse(new[]
                {
                    "001",
                    "010",
                    "100",
                })
            },
            {
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitMatrix128.Zero,
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                })
            },
            {
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "000",
                    "001",
                }),
                BitMatrix128.Identity,
                BitMatrix128.Parse(new[]
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
        public void Add(BitMatrix128 mat1, BitMatrix128 mat2, BitMatrix128 expected)
        {
            (mat1 + mat2).Should().BeEquivalentTo(expected);
            (mat2 + mat1).Should().BeEquivalentTo(expected);
            (mat1 - mat2).Should().BeEquivalentTo(expected);
            (mat1 ^ mat2).Should().BeEquivalentTo(expected);
        }

        public static TheoryData Multiply_Data => new TheoryData<BitMatrix128, BitMatrix128, BitMatrix128>
        {
            {
                BitMatrix128.Identity,
                BitMatrix128.Zero,
                BitMatrix128.Zero
            },
            {
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "010",
                }),
                BitMatrix128.Parse(new[]
                {
                    "10",
                    "11",
                    "10",
                }),
                BitMatrix128.Parse(new[]
                {
                    "00",
                    "11",
                })
            },
            {
                BitMatrix128.Parse(new[]
                {
                    "00",
                    "11",
                }),
                BitMatrix128.Zero,
                BitMatrix128.Parse(new[]
                {
                    "00",
                    "00",
                })
            },
            {
                BitMatrix128.Parse(new[]
                {
                    "00",
                    "11",
                }),
                BitMatrix128.Identity,
                BitMatrix128.Parse(new[]
                {
                    "00",
                    "11",
                })
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(BitMatrix128 mat1, BitMatrix128 mat2, BitMatrix128 expected)
        {
            (mat1 * mat2).Should().BeEquivalentTo(expected);
        }

        public static TheoryData MultiplyVector_Data => new TheoryData<BitMatrix128, bool[], UInt128>
        {
            {
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "010",
                }),
                new[]{ true, false, true},
                0b0ul
            },
            {
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "010",
                }),
                new[]{ true, true, true},
                0b10ul
            },
            {
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "010",
                }),
                new[]{ false, true, true},
                0b11ul
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(BitMatrix128 mat, bool[] vector, UInt128 expected)
        {
            (mat * vector).Should().Be(expected);
        }

        public static TheoryData MultiplyVectorNumber_Data => new TheoryData<BitMatrix128, UInt128, UInt128>
        {
            {
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "010",
                }),
                0b101ul,
                0b0ul
            },
            {
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "010",
                }),
                0b111ul,
                0b10ul
            },
            {
                BitMatrix128.Parse(new[]
                {
                    "101",
                    "010",
                }),
                0b011ul,
                0b11ul
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVectorNumber_Data))]
        public void MultiplyVectorNumber(BitMatrix128 mat, UInt128 vector, UInt128 expected)
        {
            (mat * vector).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = BitMatrix128.Parse(new[]
            {
                "111",
                "101",
                "011",
            });
            var expecteds = new[]
            {
                BitMatrix128.Parse(new[]
                {
                    "100",
                    "010",
                    "001",
                }),
                BitMatrix128.Parse(new[]
                {
                    "111",
                    "101",
                    "011",
                }),
                BitMatrix128.Parse(new[]
                {
                    "001",
                    "100",
                    "110",
                }),
                BitMatrix128.Parse(new[]
                {
                    "011",
                    "111",
                    "010",
                }),
                BitMatrix128.Parse(new[]
                {
                    "110",
                    "001",
                    "101",
                }),
                BitMatrix128.Parse(new[]
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

        public static TheoryData GaussianElimination_Data => new TheoryData<
            bool,
            BitMatrix128,
            BitMatrix128>
        {
            {
                false,
                BitMatrix128.Parse(new[]
                {
                    "1001",
                    "0101",
                    "1111",
                }),
                BitMatrix128.Parse(new[]
                {
                    "1001",
                    "0101",
                    "0011",
                })
            },
            {
                false,
                BitMatrix128.Parse(new[]
                {
                    "1001",
                    "0101",
                    "1111",
                    "1110",
                    "1100",
                }),
                BitMatrix128.Parse(new[]
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
                BitMatrix128.Parse(new[]
                {
                    "1001",
                    "0101",
                    "1111",
                    "1110",
                    "1100",
                }),
                BitMatrix128.Parse(new[]
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
        public void GaussianElimination(bool isReduced, BitMatrix128 orig, BitMatrix128 expected)
        {
            var got = orig.GaussianElimination(isReduced);
            got.Should().BeEquivalentTo(expected);
        }
    }
}
#endif