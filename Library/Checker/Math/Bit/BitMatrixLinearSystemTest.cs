using Kzrnm.Competitive.IO;
using System;

namespace Kzrnm.Competitive.MathNs.Bit;

internal class BitMatrixLinearSystemTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/system_of_linear_equations_mod_2";
    public override double? Tle => 5;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int M = cr;
        var m = BitMatrix.Parse(cr.Repeat(N).Ascii());
        var b = BinaryParser.ParseBitArray(cr.Ascii());

        var rt = m.LinearSystem(b);
        cw.WriteLine(rt.Length - 1);
        foreach (var r in rt)
            cw.WriteLine(r.ToBitString().AsSpan());

        return null;
    }
}
