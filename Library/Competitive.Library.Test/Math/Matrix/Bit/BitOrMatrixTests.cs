using System.Collections;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class BitOrMatrixTests
    {
        [Fact]
        [Trait("Category", "Normal")]
        public void Construct()
        {
            new BitOrMatrix([
                [true, false, true],
                [false, true, true],
            ])._v.ShouldBe([
                new BitArray([true, false, true]),
                new BitArray([false, true, true]),
            ]);
            new BitOrMatrix([
                new BitArray([true, false, true]),
                new BitArray([false, true, true]),
            ])._v.ShouldBe([
                new BitArray([true, false, true]),
                new BitArray([false, true, true]),
            ]);
        }


        public static TheoryData<BitOrMatrix, string> String_Data => new()
        {
            {
                new BitOrMatrix(new bool[][]{
                    [true, false,  true],
                    [false, false, false],
                    [false, false,  true],
                    [true,  true,  true],
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
            mat.ToString().Replace("\r\n", "\n").ShouldBe(text);
            BitOrMatrix.Parse(text.Split('\n')).ShouldBe(mat);
        }

        public static TheoryData<BitOrMatrix, BitOrMatrix, BitOrMatrix> Add_Data => new()
        {
            {
                BitOrMatrix.Zero,
                BitOrMatrix.Identity,
                BitOrMatrix.Identity
            },
            {
                BitOrMatrix.Parse(
                [
                    "101",
                    "000",
                    "001",
                ]),
                BitOrMatrix.Parse(
                [
                    "100",
                    "010",
                    "101",
                ]),
                BitOrMatrix.Parse(
                [
                    "101",
                    "010",
                    "101",
                ])
            },
            {
                BitOrMatrix.Parse(
                [
                    "101",
                    "000",
                    "001",
                ]),
                BitOrMatrix.Zero,
                BitOrMatrix.Parse(
                [
                    "101",
                    "000",
                    "001",
                ])
            },
            {
                BitOrMatrix.Parse(
                [
                    "101",
                    "000",
                    "001",
                ]),
                BitOrMatrix.Identity,
                BitOrMatrix.Parse(
                [
                    "101",
                    "010",
                    "001",
                ])
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void Add(BitOrMatrix mat1, BitOrMatrix mat2, BitOrMatrix expected)
        {
            (mat1 + mat2).ShouldBe(expected);
            (mat2 + mat1).ShouldBe(expected);
        }

        public static TheoryData<BitOrMatrix, BitOrMatrix, BitOrMatrix> Multiply_Data => new()
        {
            {
                BitOrMatrix.Identity,
                BitOrMatrix.Zero,
                BitOrMatrix.Zero
            },
            {
                BitOrMatrix.Parse(
                [
                    "101",
                    "010",
                ]),
                BitOrMatrix.Parse(
                [
                    "10",
                    "11",
                    "10",
                ]),
                BitOrMatrix.Parse(
                [
                    "10",
                    "11",
                ])
            },
            {
                BitOrMatrix.Parse(
                [
                    "00",
                    "01",
                ]),
                BitOrMatrix.Parse(
                [
                    "00",
                    "01",
                ]),
                BitOrMatrix.Parse(
                [
                    "00",
                    "01",
                ])
            },
            {
                BitOrMatrix.Parse(
                [
                    "11",
                    "11",
                ]),
                BitOrMatrix.Zero,
                BitOrMatrix.Parse(
                [
                    "00",
                    "00",
                ])
            },
            {
                BitOrMatrix.Parse(
                [
                    "00",
                    "11",
                ]),
                BitOrMatrix.Identity,
                BitOrMatrix.Parse(
                [
                    "00",
                    "11",
                ])
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(BitOrMatrix mat1, BitOrMatrix mat2, BitOrMatrix expected)
        {
            (mat1 * mat2).ShouldBe(expected);
        }

        public static TheoryData<BitOrMatrix, bool[], BitArray> MultiplyVector_Data => new()
        {
            {
                BitOrMatrix.Parse(
                [
                    "101",
                    "010",
                ]),
                [true, false, true],
                new BitArray([true, false])
            },
            {
                BitOrMatrix.Parse(
                [
                    "101",
                    "010",
                ]),
                [true, true, true],
                new BitArray([true, true])
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(BitOrMatrix mat, bool[] vector, BitArray expected)
        {
            (mat * vector).ShouldBe(expected);
            (mat * new BitArray(vector)).ShouldBe(expected);
            mat.Multiply(new BitArray(vector)).ShouldBe(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = BitOrMatrix.Parse(
            [
                "111",
                "101",
                "011",
            ]);
            var expecteds = new[]
            {
                BitOrMatrix.Parse(
                [
                    "100",
                    "010",
                    "001",
                ]),
                BitOrMatrix.Parse(
                [
                    "111",
                    "101",
                    "011",
                ]),
                BitOrMatrix.Parse(
                [
                    "111",
                    "111",
                    "111",
                ]),
                BitOrMatrix.Parse(
                [
                    "111",
                    "111",
                    "111",
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
    }
}
