using Kzrnm.Competitive.IO;
using System;
using System.Runtime.InteropServices;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs;

internal class MatrixRankTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/matrix_rank";
    public override double? Tle => 5 * 2;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int M = cr;
        var matrix = new ArrayMatrix<ModInt>(cr.Repeat(N * M).Select(cr => ModInt.Raw(cr)), N, M);

        var g = matrix.GaussianElimination(false);
        int max = Math.Min(N, M);
        int rank = -1;
        for (int i = 0; i < max; i++, rank++)
        {
            var row = MemoryMarshal.Cast<ModInt, uint>(g.RowSpan(i));
            if (row.IndexOfAnyExcept(0u) < 0) break;
        }

        return rank + 1;
    }
}
