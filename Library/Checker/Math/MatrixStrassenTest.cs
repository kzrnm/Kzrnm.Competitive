using AtCoder;
using Kzrnm.Competitive.IO;
using MontgomeryModInt = Kzrnm.Competitive.MontgomeryModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.MathNs;

internal class MatrixStrassenTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/matrix_product";
    public override double? Tle => 10 * 2;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int M = cr;
        int K = cr;

        var a = cr.Repeat(N * M).Select(cr => (MontgomeryModInt)cr.Int());
        var b = cr.Repeat(M * K).Select(cr => (MontgomeryModInt)cr.Int());

        var s1 = Static(a, N, M) * Static(b, M, K);
        var s2 = Montgomery(a, N, M) * Montgomery(b, M, K);

        ShouldEqual(s1, s2);

        cw.WriteGrid(s2);
        return null;
    }
    static ArrayMatrix<MontgomeryModInt> Static(MontgomeryModInt[] a, int N, int M)
        => new ArrayMatrix<MontgomeryModInt>(a, N, M);
    static SimdModMatrix<Mod998244353> Montgomery(MontgomeryModInt[] a, int N, int M)
        => new SimdModMatrix<Mod998244353>(a, N, M);
    static void ShouldEqual(ArrayMatrix<MontgomeryModInt> m1, SimdModMatrix<Mod998244353> m2)
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
