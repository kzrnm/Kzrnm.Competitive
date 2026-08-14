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
            int v;
            switch (t)
            {
                case 0: set.Add(x); break;
                case 1: set.Remove(x); break;
                case 2: cw.WriteLine(x <= set.Count ? set.FindByIndex(x - 1).Node.Value : -1); break;
                case 3: cw.WriteLine(set.UpperBoundIndex(x)); break;
                case 4: cw.WriteLine(set.TryGetReverseLowerBound(x, out v) ? v : -1); break;
                case 5: cw.WriteLine(set.TryGetLowerBound(x, out v) ? v : -1); break;
            }
        }
        return null;
    }
}
