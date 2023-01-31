using Kzrnm.Competitive.IO;
using System.Linq;

namespace Kzrnm.Competitive.MathNs
{
    internal class PrimeFactorizationTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/factorize";
        public override double? Tle => 10;
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int Q = cr;
            while (--Q >= 0)
            {
                long n = cr;
                var arr = PrimeFactorization.EnumerateFactors(n).ToArray().Sort();
                if (arr.Length == 0)
                    cw.WriteLine(0);
                else
                    cw.Write(arr.Length).Write(' ').WriteLineJoin(arr);
            }
            return null;
        }
    }
}
