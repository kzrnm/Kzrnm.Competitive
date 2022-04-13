using AtCoder;
using Kzrnm.Competitive.IO;
using System;

namespace Kzrnm.Competitive.MathNs.Fps
{
    public class FormalPowerSeriesLogTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/log_of_formal_power_series
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int[] arr = cr.Repeat(N);
            var log = new FormalPowerSeries<Mod998244353>(arr).Log().Coefficients;
            Array.Resize(ref log, N);
            cw.WriteLineJoin(log.AsSpan().Cast<StaticModInt<Mod998244353>, uint>());
            return null;
        }
    }
}
