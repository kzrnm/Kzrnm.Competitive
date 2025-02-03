using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.Graph;

internal class WeightedUnionFindTest2 : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/unionfind_with_potential_non_commutative_group";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int n = cr;
        int q = cr;

        var dsu = new WeightedUnionFind<Matrix2x2<ModInt>, MarixOp>(n);

        for (int i = 0; i < q; i++)
        {
            int t = cr;
            int u = cr;
            int v = cr;
            if (t == 0)
                cw.WriteLine(dsu.Merge(v, u, new Matrix2x2<ModInt>((cr.UInt(), cr.UInt()), (cr.UInt(), cr.UInt()))) ? 1 : 0);
            else if (dsu.Same(u, v))
            {
                var m = dsu.WeightDiff(v, u);
                var (v1, v2) = m.Row0;
                var (v3, v4) = m.Row1;
                cw.WriteLineJoin(v1, v2, v3, v4);
            }
            else
                cw.WriteLine(-1);
        }
        return null;
    }

    readonly record struct MarixOp : Internal.IWeightedUnionFindOperator<Matrix2x2<ModInt>>
    {
        [MethodImpl(256)]
        public Matrix2x2<ModInt> Operate(Matrix2x2<ModInt> x, Matrix2x2<ModInt> y) => x * y;

        public Matrix2x2<ModInt> Identity => Matrix2x2<ModInt>.MultiplicativeIdentity;

        [MethodImpl(256)]
        public Matrix2x2<ModInt> Negate(Matrix2x2<ModInt> v) => v.Inv();
    }
}