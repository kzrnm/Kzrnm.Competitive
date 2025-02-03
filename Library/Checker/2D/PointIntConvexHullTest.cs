using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.TwoDimensional;

internal class PointIntConvexHullTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/static_convex_hull";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int T = cr;
        while (--T >= 0)
        {
            int N = cr;
            var pts = cr.Repeat(N).Select(cr => new PointInt(cr, cr));
            var idx = PointInt.ConvexHull(pts, strict: true);
            cw.WriteLine(idx.Length);
            foreach (var i in idx)
                cw.WriteLine(pts[i]);
        }
        return null;
    }
}
