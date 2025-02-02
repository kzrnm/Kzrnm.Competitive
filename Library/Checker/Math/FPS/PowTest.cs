using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs.Fps;

internal class FormalPowerSeriesPowTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/pow_of_formal_power_series";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        long M = cr;
        int[] arr = cr.Repeat(N);
        var pow = new FormalPowerSeries<Mod998244353>(arr).Pow(M, N).Coefficients(N);
        cw.WriteLineJoin(pow);
        return null;
    }
}
