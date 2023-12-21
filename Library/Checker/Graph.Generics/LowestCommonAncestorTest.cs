using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Graph
{
    internal class LowestCommonAncestorTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/lca";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var tree = cr.TreeParent(N, based: 0).ToTree();

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