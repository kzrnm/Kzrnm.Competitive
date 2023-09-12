using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs
{
    internal class ModSqrtTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/sqrt_mod";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int Q = cr;
            while (--Q >= 0)
            {
                int y = cr;
                int p = cr;
                DynamicModInt<int>.Mod = p;
                var res = ModSqrt.Solve(new DynamicModInt<int>(y));
                if (res != ModSqrt.Solve(y, p))
                    return "Invalid";
                cw.WriteLine(res);
            }
            return null;
        }
    }
}
