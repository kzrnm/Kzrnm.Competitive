using Kzrnm.Competitive.IO;
using System;
using System.Collections;
using System.Text;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class BitMatrixTests
{
    [Theory]
    [MemberData(nameof(BitMatrixCase.RandomCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    [Trait("Category", "Normal")]
    public void Construct(BitMatrixCase input)
    {
        var expected = new BitArray[input.Height];
        for (int i = 0; i < expected.Length; i++)
        {
            expected[i] = new BitArray(input.Width);
            for (int j = input.Width - 1; j >= 0; j--)
            {
                expected[i][j] = input[i, j];
            }
        }
        new BitMatrix(input.ToBoolArray())._v.ShouldBe(expected);
    }

    [Theory]
    [Trait("Category", "Normal")]
    [MemberData(nameof(BitMatrixCase.RandomCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
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
        var expected = new BitMatrix(arr);
        BitMatrix.Parse(rows).ShouldBe(expected);
        BitMatrix.Parse(rowsAscii).ShouldBe(expected);
        expected.ToString().Replace("\r\n", "\n").ShouldBe(string.Join("\n", rows));
    }

    [Theory]
    [Trait("Category", "Normal")]
    [MemberData(nameof(BitMatrixCase.RandomCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void SingleMinus(BitMatrixCase input)
    {
        var mat = new BitMatrix(input.ToBoolArray());
        var expectedArray = input.ToBoolArray();
        for (int h = 0; h < expectedArray.Length; h++)
            for (int w = 0; w < expectedArray[h].Length; w++)
                expectedArray[h][w] = !expectedArray[h][w];
        var expected = new BitMatrix(expectedArray);
        (-mat).ShouldBe(expected);
        (~mat).ShouldBe(expected);
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(BitMatrixCase.RandomAddCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void Add(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitMatrix();
        var mat2 = right.ToBitMatrix();
        var expected = NaiveExpected();

        (mat1 + mat2).ShouldBe(expected);
        (mat2 + mat1).ShouldBe(expected);
        (mat1 - mat2).ShouldBe(expected);
        (mat1 ^ mat2).ShouldBe(expected);

        BitMatrix NaiveExpected()
        {
            switch (left.Kind, right.Kind)
            {
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Zero):
                    return new(Internal.ArrayMatrixKind.Zero);
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Zero):
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Identity):
                    return new(Internal.ArrayMatrixKind.Identity);

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Zero):
                    return left.ToBitMatrix();
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                    return right.ToBitMatrix();

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Identity):
                    {
                        var expectedCase = new BitMatrixCase(left);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = !expectedCase[i, i];
                        }
                        return expectedCase.ToBitMatrix();
                    }
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                    {
                        var expectedCase = new BitMatrixCase(right);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = !expectedCase[i, i];
                        }
                        return expectedCase.ToBitMatrix();
                    }
                default:
                    {
                        var expectedCase = new BitMatrixCase(left.Height, left.Width);
                        for (int h = 0; h < expectedCase.Height; h++)
                            for (int w = 0; w < expectedCase.Width; w++)
                            {
                                expectedCase[h, w] = left[h, w] ^ right[h, w];
                            }
                        return expectedCase.ToBitMatrix();
                    }
            }
        }
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(BitMatrixCase.RandomMultiplyCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void Multiply(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitMatrix();
        var mat2 = right.ToBitMatrix();
        var expected = NaiveExpected();

        (mat1 * mat2).ShouldBe(expected);

        BitMatrix NaiveExpected()
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
                    return left.ToBitMatrix();
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                    return right.ToBitMatrix();

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Zero):
                    return new BitMatrixCase(left.Height, left.Width).ToBitMatrix();
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                    return new BitMatrixCase(right.Height, right.Width).ToBitMatrix();

                default:
                    {
                        var expectedCase = new BitMatrixCase(left.Height, right.Width);
                        for (int h = 0; h < expectedCase.Height; h++)
                            for (int w = 0; w < expectedCase.Width; w++)
                                for (int t = 0; t < left.Width; t++)
                                {
                                    expectedCase[h, w] ^= left[h, t] && right[t, w];
                                }
                        return expectedCase.ToBitMatrix();
                    }
            }
        }
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(BitMatrixCase.RandomMultiplyVectorCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void MultiplyVector(BitMatrixCase left, BitArrayCase right)
    {
        var mat = left.ToBitMatrix();
        var vector = right.ToBitArray();
        var expected = NaiveExpected();

        (mat * right.ToBoolArray()).ShouldBe(expected);
        (mat * vector).ShouldBe(expected);
        mat.Multiply(vector).ShouldBe(expected);

        BitArray NaiveExpected()
        {
            var expectedCase = new BitArrayCase(new bool[left.Height]);
            for (int h = 0; h < expectedCase.Length; h++)
                for (int t = 0; t < left.Width; t++)
                {
                    expectedCase[h] ^= left[h, t] && right[t];
                }
            return expectedCase.ToBitArray();
        }
    }

    [Theory]
    [Trait("Category", "Normal")]
    [MemberData(nameof(BitMatrixCase.RandomSquareCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void Pow(BitMatrixCase input)
    {
        var orig = input.ToBitMatrix();
        var mat = input.ToBitMatrix();

        orig.Pow(1).ShouldBe(mat);
        for (int i = 2; i < 11; i++)
        {
            mat *= orig;
            orig.Pow(i).ShouldBe(mat);
        }
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(BitMatrixCase.RandomSquareCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void Inv(BitMatrixCase input)
    {
        var mat = input.ToBitMatrix();
        var inv = mat.Inv();
        if (inv.IsZero)
            return;

        var expectedCase = new BitMatrixCase(input.Height, input.Width);
        for (int i = 0; i < expectedCase.Height; i++)
        {
            expectedCase[i, i] = true;
        }
        var expected = expectedCase.ToBitMatrix();

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
