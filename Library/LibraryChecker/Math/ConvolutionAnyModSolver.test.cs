﻿using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class ConvolutionAnyModSolver
    {
        static void Main()
        {
            using (var cw = new ConsoleWriter()) Solve(new ConsoleReader(), cw);
        }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/convolution_mod_1000000007
        public double TimeoutSecond => 10;
        static void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            uint[] arr = cr.Repeat(N);
            uint[] brr = cr.Repeat(M);
            cw.WriteLineJoin(ConvolutionAnyMod.Convolution(arr, brr, 1000000007));
        }
    }
}
