using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Graph
{
    public class オイラーツアーとFenwickTreeTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/vertex_add_path_sum
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            long[] a = cr.Repeat(N);
            var gb = new GraphBuilder(N, false);
            for (var i = 1; i < N; i++)
                gb.Add(cr, cr);

            var tree = gb.ToTree();
            var et = tree.EulerianTour();
            var fw = new LongFenwickTree(2 * N);
            for (int i = 0; i < a.Length; i++)
            {
                var x = a[i];
                fw.Add(et[i].left, -x);
                fw.Add(et[i].right, x);
            }
            for (int q = 0; q < Q; q++)
            {
                int t = cr;
                if (t == 0)
                {
                    int p = cr;
                    int x = cr;
                    a[p] += x;
                    fw.Add(et[p].left, -x);
                    fw.Add(et[p].right, x);
                }
                else
                {
                    int u = cr;
                    int v = cr;
                    var lca = tree.HlDecomposition.LowestCommonAncestor(u, v);

                    long res = 0;
                    if (u != lca)
                        res += fw[et[u].right..et[lca].right];
                    if (v != lca)
                        res += fw[et[v].right..et[lca].right];
                    res += a[lca];
                    cw.WriteLine(res);
                }
            }
            return null;
        }
    }
}