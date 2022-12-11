using Kzrnm.Competitive.IO;
using System.Linq;

namespace Kzrnm.Competitive.Graph
{
    internal class 最短経路Dijkstra復元付きTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/shortest_path";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            int s = cr;
            int t = cr;
            var gb = new WULongGraphBuilder(N, true);
            for (int i = 0; i < M; i++)
                gb.Add(cr, cr, cr);

            var (len, route) = gb.ToGraph().DijkstraWithRoute(s)[t];
            if (route == null)
                return -1;
            var ra = route.ToArray().Reverse();
            cw.WriteLineJoin(len, ra.Length);
            foreach (var e in ra)
            {
                var u = e.To;
                cw.WriteLineJoin(s, u);
                s = u;
            }
            return null;
        }
    }
}