using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class ULongNthRootSolver : ISolver
    {
        public string Name => "kth_root_integer";

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
