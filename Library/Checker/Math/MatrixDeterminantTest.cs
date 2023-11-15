using AtCoder;
using Kzrnm.Competitive.IO;
using System.Linq;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using MontgomeryModInt = Kzrnm.Competitive.MontgomeryModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs
{
    internal class MatrixDeterminantTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/matrix_det";
        public override double? Tle => 5 * 2;
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            var a = cr.Repeat(N * N).Int();
            var m1 = Static(a, N);
            var m2 = Montgomery(a, N);

            var d1 = m1.Determinant();
            var d2 = m2.Determinant();

            if (d1.Value != d2.Value)
                throw new System.Exception("Difference");

            cw.WriteLine(d2);
            return null;
        }
        static ArrayMatrix<ModInt> Static(int[] a, int N)
            => new ArrayMatrix<ModInt>(a.Select(v => (ModInt)v).ToArray(), N, N);
        static SimdModMatrix<Mod998244353> Montgomery(int[] a, int N)
            => new SimdModMatrix<Mod998244353>(a.Select(v => (MontgomeryModInt)v).ToArray(), N, N);
    }
}
