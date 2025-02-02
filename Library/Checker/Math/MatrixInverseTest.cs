using AtCoder;
using Kzrnm.Competitive.IO;
using System.Linq;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using MontgomeryModInt = Kzrnm.Competitive.MontgomeryModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs;

internal class MatrixMatrixInverseTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/inverse_matrix";
    public override double? Tle => 5 * 2;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        var a = cr.Repeat(N * N).Int();
        var m1 = Static(a, N);
        var m2 = Montgomery(a, N);

        var inv1 = m1.Inv();
        var inv2 = m2.Inv();

        ShouldEqual(inv1, inv2);

        if (inv2.IsZero)
            cw.WriteLine(-1);
        else
            cw.WriteGrid(inv2);
        return null;
    }
    static ArrayMatrix<ModInt> Static(int[] a, int N)
        => new ArrayMatrix<ModInt>(a.Select(v => (ModInt)v).ToArray(), N, N);
    static SimdModMatrix<Mod998244353> Montgomery(int[] a, int N)
        => new SimdModMatrix<Mod998244353>(a.Select(v => (MontgomeryModInt)v).ToArray(), N, N);
    static void ShouldEqual(ArrayMatrix<ModInt> m1, SimdModMatrix<Mod998244353> m2)
    {
        if (m1.IsZero != m2.IsZero) throw new System.Exception("Diff IsZero");
        if (m1.Height != m2.Height) throw new System.Exception("Diff height");
        if (m1.Width != m2.Width) throw new System.Exception("Diff width");

        for (int h = 0; h < m1.Height; h++)
            for (int w = 0; w < m1.Width; w++)
                if (m1[h, w].Value != m2[h, w].Value)
                    throw new System.Exception("Difference");
    }
}
