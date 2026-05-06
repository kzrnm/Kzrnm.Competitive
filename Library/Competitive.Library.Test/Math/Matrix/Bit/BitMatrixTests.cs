using Kzrnm.Competitive.IO;
using System.Collections;
using System.Text;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

[ThousandOfTestcases]
public class BitMatrixTests
{
    [Test]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomCases), Arguments = [384])]
    [Property("Category", "Normal")]
    public async Task Construct(BitMatrixCase input)
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
        await new BitMatrix(input.ToBoolArray()).Should().BeEqualTo(new BitMatrix(expected));
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomCases), Arguments = [384])]
    public async Task String(BitMatrixCase input)
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
        await BitMatrix.Parse(rows).Should().BeEqualTo(expected);
        await BitMatrix.Parse(rowsAscii).Should().BeEqualTo(expected);
        await expected.ToString().Replace("\r\n", "\n").Should().BeEqualTo(string.Join("\n", rows));
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomCases), Arguments = [384])]
    public async Task SingleMinus(BitMatrixCase input)
    {
        var mat = new BitMatrix(input.ToBoolArray());
        var expectedArray = input.ToBoolArray();
        for (int h = 0; h < expectedArray.Length; h++)
            for (int w = 0; w < expectedArray[h].Length; w++)
                expectedArray[h][w] = !expectedArray[h][w];
        var expected = new BitMatrix(expectedArray);
        await (-mat).Should().BeEqualTo(expected);
        await (~mat).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomAddCases), Arguments = [384])]
    public async Task Add(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitMatrix();
        var mat2 = right.ToBitMatrix();
        var expected = NaiveExpected();

        await (mat1 + mat2).Should().BeEqualTo(expected);
        await (mat2 + mat1).Should().BeEqualTo(expected);
        await (mat1 - mat2).Should().BeEqualTo(expected);
        await (mat1 ^ mat2).Should().BeEqualTo(expected);

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
                        var expectedCase = new BitMatrixCase.Builder(left);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = !expectedCase[i, i];
                        }
                        return expectedCase.ToBitMatrix();
                    }
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                    {
                        var expectedCase = new BitMatrixCase.Builder(right);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = !expectedCase[i, i];
                        }
                        return expectedCase.ToBitMatrix();
                    }
                default:
                    {
                        var expectedCase = new BitMatrixCase.Builder(left.Height, left.Width);
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

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomMultiplyCases), Arguments = [180])]
    public async Task Multiply(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitMatrix();
        var mat2 = right.ToBitMatrix();
        var expected = NaiveExpected();

        await (mat1 * mat2).Should().BeEqualTo(expected);

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
                    return new BitMatrixCase.Builder(left.Height, left.Width).ToBitMatrix();
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                    return new BitMatrixCase.Builder(right.Height, right.Width).ToBitMatrix();

                default:
                    {
                        var expectedCase = new BitMatrixCase.Builder(left.Height, right.Width);
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

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomMultiplyVectorCases), Arguments = [180])]
    public async Task MultiplyVector(BitMatrixCase left, BitArrayCase right)
    {
        var mat = left.ToBitMatrix();
        var vector = right.ToBitArray();
        var expected = NaiveExpected();

        await (mat * right.ToBoolArray()).Should().BeEqualTo(expected, BitArrayEqualityComparer.Default);
        await (mat * vector).Should().BeEqualTo(expected, BitArrayEqualityComparer.Default);
        await mat.Multiply(vector).Should().BeEqualTo(expected, BitArrayEqualityComparer.Default);

        BitArray NaiveExpected()
        {
            var expectedCase = new bool[left.Height];
            for (int h = 0; h < expectedCase.Length; h++)
                for (int t = 0; t < left.Width; t++)
                {
                    expectedCase[h] ^= left[h, t] && right[t];
                }
            return new BitArray(expectedCase);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomSquareCases))]
    public async Task Pow(BitMatrixCase input)
    {
        var orig = input.ToBitMatrix();
        var mat = orig;

        await orig.Pow(1).Should().BeEqualTo(mat);
        for (int i = 2; i < 11; i++)
        {
            mat *= orig;
            await orig.Pow(i).Should().BeEqualTo(mat);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomSquareCases))]
    public async Task Inv(BitMatrixCase input)
    {
        var mat = input.ToBitMatrix();
        var inv = mat.Inv();
        if (inv.IsZero)
        {
            await mat.Determinant().Should().BeFalse();
            return;
        }

        var expectedCase = new BitMatrixCase.Builder(input.Height, input.Width);
        for (int i = 0; i < expectedCase.Height; i++)
        {
            expectedCase[i, i] = true;
        }
        var expected = expectedCase.ToBitMatrix();

        await mat.Determinant().Should().BeTrue();
        await (mat * inv).Should().BeEqualTo(expected);
        await (inv * mat).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(bool, BitMatrix, BitMatrix)> GaussianElimination_Data =>
    [
        (
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
        ),
        (
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
        ),
        (
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
        ),
    ];

    [Test]
    [MethodDataSource(nameof(GaussianElimination_Data))]
    public async Task GaussianElimination(bool isReduced, BitMatrix orig, BitMatrix expected)
    {
        var got = orig.GaussianElimination(isReduced);
        await got.Should().BeEqualTo(expected);
    }

    public static IEnumerable<(BitMatrix, bool[], BitArray[])> LinearSystem_Data =>
    [
        (
            BitMatrix.Parse(
            [
                "100",
                "010",
                "111",
            ]),
            [true, true, true,],
            [
                new BitArray([true, true, true, ]),
            ]
        ),
        (
            BitMatrix.Parse(
            [
                "100",
                "010",
                "101",
            ]),
            [true, true, true,],
            [
                new BitArray([true, true, false, ]),
            ]
        ),
        (
            BitMatrix.Parse(
            [
                "100",
                "010",
                "110",
            ]),
            [true, true, true,],
            []
        ),
        (
            BitMatrix.Parse(
            [
                "100",
                "010",
                "110",
            ]),
            [true, true, false,],
            [
                new BitArray([true, true, false, ]),
                new BitArray([false, false, true, ]),
            ]
        ),
        (
            BitMatrix.Parse(
            [
                "000",
                "000",
            ]),
            [false, false],
            [
                new BitArray([false, false, false, ]),
                new BitArray([true, false, false, ]),
                new BitArray([false, true, false, ]),
                new BitArray([false, false, true, ]),
            ]
        ),
    ];

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(LinearSystem_Data))]
    public async Task LinearSystem(BitMatrix matrix, bool[] vector, BitArray[] expected)
    {
        var got = matrix.LinearSystem(vector);
        await got.Length.Should().BeEqualTo(expected.Length);
        for (int i = 0; i < got.Length; i++)
            await got[i].Should().BeEqualTo(expected[i], BitArrayEqualityComparer.Default);
    }
}