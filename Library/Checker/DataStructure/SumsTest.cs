using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure
{
    internal class SumsTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/static_range_sum";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;
            var sums = new LongSums(cr.Repeat(n));
            for (int i = 0; i < q; i++)
            {
                int l = cr;
                int r = cr;
                cw.WriteLine(sums[l..r]);
            }
            return null;
        }
    }
}
