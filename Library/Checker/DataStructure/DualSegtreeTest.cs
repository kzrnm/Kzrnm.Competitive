using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.DataStructure;

internal class DualSegtreeTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/range_affine_point_get";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int Q = cr;
        var seg = new DualSegtree<Mod998244353AffineTransformation, Op>(cr.Repeat(N).Select(cr => new Mod998244353AffineTransformation(ModInt.Zero, ModInt.Raw(cr.Int()))));

        while (--Q >= 0)
        {
            int type = cr;
            if (type == 0)
            {
                int l = cr;
                int r = cr;
                var b = ModInt.Raw(cr);
                var c = ModInt.Raw(cr);
                seg.Apply(l, r, new(b, c));
            }
            else
            {
                int i = cr;
                cw.WriteLine(seg[i].b);
            }
        }
        return null;
    }
    readonly record struct Op : IDualSegtreeOperator<Mod998244353AffineTransformation>
    {
        public Mod998244353AffineTransformation FIdentity => new(ModInt.One, ModInt.Zero);

        [MethodImpl(256)]
        public Mod998244353AffineTransformation Composition(Mod998244353AffineTransformation nf, Mod998244353AffineTransformation cf) => nf.Apply(cf);
    }
}
