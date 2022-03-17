using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class PersistentUnionFindSolver : Solver
    {
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/persistent_unionfind
        public override double TimeoutSecond => 5;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;

            var ufs = new PersistentUnionFind[Q];
            ufs[^1] = new PersistentUnionFind(N);

            for (int i = 0; i < Q; i++)
            {
                int t = cr;
                int k = cr;
                int u = cr;
                int v = cr;
                if (t == 0)
                    ufs[i] = ufs.Get(k).Merge(u, v);
                else
                    cw.WriteLine(ufs.Get(k).Same(u, v) ? "1" : "0");
            }
        }
    }
}
