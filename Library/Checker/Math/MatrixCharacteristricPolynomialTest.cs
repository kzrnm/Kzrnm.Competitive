using Kzrnm.Competitive.IO;
using System.Linq;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs
{
    internal class MatrixCharacteristricPolynomialTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/characteristic_polynomial";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int n = cr;
            var a = cr.Repeat(n * n).Int();
            var mt = new Mod998244353ArrayMatrix(a.Select(v => (ModInt)v).ToArray(), n, n);
            cw.WriteLineJoin(mt.CharacteristricPolynomial());
            return null;
        }
    }
}
