using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Kzrnm.Competitive.MathNs;

internal class BitMatrixRankTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/matrix_rank_mod_2";
    public override double? Tle => 5 * 2;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int M = cr;

        var rows = Rows(cr, N, M);

        int max = Math.Min(N, M);
        if (M <= 8)
            return Generic<byte>(rows, max);
        else if (M <= 16)
            return Generic<ushort>(rows, max);
        else if (M <= 32)
            return Generic<uint>(rows, max);
        else if (M <= 64)
            return Generic<ulong>(rows, max);
        else
            return NonGeneric(rows, max);
    }
    static Asciis[] Rows(ConsoleReader cr, int N, int M)
    {
        if (M == 0) return Array.Empty<Asciis>();
        if (N < (1 << 20)) return cr.Repeat(N).Ascii();

        var hs = new HashSet<Asciis>(16);
        while (--N >= 0)
        {
            hs.Add(cr.Ascii());
        }
        return hs.ToArray();
    }

    static int Generic<T>(Asciis[] rows, int max) where T : unmanaged, IBinaryInteger<T>
    {
        int rank = -1;
        var matrix = BitMatrix<T>.Parse(rows);
        matrix = matrix.GaussianElimination(false);
        for (int i = 0; i < max; i++, rank++)
        {
            if (T.IsZero(matrix.Row(i))) break;
        }
        return ++rank;
    }
    static int NonGeneric(Asciis[] rows, int max)
    {
        int rank = -1;
        var matrix = BitMatrix.Parse(rows);
        matrix = matrix.GaussianElimination(false);
        for (int i = 0; i < max; i++, rank++)
        {
            var row = matrix.RowUnsafe(i);
            if ((uint)row.Lsb() >= (uint)row.Length) break;
        }
        return ++rank;
    }
}
