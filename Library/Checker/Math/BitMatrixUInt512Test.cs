#if NET7_0_OR_GREATER
using AtCoder;
using Kzrnm.Competitive.IO;
using Kzrnm.Competitive.Number;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.MathNs
{
    internal class BitMatrix512Test : BaseSolver
    {
        public override string Url => "https://yukicoder.me/problems/no/803";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            int X = cr;
            int[] A = cr.Repeat(N);
            var bits = new List<BitArray>();
            for (int b = 0; b < 31; b++)
            {
                var bb = new BitArray(N);
                for (int i = 0; i < A.Length; i++)
                    bb[i] = A[i].On(b);
                bits.Add(bb);
            }
            var xb = Enumerable.Repeat(false, 31).ToList();
            foreach (var b in X.Bits()) xb[b] = true;
            while (--M >= 0)
            {
                xb.Add(cr.Int() != 0);
                int l = cr.Int0();
                int r = cr.Int();
                var bb = new bool[N];
                bb.AsSpan()[l..r].Fill(true);
                bits.Add(new BitArray(bb));
            }
            var len = new BitMatrix<UInt512>(bits.ToArray()).LinearSystem(xb.ToArray()).Length - 1;
            return len < 0 ? 0 : StaticModInt<Mod1000000007>.Raw(2).Pow(len).Value;
        }
    }
}
#endif