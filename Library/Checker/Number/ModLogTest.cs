using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Number;

internal class ModLogTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/discrete_logarithm_mod";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int Q = cr;
        while (--Q >= 0)
        {
            int x = cr;
            int y = cr;
            int p = cr;
            cw.WriteLine(ModLog.Solve(x, y, p));
        }
        return null;
    }
}
