using Kzrnm.Competitive.IO;
using MontgomeryModInt = Kzrnm.Competitive.MontgomeryModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs
{
    internal class MatrixStrassenTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/matrix_product";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            int K = cr;
            var mat1 = new ArrayMatrix<MontgomeryModInt>(
                cr.Repeat(N).Select(cr => cr.Repeat(M).Select(cr => (MontgomeryModInt)cr.Int())));
            var mat2 = new ArrayMatrix<MontgomeryModInt>(
                cr.Repeat(M).Select(cr => cr.Repeat(K).Select(cr => (MontgomeryModInt)cr.Int())));

            cw.WriteMatrix(mat1.Strassen(mat2));
            return null;
        }
    }
}
