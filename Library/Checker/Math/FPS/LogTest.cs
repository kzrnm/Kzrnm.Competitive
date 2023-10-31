using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs.Fps
{
    internal class FormalPowerSeriesLogTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/log_of_formal_power_series";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int[] arr = cr.Repeat(N);
            var log = new FormalPowerSeries<Mod998244353>(arr).Log().Coefficients(N);
            cw.WriteLineJoin(log);
            return null;
        }
    }
}
