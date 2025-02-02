using AtCoder;
using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.Graph;

internal class HL分解とAffine変換Test : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/vertex_set_path_composite";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int Q = cr;
        var lines = cr.Repeat(N).Select<(int, int)>(cr => (cr, cr));
        var tree = cr.Tree(N, based: 0).ToTree();

        var seg1 = new Segtree<Mod998244353AffineTransformation, Op1>(N);
        var seg2 = new Segtree<Mod998244353AffineTransformation, Op2>(N);

        for (int i = 0; i < lines.Length; i++)
        {
            var a = ModInt.Raw(lines[i].Item1);
            var b = ModInt.Raw(lines[i].Item2);
            var f = new Mod998244353AffineTransformation(a, b);
            var p = tree.HlDecomposition.down[i];
            seg1[p] = f;
            seg2[p] = f;
        }

        for (int q = 0; q < Q; q++)
        {
            int t = cr;
            if (t == 0)
            {
                int p = cr;
                ModInt c = ModInt.Raw(cr);
                ModInt d = ModInt.Raw(cr);
                var f = new Mod998244353AffineTransformation(c, d);
                p = tree.HlDecomposition.down[p];
                seg1[p] = f;
                seg2[p] = f;
            }
            else
            {
                int u = cr;
                int v = cr;
                int x = cr;
                ModInt res = x;
                tree.HlDecomposition.PathQuery(u, v, true, (f, t) =>
                {
                    if (f < t)
                    {
                        res = seg2[f..t].Apply(res);
                    }
                    else
                    {
                        res = seg1[t..f].Apply(res);
                    }
                });
                cw.WriteLine(res);
            }
        }
        return null;
    }
}

readonly struct Op1 : ISegtreeOperator<Mod998244353AffineTransformation>
{
    [MethodImpl(256)]
    public Mod998244353AffineTransformation Operate(Mod998244353AffineTransformation x, Mod998244353AffineTransformation y)
        => x.Apply(y);
    static readonly Mod998244353AffineTransformation identity = new Mod998244353AffineTransformation(1, 0);
    public Mod998244353AffineTransformation Identity => identity;
}
readonly struct Op2 : ISegtreeOperator<Mod998244353AffineTransformation>
{
    [MethodImpl(256)]
    public Mod998244353AffineTransformation Operate(Mod998244353AffineTransformation x, Mod998244353AffineTransformation y)
        => y.Apply(x);
    static readonly Mod998244353AffineTransformation identity = new Mod998244353AffineTransformation(1, 0);
    public Mod998244353AffineTransformation Identity => identity;
}