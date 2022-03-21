using Kzrnm.Competitive.IO;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Collection
{
    public class AssociativeArrayTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/associative_array
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int Q = cr;
            var dic = new Dictionary<long, long>();
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
            return null;
        }
    }
}
