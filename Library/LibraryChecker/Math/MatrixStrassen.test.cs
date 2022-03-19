using Kzrnm.Competitive.IO;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using ModIntOperator = AtCoder.StaticModIntOperator<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs
{
    public class MatrixStrassenTest
    {
        static void Main() { using var cw = new ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/matrix_product
        static void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            int K = cr;
            var mat1 = new ArrayMatrix<ModInt, ModIntOperator>(
                cr.Repeat(N).Select(cr => cr.Repeat(M).Select(cr => ModInt.Raw(cr))));
            var mat2 = new ArrayMatrix<ModInt, ModIntOperator>(
                cr.Repeat(M).Select(cr => cr.Repeat(K).Select(cr => ModInt.Raw(cr))));

            cw.WriteGrid(mat1.Strassen(mat2).Value);
        }
    }
}
