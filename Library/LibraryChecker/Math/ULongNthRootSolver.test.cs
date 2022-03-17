using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class ULongNthRootSolver
    {
        static void Main() => new ULongNthRootSolver().Solve(new ConsoleReader(), new ConsoleWriter());
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/kth_root_integer
        public double TimeoutSecond => 10;
        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int Q = cr;
            for (int q = 0; q < Q; q++)
            {
                ulong a = cr;
                int k = cr;
                cw.WriteLine(NthRoots.IntegerRoot(a, k));
            }
        }
    }
}
