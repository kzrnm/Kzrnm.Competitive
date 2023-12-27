using Kzrnm.Competitive.IO;
using MontgomeryModInt = Kzrnm.Competitive.MontgomeryModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.Math
{
    internal class KthLinearRecurrenceTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/kth_term_of_linearly_recurrent_sequence";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int d = cr;
            long k = cr;
            var a = cr.Repeat(d).Select(cr => (MontgomeryModInt)cr.Int());
            var c = cr.Repeat(d).Select(cr => (MontgomeryModInt)cr.Int());
            return LinearRecurrence.Kitamasa(a, c, k);
        }
    }
}
