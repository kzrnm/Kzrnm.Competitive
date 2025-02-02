using AtCoder;
using Kzrnm.Competitive.IO;
using System.Collections.Generic;

namespace Kzrnm.Competitive.MathNs.Bit;

internal class BitMatrix512Test : BaseSolver
{
    public override string Url => "https://yukicoder.me/problems/no/803";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int M = cr;
        int X = cr;
        int[] A = cr.Repeat(N);
        var bits = new List<UInt512>();
        for (int b = 0; b < 31; b++)
        {
            var bb = new UInt512();
            for (int i = 0; i < A.Length; i++)
                if (A[i].On(b))
                    bb |= UInt512.One << i;
            bits.Add(bb);
        }
        var xb = (UInt512)X;
        for (int b = 0; b < M; b++)
        {
            xb |= (UInt512)cr.Int() << (b + 31);
            int l = cr.Int0();
            int r = cr.Int();
            var bb = (UInt512.One << (r - l)) - 1;
            bits.Add(bb << l);
        }
        var len = new BitMatrix<UInt512>(bits.ToArray()).LinearSystem(xb, width: N).Length - 1;
        return len < 0 ? 0 : StaticModInt<Mod1000000007>.Raw(2).Pow(len).Value;
    }
}