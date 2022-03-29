using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure
{
    public class LowestCommonAncestorTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/lca
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var gb = new GraphBuilder(N, false);
            for (var i = 1; i < N; i++)
                gb.Add(i, cr);

            var tree = gb.ToTree(0);
            var hl = tree.HlDecomposition;
            var lca = tree.LowestCommonAncestorDoubling();
            for (int q = 0; q < Q; q++)
            {
                int u = cr;
                int v = cr;
                var res = lca.Lca(u, v);
                if (hl.LowestCommonAncestor(u, v) != res)
                    Throw();
                cw.WriteLine(res);
            }
            return null;
            static void Throw() => throw new System.Exception();
        }
    }
}