using Kzrnm.Competitive.IO;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using ModIntOperator = AtCoder.StaticModIntOperator<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs
{
    public class MatrixDeterminantTest
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/matrix_det
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            var mat = new ArrayMatrix<ModInt, ModIntOperator>(cr.Repeat(N).Select(cr => cr.Repeat(N).Select(cr => ModInt.Raw(cr))));
            cw.WriteLine(mat.Determinant().Value);
            return null;
        }
    }
}
