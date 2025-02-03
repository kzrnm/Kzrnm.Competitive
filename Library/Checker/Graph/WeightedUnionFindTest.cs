using Kzrnm.Competitive.IO;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.Graph;

internal class WeightedUnionFindTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/unionfind_with_potential";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int n = cr;
        int q = cr;

        var dsu = new WeightedUnionFind<ModInt>(n);

        for (int i = 0; i < q; i++)
        {
            int t = cr;
            int u = cr;
            int v = cr;
            if (t == 0)
                cw.WriteLine(dsu.Merge(u, v, ModInt.Raw(cr)) ? 1 : 0);
            else
                cw.WriteLine(
                    dsu.Same(u, v) ? dsu.WeightDiff(u, v).Value : -1);
        }
        return null;
    }
}
