using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Graph
{
    internal class 閉路削除Test : BaseSolver
    {
        public override string Url => "https://yukicoder.me/problems/no/1983";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            int Q = cr;
            var g = GraphBuilder.Create(N, cr, M, false).ToGraph();
            var es = g.RemoveCycle();

            var uf = new UnionFind(N);
            foreach (var (u, v) in es)
                uf.Merge(u, v);
            while (--Q >= 0)
            {
                cw.WriteLine(uf.Same(cr.Int0(), cr.Int0()) ? "Yes" : "No");
            }
            return null;
        }
    }
}