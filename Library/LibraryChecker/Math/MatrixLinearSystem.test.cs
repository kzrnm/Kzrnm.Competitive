﻿using Kzrnm.Competitive.IO;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using ModIntOperator = AtCoder.StaticModIntOperator<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs
{
    public class MatrixLinearSystemTest
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/system_of_linear_equations
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int M = cr;
            var mat = new ArrayMatrix<ModInt, ModIntOperator>(cr.Grid(N, M, cr => ModInt.Raw(cr)));

            var r = mat.LinearSystem(cr.Repeat(N).Select(cr => ModInt.Raw(cr)));
            cw.WriteLine(r.Length - 1);
            if (r.Length > 0)
                cw.WriteGrid(r);
        }
    }
}