using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure
{
    internal class ManacherTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/enumerate_palindromes";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            string S = cr;
            var rt = Palindrome.Manacher2(S);
            return cw.WriteLineJoin(rt);
        }
    }
}
