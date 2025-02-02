using Kzrnm.Competitive.IO;
using System.Linq;

namespace Kzrnm.Competitive.Graph;

internal class CartesianTreeTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/cartesian_tree";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int n = cr;
        int[] a = cr.Repeat(n);
        var ps = CartesianTree.Create(a);
        cw.WriteLineJoin(ps.Select((p, i) => p < 0 ? i : p));
        return null;
    }
}
