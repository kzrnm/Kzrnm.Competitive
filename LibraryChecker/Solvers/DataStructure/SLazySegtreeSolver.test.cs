using Kzrnm.Competitive.IO;
using System.Linq;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class SLazySegtreeSolver : Solver
    {
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/range_affine_range_sum
        public override double TimeoutSecond => 5;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var seg = new SLazySegtree<uint, (uint b, uint c), LazySegtreeSolverOp>(cr.Repeat(N).Select(cr => (uint)cr.Int()));
            for (int q = 0; q < Q; q++)
            {
                int t = cr;
                int l = cr;
                int r = cr;
                if (t == 0)
                {
                    uint b = (uint)cr.Int();
                    uint c = (uint)cr.Int();
                    seg.Apply(l, r, (b, c));
                }
                else
                    cw.WriteLine(seg[l..r]);
            }
        }
    }
    struct LazySegtreeSolverOp : ISLazySegtreeOperator<uint, (uint b, uint c)>
    {
        const uint MOD = 998244353;
        public uint Operate(uint x, uint y) => SafeAdd(x, y);
        public uint Mapping((uint b, uint c) f, uint x, int size) => SafeAdd(SafeMul(f.b, x), SafeMul(f.c, (uint)size));
        public (uint b, uint c) Composition((uint b, uint c) f, (uint b, uint c) g) => (SafeMul(f.b, g.b), SafeAdd(SafeMul(f.b, g.c), f.c));
        public uint Identity => 0;
        public (uint b, uint c) FIdentity => (1, 0);
        static uint SafeAdd(uint a, uint b)
        {
            var r = a + b;
            return r < MOD ? r : r - MOD;
        }
        static uint SafeMul(uint a, uint b) => (uint)((ulong)a * b % MOD);
    }
}