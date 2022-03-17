using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class PersistentUnionFindSolver
    {
        static void Main() => new PersistentUnionFindSolver().Solve(new ConsoleReader(), new ConsoleWriter()).Flush();
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/persistent_unionfind
        public double TimeoutSecond => 5;
        public ConsoleWriter Solve(ConsoleReader cr, ConsoleWriter cw)
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
            return cw;
        }
    }
}
