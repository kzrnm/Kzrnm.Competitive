using Kzrnm.Competitive.IO;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Collection;

internal class SetAsAssociativeArrayTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/associative_array";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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
                cw.WriteLine(dic.GetValueOrDefault(k));
        }
        return null;
    }
}
