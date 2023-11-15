using AtCoder;
using Kzrnm.Competitive.IO;
using System.Linq;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using MontgomeryModInt = Kzrnm.Competitive.MontgomeryModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs
{
    internal class MatrixPowTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/pow_of_matrix";
        public override double? Tle => 5 * 2;
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            long K = cr;
            var a = cr.Repeat(N * N).Int();

            var m1 = Static(N, a).Pow(K);
            var m2 = Montgomery(N, a).Pow(K);

            if (K == 0)
            {
                cw.WriteGrid(ArrayMatrix<int>.NormalIdentity(N));
                return null;
            }
            ShouldEqual(m1, m2);

            cw.WriteGrid(m1);
            return null;
        }
        static ArrayMatrix<ModInt> Static(int N, int[] a)
            => new ArrayMatrix<ModInt>(a.Select(v => (ModInt)v).ToArray(), N, N);
        static SimdModMatrix<Mod998244353> Montgomery(int N, int[] a)
            => new SimdModMatrix<Mod998244353>(a.Select(v => (MontgomeryModInt)v).ToArray(), N, N);
        static void ShouldEqual(ArrayMatrix<ModInt> m1, SimdModMatrix<Mod998244353> m2)
        {
            if (m1.Height != m2.Height) throw new System.Exception("Diff height");
            if (m1.Width != m2.Width) throw new System.Exception("Diff width");

            for (int h = 0; h < m1.Height; h++)
                for (int w = 0; w < m1.Width; w++)
                    if (m1[h, w].Value != m2[h, w].Value)
                        throw new System.Exception("Difference");
        }
    }
}
