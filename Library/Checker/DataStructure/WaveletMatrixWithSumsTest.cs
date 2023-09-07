using Kzrnm.Competitive.IO;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.DataStructure
{
    internal class WaveletMatrixWithSumsTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/rectangle_sum";
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
            var wm = new LongWaveletMatrix2DWithSums(dic.Keys.Zip(dic.Values).ToArray());

            for (int q = 0; q < Q; q++)
            {
                int l = cr;
                int d = cr;
                int r = cr;
                int u = cr;
                cw.WriteLine(wm.RectSum(l, r, d, u));
            }
            return null;
        }
    }
}
