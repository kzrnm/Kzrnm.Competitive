﻿using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class BinaryTrieSolver
    {
        static void Main()
        {
            using (var cw = new ConsoleWriter()) Solve(new ConsoleReader(), cw);
        }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/set_xor_min
        public double TimeoutSecond => 5;
        static void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int N = cr;
            var bt = new BinaryTrie(30);
            for (int q = 0; q < N; q++)
            {
                int t = cr;
                uint x = cr;
                if (t == 0)
                {
                    if (bt.Count(x) == 0) bt.Increment(x);
                }
                else if (t == 1)
                {
                    if (bt.Count(x) != 0) bt.Decrement(x);
                }
                else
                    cw.WriteLine(bt.MinElement(x).Num);
            }
        }
    }
}
