using Kzrnm.Competitive.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Graph.Generics;

internal class 全方位木DP2 : BaseSolver
{
    public override string Url => "https://yukicoder.me/problems/no/768";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        var tree = cr.Tree(N).ToTree();
        var dp = tree.Rerooting().Run<bool, Op>().Select((b, i) => (b, i)).Where(t => !t.b).Select(t => t.i + 1).ToArray();
        cw.WriteLine(dp.Length);
        if (dp.Length > 0) cw.WriteLines(dp);
        return null;
    }
    readonly struct Op : IRerootingOperator<bool, GraphEdge>
    {
        public bool Identity => default;

        [MethodImpl(256)]
        public bool Merge(bool x, bool y) => x || y;
        [MethodImpl(256)]
        public bool Propagate(bool x, int parent, GraphEdge edge) => !x;
    }
}