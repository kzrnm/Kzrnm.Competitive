using Kzrnm.Competitive.IO;
using System.Linq;

namespace Kzrnm.Competitive.Collection;

internal class SetTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/predecessor_problem";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int Q = cr;
        Asciis T = cr;
        var set = new Set<int>(T.Select((c, i) => (c, i)).Where(t => t.c == '1').Select(t => t.i));
        for (int i = 0; i < Q; i++)
        {
            int t = cr;
            int k = cr;
            _ = t switch
            {
                0 => set.Add(k),
                1 => set.Remove(k),
                2 => cw.WriteLine(set.FindNode(k) is null ? 0 : 1),
                3 => cw.WriteLine(set.FindNodeLowerBound(k)?.Value ?? -1),
                _ => (object)cw.WriteLine(set.FindNodeReverseLowerBound(k)?.Value ?? -1),
            };
        }
        return null;
    }
}
