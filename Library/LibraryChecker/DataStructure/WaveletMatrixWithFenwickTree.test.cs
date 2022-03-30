using AtCoder;
using AtCoder.Extension;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.DataStructure
{
    public class WaveletMatrixWithFenwickTreeTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/point_add_rectangle_sum
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var dic = new Dictionary<(int x, int y), long>();
            for (int i = 0; i < N; i++)
            {
                int x = cr;
                int y = cr;
                int w = cr;
                var tup = (x, y);
                dic.TryGetValue(tup, out var ww);
                dic[tup] = ww + w;
            }
            var queries = cr.Repeat(Q).Select(cr =>
            {
                int t = cr;
                int l = cr;
                int d = cr;
                int r = cr;
                if (t == 0)
                    return (l, d, r, u: -1);
                int u = cr;
                return (l, d, r, u);
            });
            foreach (var (x, y, _, _) in queries)
            {
                var tup = (x, y);
                dic.TryAdd(tup, 0);
            }
            var ps = dic.Keys.ToArray();
            var vs = dic.Values.ToArray();
            Array.Sort(ps, vs);
            var wm = new WaveletMatrixWithFenwickTree<int, long, LongOperator>(ps.Zip(vs, (p, v) => (p.y, v)).ToArray());

            foreach (var (l, d, r, u) in queries)
            {
                if (u < 0)
                {
                    var ix = ps.LowerBound((l, d));
                    wm.PointAdd(ix, r);
                }
                else
                {
                    var a = ps.LowerBound((l, -1));
                    var c = ps.LowerBound((r, -1));
                    cw.WriteLine(wm.RectSum(a, c, d, u));
                }
            }
            return null;
        }
    }
}
