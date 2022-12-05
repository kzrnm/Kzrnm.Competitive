using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.DataStructure
{
    internal class DisjointSparseTable : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/static_range_sum";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var st = new DisjointSparseTable<long, Op>(cr.Repeat(N));
            for (int i = 0; i < Q; i++)
            {
                int l = cr;
                int r = cr;
                cw.WriteLine(st[l..r]);
            }
            return null;
        }
        struct Op : ISparseTableOperator<long> { [MethodImpl(256)] public long Operate(long x, long y) => x + y; }
    }
}
