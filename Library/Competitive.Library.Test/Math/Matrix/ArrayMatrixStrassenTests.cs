using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class ArrayMatrixStrassenTests
{
    public static IEnumerable<(ArrayMatrix<byte> matInt1, ArrayMatrix<byte> matInt2)> RandomStrassen_Data()
    {
        var rnd = new Random(227);
        for (int q = 0; q < 100; q++)
        {
            var size = rnd.Next(1, 20);
            var h1 = rnd.Next(1, 20);
            var w2 = rnd.Next(1, 20);

            var b1 = new byte[h1 * size];
            var b2 = new byte[w2 * size];
            rnd.NextBytes(b1);
            rnd.NextBytes(b2);

            yield return (new(b1, h1, size), new(b2, size, w2));
        }
    }

    [Theory]
    [Trait("Category", "Operator")]
    [MemberData(nameof(RandomStrassen_Data))]
    public void RandomStrassen(ArrayMatrix<byte> matInt1, ArrayMatrix<byte> matInt2)
    {
        var mat1 = new ArrayMatrix<MontgomeryModInt<Mod1000000007>>(matInt1._v.Select(n => (MontgomeryModInt<Mod1000000007>)(uint)n).ToArray(), matInt1.Height, matInt1.Width);
        var mat2 = new ArrayMatrix<MontgomeryModInt<Mod1000000007>>(matInt2._v.Select(n => (MontgomeryModInt<Mod1000000007>)(uint)n).ToArray(), matInt2.Height, matInt2.Width);

        var expected = ModInt2Int(mat1 * mat2);

        ModInt2Int(mat1.Strassen(mat2)).ShouldBe(expected);
    }

    [Fact]
    [Trait("Category", "Operator")]
    public void LargeRandomStrassen()
    {
        var rnd = new Random(227);
        var v1 = new MontgomeryModInt<Mod998244353>[200 * 200];
        var v2 = new MontgomeryModInt<Mod998244353>[200 * 200];
        for (int q = 0; q < 5; q++)
        {
            for (int i = 0; i < 200 * 200; i++)
            {
                v1[i] = rnd.Next();
                v2[i] = rnd.Next();
            }

            var mat1 = new ArrayMatrix<MontgomeryModInt<Mod998244353>>(v1, 200, 200);
            var mat2 = new ArrayMatrix<MontgomeryModInt<Mod998244353>>(v2, 200, 200);

            var got = ModInt2Int(mat1.Strassen(mat2));
            var expected = ModInt2Int(mat1 * mat2);
            got.ShouldBe(expected);
        }
    }

    static ArrayMatrix<int> ModInt2Int<T>(ArrayMatrix<T> mat) where T : IModInt<T>
       => mat.kind switch
       {
           ArrayMatrixKind.Normal
               => new(mat._v.Select(v => v.Value).ToArray(), mat.Height, mat.Width),
           _ => new(mat.kind),
       };


    [Fact]
    [Trait("Category", "Normal")]
    public void Pow()
    {
        var orig = new ArrayMatrix<MontgomeryModInt<Mod998244353>>(new MontgomeryModInt<Mod998244353>[,]
        {
            { 1, 2 },
            { 3, 4 },
        });
        var cur = orig;
        for (int i = 1; i < 10; i++)
        {
            orig.Pow(i).ShouldBe(cur);
            cur *= orig;
        }
    }
}
