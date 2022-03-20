﻿using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure
{
    public class SumsTest
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/static_range_sum
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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
