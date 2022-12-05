using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs
{
    internal class ULongNthRootTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/kth_root_integer";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int Q = cr;
            for (int q = 0; q < Q; q++)
            {
                ulong a = cr;
                int k = cr;
                cw.WriteLine(NthRoots.IntegerRoot(a, k));
            }
            return null;
        }
    }
}
