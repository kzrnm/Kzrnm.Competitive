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

            (int l, int r, int a, long b)[] lines = cr.Repeat(N).Select<(int l, int r, int a, long b)>(cr => (cr, cr, cr, cr));
            (int t, int l, int r, int a, long b)[] queries = cr.Repeat(Q).Select<(int t, int l, int r, int a, long b)>(cr =>
            {
                int t = cr;
                if (t == 0)
                    return (0, cr, cr, cr, cr);
                return (1, cr, 0, 0, 0);
            });
            var zc = new ZahyoCompress<long>();
            foreach ((int l, int r, int a, long b) in lines)
            {
                zc.Add(l);
                zc.Add(r);
            }
            foreach ((int t, int l, int r, int a, long b) in queries)
            {
                zc.Add(l);
                zc.Add(r);
            }
            zc.Compress();
            var ch = new LongMinConvexHullTrick(zc.Original);
            foreach ((int l, int r, int a, long b) in lines)
                ch.AddSegmentLine(a, b, zc.NewTable[l], zc.NewTable[r]);

            foreach ((int t, int l, int r, int a, long b) in queries)
            {
                if (t == 0)
                    ch.AddSegmentLine(a, b, zc.NewTable[l], zc.NewTable[r]);
                else
                    _ = ch.Query(zc.NewTable[l]) switch { long.MaxValue => cw.WriteLine("INFINITY"), var v => cw.WriteLine(v) };
            }
            return null;
        }
    }
}
