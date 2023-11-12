using Kzrnm.Competitive.IO;
using System;
using System.Runtime.InteropServices;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.Collection
{
    internal class AndOrConvolutionTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/bitwise_and_convolution";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int size = 1 << cr.Int();
            var a = MemoryMarshal.Cast<int, ModInt>(cr.Repeat(size).Int());
            var b = MemoryMarshal.Cast<int, ModInt>(cr.Repeat(size).Int());
            var and = AndConvolution.Convolution(a, b);

            a.Reverse();
            b.Reverse();
            var or = OrConvolution.Convolution(a, b);
            or.AsSpan().Reverse();

            if (!and.AsSpan().SequenceEqual(or))
                throw new Exception("Reversed OrConvolution must be the same as AndConvolution");
            cw.WriteLineJoin(and);
            return null;
        }
    }
}
