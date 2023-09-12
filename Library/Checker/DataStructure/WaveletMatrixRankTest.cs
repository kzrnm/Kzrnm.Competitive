using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure
{
    internal class WaveletMatrixRankTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/static_range_frequency";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            int[] a = cr.Repeat(N);
            var wm = new WaveletMatrix<int>(a);
            for (int q = 0; q < Q; q++)
            {
                int l = cr;
                int r = cr;
                int x = cr;
                var res0 = wm.Rank(l, r, x);
                var res1 = wm.Rank(r, x) - wm.Rank(l, x);
                var res2 = wm.RangeFreq(l, r, x, x + 1);
                if (res0 != res1 || res1 != res2) Throw(res0, res1, res2);
                cw.WriteLine(res1);
            }
            return null;
        }
        static void Throw(int res0, int res1, int res2) => throw new System.Exception($"Result failed. res0 = {res0}, res1 = {res1}, res2 = {res2}");
        /*
         * 実は二分探索の方が高速
        static ConsoleOutput? Solve2(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var a = cr.Repeat(N).Select<(int Value, int Index)>((cr, i) => (cr, i)).Sort();
            for (int q = 0; q < Q; q++)
            {
                int l = cr;
                int r = cr;
                int x = cr;
                var upper = AtCoder.Extension.BinarySearchExtension.LowerBound(a, (x, r));
                var lower = AtCoder.Extension.BinarySearchExtension.LowerBound(a, (x, l));
                cw.WriteLine(upper - lower);
            }
            return null;
        }
        */
    }
}
