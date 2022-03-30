using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Graph
{
    public class HL分解の部分木Test
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/vertex_add_subtree_sum
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            int[] a = cr.Repeat(N);
            var gb = new GraphBuilder(N, false);
            for (var i = 1; i < N; i++)
                gb.Add(i, cr);
            var tree = gb.ToTree();
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