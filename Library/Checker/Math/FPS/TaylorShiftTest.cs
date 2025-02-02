using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs.Fps;

internal class FormalPowerSeriesTaylorShiftTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/polynomial_taylor_shift";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        MontgomeryModInt<Mod998244353> c = cr;
        var a = cr.Repeat(N).Select<MontgomeryModInt<Mod998244353>>(cr => cr);
        var fps = new FormalPowerSeries<Mod998244353>(a);
        cw.WriteLineJoin(fps.TaylorShift(c).Coefficients(N));
        return null;
    }
}
