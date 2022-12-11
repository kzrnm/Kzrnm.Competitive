using AtCoder;
using Kzrnm.Competitive.IO;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using ModIntOperator = AtCoder.StaticModIntOperator<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs
{
    internal class ModSqrtTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/sqrt_mod";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            for (int i = 0; i < N; i++)
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
