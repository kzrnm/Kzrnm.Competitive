using Kzrnm.Competitive.IO;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Graph
{
    internal class TreeDiameterTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/tree_diameter";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            var gb = new WULongGraphBuilder(N, false);
            for (var i = 1; i < N; i++)
                gb.Add(cr, cr, cr);
            var root = gb.ToTree(0).AsArray().MaxBy(n => n.DepthLength).index;
            var tree = gb.ToTree(root);
            var ix = tree.AsArray().MaxBy(n => n.DepthLength).index;

            var list = new List<int>();
            ulong sum = 0;
            while (ix >= 0)
            {
                list.Add(ix);
                var e = tree[ix].Parent;
                ix = e.To;
                sum += e.Value;
            }
            cw.WriteLineJoin(sum, list.Count);
            cw.WriteLineJoin(list.AsSpan());
            return null;
        }
    }
}