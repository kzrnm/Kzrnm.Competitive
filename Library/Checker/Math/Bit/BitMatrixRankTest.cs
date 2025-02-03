using Kzrnm.Competitive.IO;
using System;

namespace Kzrnm.Competitive.MathNs;

internal class BitMatrixRankTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/matrix_rank_mod_2";
    public override double? Tle => 5 * 2;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int M = cr;
        var matrix = BitMatrix.Parse(cr.Repeat(N).Ascii());

        var g = matrix.GaussianElimination(false);
        int max = Math.Min(N, M);
        int rank = -1;
        for (int i = 0; i < max; i++, rank++)
        {
            var row = g.RowUnsafe(i);
            if ((uint)row.Lsb() >= (uint)row.Length) break;
        }

        return rank + 1;
    }
}
