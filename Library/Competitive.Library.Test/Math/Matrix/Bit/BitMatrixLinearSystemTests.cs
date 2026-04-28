using System.Collections;
using System.Numerics;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class BitMatrixLinearSystemTests
{
    public static IEnumerable<(BitMatrixCase, BitArrayCase)> LinearSystemCases(int max)
    {
        var rnd = new Random(227);
        for (int q = 0; q < 256; q++)
        {
            int height = rnd.Next(1, max + 1);
            int width = rnd.Next(1, max + 1);
            BitMatrixCase matCase;
            do
            {
                matCase = BitMatrixCase.MakeRandomCase(rnd, height, width);
            }
            while (!BitMatrixCaseHasLast(matCase));
            yield return (matCase, BitArrayCase.MakeRandomCase(rnd, height));
        }
        static bool BitMatrixCaseHasLast(BitMatrixCase c)
        {
            for (int h = 0; h < c.Height; h++)
                if (c[h, c.Width - 1])
                    return true;
            return false;
        }
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(LinearSystemCases), Arguments = [63])]
    public async Task LinearSystem64(BitMatrixCase c, BitArrayCase vector)
    {
        var mat = c.ToBitMatrix();
        var mat64 = c.ToBitMatrix64();
        await LinearSystemTestImpl(mat, mat64, vector);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(LinearSystemCases), Arguments = [127])]
    public async Task LinearSystem128(BitMatrixCase c, BitArrayCase vector)
    {
        var mat = c.ToBitMatrix();
        var mat128 = c.ToBitMatrix128();
        await LinearSystemTestImpl(mat, mat128, vector);
    }


    [Test, MultipleAssertions]
    [MethodDataSource(nameof(LinearSystemCases), Arguments = [240])]
    public async Task LinearSystemBitArrayOrBoolArray(BitMatrixCase c, BitArrayCase vector)
    {
        var mat = c.ToBitMatrix();
        var bitsResult = mat.LinearSystem(vector.ToBitArray());
        var boolsResult = mat.LinearSystem(vector.ToBoolArray());

        await bitsResult.Length.Should().BeEqualTo(boolsResult.Length);
        for (int i = 0; i < bitsResult.Length; i++)
        {
            await bitsResult[i].Cast<bool>().Should().BeEquivalentOrderTo(boolsResult[i].Cast<bool>());
        }
    }


    static async Task LinearSystemTestImpl<T>(BitMatrix mat, BitMatrix<T> matT, BitArrayCase vector) where T : unmanaged, IBinaryInteger<T>
    {
        var genericResult = matT.LinearSystem(ToNumber<T>(vector.ToBitArray()));
        var bitsResult = ToNumber<T>(mat.LinearSystem(vector.ToBitArray()));
        if (genericResult.Length != bitsResult.Length)
        {
            matT.LinearSystem(ToNumber<T>(vector.ToBitArray()));
            ToNumber<T>(mat.LinearSystem(vector.ToBitArray()));
        }

        await genericResult.Length.Should().BeEqualTo(bitsResult.Length);
        await genericResult.Should().BeEquivalentOrderTo(bitsResult);
    }

    static T ToNumber<T>(BitArray bits) where T : IBinaryInteger<T>
        => bits.OnBits().Select(b => T.One << b).Aggregate(T.Zero, (a, b) => a | b);
    static T[] ToNumber<T>(BitArray[] bs) where T : IBinaryInteger<T>
    {
        var rt = new T[bs.Length];
        for (int i = 0; i < rt.Length; i++)
        {
            rt[i] = ToNumber<T>(bs[i]);
        }
        return rt;
    }
}