using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Collection
{
    public class SetAsAssociativeArrayTest
    {
        static void Main() { using var cw = new ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/associative_array
        static void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int Q = cr;
            var dic = new SetDictionary<long, long>();
            for (int i = 0; i < Q; i++)
            {
                int t = cr;
                long k = cr;
                if (t == 0)
                {
                    long v = cr;
                    dic[k] = v;
                }
                else
                    cw.WriteLine(dic.Get(k));
            }
        }
    }
}
