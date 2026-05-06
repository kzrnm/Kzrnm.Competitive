using Kzrnm.Competitive.IO;
using System.Collections;
using System.Text;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

[ThousandOfTestcases]
public class BitOrMatrixTests
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
        await new BitOrMatrix(input.ToBoolArray()).Should().BeEqualTo(new BitOrMatrix(expected));
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
        var expected = new BitOrMatrix(arr);
        await BitOrMatrix.Parse(rows).Should().BeEqualTo(expected);
        await BitOrMatrix.Parse(rowsAscii).Should().BeEqualTo(expected);
        await expected.ToString().Replace("\r\n", "\n").Should().BeEqualTo(string.Join("\n", rows));
    }


    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomAddCases), Arguments = [384])]
    public async Task Add(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitOrMatrix();
        var mat2 = right.ToBitOrMatrix();
        var expected = NaiveExpected();

        await (mat1 + mat2).Should().BeEqualTo(expected);
        await (mat2 + mat1).Should().BeEqualTo(expected);

        BitOrMatrix NaiveExpected()
        {
            switch (left.Kind, right.Kind)
            {
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Zero):
                    return new(Internal.ArrayMatrixKind.Zero);
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Zero):
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Identity):
                    return new(Internal.ArrayMatrixKind.Identity);

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Zero):
                    return left.ToBitOrMatrix();
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                    return right.ToBitOrMatrix();

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Identity):
                    {
                        var expectedCase = new BitMatrixCase.Builder(left);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = true;
                        }
                        return expectedCase.ToBitOrMatrix();
                    }
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                    {
                        var expectedCase = new BitMatrixCase.Builder(right);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = true;
                        }
                        return expectedCase.ToBitOrMatrix();
                    }
                default:
                    {
                        var expectedCase = new BitMatrixCase.Builder(left.Height, left.Width);
                        for (int h = 0; h < expectedCase.Height; h++)
                            for (int w = 0; w < expectedCase.Width; w++)
                            {
                                expectedCase[h, w] = left[h, w] | right[h, w];
                            }
                        return expectedCase.ToBitOrMatrix();
                    }
            }
        }
    }

    [Test]
    [Property("Category", "Operator")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomMultiplyCases), Arguments = [180])]
    public async Task Multiply(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitOrMatrix();
        var mat2 = right.ToBitOrMatrix();
        var expected = NaiveExpected();

        await (mat1 * mat2).Should().BeEqualTo(expected);

        BitOrMatrix NaiveExpected()
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
                    return left.ToBitOrMatrix();
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                    return right.ToBitOrMatrix();

                case (Internal.ArrayMatrixKind.Normal, Internal.ArrayMatrixKind.Zero):
                    return new BitMatrixCase.Builder(left.Height, left.Width).ToBitOrMatrix();
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                    return new BitMatrixCase.Builder(right.Height, right.Width).ToBitOrMatrix();

                default:
                    {
                        var expectedCase = new BitMatrixCase.Builder(left.Height, right.Width);
                        for (int h = 0; h < expectedCase.Height; h++)
                            for (int w = 0; w < expectedCase.Width; w++)
                                for (int t = 0; t < left.Width; t++)
                                {
                                    expectedCase[h, w] |= left[h, t] && right[t, w];
                                }
                        return expectedCase.ToBitOrMatrix();
                    }
            }
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomMultiplyVectorCases), Arguments = [180])]
    public async Task MultiplyVector(BitMatrixCase left, BitArrayCase right)
    {
        var mat = left.ToBitOrMatrix();
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
                    expectedCase[h] |= left[h, t] && right[t];
                }
            return new BitArray(expectedCase);
        }
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomSquareCases))]
    public async Task Pow(BitMatrixCase input)
    {
        var orig = input.ToBitOrMatrix();
        var mat = input.ToBitOrMatrix();

        await orig.Pow(1).Should().BeEqualTo(mat);
        for (int i = 2; i < 11; i++)
        {
            mat *= orig;
            await orig.Pow(i).Should().BeEqualTo(mat);
        }
    }
}