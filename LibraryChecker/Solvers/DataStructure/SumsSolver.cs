using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class SumsSolver : Solver
    {
        public override string Name => "static_range_sum";
        public override double TimeoutSecond => 5;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;
            var sums = new LongSums(cr.Repeat(n));
            for (int i = 0; i < q; i++)
            {
                int l = cr;
                int r = cr;
                cw.WriteLine(sums[l..r]);
            }
        }
    }
}
