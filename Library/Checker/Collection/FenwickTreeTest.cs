using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Collection
{
    internal class FenwickTreeTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/point_add_range_sum";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var fw = new LongFenwickTree(N);
            fw.Add(cr.Repeat<long>(N));
            while (--Q >= 0)
            {
                int t = cr;
                int l = cr;
                int r = cr;
                if (t == 0)
                    fw.Add(l, r);
                else
                    cw.WriteLine(fw[l..r]);
            }
            return null;
        }
    }
}
