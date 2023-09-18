using AtCoder;
using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.MathNs
{
    internal class ConvolutionULongTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/convolution_mod";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            uint[] arr = cr.Repeat(N);
            uint[] brr = cr.Repeat(M);
            cw.WriteLineJoin(NumberTheoreticTransform.Convolution<Mod998244353>(arr, brr));
            return null;
        }
    }
    internal class Convolution1000000007Test : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/convolution_mod_1000000007";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            uint[] arr = cr.Repeat(N);
            uint[] brr = cr.Repeat(M);
            cw.WriteLineJoin(NumberTheoreticTransform.Convolution(arr, brr, 1000000007));
            return null;
        }
    }
}
