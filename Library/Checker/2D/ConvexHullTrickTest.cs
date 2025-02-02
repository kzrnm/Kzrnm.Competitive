using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.TwoDimensional;

internal class ConvexHullTrickTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/line_add_get_min";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int Q = cr;
        var lines = cr.Repeat(N).Select<(long a, long b)>(cr => (cr, cr));
        var queries = cr.Repeat(Q).Select(ToQuery);
        var zc = new ZahyoCompress<long>();
        foreach ((long a, long b) in queries)
            if (b == INF)
                zc.Add(a);
        zc.Compress();
        var ch = new LongMinConvexHullTrick(zc.Original, 1 + (long)1e9, INF);
        foreach ((long a, long b) in lines)
            ch.AddLine(a, b);
        foreach ((long a, long b) in queries)
        {
            if (b == INF)
            {
                var res = ch.Query(zc.NewTable[a]);
                cw.WriteLine(res);
            }
            else
                ch.AddLine(a, b);
        }
        return null;
    }
    const long INF = (long)7e18;
    static (long a, long b) ToQuery(ConsoleReader cr)
    {
        int t = cr;
        long a = cr;
        long b = INF;
        if (t == 0)
            b = cr;
        return (a, b);
    }
}
