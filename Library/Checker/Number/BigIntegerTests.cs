#if NET7_0_OR_GREATER
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Number
{
    internal class BigIntegerAdditionTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/addition_of_big_integers";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            return (BigIntegerDecimal.Parse(cr.AsciiChars()) + BigIntegerDecimal.Parse(cr.AsciiChars())).ToString();
        }
    }
    //internal class BigIntegerDivisionTest : BaseSolver
    //{
    //    public override string Url => "https://judge.yosupo.jp/problem/division_of_big_integers";
    //    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    //    {
    //        var (q, r) = BigIntegerDecimal.DivRem(BigIntegerDecimal.Parse(cr.AsciiChars()), BigIntegerDecimal.Parse(cr.AsciiChars()));
    //        cw.WriteLineJoin(q, r);
    //        return null;
    //    }
    //}
}
#endif