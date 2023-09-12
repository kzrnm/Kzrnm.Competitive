using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.DataStructure
{
    internal class SparseTable : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/staticrmq";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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
            return null;
        }
        readonly struct Op : ISparseTableOperator<int>
        {
            [MethodImpl(256)]
            public int Operate(int x, int y) => System.Math.Min(x, y);
        }
    }
}
