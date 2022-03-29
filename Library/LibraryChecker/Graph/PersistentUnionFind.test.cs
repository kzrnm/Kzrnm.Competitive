using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Graph
{
    public class PersistentUnionFindTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/persistent_unionfind
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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
            return null;
        }
    }
}
