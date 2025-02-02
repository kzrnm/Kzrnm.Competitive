using Kzrnm.Competitive.IO;
using System;
using System.Linq;
using System.Text;
using BitMatrix128 = Kzrnm.Competitive.BitMatrix<System.UInt128>;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class BitMatrix128Tests
{
    [Theory]
    [MemberData(nameof(BitMatrixCase.RandomCases), 128, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    [Trait("Category", "Normal")]
    public void Construct(BitMatrixCase input)
    {
        if (input.Width > 128)
            throw new InvalidOperationException();
        var expected = new UInt128[input.Height];
        for (int i = 0; i < expected.Length; i++)
        {
            for (int j = input.Width - 1; j >= 0; j--)
            {
                if (input[i, j])
                    expected[i] |= UInt128.One << j;
            }
        }
        new BitMatrix128(input.ToBoolArray())._v.ShouldBe(expected);
    }

    [Theory]
    [Trait("Category", "Normal")]
    [MemberData(nameof(BitMatrixCase.RandomCases), 128, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
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
        var expected = new BitMatrix128(arr);
        BitMatrix128.Parse(rows).ShouldBe(expected);
        BitMatrix128.Parse(rowsAscii).ShouldBe(expected);
        expected.ToString().Replace("\r\n", "\n").ShouldBe(string.Join("\n", rows.Select(r => r.PadRight(128, '0'))));
    }

    [Theory]
    [Trait("Category", "Normal")]
    [MemberData(nameof(BitMatrixCase.RandomCases), 128, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void SingleMinus(BitMatrixCase input)
    {
        var mat = new BitMatrix128(input.ToBoolArray());
        var expectedArray = input.ToBoolArray();
        for (int h = 0; h < expectedArray.Length; h++)
        {
            for (int w = 0; w < expectedArray[h].Length; w++)
                expectedArray[h][w] = !expectedArray[h][w];
            Array.Resize(ref expectedArray[h], 128);
            expectedArray[h].AsSpan(input.Width).Fill(true);
        }
        var expected = new BitMatrix128(expectedArray);
        (-mat).ShouldBe(expected);
        (~mat).ShouldBe(expected);
    }


    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(BitMatrixCase.RandomAddCases), 128, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void Add(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitMatrix128();
        var mat2 = right.ToBitMatrix128();
        var expected = NaiveExpected();

        (mat1 + mat2).ShouldBe(expected);
        (mat2 + mat1).ShouldBe(expected);
        (mat1 - mat2).ShouldBe(expected);
        (mat1 ^ mat2).ShouldBe(expected);

        BitMatrix128 NaiveExpected()
        {
            switch (left.Kind, right.Kind)
            {
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Zero):
                    return new(Internal.ArrayMatrixKind.Zero);
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Zero):
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Identity):
                    return new(Internal.ArrayMatrixKind.Identity);

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Zero):
                    return left.ToBitMatrix128();
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                    return right.ToBitMatrix128();

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Identity):
                    {
                        var expectedCase = new BitMatrixCase(left);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = !expectedCase[i, i];
                        }
                        return expectedCase.ToBitMatrix128();
                    }
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                    {
                        var expectedCase = new BitMatrixCase(right);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = !expectedCase[i, i];
                        }
                        return expectedCase.ToBitMatrix128();
                    }
                default:
                    {
                        var expectedCase = new BitMatrixCase(left.Height, left.Width);
                        for (int h = 0; h < expectedCase.Height; h++)
                            for (int w = 0; w < expectedCase.Width; w++)
                            {
                                expectedCase[h, w] = left[h, w] ^ right[h, w];
                            }
                        return expectedCase.ToBitMatrix128();
                    }
            }
        }
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(BitMatrixCase.RandomMultiplyCases), 128, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void Multiply(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitMatrix128();
        var mat2 = right.ToBitMatrix128();
        var expected = NaiveExpected();

        (mat1 * mat2).ShouldBe(expected);

        BitMatrix128 NaiveExpected()
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
                    return left.ToBitMatrix128();
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                    return right.ToBitMatrix128();

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Zero):
                    return new BitMatrixCase(left.Height, left.Width).ToBitMatrix128();
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                    return new BitMatrixCase(right.Height, right.Width).ToBitMatrix128();

                default:
                    {
                        var expectedCase = new BitMatrixCase(left.Height, right.Width);
                        for (int h = 0; h < expectedCase.Height; h++)
                            for (int w = 0; w < right.Width; w++)
                                for (int t = 0; t < left.Width; t++)
                                {
                                    expectedCase[h, w] ^= left[h, t] && right[t, w];
                                }
                        return expectedCase.ToBitMatrix128();
                    }
            }
        }
    }


    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(BitMatrixCase.RandomMultiplyVectorCases), 128, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void MultiplyVector(BitMatrixCase left, BitArrayCase right)
    {
        var mat = left.ToBitMatrix128();
        var vector = ToNumber(right);
        var expected = NaiveExpected();

        (mat * right.ToBoolArray()).ShouldBe(expected);
        (mat * vector).ShouldBe(expected);
        mat.Multiply(vector).ShouldBe(expected);

        UInt128 NaiveExpected()
        {
            var expectedCase = new BitArrayCase(new bool[left.Height]);
            for (int h = 0; h < expectedCase.Length; h++)
                for (int t = 0; t < left.Width; t++)
                {
                    expectedCase[h] ^= left[h, t] && right[t];
                }
            return ToNumber(expectedCase);
        }

        static UInt128 ToNumber(BitArrayCase c)
        {
            UInt128 result = 0;
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i])
                    result |= UInt128.One << i;
            }
            return result;
        }
    }

    [Theory]
    [Trait("Category", "Normal")]
    [MemberData(nameof(BitMatrixCase.RandomPowCasesFixedSize), 128, MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
    public void Pow(BitMatrixCase input)
    {
        var orig = input.ToBitMatrix128();
        var mat = input.ToBitMatrix128();

        orig.Pow(1).ShouldBe(mat);
        for (int i = 2; i < 11; i++)
        {
            mat *= orig;
            orig.Pow(i).ShouldBe(mat);
        }
    }

    public static TheoryData<bool, BitMatrix128, BitMatrix128> GaussianElimination_Data => new()
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
        got.ShouldBe(expected);
    }
}
