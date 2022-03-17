﻿using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class WaveletMatrixSolver
    {
        static void Main()
        {
            using (var cw = new ConsoleWriter()) Solve(new ConsoleReader(), cw);
        }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/range_kth_smallest
        public double TimeoutSecond => 5;
        static void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;
            int[] a = cr.Repeat(n);
            var mat = new WaveletMatrix<int>(a);
            for (int i = 0; i < q; i++)
            {
                int l = cr;
                int r = cr;
                int k = cr;
                cw.WriteLine(mat.KthSmallest(l, r, k));
            }
        }
    }
}

