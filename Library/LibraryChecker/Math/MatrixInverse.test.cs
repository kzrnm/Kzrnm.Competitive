using Kzrnm.Competitive.IO;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using ModIntOperator = AtCoder.StaticModIntOperator<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs
{
    internal class MatrixMatrixInverseTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/inverse_matrix";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            var mat = new ArrayMatrix<ModInt, ModIntOperator>(cr.Grid(N, N, cr => ModInt.Raw(cr))).Inv();
            if (mat.IsZero)
                cw.WriteLine(-1);
            else
                cw.WriteGrid(mat.Value);
            return null;
        }
    }
}
