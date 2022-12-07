using Kzrnm.Competitive.IO;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

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
            var mat1 = new Mod998244353ArrayMatrix(
                cr.Repeat(N).Select(cr => cr.Repeat(M).Select(cr => ModInt.Raw(cr))));
            var mat2 = new Mod998244353ArrayMatrix (
                cr.Repeat(M).Select(cr => cr.Repeat(K).Select(cr => ModInt.Raw(cr))));

            cw.WriteGrid(mat1.Strassen(mat2).Value);
            return null;
        }
    }
}
