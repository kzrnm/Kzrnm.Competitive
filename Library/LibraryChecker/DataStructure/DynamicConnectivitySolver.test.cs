using AtCoder;
using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class DynamicConnectivitySolver
    {
        static void Main()
        {
            using (var cw = new ConsoleWriter()) Solve(new ConsoleReader(), cw);
        }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/dynamic_graph_vertex_add_component_sum
        public double TimeoutSecond => 10;
        static void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int k = cr;
            var dc = new DynamicConnectivity<long, Op>(n);
            for (int i = 0; i < n; i++)
                dc[i] = cr;
            for (int i = 0; i < k; i++)
            {
                int x = cr;
                if (x == 0)
                {
                    int u = cr;
                    int v = cr;
                    dc.Link(u, v);
                }
                else if (x == 1)
                {
                    int u = cr;
                    int v = cr;
                    dc.Cut(u, v);
                }
                else if (x == 2)
                {
                    int v = cr;
                    int a = cr;
                    dc.Apply(v, a);
                }
                else if (x == 3)
                {
                    int v = cr;
                    cw.WriteLine(dc.Prod(v));
                }
            }
        }
        struct Op : ISegtreeOperator<long>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public long Operate(long x, long y) => x + y;
            public long Identity => default;
        }
    }
}
