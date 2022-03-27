using AtCoder;
using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.DataStructure
{
    public class オイラーツアーとSegtreeTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/vertex_set_path_composite
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var lines = cr.Repeat(N).Select(cr => (ModInt.Raw(cr), ModInt.Raw(cr)));
            var gb = new GraphBuilder(N, false);
            for (var i = 1; i < N; i++)
                gb.Add(cr, cr);

            var et = gb.ToTree().EulerianTour();
            var seg1 = new Segtree<(ModInt, ModInt), Op>(2 * N);
            var seg2 = new Segtree<(ModInt, ModInt), Op>(2 * N);

            for (int i = 0; i < N; i++)
            {
                var (l, r) = et[i];
                var ainv = lines[i].Item1.Inv();
                var inv = (ainv, -lines[i].Item2 * ainv);
                seg1[l] = inv;
                seg1[r] = lines[i];
                seg2[l] = lines[i];
                seg2[r] = inv;
            }

            for (int q = 0; q < Q; q++)
            {
                int t = cr;
                if (t == 0)
                {
                    int p = cr;
                    ModInt c = ModInt.Raw(cr);
                    ModInt d = ModInt.Raw(cr);

                    var (l, r) = et[p];
                    lines[p] = (c, d);
                    var ainv = lines[p].Item1.Inv();
                    var inv = (ainv, -lines[p].Item2 * ainv);
                    seg1[l] = inv;
                    seg1[r] = lines[p];
                    seg2[l] = lines[p];
                    seg2[r] = inv;
                }
                else
                {
                    int u = cr;
                    int v = cr;
                    int x = cr;
                    var lca = et.LowestCommonAncestor(u, v);

                    var f = new Op().Operate(
                        seg1[et[u].right..et[lca].right],
                        seg2[et[lca].left..(et[v].left + 1)]);
                    cw.WriteLine(f.Item1 * x + f.Item2);
                }
            }
            return null;
        }
    }

    struct Op : ISegtreeOperator<(ModInt, ModInt)>
    {
        [MethodImpl(256)]
        public (ModInt, ModInt) Operate((ModInt, ModInt) x, (ModInt, ModInt) y)
        {
            var (a, b) = x;
            var (c, d) = y;
            return (c * a, c * b + d);
        }
        private readonly static (ModInt, ModInt) identity = (1, 0);
        public (ModInt, ModInt) Identity => identity;
    }
}