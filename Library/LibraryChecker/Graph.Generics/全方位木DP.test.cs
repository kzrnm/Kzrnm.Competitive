using Kzrnm.Competitive.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Graph.Generics
{
    internal class 全方位木DP
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_5_A
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            var gb = new WLongGraphBuilder(N, false);
            for (int i = 1; i < N; i++)
                gb.Add(cr, cr, cr);

            var tree = gb.ToTree(0);
            var dp = tree.Rerooting().Run<(long First, long Second), Op>();
            return dp.Max(t => t.First + t.Second);
        }

        struct Op : IRerootingOperator<(long First, long Second), WEdge<long>>
        {
            public (long First, long Second) Identity => (0, 0);
            [MethodImpl(256)]
            public (long First, long Second) Merge((long First, long Second) x1, (long First, long Second) x2)
            {
                var f1 = x1.First;
                var f2 = x2.First;
                if (f1 < f2) (f1, f2) = (f2, f1);

                return (f1, System.Math.Max(f2, System.Math.Max(x1.Second, x2.Second)));
            }
            [MethodImpl(256)]

            public (long First, long Second) Propagate((long First, long Second) x, int parent, WEdge<long> childEdge)
            {
                return (x.First + childEdge.Value, 0);
            }
        }
    }
}