using Kzrnm.Competitive.IO;
using System.Linq;

namespace Kzrnm.Competitive.Graph.Generics;

internal class 最小全域木Kruskal : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/minimum_spanning_tree";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int M = cr;
        var g = cr.GraphWithEdgeIndex<ulong>(N, M, false, 0).ToGraph();
        var t = g.MinimumSpanningTreePrim();
        cw.WriteLine(t.Cost).WriteLineJoin(t.Edges.Select(e => e.edge.Data));
        return null;
    }
}