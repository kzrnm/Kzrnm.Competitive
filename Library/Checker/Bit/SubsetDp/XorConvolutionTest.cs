using Kzrnm.Competitive.IO;
using System.Runtime.InteropServices;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.Collection
{
    internal class XorConvolutionTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/bitwise_xor_convolution";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int size = 1 << cr.Int();
            var a = MemoryMarshal.Cast<int, ModInt>(cr.Repeat(size).Int());
            var b = MemoryMarshal.Cast<int, ModInt>(cr.Repeat(size).Int());
            cw.WriteLineJoin(XorConvolution.Convolution(a, b));
            return null;
        }
    }
}
