using Kzrnm.Competitive.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Graph;

internal class LinkCutTreeTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/dynamic_tree_vertex_add_path_sum";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int Q = cr;
        var lct = new LinkCutTree<long, long, Op>();
        var nodes = Enumerable.Range(0, N).Select(i => lct.MakeNode(i, cr)).ToArray();
        for (int i = 0; i < N - 1; i++)
        {
            lct.Link(nodes[cr], nodes[cr]);
        }
        for (int q = 0; q < Q; q++)
        {
            int T = cr;
            if (T == 0)
            {
                int u = cr;
                int v = cr;
                int w = cr;
                int x = cr;
                lct.Evert(nodes[u]);
                lct.Cut(nodes[v]);
                lct.Link(nodes[w], nodes[x]);
            }
            else if (T == 1)
            {
                int p = cr;
                int x = cr;
                lct.Evert(nodes[p]);
                lct.SetPropagate(nodes[p], x);
            }
            else
            {
                int u = cr;
                int v = cr;
                lct.Evert(nodes[u]);
                lct.Expose(nodes[v]);
                cw.WriteLine(nodes[v].Sum);
            }
        }
        return null;
    }

    readonly struct Op : ILinkCutTreeOperator<long, long>
    {
        [MethodImpl(256)]
        public long Operate(long x, long y) => x + y;
        [MethodImpl(256)]
        public long Mapping(long f, long x, int size) => size * f + x;
        [MethodImpl(256)]
        public long Composition(long nf, long cf) => nf + cf;

        public long Identity => default;

        public long FIdentity => default;

        [MethodImpl(256)]
        public long Inverse(long v) => v;
    }
}