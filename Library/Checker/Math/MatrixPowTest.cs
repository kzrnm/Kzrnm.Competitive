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

            for (int h = 0; h < N; h++)
                for (int w = 0; w < N; w++)
                    if (m1[h, w].Value != m2[h, w].Value)
                        throw new System.Exception("Difference");

            cw.WriteGrid(m1);
            return null;
        }
        static Mod998244353ArrayMatrix Static(int N, int[] a)
            => new Mod998244353ArrayMatrix(a.Select(v => (ModInt)v).ToArray(), N, N);
        static ArrayMatrix<MontgomeryModInt> Montgomery(int N, int[] a)
            => new ArrayMatrix<MontgomeryModInt>(a.Select(v => (MontgomeryModInt)v).ToArray(), N, N);
    }
}
