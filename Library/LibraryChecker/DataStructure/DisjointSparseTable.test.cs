using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.DataStructure
{
    public class DisjointSparseTable
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/static_range_sum
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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
