using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Number
{
    internal class BigIntegerDecimalAdditionTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/addition_of_big_integers";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int Q = cr;
            while (--Q >= 0)
                cw.WriteLine(BigIntegerDecimal.Parse(cr.AsciiChars()) + BigIntegerDecimal.Parse(cr.AsciiChars()));
            return null;
        }
    }
    internal class BigIntegerDecimalMultiplicationTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/multiplication_of_big_integers";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int Q = cr;
            while (--Q >= 0)
                cw.WriteLine(BigIntegerDecimal.Parse(cr.AsciiChars()) * BigIntegerDecimal.Parse(cr.AsciiChars()));
            return null;
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