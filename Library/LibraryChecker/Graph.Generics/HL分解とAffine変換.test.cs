using AtCoder;
using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using ModIntOperator = AtCoder.StaticModIntOperator<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.DataStructure
{
    public class HL分解とAffine変換Test
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/vertex_set_path_composite
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var lines = cr.Repeat(N).Select<(int, int)>(cr => (cr, cr));
            var gb = new GraphBuilder(N, false);
            for (var i = 1; i < N; i++)
                gb.Add(cr, cr);

            var tree = gb.ToTree();
            var seg1 = new Segtree<AffineTransformation<ModInt, ModIntOperator>, Op1>(N);
            var seg2 = new Segtree<AffineTransformation<ModInt, ModIntOperator>, Op2>(N);

            for (int i = 0; i < lines.Length; i++)
            {
                var a = ModInt.Raw(lines[i].Item1);
                var b = ModInt.Raw(lines[i].Item2);
                var f = new AffineTransformation<ModInt, ModIntOperator>(a, b);
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
                    var f = new AffineTransformation<ModInt, ModIntOperator>(c, d);
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

    struct Op1 : ISegtreeOperator<AffineTransformation<ModInt, ModIntOperator>>
    {
        [MethodImpl(256)]
        public AffineTransformation<ModInt, ModIntOperator> Operate(AffineTransformation<ModInt, ModIntOperator> x, AffineTransformation<ModInt, ModIntOperator> y)
            => y * x;
        private readonly static AffineTransformation<ModInt, ModIntOperator> identity = new AffineTransformation<ModInt, ModIntOperator>(1, 0);
        public AffineTransformation<ModInt, ModIntOperator> Identity => identity;
    }
    struct Op2 : ISegtreeOperator<AffineTransformation<ModInt, ModIntOperator>>
    {
        [MethodImpl(256)]
        public AffineTransformation<ModInt, ModIntOperator> Operate(AffineTransformation<ModInt, ModIntOperator> x, AffineTransformation<ModInt, ModIntOperator> y)
            => x * y;
        private readonly static AffineTransformation<ModInt, ModIntOperator> identity = new AffineTransformation<ModInt, ModIntOperator>(1, 0);
        public AffineTransformation<ModInt, ModIntOperator> Identity => identity;
    }
}