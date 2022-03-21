using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure
{
    public class BinaryTrieTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/set_xor_min
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            var bt = new BinaryTrie(30);
            for (int q = 0; q < N; q++)
            {
                int t = cr;
                uint x = cr;
                if (t == 0)
                {
                    if (bt.Count(x) == 0) bt.Increment(x);
                }
                else if (t == 1)
                {
                    if (bt.Count(x) != 0) bt.Decrement(x);
                }
                else
                    cw.WriteLine(bt.MinElement(x).Num);
            }
            return null;
        }
    }
}
