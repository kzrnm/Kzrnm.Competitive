using Kzrnm.Competitive.IO;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.DataStructure
{
    internal class WaveletMatrixWithFenwickTreeTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/point_add_rectangle_sum";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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
            foreach (var (x, y, _, u) in queries)
                if (u < 0)
                    dic.TryAdd((x, y), 0);
            var wm = new LongWaveletMatrix2DWithFenwickTree(dic.Keys.Zip(dic.Values).ToArray());

            foreach (var (l, d, r, u) in queries)
            {
                if (u < 0)
                    wm.PointAdd(l, d, r);
                else
                    cw.WriteLine(wm.RectSum(l, r, d, u));
            }
            return null;
        }
    }
}
