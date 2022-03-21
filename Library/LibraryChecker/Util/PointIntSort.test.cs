using AtCoder.Extension;
using Kzrnm.Competitive.IO;
using System;

namespace Kzrnm.Competitive.MathNs
{
    public class PointIntSortTest
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/sort_points_by_argument
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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
