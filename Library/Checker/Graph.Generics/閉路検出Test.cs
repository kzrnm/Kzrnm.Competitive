using Kzrnm.Competitive.IO;
using System.Linq;

namespace Kzrnm.Competitive.Graph
{
    internal class 閉路検出Test : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/cycle_detection";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            var gb = new GraphBuilder<int>(N, true);
            for (var i = 0; i < M; i++)
                gb.Add(cr, cr, i);
            var graph = gb.ToGraph();
            var edges = graph.GetCycleDfs().edges;
            if (edges == null)
                return -1;
            cw.WriteLine(edges.Length);
            cw.WriteLines(edges.Select(e => e.Data));
            return null;
        }
    }
}