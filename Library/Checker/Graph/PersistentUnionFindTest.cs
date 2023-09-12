using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Graph
{
    internal class PersistentUnionFindTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/persistent_unionfind";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var G = new PersistentUnionFind[Q + 1];
            G[^1] = new PersistentUnionFind(N);
            for (int i = 0; i < Q; i++)
            {
                int ty = cr;
                int k = cr;
                int u = cr;
                int v = cr;
                if (ty == 0)
                {
                    G[i] = G.Get(k).Merge(u, v);
                }
                else
                {
                    cw.WriteLine(G.Get(k).Same(u, v) ? 1 : 0);
                }
            }
            return null;
        }
    }
}
