using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure
{
    public class WaveletMatrixKthSmallestTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/range_kth_smallest
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;
            int[] a = cr.Repeat(n);
            var mat = new WaveletMatrix<int>(a);
            for (int i = 0; i < q; i++)
            {
                int l = cr;
                int r = cr;
                int k = cr;
                cw.WriteLine(mat.KthSmallest(l, r, k));
            }
            return null;
        }
    }
}

