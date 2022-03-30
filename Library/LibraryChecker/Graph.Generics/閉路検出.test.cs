using System.Linq;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Graph
{
    public class 閉路検出Test
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/cycle_detection
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            var gb = new GraphBuilder<int>(N, true);
            for (var i = 0; i < M; i++)
                gb.Add(cr, cr, i);
            var graph = gb.ToGraph();
            var edges = graph.GetCycleDFS().edges;
            if (edges == null)
                return -1;
            cw.WriteLine(edges.Length);
            cw.WriteLines(edges.Select(e => e.Data));
            return null;
        }
    }
}