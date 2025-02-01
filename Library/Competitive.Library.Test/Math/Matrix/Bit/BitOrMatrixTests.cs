using Kzrnm.Competitive.IO;
using System;
using System.Collections;
using System.Text;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class BitOrMatrixTests
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
            new BitOrMatrix(input.ToBoolArray())._v.ShouldBe(expected);
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
            var expected = new BitOrMatrix(arr);
            BitOrMatrix.Parse(rows).ShouldBe(expected);
            BitOrMatrix.Parse(rowsAscii).ShouldBe(expected);
            expected.ToString().Replace("\r\n", "\n").ShouldBe(string.Join("\n", rows));
        }


        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(BitMatrixCase.RandomAddCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
        public void Add(BitMatrixCase left, BitMatrixCase right)
        {
            var mat1 = left.ToBitOrMatrix();
            var mat2 = right.ToBitOrMatrix();
            var expected = NaiveExpected();

            (mat1 + mat2).ShouldBe(expected);
            (mat2 + mat1).ShouldBe(expected);

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
                            var expectedCase = new BitMatrixCase(left);
                            for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                            {
                                expectedCase[i, i] = true;
                            }
                            return expectedCase.ToBitOrMatrix();
                        }
                    case (Internal.ArrayMatrixKind.Identity, Internal.ArrayMatrixKind.Normal):
                        {
                            var expectedCase = new BitMatrixCase(right);
                            for (int i = Math.Min(expectedCase.Height, expectedCase.Width) - 1; i >= 0; i--)
                            {
                                expectedCase[i, i] = true;
                            }
                            return expectedCase.ToBitOrMatrix();
                        }
                    default:
                        {
                            var expectedCase = new BitMatrixCase(left.Height, left.Width);
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

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(BitMatrixCase.RandomMultiplyCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
        public void Multiply(BitMatrixCase left, BitMatrixCase right)
        {
            var mat1 = left.ToBitOrMatrix();
            var mat2 = right.ToBitOrMatrix();
            var expected = NaiveExpected();

            (mat1 * mat2).ShouldBe(expected);

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
                        return new BitMatrixCase(left.Height, left.Width).ToBitOrMatrix();
                    case (Internal.ArrayMatrixKind.Zero, Internal.ArrayMatrixKind.Normal):
                        return new BitMatrixCase(right.Height, right.Width).ToBitOrMatrix();

                    default:
                        {
                            var expectedCase = new BitMatrixCase(left.Height, right.Width);
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

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(BitMatrixCase.RandomMultiplyVectorCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
        public void MultiplyVector(BitMatrixCase left, BitArrayCase right)
        {
            var mat = left.ToBitOrMatrix();
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
                        expectedCase[h] |= left[h, t] && right[t];
                    }
                return expectedCase.ToBitArray();
            }
        }

        [Theory]
        [Trait("Category", "Normal")]
        [MemberData(nameof(BitMatrixCase.RandomSquareCases), MemberType = typeof(BitMatrixCase), DisableDiscoveryEnumeration = true)]
        public void Pow(BitMatrixCase input)
        {
            var orig = input.ToBitOrMatrix();
            var mat = input.ToBitOrMatrix();

            orig.Pow(1).ShouldBe(mat);
            for (int i = 2; i < 11; i++)
            {
                mat *= orig;
                orig.Pow(i).ShouldBe(mat);
            }
        }
    }
}
