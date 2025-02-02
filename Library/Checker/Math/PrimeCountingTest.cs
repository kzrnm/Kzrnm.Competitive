using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs;

internal class PrimeCountingTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/counting_primes";
    public override double? Tle => 5;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        ulong n = cr;
        return PrimeCounting.Count(n);
    }
}
