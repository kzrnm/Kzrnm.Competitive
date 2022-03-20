using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs
{
    public class ULongNthRootTest
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/kth_root_integer
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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
