using AtCoder;
using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;

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
            cw.WriteLines(mo.SolveStrict<long, St>(new St(ZahyoCompress.CompressedArray(A))));
            return null;
        }

        class St : IMoAlgorithmStateStrict<long>
        {
            [MethodImpl(256)]
            public void AddLeft(int idx)
            {
                var v = a[idx];
                Current += fw[..v];
                fw.Add(v, 1);
            }

            [MethodImpl(256)]
            public void AddRight(int idx)
            {
                var v = a[idx];
                Current += fw[(v + 1)..];
                fw.Add(v, 1);
            }

            [MethodImpl(256)]
            public void RemoveLeft(int idx)
            {
                var v = a[idx];
                Current -= fw[..v];
                fw.Add(v, -1);
            }

            [MethodImpl(256)]
            public void RemoveRight(int idx)
            {
                var v = a[idx];
                Current -= fw[(v + 1)..];
                fw.Add(v, -1);
            }
            private readonly int[] a;
            private readonly LongFenwickTree fw;

            public St(int[] a)
            {
                this.a = a;
                fw = new LongFenwickTree(a.Length);
                Current = 0;
            }

            public long Current { get; set; }
        }
    }
}
