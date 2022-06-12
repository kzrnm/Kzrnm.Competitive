using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs
{
    public class MoAlgorithmTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/static_range_inversions_query
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            int[] A = cr.Repeat(N);
            var mo = new MoAlgorithm();
            for (int q = 0; q < Q; q++)
            {
                int l = cr;
                int r = cr;
                mo.AddQuery(l, r);
            }
            var a = ZahyoCompress.CompressedArray(A);
            var fw = new LongFenwickTree(a.Length);
            long Current = 0;
            var res = new long[Q];
            mo.SolveStrict(
                idx =>
                {
                    var v = a[idx];
                    Current += fw[..v];
                    fw.Add(v, 1);
                },
                idx =>
                {
                    var v = a[idx];
                    Current += fw[(v + 1)..];
                    fw.Add(v, 1);
                },
                idx =>
                {
                    var v = a[idx];
                    Current -= fw[..v];
                    fw.Add(v, -1);
                },
                idx =>
                {
                    var v = a[idx];
                    Current -= fw[(v + 1)..];
                    fw.Add(v, -1);
                },
                idx => res[idx] = Current);
            cw.WriteLines(res);
            return null;
        }
    }
}
