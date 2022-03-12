using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class UnionFindSolver : Solver
    {
        public override string Name => "unionfind";
        public override double TimeoutSecond => 5;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;

            var dsu = new UnionFind(n);

            for (int i = 0; i < q; i++)
            {
                int t = cr;
                int u = cr;
                int v = cr;
                if (t == 0)
                    dsu.Merge(u, v);
                else
                    cw.WriteLine(dsu.Same(u, v) ? 1 : 0);
            }
        }
    }
}
