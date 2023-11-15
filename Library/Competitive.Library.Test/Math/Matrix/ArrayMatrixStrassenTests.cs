using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
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
        [TupleMemberData(nameof(RandomStrassen_Data))]
        public void RandomStrassen(ArrayMatrix<byte> matInt1, ArrayMatrix<byte> matInt2)
        {
            var mat1 = new ArrayMatrix<MontgomeryModInt<Mod1000000007>>(matInt1.Value.Select(n => (MontgomeryModInt<Mod1000000007>)(uint)n).ToArray(), matInt1.Height, matInt1.Width);
            var mat2 = new ArrayMatrix<MontgomeryModInt<Mod1000000007>>(matInt2.Value.Select(n => (MontgomeryModInt<Mod1000000007>)(uint)n).ToArray(), matInt2.Height, matInt2.Width);

            var expected = ModInt2Int(mat1 * mat2);

            ModInt2Int(mat1.Strassen(mat2)).Should().Be(expected);
        }

        static ArrayMatrix<int> ModInt2Int<T>(ArrayMatrix<T> mat) where T : IModInt<T>
           => mat.kind switch
           {
               ArrayMatrixKind.Normal
                   => new(mat.Value.Select(v => v.Value).ToArray(), mat.Height, mat.Width),
               _ => new(mat.kind),
           };
    }
}
