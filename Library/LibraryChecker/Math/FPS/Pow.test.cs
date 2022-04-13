using AtCoder;
using Kzrnm.Competitive.IO;
using System;

namespace Kzrnm.Competitive.MathNs.Fps
{
    public class FormalPowerSeriesPowTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/pow_of_formal_power_series
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            int[] arr = cr.Repeat(N);
            var pow = new FormalPowerSeries<Mod998244353>(arr).Pow(M).Coefficients;
            Array.Resize(ref pow, N);
            cw.WriteLineJoin(pow.AsSpan().Cast<StaticModInt<Mod998244353>, uint>());
            return null;
        }
    }
}
