using AtCoder;
using Kzrnm.Competitive.IO;
using System.Linq;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using MontgomeryModInt = Kzrnm.Competitive.MontgomeryModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs;

internal class MatrixLinearSystemTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/system_of_linear_equations";
    public override double? Tle => 5 * 2;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int M = cr;
        var a = cr.Repeat(N * M).Int();
        var b = cr.Repeat(N).Int();

        var m1 = Static(a, N, M);
        var m2 = Montgomery(a, N, M);

        var r1 = m1.LinearSystem(b.Select(v => (ModInt)v).ToArray());
        var r2 = m2.LinearSystem(b.Select(v => (MontgomeryModInt)v).ToArray());

        ShouldEqual(r1, r2);

        cw.WriteLine(r2.Length - 1);
        if (r2.Length > 0)
            cw.WriteGrid(r2);
        return null;
    }
    static ArrayMatrix<ModInt> Static(int[] a, int N, int M)
        => new ArrayMatrix<ModInt>(a.Select(v => (ModInt)v).ToArray(), N, M);
    static SimdModMatrix<Mod998244353> Montgomery(int[] a, int N, int M)
        => new SimdModMatrix<Mod998244353>(a.Select(v => (MontgomeryModInt)v).ToArray(), N, M);
    static void ShouldEqual(ModInt[][] m1, MontgomeryModInt[][] m2)
    {
        if (m1.Length != m2.Length) throw new System.Exception("Diff height");
        if (m1.Length > 0 && m1[0].Length != m2[0].Length) throw new System.Exception("Diff width");

        for (int h = 0; h < m1.Length; h++)
            for (int w = 0; w < m1[0].Length; w++)
                if (m1[h][w].Value != m2[h][w].Value)
                    throw new System.Exception("Difference");
    }
}
