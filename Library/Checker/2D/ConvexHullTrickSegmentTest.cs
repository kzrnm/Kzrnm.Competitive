using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.TwoDimensional
{
    internal class ConvexHullTrickSegmentTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/segment_add_get_min";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var lines = cr.Repeat(N).Select<(long l, long r, long a, long b)>(cr => (cr, cr, cr, cr));
            var queries = cr.Repeat(Q).Select(ToQuery);
            var zc = new ZahyoCompress<long>();
            foreach ((long l, long r, long a, long b) in lines)
            {
                zc.Add(l);
                zc.Add(r);
            }
            foreach ((long l, long r, long a, long b) in queries)
            {
                zc.Add(l);
                zc.Add(r);
            }
            zc.Add(INF);
            zc.Compress();
            var ch = new LongMinConvexHullTrick(zc.Original, 1 + (long)1e9, INF);
            foreach ((long l, long r, long a, long b) in lines)
                ch.AddSegmentLine(a, b, zc.NewTable[l], zc.NewTable[r]);
            foreach ((long l, long r, long a, long b) in queries)
            {
                if (r == INF)
                {
                    var res = ch.Query(zc.NewTable[l]);
                    if (res == INF)
                        cw.WriteLine("INFINITY");
                    else
                        cw.WriteLine(res);
                }
                else
                    ch.AddSegmentLine(a, b, zc.NewTable[l], zc.NewTable[r]);
            }
            return null;
        }
        const long INF = (long)7e18;
        static (long l, long r, long a, long b) ToQuery(ConsoleReader cr)
        {
            int t = cr;
            long l = cr;
            long r = INF;
            long a = INF;
            long b = INF;
            if (t == 0)
            {
                r = cr;
                a = cr;
                b = cr;
            }
            return (l, r, a, b);
        }
    }
}
