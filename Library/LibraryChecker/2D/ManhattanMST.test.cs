﻿using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.TwoDimensional
{
    public class ManhattanMSTTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/manhattanmst
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            var pts = cr.Repeat(N).Select<(long, long)>(cr => (cr, cr));

            var res = ManhattanMST.Solve(pts);
            long x = 0;

            foreach (var (i1, i2) in res)
            {
                x += System.Math.Abs(pts[i1].Item1 - pts[i2].Item1)
                    + System.Math.Abs(pts[i1].Item2 - pts[i2].Item2);
            }
            cw.WriteLine(x);
            cw.WriteGrid(res);
            return null;
        }
    }
}
