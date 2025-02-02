using System.Linq;
using System;
using Kzrnm.Competitive.IO;
using System.Text;
using System.Collections;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class BitMatrix64Tests
{
    [Theory]
    [MemberData(nameof(BitMatrixCase.RandomCases), 64, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    [Trait("Category", "Normal")]
    public void Construct(BitMatrixCase input)
    {
        if (input.Width > 64)
            throw new InvalidOperationException();
        var expected = new ulong[input.Height];
        for (int i = 0; i < expected.Length; i++)
        {
            for (int j = input.Width - 1; j >= 0; j--)
            {
                if (input[i, j])
                    expected[i] |= 1ul << j;
            }
        }
        new BitMatrix64(input.ToBoolArray())._v.ShouldBe(expected);
    }

    [Theory]
    [Trait("Category", "Normal")]
    [MemberData(nameof(BitMatrixCase.RandomCases), 64, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void String(BitMatrixCase input)
    {
        var arr = input.ToBoolArray();
        var rowsAscii = new Asciis[arr.Length];
        var rows = new string[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            var row = new char[input.Width];
            for (int j = 0; j < row.Length; j++)
            {
                row[j] = input[i, j] ? '1' : '0';
            }
            rows[i] = new string(row);
            rowsAscii[i] = new(Encoding.ASCII.GetBytes(row));
        }
        var expected = new BitMatrix64(arr);
        BitMatrix64.Parse(rows).ShouldBe(expected);
        BitMatrix64.Parse(rowsAscii).ShouldBe(expected);
        expected.ToString().Replace("\r\n", "\n").ShouldBe(string.Join("\n", rows.Select(r => r.PadRight(64, '0'))));
    }

    [Theory]
    [Trait("Category", "Normal")]
    [MemberData(nameof(BitMatrixCase.RandomCases), 64, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void SingleMinus(BitMatrixCase input)
    {
        var mat = new BitMatrix64(input.ToBoolArray());
        var expectedArray = input.ToBoolArray();
        for (int h = 0; h < expectedArray.Length; h++)
        {
            for (int w = 0; w < expectedArray[h].Length; w++)
                expectedArray[h][w] = !expectedArray[h][w];
            Array.Resize(ref expectedArray[h], 64);
            expectedArray[h].AsSpan(input.Width).Fill(true);
        }
        var expected = new BitMatrix64(expectedArray);
        (-mat).ShouldBe(expected);
        (~mat).ShouldBe(expected);
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(BitMatrixCase.RandomAddCases), 64, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void Add(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitMatrix64();
        var mat2 = right.ToBitMatrix64();
        var expected = NaiveExpected();

        (mat1 + mat2).ShouldBe(expected);
        (mat2 + mat1).ShouldBe(expected);
        (mat1 - mat2).ShouldBe(expected);
        (mat1 ^ mat2).ShouldBe(expected);

        BitMatrix64 NaiveExpected()
        {
            switch (left.Kind, right.Kind)
            {
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Zero):
                    return new(Internal.ArrayMatrixKind.Zero);
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Zero):
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Identity):
                    return new(Internal.ArrayMatrixKind.Identity);

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Zero):
                    return left.ToBitMatrix64();
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                    return right.ToBitMatrix64();

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Identity):
                    {
                        var expectedCase = new BitMatrixCase(left);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = !expectedCase[i, i];
                        }
                        return expectedCase.ToBitMatrix64();
                    }
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                    {
                        var expectedCase = new BitMatrixCase(right);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = !expectedCase[i, i];
                        }
                        return expectedCase.ToBitMatrix64();
                    }
                default:
                    {
                        var expectedCase = new BitMatrixCase(left.Height, left.Width);
                        for (int h = 0; h < expectedCase.Height; h++)
                            for (int w = 0; w < expectedCase.Width; w++)
                            {
                                expectedCase[h, w] = left[h, w] ^ right[h, w];
                            }
                        return expectedCase.ToBitMatrix64();
                    }
            }
        }
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(BitMatrixCase.RandomMultiplyCases), 64, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void Multiply(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitMatrix64();
        var mat2 = right.ToBitMatrix64();
        var expected = NaiveExpected();

        (mat1 * mat2).ShouldBe(expected);

        BitMatrix64 NaiveExpected()
        {
            switch (left.Kind, right.Kind)
            {
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Zero):
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Zero):
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Identity):
                    return new(Internal.ArrayMatrixKind.Zero);

                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Identity):
                    return new(Internal.ArrayMatrixKind.Identity);

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Identity):
                    return left.ToBitMatrix64();
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                    return right.ToBitMatrix64();

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Zero):
                    return new BitMatrixCase(left.Height, left.Width).ToBitMatrix64();
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                    return new BitMatrixCase(right.Height, right.Width).ToBitMatrix64();

                default:
                    {
                        var expectedCase = new BitMatrixCase(left.Height, 64);
                        for (int h = 0; h < expectedCase.Height; h++)
                            for (int w = 0; w < right.Width; w++)
                                for (int t = 0; t < left.Width; t++)
                                {
                                    expectedCase[h, w] ^= left[h, t] && right[t, w];
                                }
                        return expectedCase.ToBitMatrix64();
                    }
            }
        }
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(BitMatrixCase.RandomMultiplyVectorCases), 64, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void MultiplyVector(BitMatrixCase left, BitArrayCase right)
    {
        var mat = left.ToBitMatrix64();
        var vector = ToNumber(right);
        var expected = NaiveExpected();

        (mat * right.ToBoolArray()).ShouldBe(expected);
        (mat * vector).ShouldBe(expected);
        mat.Multiply(vector).ShouldBe(expected);

        ulong NaiveExpected()
        {
            var expectedCase = new BitArrayCase(new bool[left.Height]);
            for (int h = 0; h < expectedCase.Length; h++)
                for (int t = 0; t < left.Width; t++)
                {
                    expectedCase[h] ^= left[h, t] && right[t];
                }
            return ToNumber(expectedCase);
        }

        static ulong ToNumber(BitArrayCase c)
        {
            ulong result = 0;
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i])
                    result |= 1ul << i;
            }
            return result;
        }
    }


    [Theory]
    [Trait("Category", "Normal")]
    [MemberData(nameof(BitMatrixCase.RandomPowCasesFixedSize), 64, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void Pow(BitMatrixCase input)
    {
        var orig = input.ToBitMatrix64();
        var mat = input.ToBitMatrix64();

        orig.Pow(1).ShouldBe(mat);
        for (int i = 2; i < 11; i++)
        {
            mat *= orig;
            orig.Pow(i).ShouldBe(mat);
        }
    }














    public static TheoryData<bool, BitMatrix64, BitMatrix64> GaussianElimination_Data => new()
    {
        {
            false,
            BitMatrix64.Parse(
            [
                "1001",
                "0101",
                "1111",
            ]),
            BitMatrix64.Parse(
            [
                "1001",
                "0101",
                "0011",
            ])
        },
        {
            false,
            BitMatrix64.Parse(
            [
                "1001",
                "0101",
                "1111",
                "1110",
                "1100",
            ]),
            BitMatrix64.Parse(
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
            BitMatrix64.Parse(
            [
                "1001",
                "0101",
                "1111",
                "1110",
                "1100",
            ]),
            BitMatrix64.Parse(
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
    public void GaussianElimination(bool isReduced, BitMatrix64 orig, BitMatrix64 expected)
    {
        var got = orig.GaussianElimination(isReduced);
        got.ShouldBe(expected);
    }

    public static TheoryData<BitMatrix64, ulong, ulong[]> LinearSystem_Data => new()
    {
        {
            BitMatrix64.Parse(
            [
                "100",
                "010",
                "111",
            ]),
            0b111ul,
            [0b111ul]
        },
        {
            BitMatrix64.Parse(
            [
                "100",
                "010",
                "101",
            ]),
            0b111ul,
            [0b011ul]
        },
        {
            BitMatrix64.Parse(
            [
                "100",
                "010",
                "110",
            ]),
            0b111ul,
            []
        },
        {
            BitMatrix64.Parse(
            [
                "100",
                "010",
                "110",
            ]),
            0b110,
            [
                0b010ul,
            ]
        },
        {
            BitMatrix64.Parse(
            [
                "000",
                "000",
            ]),
            0ul,
            [
                0ul,
                1ul,
            ]
        },
    };

    [Theory]
    [MemberData(nameof(LinearSystem_Data))]
    public void LinearSystem(BitMatrix64 matrix, ulong vector, ulong[] expected)
    {
        var got = matrix.LinearSystem(vector);
        got.Length.ShouldBe(expected.Length);
        for (int i = 0; i < got.Length; i++)
            got[i].ShouldBe(expected[i], $"got[{i}]");
    }
}
