using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Graph
{
    internal class HL分解の部分木Test : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/vertex_add_subtree_sum";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            int[] a = cr.Repeat(N);
            var tree = cr.TreeParent(N, based: 0).ToTree();

            var fw = new LongFenwickTree(N);
            for (var i = 0; i < a.Length; i++)
                fw.Add(tree.HlDecomposition.down[i], a[i]);
            for (int q = 0; q < Q; q++)
            {
                int t = cr;
                if (t == 0)
                {
                    int u = cr;
                    int x = cr;
                    fw.Add(tree.HlDecomposition.down[u], x);
                }
                else
                {
                    int u = cr;
                    var l = tree.HlDecomposition.down[u];
                    var r = tree.HlDecomposition.up[u];
                    cw.WriteLine(fw[l..r]);
                }
            }
            return null;
        }
    }
}