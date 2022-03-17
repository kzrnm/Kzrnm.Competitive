﻿using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class SumsSolver
    {
        static void Main()
        {
            using (var cw = new ConsoleWriter()) Solve(new ConsoleReader(), cw);
        }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/static_range_sum
        public double TimeoutSecond => 5;
        static void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;
            var sums = new LongSums(cr.Repeat(n));
            for (int i = 0; i < q; i++)
            {
                int l = cr;
                int r = cr;
                cw.WriteLine(sums[l..r]);
            }
        }
    }
}
