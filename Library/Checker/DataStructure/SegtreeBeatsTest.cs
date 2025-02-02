using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure;

internal class SegtreeBeatsTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/range_chmin_chmax_add_range_sum";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int Q = cr;
        long[] a = cr.Repeat(N);
        var seg = new SegtreeBeats(a);
        for (int q = 0; q < Q; q++)
        {
            int t = cr;
            int l = cr;
            int r = cr;
            if (t == 3)
                cw.WriteLine(seg[l..r].sum);
            else
            {
                long b = cr;
                if (t == 0)
                    seg.Apply(l, r, SegtreeBeats.MinOp(b));
                else if (t == 1)
                    seg.Apply(l, r, SegtreeBeats.MaxOp(b));
                else
                    seg.Apply(l, r, SegtreeBeats.AddOp(b));
            }
        }
        return null;
    }
}