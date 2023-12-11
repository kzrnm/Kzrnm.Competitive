using System;
using System.Linq;
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
                [true, false, true],
                [false, true, true],
            })._v.Should().BeEquivalentTo(new UInt128[]
            {
                0b101ul,
                0b110ul,
            });
            new BitMatrix128(new UInt128[]
            {
                0b101ul,
                0b110ul,
            })._v.Should().BeEquivalentTo(new UInt128[]
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
                    new UInt128(0b0101010101010101010101010101010101010101010101010101010101010101,0b0101010101010101010101010101010101010101010101010101010101010101),
                    new UInt128(0b1010101010101010101010101010101010101010101010101010101010101010,0b1010101010101010101010101010101010101010101010101010101010101010),
                    new UInt128(0b1000000000000000000000000000000000000000000000000000000000000000,0b10),
                })
            },
            {
                """
                11001
                10010
                """,
                new BitMatrix128(new UInt128[]
                {
                    0b10011,
                    0b01001,
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
                new BitMatrix128([
                    [true, false,  true],
                    [false, false, false],
                    [false, false,  true],
                    [true,  true,  true],
                ]),
                $$$"""
                {{{"101".PadRight(128, '0')}}}
                {{{"000".PadRight(128, '0')}}}
                {{{"001".PadRight(128, '0')}}}
                {{{"111".PadRight(128, '0')}}}
                """.Replace("\r\n", "\n")
            },
            {
                new BitMatrix128(new[]
                {
                    new[] {  false },
                }),
                "0".PadRight(128, '0')
            },
            {
                new BitMatrix128(new[]
                {
                    new[] {  true },
                }),
                "1".PadRight(128, '0')
            },
            {
                new BitMatrix128(new[]
                {
                    new UInt128(0b101, 0b1100111),
                }),
                "11100110000000000000000000000000000000000000000000000000000000001010000000000000000000000000000000000000000000000000000000000000"
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
            var mat = BitMatrix128.Parse(
            [
                "101",
                "000",
                "001",
                "111",
            ]);
            var expected = BitMatrix128.Parse(
            [
                "010".PadRight(128, '1'),
                "111".PadRight(128, '1'),
                "110".PadRight(128, '1'),
                "000".PadRight(128, '1'),
            ]);
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
                BitMatrix128.Parse(
                [
                    "101",
                    "000",
                    "001",
                ]),
                BitMatrix128.Parse(
                [
                    "100",
                    "010",
                    "101",
                ]),
                BitMatrix128.Parse(
                [
                    "001",
                    "010",
                    "100",
                ])
            },
            {
                BitMatrix128.Parse(
                [
                    "101",
                    "000",
                    "001",
                ]),
                BitMatrix128.Zero,
                BitMatrix128.Parse(
                [
                    "101",
                    "000",
                    "001",
                ])
            },
            {
                BitMatrix128.Parse(
                [
                    "101",
                    "000",
                    "001",
                ]),
                BitMatrix128.Identity,
                BitMatrix128.Parse(
                [
                    "001",
                    "010",
                    "000",
                ])
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
                BitMatrix128.Parse(
                [
                    "101",
                    "010",
                ]),
                BitMatrix128.Parse(
                [
                    "10",
                    "11",
                    "10",
                ]),
                BitMatrix128.Parse(
                [
                    "00",
                    "11",
                ])
            },
            {
                BitMatrix128.Parse(
                [
                    "00",
                    "11",
                ]),
                BitMatrix128.Zero,
                BitMatrix128.Parse(
                [
                    "00",
                    "00",
                ])
            },
            {
                BitMatrix128.Parse(
                [
                    "00",
                    "11",
                ]),
                BitMatrix128.Identity,
                BitMatrix128.Parse(
                [
                    "00",
                    "11",
                ])
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(BitMatrix128 mat1, BitMatrix128 mat2, BitMatrix128 expected)
        {
            (mat1 * mat2).Should().BeEquivalentTo(expected);
        }

        public static TheoryData MultiplyVector_Data => new TheoryData<BitMatrix128, bool[], UInt128, UInt128>
        {
            {
                BitMatrix128.Parse(
                [
                    "101",
                    "010",
                ]),
                new[]{ true, false, true},
                0b101ul,
                0b0ul
            },
            {
                BitMatrix128.Parse(
                [
                    "101",
                    "010",
                ]),
                new[]{ true, true, true},
                0b111ul,
                0b10ul
            },
            {
                BitMatrix128.Parse(
                [
                    "101",
                    "010",
                ]),
                new[]{ false, true, true},
                0b110ul,
                0b11ul
            },
            {
                BitMatrix128.Parse(
                    Enumerable.Repeat(new string('0', 128), 128)
                    .Select((s, i) => s.Remove(i, 1).Insert(i, "1"))
                    .ToArray()
                ),
                new[] { true, false, false, true, false, true, false, false, true, true, false, true, false, true, true, true, true, true, false, true, false, false, false, false, true, true, true, false, true, false, true, true, false, false, false, false, true, false, true, false, true, false, true, true, false, true, false, false, true, true, true, false, false, true, false, true, false, true, false, true, false, false, true, true, true, false, true, false, true, false, true, false, true, false, true, false, true, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, false, true, false, false, false, false, true, false, true, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, false, true },
                new UInt128(0b1001010101010101010101101000010010101010101010101011010101010101ul, 0b1100101010100111001011010101000011010111000010111110101100101001ul),
                new UInt128(0b1001010101010101010101101000010010101010101010101011010101010101ul, 0b1100101010100111001011010101000011010111000010111110101100101001ul)
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(BitMatrix128 mat, bool[] vector, UInt128 vectorArray, UInt128 expected)
        {
            (mat * vector).Should().Be(expected);
            (mat * vectorArray).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = BitMatrix128.Parse(
            [
                "111",
                "101",
                "011",
            ]);
            var expecteds = new[]
            {
                BitMatrix128.Parse(
                [
                    "100",
                    "010",
                    "001",
                ]),
                BitMatrix128.Parse(
                [
                    "111",
                    "101",
                    "011",
                ]),
                BitMatrix128.Parse(
                [
                    "001",
                    "100",
                    "010",
                ]),
                BitMatrix128.Parse(
                [
                    "111",
                    "011",
                    "101",
                ]),
                BitMatrix128.Parse(
                [
                    "001",
                    "010",
                    "100",
                ]),
                BitMatrix128.Parse(
                [
                    "111",
                    "101",
                    "011",
                ]),
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
                BitMatrix128.Parse(
                [
                    "1001",
                    "0101",
                    "1111",
                ]),
                BitMatrix128.Parse(
                [
                    "1001",
                    "0101",
                    "0011",
                ])
            },
            {
                false,
                BitMatrix128.Parse(
                [
                    "1001",
                    "0101",
                    "1111",
                    "1110",
                    "1100",
                ]),
                BitMatrix128.Parse(
                [
                    "1001",
                    "0101",
                    "0011",
                    "0001",
                    "0000",
                ])
            },
            {
                true,
                BitMatrix128.Parse(
                [
                    "1001",
                    "0101",
                    "1111",
                    "1110",
                    "1100",
                ]),
                BitMatrix128.Parse(
                [
                    "1000",
                    "0100",
                    "0010",
                    "0001",
                    "0000",
                ])
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
