using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs.Bit;

internal class BitMatrixInverseTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/inverse_matrix_mod_2";
    public override double? Tle => 5;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        var a = BitMatrix.Parse(cr.Repeat(N).Ascii());
        var inv = a.Inv();
        if (inv.IsZero)
            return -1;
        cw.WriteLine(inv);
        return null;
    }
}
