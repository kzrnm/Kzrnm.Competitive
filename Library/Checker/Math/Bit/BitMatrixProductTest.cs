using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs.Bit;

internal class BitMatrixProductTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/matrix_product_mod_2";
    public override double? Tle => 5;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int M = cr;
        int K = cr;
        var a = BitMatrix.Parse(cr.Repeat(N).Ascii());
        var b = BitMatrix.Parse(cr.Repeat(M).Ascii());
        var c = a * b;
        cw.WriteLine(c);
        return null;
    }
}
