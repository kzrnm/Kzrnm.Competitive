using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs.Fps;

internal class FormalPowerSeriesExpTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/exp_of_formal_power_series";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int[] arr = cr.Repeat(N);
        var exp = new FormalPowerSeries<Mod998244353>(arr).Exp().Coefficients(N);
        cw.WriteLineJoin(exp);
        return null;
    }
}
