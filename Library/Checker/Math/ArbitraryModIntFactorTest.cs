using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs;

internal class ArbitraryModIntFactorTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/binomial_coefficient";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int T = cr;
        var afm = new ArbitraryModIntFactor(cr);
        while (--T >= 0)
        {
            long n = cr;
            long k = cr;
            cw.WriteLine(afm.Combination(n, k));
        }
        return null;
    }
}
