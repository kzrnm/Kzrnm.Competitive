using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Collection;

internal class SetTest2 : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/ordered_set";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int Q = cr;
        var set = new Set<int>(cr.Repeat(N).Int());
        for (int i = 0; i < Q; i++)
        {
            int t = cr;
            int x = cr;
            switch (t)
            {
                case 0: set.Add(x); break;
                case 1: set.Remove(x); break;
                case 2: cw.WriteLine(x <= set.Count ? set.FindByIndex(x - 1).Value : -1); break;
                case 3: cw.WriteLine(set.UpperBoundIndex(x)); break;
                case 4: cw.WriteLine(set.FindNodeReverseLowerBound(x)?.Value ?? -1); break;
                case 5: cw.WriteLine(set.FindNodeLowerBound(x)?.Value ?? -1); break;
            }
        }
        return null;
    }
}
