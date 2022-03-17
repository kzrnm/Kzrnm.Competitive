﻿using Kzrnm.Competitive.IO;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using ModIntOperator = AtCoder.StaticModIntOperator<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.Solvers.Math
{
    public class MatrixStrassenSolver
    {
        static void Main() => new MatrixStrassenSolver().Solve(new ConsoleReader(), new ConsoleWriter()).Flush();
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/matrix_product
        public double TimeoutSecond => 10;
        public ConsoleWriter Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            N = cr;
            M = cr;
            K = cr;
            var mat1 = new ArrayMatrix<ModInt, ModIntOperator>(
                cr.Repeat(N).Select(cr => cr.Repeat(M).Select(cr => ModInt.Raw(cr))));
            var mat2 = new ArrayMatrix<ModInt, ModIntOperator>(
                cr.Repeat(M).Select(cr => cr.Repeat(K).Select(cr => ModInt.Raw(cr))));

            cw.WriteGrid(mat1.Strassen(mat2).Value);
            return cw;
        }
        int N, M, K;
    }
}