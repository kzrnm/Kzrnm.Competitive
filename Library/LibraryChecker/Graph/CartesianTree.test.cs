using Kzrnm.Competitive.IO;
using System.Linq;

namespace Kzrnm.Competitive.Graph
{
    public class CartesianTreeTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/cartesian_tree
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int n = cr;
            int[] a = cr.Repeat(n);
            var ps = CartesianTree.Create(a);
            cw.WriteLineJoin(ps.Select((p, i) => p < 0 ? i : p));
            return null;
        }
    }
}
