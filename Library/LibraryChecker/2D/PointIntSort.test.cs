using AtCoder.Extension;
using Kzrnm.Competitive.IO;
using System;

namespace Kzrnm.Competitive.TwoDimensional
{
    internal class PointIntSortTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/sort_points_by_argument";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            var pts = cr.Repeat(N).Select(cr => new PointInt(cr, cr)).Sort().AsSpan();
            var ix = pts.LowerBound(new PointInt(-1_000_100_000, 0));
            cw.WriteLines(pts[ix..]);
            cw.WriteLines(pts[..ix]);
            return null;
        }
    }
}
