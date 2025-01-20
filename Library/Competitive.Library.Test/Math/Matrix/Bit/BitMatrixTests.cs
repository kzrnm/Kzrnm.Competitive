using System.Collections;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class BitMatrixTests
    {
        [Fact]
        [Trait("Category", "Normal")]
        public void Construct()
        {
            new BitMatrix([
                [true, false, true],
                [false, true, true],
            ])._v.ShouldBe([
                new BitArray([true, false, true]),
                new BitArray([false, true, true]),
            ]);
            new BitMatrix([
                new BitArray([true, false, true]),
                new BitArray([false, true, true]),
            ])._v.ShouldBe([
                new BitArray([true, false, true]),
                new BitArray([false, true, true]),
            ]);
        }

        public static TheoryData<string, BitMatrix> Parse_Data => new()
        {
            {
                """
                10001
                00110
                """,
                new BitMatrix([
                    new BitArray([true, false, false, false, true ]),
                    new BitArray([false, false, true, true, false ]),
                ])
            },
            {
                """
                10101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010
                01010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101
                01000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001
                """,
                new BitMatrix([
                    new BitArray([
                        0b01010101010101010101010101010101,
                        0b01010101010101010101010101010101,
                        0b01010101010101010101010101010101,
                        0b01010101010101010101010101010101,
                    ]),
                    new BitArray([
                        unchecked((int)0b10101010101010101010101010101010),
                        unchecked((int)0b10101010101010101010101010101010),
                        unchecked((int)0b10101010101010101010101010101010),
                        unchecked((int)0b10101010101010101010101010101010),
                    ]),
                    new BitArray([
                        0b10,
                        0,
                        0,
                        unchecked((int)0b10000000000000000000000000000000),
                    ]),
                ])
            },
            {
                $"""
                {new string('0', 130)}
                {new string('1', 130)}
                """,
                new BitMatrix([
                    new BitArray(130, false),
                    new BitArray(130, true),
                ])
            },
        };
        [Theory]
        [Trait("Category", "Normal")]
        [MemberData(nameof(Parse_Data))]
        public void Parse(string text, BitMatrix mat)
        {
            BitMatrix.Parse(text.Split('\n')).ShouldBe(mat);
        }

        public static TheoryData<BitMatrix, string> String_Data => new()
        {
            {
                new BitMatrix([
                    [true, false,  true],
                    [false, false, false],
                    [false, false,  true],
                    [true,  true,  true],
                ]),
                """
                101
                000
                001
                111
                """.Replace("\r\n", "\n")
            },
            {
                new BitMatrix([
                    [false],
                ]),
                "0"
            },
            {
                new BitMatrix([
                    [true],
                ]),
                "1"
            },
        };
        [Theory]
        [Trait("Category", "Normal")]
        [MemberData(nameof(String_Data))]
        public void String(BitMatrix mat, string text)
        {
            mat.ToString().Replace("\r\n", "\n").ShouldBe(text);
            BitMatrix.Parse(text.Split('\n')).ShouldBe(mat);
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            var mat = BitMatrix.Parse(
            [
                "101",
                "000",
                "001",
                "111",
            ]);
            var expected = BitMatrix.Parse(
            [
                "010",
                "111",
                "110",
                "000",
            ]);
            (-mat).ShouldBe(expected);
            (~mat).ShouldBe(expected);
        }

        public static TheoryData<BitMatrix, BitMatrix, BitMatrix> Add_Data => new()
        {
            {
                BitMatrix.Zero,
                BitMatrix.Identity,
                BitMatrix.Identity
            },
            {
                BitMatrix.Parse(
                [
                    "101",
                    "000",
                    "001",
                ]),
                BitMatrix.Parse(
                [
                    "100",
                    "010",
                    "101",
                ]),
                BitMatrix.Parse(
                [
                    "001",
                    "010",
                    "100",
                ])
            },
            {
                BitMatrix.Parse(
                [
                    "101",
                    "000",
                    "001",
                ]),
                BitMatrix.Zero,
                BitMatrix.Parse(
                [
                    "101",
                    "000",
                    "001",
                ])
            },
            {
                BitMatrix.Parse(
                [
                    "101",
                    "000",
                    "001",
                ]),
                BitMatrix.Identity,
                BitMatrix.Parse(
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
        public void Add(BitMatrix mat1, BitMatrix mat2, BitMatrix expected)
        {
            (mat1 + mat2).ShouldBe(expected);
            (mat2 + mat1).ShouldBe(expected);
            (mat1 - mat2).ShouldBe(expected);
            (mat1 ^ mat2).ShouldBe(expected);
        }

        public static TheoryData<BitMatrix, BitMatrix, BitMatrix> Multiply_Data => new()
        {
            {
                BitMatrix.Identity,
                BitMatrix.Zero,
                BitMatrix.Zero
            },
            {
                BitMatrix.Parse(
                [
                    "101",
                    "010",
                ]),
                BitMatrix.Parse(
                [
                    "10",
                    "11",
                    "10",
                ]),
                BitMatrix.Parse(
                [
                    "00",
                    "11",
                ])
            },
            {
                BitMatrix.Parse(
                [
                    "00",
                    "11",
                ]),
                BitMatrix.Zero,
                BitMatrix.Parse(
                [
                    "00",
                    "00",
                ])
            },
            {
                BitMatrix.Parse(
                [
                    "00",
                    "11",
                ]),
                BitMatrix.Identity,
                BitMatrix.Parse(
                [
                    "00",
                    "11",
                ])
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(BitMatrix mat1, BitMatrix mat2, BitMatrix expected)
        {
            (mat1 * mat2).ShouldBe(expected);
        }

        public static TheoryData<BitMatrix, bool[], BitArray> MultiplyVector_Data => new()
        {
            {
                BitMatrix.Parse(
                [
                    "101",
                    "010",
                ]),
                [true, false, true],
                new BitArray([false, false])
            },
            {
                BitMatrix.Parse(
                [
                    "101",
                    "010",
                ]),
                [true, true, true],
                new BitArray([false, true])
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(BitMatrix mat, bool[] vector, BitArray expected)
        {
            (mat * vector).ShouldBe(expected);
            (mat * new BitArray(vector)).ShouldBe(expected);
            mat.Multiply(new BitArray(vector)).ShouldBe(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = BitMatrix.Parse(
            [
                "111",
                "101",
                "011",
            ]);
            var expecteds = new[]
            {
                BitMatrix.Parse(
                [
                    "100",
                    "010",
                    "001",
                ]),
                BitMatrix.Parse(
                [
                    "111",
                    "101",
                    "011",
                ]),
                BitMatrix.Parse(
                [
                    "001",
                    "100",
                    "110",
                ]),
                BitMatrix.Parse(
                [
                    "011",
                    "111",
                    "010",
                ]),
                BitMatrix.Parse(
                [
                    "110",
                    "001",
                    "101",
                ]),
                BitMatrix.Parse(
                [
                    "010",
                    "011",
                    "100",
                ]),
            };
            var cur = orig;
            for (int i = 1; i < expecteds.Length; i++)
            {
                cur.ShouldBe(expecteds[i]);
                orig.Pow(i).ShouldBe(cur);
                cur *= orig;
            }
        }

        public static TheoryData<BitMatrix> Inv_Data => new()
        {
            {
                BitMatrix.Parse(
                [
                    "101",
                    "010",
                    "011",
                ])
            },
            {
                BitMatrix.Parse(
                [
                    "100",
                    "010",
                    "111",
                ])
            },
            {
                BitMatrix.Parse(
                [
                    "001",
                    "010",
                    "111",
                ])
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Inv_Data))]
        public void Inv(BitMatrix mat)
        {
            var inv = mat.Inv();
            var expected = BitMatrix.Parse(
            [
                "100",
                "010",
                "001",
            ]);
            (mat * inv).ShouldBe(expected);
            (inv * mat).ShouldBe(expected);
        }

        public static TheoryData<bool, BitMatrix, BitMatrix> GaussianElimination_Data => new()
        {
            {
                false,
                BitMatrix.Parse(
                [
                    "1001",
                    "0101",
                    "1111",
                ]),
                BitMatrix.Parse(
                [
                    "1001",
                    "0101",
                    "0011",
                ])
            },
            {
                false,
                BitMatrix.Parse(
                [
                    "1001",
                    "0101",
                    "1111",
                    "1110",
                    "1100",
                ]),
                BitMatrix.Parse(
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
                BitMatrix.Parse(
                [
                    "1001",
                    "0101",
                    "1111",
                    "1110",
                    "1100",
                ]),
                BitMatrix.Parse(
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
        public void GaussianElimination(bool isReduced, BitMatrix orig, BitMatrix expected)
        {
            var got = orig.GaussianElimination(isReduced);
            got.ShouldBe(expected);
        }

        public static TheoryData<BitMatrix, bool[], BitArray[]> LinearSystem_Data => new()
        {
            {
                BitMatrix.Parse(
                [
                    "100",
                    "010",
                    "111",
                ]),
                [true, true, true,],
                new[]
                {
                    new BitArray([true, true, true, ]),
                }
            },
            {
                BitMatrix.Parse(
                [
                    "100",
                    "010",
                    "101",
                ]),
                [true, true, true,],
                new[]
                {
                    new BitArray([true, true, false, ]),
                }
            },
            {
                BitMatrix.Parse(
                [
                    "100",
                    "010",
                    "110",
                ]),
                [true, true, true,],
                []
            },
            {
                BitMatrix.Parse(
                [
                    "100",
                    "010",
                    "110",
                ]),
                [true, true, false,],
                new[]
                {
                    new BitArray([true, true, false, ]),
                    new BitArray([false, false, true, ]),
                }
            },
            {
                BitMatrix.Parse(
                [
                    "000",
                    "000",
                ]),
                [false, false],
                new[]
                {
                    new BitArray([false, false, false, ]),
                    new BitArray([true, false, false, ]),
                    new BitArray([false, true, false, ]),
                    new BitArray([false, false, true, ]),
                }
            },
        };

        [Theory]
        [MemberData(nameof(LinearSystem_Data))]
        public void LinearSystem(BitMatrix matrix, bool[] vector, BitArray[] expected)
        {
            var got = matrix.LinearSystem(vector);
            got.Length.ShouldBe(expected.Length);
            for (int i = 0; i < got.Length; i++)
                got[i].ShouldBe(expected[i], $"got[{i}]");
        }
    }
}
