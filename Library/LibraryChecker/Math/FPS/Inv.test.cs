using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs.Fps
{
    public class FormalPowerSeriesInvTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/inv_of_formal_power_series
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int[] arr = cr.Repeat(N);
            cw.WriteLineJoin(new FormalPowerSeries<Mod998244353>(arr).Inv().Coefficients);
            return null;
        }
    }
}
