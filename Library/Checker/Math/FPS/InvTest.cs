using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs.Fps
{
    internal class FormalPowerSeriesInvTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/inv_of_formal_power_series";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int[] arr = cr.Repeat(N);
            cw.WriteLineJoin(new FormalPowerSeries<Mod998244353>(arr).Inv().Coefficients(N));
            return null;
        }
    }
}
