using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs
{
    internal class ConvolutionLargeMod998244353Test : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/convolution_mod_large";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            uint[] arr = cr.Repeat(N);
            uint[] brr = cr.Repeat(M);
            cw.WriteLineJoin(ConvolutionLarge.Convolution<Mod998244353>(arr, brr));
            return null;
        }
    }
}
