using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Graph.Generics
{
    internal class 最小有向全域木Test : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/directedmst";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            int S = cr;
            var gb = new WLongGraphBuilder(N, true);
            for (int i = 0; i < M; i++)
                gb.Add(cr, cr, cr);

            var mst = gb.ToGraph().DirectedMinimumSpanningTree(S);
            cw.WriteLine(mst.Cost);
            var res = new int[N];
            res[S] = S;
            foreach (var (f, e) in mst.Edges)
                res[e.To] = f;
            cw.WriteLineJoin(res);
            return null;
        }
    }
}