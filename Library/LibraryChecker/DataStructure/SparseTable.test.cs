using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.DataStructure
{
    public class SparseTable
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/staticrmq
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;

            var st = new SparseTable<int, Op>(cr.Repeat(n));
            for (int i = 0; i < q; i++)
            {
                int l = cr;
                int r = cr;
                cw.WriteLine(st[l..r]);
            }
        }
        struct Op : ISparseTableOperator<int>
        {
            [MethodImpl(256)]
            public int Operate(int x, int y) => System.Math.Min(x, y);
        }
    }
}
