using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.DataStructure;

internal class DisjointSparseTable : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/static_range_sum";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int Q = cr;
        var a = cr.Repeat(N).Long();
        var st = new DisjointSparseTable<long, Op>(a);
        while (--Q >= 0)
        {
            int u = cr;
            int v = cr;
            cw.WriteLine(st[u..v]);
        }
        return null;
    }
    readonly struct Op : ISparseTableOperator<long>
    {
        [MethodImpl(256)]
        public long Operate(long x, long y) => x + y;
    }
}
