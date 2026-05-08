using Kzrnm.Competitive.IO;
using System.Text;
using BitMatrix128 = Kzrnm.Competitive.BitMatrix<System.UInt128>;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class BitMatrix128Tests
{
    [Test, MultipleAssertions]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomCases), Arguments = [128])]
    [Property("Category", "Normal")]
    public async Task Construct(BitMatrixCase input)
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
        await new BitMatrix128(input.ToBoolArray())._v.Should().BeStrictlyEquivalentTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomCases), Arguments = [128])]
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
        var expected = new BitMatrix128(arr);
        await BitMatrix128.Parse(rows).Should().BeEqualTo(expected);
        await BitMatrix128.Parse(rowsAscii).Should().BeEqualTo(expected);
        await expected.ToString().Replace("\r\n", "\n").Should().BeEqualTo(string.Join("\n", rows.Select(r => r.PadRight(128, '0'))));
    }

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomCases), Arguments = [128])]
    public async Task SingleMinus(BitMatrixCase input)
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
        await (-mat).Should().BeEqualTo(expected);
        await (~mat).Should().BeEqualTo(expected);
    }


    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomAddCases), Arguments = [128])]
    public async Task Add(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitMatrix128();
        var mat2 = right.ToBitMatrix128();
        var expected = NaiveExpected();

        await (mat1 + mat2).Should().BeEqualTo(expected);
        await (mat2 + mat1).Should().BeEqualTo(expected);
        await (mat1 - mat2).Should().BeEqualTo(expected);
        await (mat1 ^ mat2).Should().BeEqualTo(expected);

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
                        var expectedCase = new BitMatrixCase.Builder(left);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = !expectedCase[i, i];
                        }
                        return expectedCase.ToBitMatrix128();
                    }
                case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                    {
                        var expectedCase = new BitMatrixCase.Builder(right);
                        for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                        {
                            expectedCase[i, i] = !expectedCase[i, i];
                        }
                        return expectedCase.ToBitMatrix128();
                    }
                default:
                    {
                        var expectedCase = new BitMatrixCase.Builder(left.Height, left.Width);
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

    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomMultiplyCases), Arguments = [128])]
    public async Task Multiply(BitMatrixCase left, BitMatrixCase right)
    {
        var mat1 = left.ToBitMatrix128();
        var mat2 = right.ToBitMatrix128();
        var expected = NaiveExpected();

        await (mat1 * mat2).Should().BeEqualTo(expected);

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
                    return new BitMatrixCase.Builder(left.Height, left.Width).ToBitMatrix128();
                case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                    return new BitMatrixCase.Builder(right.Height, right.Width).ToBitMatrix128();

                default:
                    {
                        var expectedCase = new BitMatrixCase.Builder(left.Height, right.Width);
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


    [Test, MultipleAssertions]
    [Property("Category", "Operator")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomMultiplyVectorCases), Arguments = [128])]
    public async Task MultiplyVector(BitMatrixCase left, BitArrayCase right)
    {
        var mat = left.ToBitMatrix128();
        var vector = ToNumber(right);
        var expected = NaiveExpected();

        await (mat * right.ToBoolArray()).Should().BeEqualTo(expected);
        await (mat * vector).Should().BeEqualTo(expected);
        await mat.Multiply(vector).Should().BeEqualTo(expected);

        UInt128 NaiveExpected()
        {
            var expectedCase = new bool[left.Height];
            for (int h = 0; h < expectedCase.Length; h++)
                for (int t = 0; t < left.Width; t++)
                {
                    expectedCase[h] ^= left[h, t] && right[t];
                }
            return ToNumber(new(expectedCase));
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

    [Test, MultipleAssertions]
    [Property("Category", "Normal")]
    [MethodDataSource(typeof(BitMatrixCase), nameof(BitMatrixCase.RandomPowCasesFixedSize), Arguments = [128])]
    public async Task Pow(BitMatrixCase input)
    {
        var orig = input.ToBitMatrix128();
        var mat = input.ToBitMatrix128();

        await orig.Pow(1).Should().BeEqualTo(mat);
        for (int i = 2; i < 11; i++)
        {
            mat *= orig;
            await orig.Pow(i).Should().BeEqualTo(mat);
        }
    }

    public static IEnumerable<(bool, BitMatrix128, BitMatrix128)> GaussianElimination_Data =>
    [
        (
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
        ),
        (
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
        ),
        (
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
        ),
    ];

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(GaussianElimination_Data))]
    public async Task GaussianElimination(bool isReduced, BitMatrix128 orig, BitMatrix128 expected)
    {
        var got = orig.GaussianElimination(isReduced);
        await got.Should().BeEqualTo(expected);
    }
}